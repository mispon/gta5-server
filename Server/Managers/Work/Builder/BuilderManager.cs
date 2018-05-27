using System.Collections.Generic;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Builder.Interfaces;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;

namespace gta_mp_server.Managers.Work.Builder {
    /// <summary>
    /// Логика работы строителем
    /// </summary>
    internal class BuilderManager : Script, IBuilderManager {
        internal const string BUILDER_POINT_KEY = "BuilderPoint";
        private const string BUILDER_POINT_VALUE = "BuilderPoint_{0}";
        private const string POINT_INDEX = "BuilderPointIndex";
        private const string ON_TARGET = "OnBuildingTarget";
        private const float COLSHAPE_RANGE = 1.2F;

        private static Dictionary<int, WorkPoint> _points;
        private readonly Dictionary<int, WorkReward> _rewards = new Dictionary<int, WorkReward> {
            [1] = new WorkReward { Salary = 10, Exp = 10, WorkExp = 12 },
            [2] = new WorkReward { Salary = 14, Exp = 12, WorkExp = 13 },
            [3] = new WorkReward { Salary = 20, Exp = 14, WorkExp = 14 },
            [4] = new WorkReward { Salary = 28, Exp = 16, WorkExp = 15 },
            [5] = new WorkReward { Salary = 38, Exp = 18, WorkExp = 0 }
        };

        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IWorkInfoManager _workInfoManager;

        public BuilderManager() {}
        public BuilderManager(IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager) {
            _playerInfoManager = playerInfoManager;
            _workInfoManager = workInfoManager;
        }

        /// <summary>
        /// Проинициализировать работу строителе
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.PROCESS_BUILDER_POINT, ProcessPoint);
            ClientEventHandler.Add(ClientEvent.FINISH_BUILDER_POINT, FinishPoint);
            CreatePoints();
        }

        /// <summary>
        /// Обработчик рабочей точки
        /// </summary>
        private void ProcessPoint(Client player, object[] args) {
            if (!player.hasData(POINT_INDEX)) return;
            var point = _points[(int) player.getData(POINT_INDEX)];
            ResetPointData(player);
            API.triggerClientEvent(player, ServerEvent.HIDE_BUILDER_POINT);
            API.setEntityPosition(player, point.Position);
            API.moveEntityRotation(player, point.WorkerRotation, 500);
            ActionHelper.SetAction(player, 600, () => API.playPlayerScenario(player, BuilderDataGetter.GetScenario()));
            API.triggerClientEvent(player, ServerEvent.SET_PROGRESS_ACTION, 6, ClientEvent.FINISH_BUILDER_POINT);
        }

        /// <summary>
        /// Завершение рабочей точки
        /// </summary>
        private void FinishPoint(Client player, object[] args) {
            API.stopPlayerAnimation(player);
            var workInfo = _workInfoManager.GetActiveWork(player);
            if (workInfo == null) return;
            var reward = _rewards[workInfo.Level];
            _workInfoManager.SetSalary(player, WorkType.Builder, reward.Salary);
            _workInfoManager.SetExperience(player, WorkType.Builder, reward.WorkExp);
            _playerInfoManager.SetExperience(player, reward.Exp);
            ShowNextPoint(player);
        }

        /// <summary>
        /// Показывает следующую рабочую точку
        /// </summary>
        internal static void ShowNextPoint(Client player) {
            var pointNumber = GetNextPointNumber(player);
            player.setData(BUILDER_POINT_KEY, string.Format(BUILDER_POINT_VALUE, pointNumber));
            API.shared.triggerClientEvent(player, ServerEvent.SHOW_BUILDER_POINT, _points[pointNumber].Position);
        }

        /// <summary>
        /// Возвращает номер следующей случайной точки
        /// </summary>
        private static int GetNextPointNumber(Client player) {
            var lastPoint = player.hasData(POINT_INDEX) ? (int) player.getData(POINT_INDEX) : -1;
            int result;
            do {
                result = ActionHelper.Random.Next(_points.Count);
            } while (result == lastPoint);
            return result;
        }

        /// <summary>
        /// Создает рабочую точку
        /// </summary>
        private void CreatePoints() {
            _points = new Dictionary<int, WorkPoint>();
            for (var i = 0; i < BuilderDataGetter.Points.Count; i++) {
                var point = BuilderDataGetter.Points[i];
                var colShape = API.createSphereColShape(point.Position, COLSHAPE_RANGE);
                colShape.setData(BUILDER_POINT_KEY, string.Format(BUILDER_POINT_VALUE, i));
                colShape.setData(POINT_INDEX, i);
                colShape.onEntityEnterColShape += EnterToWorkPoint;
                colShape.onEntityExitColShape += (shape, entity) => ExitFromWorkPoint(entity);
                _points.Add(i, point);
            }
        }

        /// <summary>
        /// Обработчик входа в точку
        /// </summary>
        private void EnterToWorkPoint(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PointCorrect(shape, player)) {
                return;
            }
            player.setData(POINT_INDEX, (int) shape.getData(POINT_INDEX));
            player.setSyncedData(ON_TARGET, true);
        }

        /// <summary>
        /// Обработчик выхода из точки
        /// </summary>
        private void ExitFromWorkPoint(NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
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

        /// <summary>
        /// Проверяет корректность точки
        /// </summary>
        private static bool PointCorrect(ColShape shape, Client player) {
            if (!(PlayerHelper.PlayerCorrect(player) && player.hasData(BUILDER_POINT_KEY))) {
                return false;
            }
            return shape.getData(BUILDER_POINT_KEY) == player.getData(BUILDER_POINT_KEY);
        }
    }
}