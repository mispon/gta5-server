using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles;
using gta_mp_server.Managers.Work.Bistro.Interfaces;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Ninject;
using Ninject.Syntax;

namespace gta_mp_server.Managers.Work.Bistro {
    /// <summary>
    /// Работа в закусочной
    /// </summary>
    internal class BistroManager : Script, IBistroManager {
        internal const string POINT_NUMBER_KEY = "PointNumber";
        private const string SUCCESS_DELIVERY_KEY = "SuccessDelivery";
        private const string ON_DELIVERY_POINT = "OnDeliveryPoint";
        private const int FAGIO_SPEED = 23; // м/с
        private const int FLAG = (int) (AnimationFlags.OnlyAnimateUpperBody | AnimationFlags.AllowPlayerControl);
        
        private readonly Dictionary<int, WorkReward> _truckRewards = new Dictionary<int, WorkReward> {
            [1] = new WorkReward {Salary = 2, Exp = 30, WorkExp = 12},
            [2] = new WorkReward {Salary = 3, Exp = 34, WorkExp = 13},
            [3] = new WorkReward {Salary = 3, Exp = 38, WorkExp = 14},
            [4] = new WorkReward {Salary = 4, Exp = 42, WorkExp = 15},
            [5] = new WorkReward {Salary = 5, Exp = 46, WorkExp = 0}
        };
        private readonly Dictionary<int, WorkReward> _deliveryRewards = new Dictionary<int, WorkReward> {
            [1] = new WorkReward {Salary = 20, Exp = 17, WorkExp = 12},
            [2] = new WorkReward {Salary = 26, Exp = 20, WorkExp = 13},
            [3] = new WorkReward {Salary = 34, Exp = 23, WorkExp = 14},
            [4] = new WorkReward {Salary = 44, Exp = 26, WorkExp = 15},
            [5] = new WorkReward {Salary = 56, Exp = 30, WorkExp = 0}
        };

        private readonly int _range = BistroPositionsGetter.DeliveryPoints.Count;
        private readonly Random _random = new Random();
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IWorkInfoManager _workInfoManager;
        private readonly IPointCreator _pointCreator;

        public BistroManager() : this(ServerKernel.Kernel) {
            ClientEventHandler.Add(ClientEvent.SET_DELIVERY_FAIL, SetDeliveryFail);
            ClientEventHandler.Add(ClientEvent.COMPLETE_DELIVERY, CompleteDelivery);
        }

        public BistroManager(IResolutionRoot kernel) {
            _playerInfoManager = kernel.Get<IPlayerInfoManager>();
            _workInfoManager = kernel.Get<IWorkInfoManager>();
            _pointCreator = kernel.Get<IPointCreator>();
        }

        /// <summary>
        /// Выдать награду водителю фургона с едой
        /// </summary>
        public void SetTruckDriverRevard(string driverName, int foodPrice, string clientName) {
            var truckDriver = API.getAllPlayers().First(e => e.socialClubName == driverName);
            API.sendNotificationToPlayer(truckDriver, $"~g~{clientName} купил у вас еду");
            var workLevel = _workInfoManager.GetWorkInfo(truckDriver, WorkType.FoodTrunk).Level;
            var reward = _truckRewards[workLevel];
            _workInfoManager.SetSalary(truckDriver, WorkType.FoodTrunk, foodPrice * reward.Salary);
            _workInfoManager.SetExperience(truckDriver, WorkType.FoodTrunk, reward.WorkExp);
            _playerInfoManager.SetExperience(truckDriver, reward.Exp);
        }

        /// <summary>
        /// Создает точки доставки
        /// </summary>
        public void CreatePoints() {
            var deliveryPoints = BistroPositionsGetter.DeliveryPoints;
            for (var num = 0; num < deliveryPoints.Count; num++) {
                var point = deliveryPoints[num];
                var shape = API.createSphereColShape(point.Point, 1.5f);
                var ped = _pointCreator.CreatePed(point.Hash, point.PedPosition, point.PedRotation);
                shape.onEntityEnterColShape += (colShape, entity) => PlayerComeToPoint(colShape, entity, ped, point.IsMale);
                shape.onEntityExitColShape += (colShape, entity) => PlayerAwayFromPoint(colShape, entity, ped);
                shape.setData(POINT_NUMBER_KEY, num);
            }
        }

        /// <summary>
        /// Показать точку доставки еды
        /// </summary>
        public void ShowNextDeliveryPoint(Client player) {
            var pointPosition = GetNextPointPosition(player);
            var distance = Vector3.Distance(player.position, pointPosition);
            var deliverySeconds = TimeSpan.FromSeconds(CalculateSeconds(distance)).TotalSeconds;
            player.setData(SUCCESS_DELIVERY_KEY, true);
            API.triggerClientEvent(player, ServerEvent.SET_TIMER, (int) deliverySeconds, "SetDeliveryFail");
            API.triggerClientEvent(player, ServerEvent.SHOW_DELIVERY_POINT, pointPosition);
        }

        /// <summary>
        /// Возвращает количество секунд до следующей точки для таймера
        /// </summary>
        private static double CalculateSeconds(float distance) {
            var kilometers = distance / VehicleManager.ONE_KILOMETER;
            return kilometers * 1000 / FAGIO_SPEED;
        }

        /// <summary>
        /// Завершает доставку и выдает награду
        /// </summary>
        private void CompleteDelivery(Client player, object[] args) {
            API.playPlayerAnimation(player, FLAG, "mp_common", "givetake2_a");
            var workLevel = _workInfoManager.GetWorkInfo(player, WorkType.FoodDeliveryMan).Level;
            var reward = _deliveryRewards[workLevel];
            var success = (bool) player.getData(SUCCESS_DELIVERY_KEY);
            _workInfoManager.SetSalary(player, WorkType.FoodDeliveryMan, success ? (int) (reward.Salary * 1.2) : reward.Salary);
            _workInfoManager.SetExperience(player, WorkType.FoodDeliveryMan, reward.WorkExp);
            _playerInfoManager.SetExperience(player, reward.Exp);
            API.triggerClientEvent(player, ServerEvent.STOP_TIMER);
            player.resetSyncedData(ON_DELIVERY_POINT);
            ShowNextDeliveryPoint(player);
        }

        /// <summary>
        /// Игрок подошел к точке доставки
        /// </summary>
        private void PlayerComeToPoint(ColShape shape, NetHandle entity, Ped ped, bool pedIsMale) {
            var player = API.getPlayerFromHandle(entity);
            if (!PointCorrect(player, shape, true)) {
                return;
            }
            player.setSyncedData(ON_DELIVERY_POINT, true);
            var success = (bool) player.getData(SUCCESS_DELIVERY_KEY);
            if (success) {
                var animDict = pedIsMale ? "amb@world_human_cheering@male_d" : "amb@world_human_cheering@female_d";
                API.playPedAnimation(ped, true, animDict, "base");
            }
        }

        /// <summary>
        /// Игрок отошел от точки доставки
        /// </summary>
        private void PlayerAwayFromPoint(ColShape shape, NetHandle entity, Ped ped) {
            var player = API.getPlayerFromHandle(entity);
            if (!PointCorrect(player, shape, false)) {
                return;
            }
            player.resetSyncedData(ON_DELIVERY_POINT);
            API.stopPlayerAnimation(player);
            API.stopPedAnimation(ped);
        }

        /// <summary>
        /// Проверка корректности точки доставки
        /// </summary>
        private static bool PointCorrect(Client player, ColShape shape, bool onEnter) {
            var result = PlayerHelper.PlayerCorrect(player) && player.hasData(WorkData.IS_FOOD_DELIVERYMAN);
            if (onEnter) {
                var playerTargetPoint = (int) (player?.getSyncedData(POINT_NUMBER_KEY) ?? Validator.INVALID_ID);
                result &= playerTargetPoint == (int) shape.getData(POINT_NUMBER_KEY);
            }
            return result;
        }

        /// <summary>
        /// Возвращает следующую координату точки доставки
        /// </summary>
        private Vector3 GetNextPointPosition(Client player) {
            int nextNumber;
            if (player.hasSyncedData(POINT_NUMBER_KEY)) {
                var currentPointNumber = (int) player.getSyncedData(POINT_NUMBER_KEY);
                while ((nextNumber = _random.Next(_range)) == currentPointNumber) {}
            }
            else {
                nextNumber = _random.Next(_range);
            }
            player.setSyncedData(POINT_NUMBER_KEY, nextNumber);
            return BistroPositionsGetter.DeliveryPoints[nextNumber].Point;
        }

        /// <summary>
        /// Установка успешности доставки
        /// </summary>
        private static void SetDeliveryFail(Client player, object[] args) {
            player.setData(SUCCESS_DELIVERY_KEY, false);
        }
    }
}