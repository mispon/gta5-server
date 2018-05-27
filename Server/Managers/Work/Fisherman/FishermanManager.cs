using System.Collections.Generic;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Fisherman.Interfaces;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;

namespace gta_mp_server.Managers.Work.Fisherman {
    /// <summary>
    /// Логика работы рыбаком
    /// </summary>
    internal class FishermanManager : Script, IFishermanManager {
        internal const string FISH_BAITS_COUNT = "FishBaitsCount";
        internal const string POINT_BAITS_COUNT = "PointBaitsCount";
        internal const string FISHERMAN_POINT_KEY = "FishermanPoint";
        private const string FISHERMAN_POINT_VALUE = "FishermanPoint_{0}";
        private const string POINT_INDEX = "FishermanPointIndex";
        private const string ON_TARGET = "OnFishermanTarget";
        private const string FISH_BITES_EVENT = "FishBitesEvent";

        private static readonly object[] _badResult = { false };
        private static readonly Dictionary<int, WorkPoint> _pierPoints = new Dictionary<int, WorkPoint>();
        private static readonly Dictionary<int, WorkPoint> _boatPoints = new Dictionary<int, WorkPoint>();
        private readonly Dictionary<int, WorkReward> _rewards = new Dictionary<int, WorkReward> {
            [1] = new WorkReward {Salary = 10, Exp = 7, WorkExp = 12},
            [2] = new WorkReward {Salary = 11, Exp = 8, WorkExp = 13},
            [3] = new WorkReward {Salary = 13, Exp = 9, WorkExp = 14},
            [4] = new WorkReward {Salary = 14, Exp = 10, WorkExp = 15},
            [5] = new WorkReward {Salary = 16, Exp = 12, WorkExp = 0}
        };

        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IWorkInfoManager _workInfoManager;

        public FishermanManager() {}
        public FishermanManager(IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager) {
            _playerInfoManager = playerInfoManager;
            _workInfoManager = workInfoManager;
        }

        /// <summary>
        /// Проинициализировать работу
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.PROCESS_FISHERMAN_POINT, ProcessPoint);
            ClientEventHandler.Add(ClientEvent.FINISH_FISHERMAN_POINT, FinishPoint);
            CreatePoints(true);
            CreatePoints(false);
        }

        /// <summary>
        /// Обработчик рабочей точки
        /// </summary>
        private void ProcessPoint(Client player, object[] args) {
            var index = (int) player.getData(POINT_INDEX);
            var point = player.hasSyncedData(WorkData.IS_FISHERMAN_ON_BOAT) ? _boatPoints[index] : _pierPoints[index];
            ResetPointData(player);
            player.setData(FISH_BAITS_COUNT, (int) player.getData(FISH_BAITS_COUNT) - 1);
            API.triggerClientEvent(player, ServerEvent.HIDE_FISHERMAN_POINT);
            if (player.hasSyncedData(WorkData.IS_FISHERMAN)) {
                API.setEntityPosition(player, point.Position);
                API.moveEntityRotation(player, point.WorkerRotation, 500);
            }
            ActionHelper.SetAction(player, 600, () => {
                API.playPlayerScenario(player, "WORLD_HUMAN_STAND_FISHING");
                SetFishBites(player);
            });
        }

        /// <summary>
        /// Запускает событие клюющей рыбы
        /// </summary>
        private void SetFishBites(Client player) {
            var bitesTimeout = ActionHelper.Random.Next(10, 25) * 1000;
            ActionHelper.SetAction(player, bitesTimeout, () => {
                player.setSyncedData(FISH_BITES_EVENT, true);
                API.triggerClientEvent(player, ServerEvent.SHOW_SUBTITLE, "Клюет! Нажимите ~y~E~w~, чтобы поймать", 1500);
                ActionHelper.SetAction(player, 3000, () => {
                    if (player.hasSyncedData(FISH_BITES_EVENT)) {
                        FinishPoint(player, _badResult);
                    }
                });
            });
        }

        /// <summary>
        /// Завершение рабочей точки
        /// </summary>
        private void FinishPoint(Client player, object[] args) {
            player.resetSyncedData(FISH_BITES_EVENT);
            if ((bool) args[0]) {
                var workLevel = _workInfoManager.GetActiveWork(player).Level;
                var reward = _rewards[workLevel];
                var isBoat = player.hasSyncedData(WorkData.IS_FISHERMAN_ON_BOAT);
                var salary = isBoat ? reward.Salary * 2 : reward.Salary;
                var exp = isBoat ? (int) (reward.Exp * 1.5) : reward.Exp;
                _workInfoManager.SetSalary(player, WorkType.Fisherman, salary);
                _workInfoManager.SetExperience(player, WorkType.Fisherman, reward.WorkExp);
                _playerInfoManager.SetExperience(player, exp);
            }
            else {
                API.sendNotificationToPlayer(player, "~m~Вы упустили рыбу");
            }
            var pointBaits = (int) player.getData(POINT_BAITS_COUNT);
            if (pointBaits < 5) {
                SetFishBites(player);
                player.setData(POINT_BAITS_COUNT, pointBaits + 1);
            }
            else {
                API.stopPlayerAnimation(player);
                ShowNextPoint(player);
                player.setData(POINT_BAITS_COUNT, 0);
            }
        }

        /// <summary>
        /// Показывает следующую рабочую точку
        /// </summary>
        internal static void ShowNextPoint(Client player) {
            var isBoat = player.hasSyncedData(WorkData.IS_FISHERMAN_ON_BOAT);
            var points = isBoat ? _boatPoints : _pierPoints;
            var pointNumber = GetNextPointNumber(player, points.Count);
            player.setData(FISHERMAN_POINT_KEY, string.Format(FISHERMAN_POINT_VALUE, pointNumber));
            API.shared.triggerClientEvent(player, ServerEvent.SHOW_FISHERMAN_POINT, points[pointNumber].Position, isBoat);
        }

        /// <summary>
        /// Возвращает номер следующей случайной точки
        /// </summary>
        private static int GetNextPointNumber(Client player, int pointsCount) {
            var lastPoint = player.hasData(POINT_INDEX) ? (int)player.getData(POINT_INDEX) : -1;
            int result;
            do {
                result = ActionHelper.Random.Next(pointsCount);
            } while (result == lastPoint);
            return result;
        }

        /// <summary>
        /// Создает рабочие точки
        /// </summary>
        private void CreatePoints(bool forBoat) {
            var points = forBoat ? FishermanDataGetter.BoatPoints : FishermanDataGetter.PierPoints;
            for (var i = 0; i < points.Count; i++) {
                var colShape = API.createSphereColShape(points[i].Position, forBoat ? 5f : 2f);
                colShape.setData(FISHERMAN_POINT_KEY, string.Format(FISHERMAN_POINT_VALUE, i));
                colShape.setData(POINT_INDEX, i);
                colShape.onEntityEnterColShape += (shape, entity) => EnterToFishPoint(colShape, entity, forBoat);
                colShape.onEntityExitColShape += (shape, entity) => ExitFromFishPoint(entity);
                if (forBoat) {
                    _boatPoints.Add(i, points[i]);
                }
                else {
                    _pierPoints.Add(i, points[i]);
                }
            }
        }

        /// <summary>
        /// Обработчик входа в точку
        /// </summary>
        private void EnterToFishPoint(ColShape shape, NetHandle entity, bool boatPoint) {
            var player = API.getPlayerFromHandle(entity);
            if (!PointCorrect(shape, player, boatPoint)) {
                return;
            }
            player.setData(POINT_INDEX, (int) shape.getData(POINT_INDEX));
            player.setSyncedData(ON_TARGET, true);
        }

        /// <summary>
        /// Проверяет корректность точки
        /// </summary>
        private static bool PointCorrect(ColShape shape, Client player, bool boatPoint) {
            if (!(PlayerHelper.PlayerCorrect(player, boatPoint) && player.hasData(FISHERMAN_POINT_KEY))) {
                return false;
            }
            if (!player.hasData(FISH_BAITS_COUNT) || (int) player.getData(FISH_BAITS_COUNT) == 0) {
                API.shared.sendNotificationToPlayer(player, "~r~У вас нет наживки", true);
                return false;
            }
            return shape.getData(FISHERMAN_POINT_KEY) == player.getData(FISHERMAN_POINT_KEY);
        }

        /// <summary>
        /// Обработчик выхода из точки
        /// </summary>
        private void ExitFromFishPoint(NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player, true)) {
                return;
            }
            ResetPointData(player);
        }

        /// <summary>
        /// Удаляет данные о точке
        /// </summary>
        private static void ResetPointData(Client player) {
            player.resetData(POINT_INDEX);
            player.resetSyncedData(ON_TARGET);
        }
    }
}