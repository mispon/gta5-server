using System;
using System.Linq;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.BusDriver;
using gta_mp_server.Managers.Work.Interfaces;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.MenuHandlers.Work {
    /// <summary>
    /// Обработчик меню работы водителем автобуса
    /// </summary>
    internal class BusDriverMenuHandler : BaseWorkMenu {
        private const int MIN_LEVEL = 4;

        private readonly IWorkEquipmentManager _workEquipmentManager;

        public BusDriverMenuHandler() { }

        public BusDriverMenuHandler(IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager, IWorkEquipmentManager workEquipmentManager)
            : base(playerInfoManager, workInfoManager) {
            _workEquipmentManager = workEquipmentManager;
        }

        /// <summary>
        /// Инициализировать меню
        /// </summary>
        public override void Initialize() {
            ClientEventHandler.Add(ClientEvent.WORK_ON_BUS, WorkAsBusDriver);
            ClientEventHandler.Add(ClientEvent.BUS_DRIVER_SALARY, GetSalary);
        }

        /// <summary>
        /// Начать работу водителем автобуса
        /// </summary>
        private void WorkAsBusDriver(Client player, object[] args) {
            if (HasActiveWork(player) || !CanWork(player, MIN_LEVEL)) {
                return;
            }
            var routeValue = GetFreestRoute();
            BusDriverManager.RouteDrivers[routeValue] += 1;
            var firstPoint = BusDriverManager.GetRouteShapeKey(routeValue);
            player.setData(BusDriverManager.SHAPE_KEY, firstPoint);
            player.setData(BusDriverManager.ROUTE_KEY, routeValue);
            StartWork(player);
            _workEquipmentManager.SetEquipment(player);
            NotifyByRoute(player, routeValue);
        }

        /// <summary>
        /// Возвращает самую свободную ветку
        /// </summary>
        private static string GetFreestRoute() {
            var result = BusDriverManager.RouteDrivers.First();
            foreach (var route in BusDriverManager.RouteDrivers) {
                if (route.Value >= result.Value) {
                    continue;
                }
                result = route;
            }
            return result.Key;
        }

        /// <summary>
        /// Общие действия начала работы водителем
        /// </summary>
        private void StartWork(Client player) {
            WorkInfoManager.CreateInfoIfNeed(player, WorkType.BusDriver);
            WorkInfoManager.SetActivity(player, WorkType.BusDriver, true);
            player.setData(WorkData.IS_BUS_DRIVER, true);
            API.triggerClientEvent(player, ServerEvent.HIDE_ONEIL_MENU);
        }

        /// <summary>
        /// Получить зарплату
        /// </summary>
        private void GetSalary(Client player, object[] objects) {
            var activeWork = WorkInfoManager.GetActiveWork(player);
            if (!WorkIsCorrect(player, activeWork, () => activeWork.Type == WorkType.BusDriver)) {
                return;
            }
            WorkInfoManager.SetActivity(player, WorkType.BusDriver, false);
            DecreaseRouteDriversCount(player);
            player.resetData(WorkData.IS_BUS_DRIVER);
            player.resetData(BusDriverManager.SHAPE_KEY);
            player.resetData(BusDriverManager.ROUTE_KEY);
            PlayerInfoManager.SetPlayerClothes(player);
            PayOut(player, activeWork);
            API.triggerClientEvent(player, ServerEvent.HIDE_ONEIL_MENU);
        }

        /// <summary>
        /// Вычесть водителя из общего количества на ветке
        /// </summary>
        private static void DecreaseRouteDriversCount(Client player) {
            var route = (string) player.getData(BusDriverManager.ROUTE_KEY);
            if (BusDriverManager.RouteDrivers.ContainsKey(route)) {
                BusDriverManager.RouteDrivers[route] -= 1;
            }
        }

        /// <summary>
        /// Оповестить игрока о выбранном маршруте
        /// </summary>
        private void NotifyByRoute(Client player, string routeValue) {
            const string message = "Вы начали работу на ~b~{0} ~w~ветке";
            string route;
            switch (routeValue) {
                case BusDriverManager.RED_ROUTE:
                    route = "красной";
                    break;
                case BusDriverManager.BLUE_ROUTE:
                    route = "синей";
                    break;
                case BusDriverManager.YELLOW_ROUTE:
                    route = "желтой";
                    break;
                case BusDriverManager.GREEN_ROUTE:
                    route = "зеленой";
                    break;
                default:
                    throw new ArgumentException("Пришел неизвестный ключ маршрута");
            }
            API.sendNotificationToPlayer(player, string.Format(message, route));
        }
    }
}