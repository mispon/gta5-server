using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Helpers;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Places;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Ninject;
using Ninject.Syntax;

namespace gta_mp_server.Managers.Work.BusDriver {
    /// <summary>
    /// Работа водителем автобуса
    /// </summary>
    internal class BusDriverManager : Script {
        private const int TICKET_COST = 5;
        private const int NOTIFY_DISTANCE = 30;
        internal const string ROUTE_KEY = "WorkRoute";
        internal const string SHAPE_KEY = "BusStop";
        internal const string RED_ROUTE = "red_route";
        internal const string BLUE_ROUTE = "blue_route";
        internal const string YELLOW_ROUTE = "yellow_route";
        internal const string GREEN_ROUTE = "green_route";

        /// <summary>
        /// Количество водителей на линиях маршрутов
        /// </summary>
        internal static Dictionary<string, int> RouteDrivers = new Dictionary<string, int> {
            {RED_ROUTE, 0}, {BLUE_ROUTE, 0}, {GREEN_ROUTE, 0},  {YELLOW_ROUTE, 0}
        };

        private readonly Dictionary<long, WorkReward> _workReward = new Dictionary<long, WorkReward> {
            [1] = new WorkReward { Salary = 20, Exp = 14, WorkExp = 12 },
            [2] = new WorkReward { Salary = 22, Exp = 16, WorkExp = 14 },
            [3] = new WorkReward { Salary = 24, Exp = 18, WorkExp = 17 },
            [4] = new WorkReward { Salary = 26, Exp = 21, WorkExp = 20 },
            [5] = new WorkReward { Salary = 28, Exp = 25, WorkExp = 0 }
        };
        
        private readonly HashSet<BusPoint> _points = new HashSet<BusPoint>();

        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IWorkInfoManager _workInfoManager;

        public BusDriverManager() : this(ServerKernel.Kernel) {
            API.onPlayerEnterVehicle += OnBusEnter;
            API.onPlayerExitVehicle += OnBusExit;
            CreateRoute(RED_ROUTE, WorkData.RED_SHAPE_VALUE);
            CreateRoute(BLUE_ROUTE, WorkData.BLUE_SHAPE_VALUE);
            CreateRoute(YELLOW_ROUTE, WorkData.YELLOW_SHAPE_VALUE);
            CreateRoute(GREEN_ROUTE, WorkData.GREEN_SHAPE_VALUE);
        }

        public BusDriverManager(IResolutionRoot kernel) {
            _playerInfoManager = kernel.Get<IPlayerInfoManager>();
            _workInfoManager = kernel.Get<IWorkInfoManager>();
        }

        /// <summary>
        /// Обработчик входа в автобус
        /// </summary>
        private void OnBusEnter(Client player, NetHandle vehicle, int seat) {
            if (API.getEntityData(vehicle, BusDepot.BUS_KEY) == null) {
                return;
            }
            if (seat < 0) { 
                OnDriverEnter(player);
            }
            else {
                OnPassengerEnter(player);
            }
        }

        /// <summary>
        /// Обработчик выхода из автобуса
        /// </summary>
        private void OnBusExit(Client player, NetHandle vehicle, int seat) {
            if (API.getEntityData(vehicle, BusDepot.BUS_KEY) == null) {
                return;
            }
            if (seat < 0) {
                OnDriverExit(player);
            }
            else {
                OnPassengerExit(player);
            }
        }

        /// <summary>
        /// Зашел водитель
        /// </summary>
        private void OnDriverEnter(Client player) {
            if (player.getData(WorkData.IS_BUS_DRIVER) == null) {
                API.sendNotificationToPlayer(player, "~r~Вы не работаете водителем автобуса");
                API.warpPlayerOutOfVehicle(player);
                return;
            }
            API.sendNotificationToPlayer(player, "Двигайтесь по ~b~маршруту~w~, подбирая людей");
            var route = (string) player.getData(ROUTE_KEY);
            API.triggerClientEvent(player, ClientEvent.SHOW_NEXT_BUS_STOP, GetRoutePoints(route).First());
        }

        /// <summary>
        /// Вышел водитель
        /// </summary>
        private void OnDriverExit(Client player) {
            if (player.getData(WorkData.IS_BUS_DRIVER) == null) {
                return;
            }
            var position = API.getEntityPosition(player);
            var distance = Vector3.Distance(position, MainPosition.BusDriver);
            if (distance >= NOTIFY_DISTANCE) {
                API.sendNotificationToPlayer(player, "Чтобы получить ~b~зарплату~w~, вернитесь в депо");
            }
            API.triggerClientEvent(player, ClientEvent.HIDE_BUS_ROUTE);
        }

        /// <summary>
        /// Зашел пассажир
        /// </summary>
        private void OnPassengerEnter(Client player) {
            var balance = _playerInfoManager.GetInfo(player).Balance;
            if (balance < TICKET_COST) {
                player.warpOutOfVehicle();
                API.sendNotificationToPlayer(player, "~r~У вас недостаточно денег на билет", true);
                return;
            }
            API.sendNotificationToPlayer(player, $"Стоимость проезда ~b~{TICKET_COST}$");
        }

        /// <summary>
        /// Вышел пассажир
        /// </summary>
        private void OnPassengerExit(Client player) {
            _playerInfoManager.SetBalance(player, -TICKET_COST);
            API.sendNotificationToPlayer(player, $"Списано ~b~{TICKET_COST}$");
        }

        /// <summary>
        /// Построить маршрут
        /// </summary>
        private void CreateRoute(string routeKey, string shapeValue) {
            var positions = GetRoutePoints(routeKey);
            for (var i = 0; i < positions.Count; i++) {
                var position = positions[i];
                var nextPosition = i + 1 < positions.Count ? positions[i + 1] : positions[0];
                var shape = CreateBusPointShape(shapeValue, position);
                shape.setData(SHAPE_KEY, string.Format(shapeValue, i));
                var busPoint = CreateBusPoint(shape, i, positions, nextPosition);
                _points.Add(busPoint);
            }
        }

        /// <summary>
        /// Создать обработчик событий автобусной остановки
        /// </summary>
        private ColShape CreateBusPointShape(string shapeValue, Vector3 position) {
            var shape = API.createSphereColShape(position, 4F);
            shape.onEntityEnterColShape += (colShape, entity) => {
                var player = API.getPlayerFromHandle(entity);
                if (!ConditionsCorrect(player, shape)) {
                    return;
                }
                ActionHelper.SetAction(player, 3000, () => {
                    ShowNextStop(player, shape, shapeValue);
                    SetPlayerReward(player);
                });
            };
            shape.onEntityExitColShape += (colShape, entity) => {
                var player = API.getPlayerFromHandle(entity);
                if (!ConditionsCorrect(player, shape)) {
                    return;
                }
                ActionHelper.CancelAction(player);
            };
            return shape;
        }

        /// <summary>
        /// Показать следующую остановку
        /// </summary>
        private void ShowNextStop(Client player, ColShape shape, string shapeValue) {
            var currentStop = _points.First(e => e.ColShape == shape);
            player.setData(SHAPE_KEY, string.Format(shapeValue, currentStop.NextPointNumber));
            API.triggerClientEvent(player, ClientEvent.SHOW_NEXT_BUS_STOP, currentStop.NextPoint);
        }

        /// <summary>
        /// Выдать вознаграждение
        /// </summary>
        private void SetPlayerReward(Client player) {
            var workLevel = _workInfoManager.GetWorkInfo(player, WorkType.BusDriver).Level;
            var reward = _workReward[workLevel];
            _workInfoManager.SetSalary(player, WorkType.BusDriver, reward.Salary);
            _workInfoManager.SetExperience(player, WorkType.BusDriver, reward.WorkExp);
            _playerInfoManager.SetExperience(player, reward.Exp);
        }

        /// <summary>
        /// Создать точку остановки автобуса
        /// </summary>
        private static BusPoint CreateBusPoint(ColShape shape, int i, IReadOnlyCollection<Vector3> positions, Vector3 nextPosition) {
            var busPoint = new BusPoint {
                ColShape = shape,
                NextPoint = nextPosition,
                NextPointNumber = i + 1 < positions.Count ? i + 1 : 0
            };
            return busPoint;
        }

        /// <summary>
        /// Проверка корректности водителя автобуса
        /// </summary>
        private static bool ConditionsCorrect(Client player, ColShape shape) {
            return PlayerHelper.PlayerCorrect(player, true) && 
                   PlayerHelper.GetData(player, WorkData.IS_BUS_DRIVER, false) && 
                   player?.getData(SHAPE_KEY) == shape.getData(SHAPE_KEY);
        }

        /// <summary>
        /// Возвращает ключ точки маршрута по его значению
        /// </summary>
        public static string GetRouteShapeKey(string routeValue) {
            switch (routeValue) {
                case RED_ROUTE:
                    return string.Format(WorkData.RED_SHAPE_VALUE, 0);
                case BLUE_ROUTE:
                    return string.Format(WorkData.BLUE_SHAPE_VALUE, 0);
                case YELLOW_ROUTE:
                    return string.Format(WorkData.YELLOW_SHAPE_VALUE, 0);
                case GREEN_ROUTE:
                    return string.Format(WorkData.GREEN_SHAPE_VALUE, 0);
                default:
                    throw new ArgumentException("Пришел неизвестный ключ маршрута");
            }
        }

        /// <summary>
        /// Возвращает точки выбранного маршрута
        /// </summary>
        private static List<Vector3> GetRoutePoints(string route) {
            switch (route) {
                case RED_ROUTE:
                    return BusPositionGetter.GetRedRoute();
                case BLUE_ROUTE:
                    return BusPositionGetter.GetBlueRoute();
                case GREEN_ROUTE:
                    return BusPositionGetter.GetGreenRoute();
                case YELLOW_ROUTE:
                    return BusPositionGetter.GetYellowRoute();
                default:
                    return null;
            }
        }

        /// <summary>
        /// Остановки автобуса
        /// </summary>
        private class BusPoint {
            public ColShape ColShape { get; set; }
            public Vector3 NextPoint { get; set; }
            public int NextPointNumber { get; set; }
        }
    }
}