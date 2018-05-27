using System.Collections.Generic;
using System.Threading.Tasks;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.Places.FarmPlace;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Farmer.Interfaces;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Work.Farmer {
    /// <summary>
    /// Логика работы фермером
    /// </summary>
    internal class FarmerManager : Script, IFarmerManager {
        internal const string FARMER_POINT_KEY = "FarmerPoint";
        private const string FARMER_POINT_VALUE = "FarmerPoint_{0}";
        private const string POINT_INDEX = "FarmPointIndex";
        private const string POINTS_PROCESSED = "FarmPointsProcessed";
        private const string ON_TARGET = "OnFarmerTarget";
        private const string HARVEST_COUNT = "HarvestCount";
        private const string HARVEST_KEY = "HarvestObject";

        private static readonly Dictionary<int, WorkPoint> _farmPoints = new Dictionary<int, WorkPoint>();
        private readonly Dictionary<int, WorkReward> _rewards = new Dictionary<int, WorkReward> {
            [1] = new WorkReward {Salary = 15, Exp = 8, WorkExp = 13},
            [2] = new WorkReward {Salary = 17, Exp = 10, WorkExp = 15},
            [3] = new WorkReward {Salary = 19, Exp = 12, WorkExp = 17},
            [4] = new WorkReward {Salary = 21, Exp = 14, WorkExp = 19},
            [5] = new WorkReward {Salary = 23, Exp = 16, WorkExp = 0}
        };

        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IWorkInfoManager _workInfoManager;

        public FarmerManager() {}
        public FarmerManager(IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager) {
            _playerInfoManager = playerInfoManager;
            _workInfoManager = workInfoManager;
        }

        /// <summary>
        /// Инициализация работы
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.PROCESS_FARMER_POINT, ProcessPoint);
            CreateFarmPoints();
            CreateFarmEndPoint();
        }

        /// <summary>
        /// Обработать точку сбора
        /// </summary>
        private async void ProcessPoint(Client player, object[] args) {
            player.resetSyncedData(ON_TARGET);
            API.triggerClientEvent(player, ServerEvent.HIDE_FARMER_POINT);
            API.playPlayerScenario(player, "WORLD_HUMAN_GARDENER_PLANT");
            var timeout = Farm.Instance.HasBuff() ? 2500 : 5000;
            await CollectHarvest(player, timeout);
            API.stopPlayerAnimation(player);
            FinishProcessing(player);
        }

        /// <summary>
        /// Собирать урожай
        /// </summary>
        private async Task CollectHarvest(Client player, int timeout, int repeats = 1) {
            var harvestCount = player.hasData(HARVEST_COUNT) ? (int) player.getData(HARVEST_COUNT) : 0;
            player.setData(HARVEST_COUNT, ++harvestCount);
            await Task.Delay(timeout);
            if (repeats < 5) {
                API.sendNotificationToPlayer(player, $"Собрано ~b~{harvestCount} ед. ~w~урожая");
                await CollectHarvest(player, timeout, repeats + 1);
            }
        }

        /// <summary>
        /// Завершает обработку точки сбора
        /// </summary>
        private void FinishProcessing(Client player) {
            var pointsProcessed = player.hasData(POINTS_PROCESSED) ? (int) player.getData(POINTS_PROCESSED) : 0;
            pointsProcessed++;
            if (pointsProcessed == 3) {
                ShowEndPoint(player);
            }
            else {
                player.setData(POINTS_PROCESSED, pointsProcessed);
                ShowNextPoint(player);
            }
        }

        /// <summary>
        /// Показать точку завершения сбора
        /// </summary>
        private void ShowEndPoint(Client player) {
            var harvest = API.createObject(1388308576, player.position, player.rotation);
            API.attachEntityToEntity(harvest, player, "SKEL_R_HAND", new Vector3(0.2, -0.04, -0.3), new Vector3(70, 150, 50));
            API.playPlayerAnimation(player, PlayerHelper.LOADER_FLAGS, "anim@heists@box_carry@", "run");
            player.setData(HARVEST_KEY, harvest);
            player.resetData(HARVEST_COUNT);
            player.resetData(POINTS_PROCESSED);
            API.triggerClientEvent(player, ServerEvent.SHOW_FARMER_END_POINT, FarmDataGetter.FarmEndPoint);
        }

        /// <summary>
        /// Показывает следующую рабочую точку
        /// </summary>
        internal static void ShowNextPoint(Client player) {
            var pointNumber = GetNextPointNumber(player);
            player.setData(FARMER_POINT_KEY, string.Format(FARMER_POINT_VALUE, pointNumber));
            API.shared.triggerClientEvent(player, ServerEvent.SHOW_FARMER_POINT, _farmPoints[pointNumber].Position);
        }

        /// <summary>
        /// Возвращает номер следующей случайной точки
        /// </summary>
        private static int GetNextPointNumber(Client player) {
            var lastPoint = player.hasData(POINT_INDEX) ? (int) player.getData(POINT_INDEX) : -1;
            int result;
            do {
                result = ActionHelper.Random.Next(_farmPoints.Count);
            } while (result == lastPoint);
            return result;
        }

        /// <summary>
        /// Создает точки сбора урожая
        /// </summary>
        private void CreateFarmPoints() {
            for (var i = 0; i < FarmDataGetter.FarmPoints.Count; i++) {
                var pointPosition = FarmDataGetter.FarmPoints[i];
                var shape = API.createCylinderColShape(pointPosition, 1.5f, 3);
                shape.setData(FARMER_POINT_KEY, string.Format(FARMER_POINT_VALUE, i));
                shape.setData(POINT_INDEX, i);
                shape.onEntityEnterColShape += OnPlayerEnterFarmPoint;
                shape.onEntityExitColShape += OnPlayerExitFarmPoint;
                _farmPoints.Add(i, new WorkPoint {Position = pointPosition});
            }
        }

        /// <summary>
        /// Обработчик входа в точку сбора
        /// </summary>
        private void OnPlayerEnterFarmPoint(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PointCorrect(shape, player)) {
                return;
            }
            player.setData(POINT_INDEX, (int) shape.getData(POINT_INDEX));
            player.setSyncedData(ON_TARGET, true);
        }

        /// <summary>
        /// Проверяет корректность точки
        /// </summary>
        private static bool PointCorrect(ColShape shape, Client player) {
            if (!(PlayerHelper.PlayerCorrect(player) && player.hasData(FARMER_POINT_KEY))) {
                return false;
            }
            return shape.getData(FARMER_POINT_KEY) == player.getData(FARMER_POINT_KEY);
        }

        /// <summary>
        /// Обработчик выхода из точки сбора
        /// </summary>
        private void OnPlayerExitFarmPoint(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            player.resetData(POINT_INDEX);
            player.resetSyncedData(ON_TARGET);
        }

        /// <summary>
        /// Создает точку сдачи груза
        /// </summary>
        private void CreateFarmEndPoint() {
            var shape = API.createCylinderColShape(FarmDataGetter.FarmEndPoint, 2f, 3);
            shape.onEntityEnterColShape += (colShape, entity) => {
                var player = API.getPlayerFromHandle(entity);
                if (!(PlayerHelper.PlayerCorrect(player) && player.hasData(WorkData.IS_FARMER) && player.hasData(HARVEST_KEY))) {
                    return;
                }
                PutHarvest(player);
                SetReward(player);
                ShowNextPoint(player);
                API.triggerClientEvent(player, ServerEvent.HIDE_FARMER_END_POINT);
            };
        }

        /// <summary>
        /// Сдает груз игрока
        /// </summary>
        internal static void PutHarvest(Client player) {
            if (!player.hasData(HARVEST_KEY)) {
                return;
            }
            var harvest = (Object) player.getData(HARVEST_KEY);
            player.resetData(HARVEST_KEY);
            API.shared.deleteEntity(harvest);
            API.shared.stopPlayerAnimation(player);
            Farm.Instance.AddHarvest();
        }

        /// <summary>
        /// Выдает награду игроку
        /// </summary>
        private void SetReward(Client player) {
            var workLevel = _workInfoManager.GetWorkInfo(player, WorkType.Farmer).Level;
            var reward = _rewards[workLevel];
            _playerInfoManager.SetExperience(player, reward.Exp);
            _workInfoManager.SetSalary(player, WorkType.Farmer, reward.Salary);
            _workInfoManager.SetExperience(player, WorkType.Farmer, reward.WorkExp);
        }
    }
}