using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.DrivingSchool.Interfaces;
using gta_mp_server.Managers.MenuHandlers;
using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.DrivingSchool {
    internal class DriverPracticeExamManager : Script, IDriverPracticeExamManager {
        private const string SHAPE_KEY = "ColshapeKey";
        private const string SHAPE_VALUE = "colshape_{0}";
        private const int EXAM_TIME = 150;

        private readonly HashSet<ExamPoint> _examPoints = new HashSet<ExamPoint>();
        private readonly IPlayerInfoManager _playerInfoManager;

        public DriverPracticeExamManager() {}
        public DriverPracticeExamManager(IPlayerInfoManager playerInfoManager) {
            _playerInfoManager = playerInfoManager;
            ClientEventHandler.Add(ClientEvent.TIME_OF_EXAM_WAS, TimeOfExamWas);
            ClientEventHandler.Add(ClientEvent.DAMAGE_COUNT_EXCEEDED, DamageCountExceeded);
            CreateShapes();
        }

        /// <summary>
        /// Начать экзамен
        /// </summary>
        public void Start(Client player) {
            API.sendNotificationToPlayer(player, "~p~Не покидайте машину. Иначе вы не пройдете экзамен");
            player.setData(SHAPE_KEY, string.Format(SHAPE_VALUE, 0));
            API.triggerClientEvent(player, ServerEvent.SET_TIMER, EXAM_TIME, ClientEvent.TIME_OF_EXAM_WAS);
            API.triggerClientEvent(player, ClientEvent.SHOW_NEXT_DRIVING_EXAM_POINT, GetPointPositions().First());
        }

        /// <summary>
        /// Завершить экзамен
        /// </summary>
        public void Finish(Client player, bool success) {
            API.triggerClientEvent(player, ServerEvent.STOP_TIMER);
            API.triggerClientEvent(player, ClientEvent.FINISH_PRACTICE_EXAM);
            var info = _playerInfoManager.GetInfo(player);
            if (success) {
                info.Driver.PassedPracticeB = true;
                API.sendColoredNotificationToPlayer(player, "Вы успешно сдали практический экзамен!", 0, 18);
                API.sendNotificationToPlayer(player, "Получено: ~g~водительская лицензия");
            }
            else {
                info.Driver.TimeToNextTry = DateTime.Now.AddHours(1);
                API.sendNotificationToPlayer(player, "~r~Вы не прошли экзамен, попробуйте чуть позже");
            }
            WrapPlayerOutOfVehicle(player);
            player.resetData(DrivingSchoolMenuHandler.PRACTICE_EXAM_KEY);
            _playerInfoManager.RefreshUI(player, info);
        }

        /// <summary>
        /// Вытащить игрока из машины
        /// </summary>
        private void WrapPlayerOutOfVehicle(Client player) {
            ActionHelper.SetAction(player, 5000, () => API.warpPlayerOutOfVehicle(player));
        }

        /// <summary>
        /// Отобразить следующий маркер
        /// </summary>
        private void ShowNextMarker(ColShape shape, NetHandle handle) {
            var player = API.getPlayerFromHandle(handle);
            if (!ConditionsCorrect(player, shape)) {
                return;
            }
            var currentPoint = _examPoints.First(e => e.ColShape == shape);
            player.setData(SHAPE_KEY, string.Format(SHAPE_VALUE, currentPoint.NextPointNumber));
            if (currentPoint.NextPoint == null) {
                Finish(player, true);
                return;
            }
            API.triggerClientEvent(player, ClientEvent.SHOW_NEXT_DRIVING_EXAM_POINT, currentPoint.NextPoint);
        }

        /// <summary>
        /// Проверяет корректность условий прохождения экзамена
        /// </summary>
        private static bool ConditionsCorrect(Client player, ColShape shape) {
            if (!PlayerHelper.PlayerCorrect(player, true)) {
                return false;
            }
            return player.hasData("OnPracticeExam") && player.getData(SHAPE_KEY) == shape.getData(SHAPE_KEY);
        }

        /// <summary>
        /// Время экзамена вышло
        /// </summary>
        private void TimeOfExamWas(Client player, object[] objects) {
            API.sendNotificationToPlayer(player, "~r~Время вышло", true);
            Finish(player, false);
        }

        /// <summary>
        /// Превышено количество повреждений
        /// </summary>
        private void DamageCountExceeded(Client player, object[] objects) {
            API.sendNotificationToPlayer(player, "~r~Превышено количество повреждений", true);
            Finish(player, false);
        }

        /// <summary>
        /// Создает список контрольных точек, по которым нужно проехать
        /// </summary>
        private void CreateShapes() {
            var pointPositions = GetPointPositions();
            for (var i = 0; i < pointPositions.Count; i++) {
                var position = pointPositions[i];
                var nextPosition = i + 1 < pointPositions.Count
                    ? pointPositions[i + 1]
                    : null;
                var shape = API.createSphereColShape(position, 3F);
                shape.setData(SHAPE_KEY, string.Format(SHAPE_VALUE, i));
                shape.onEntityEnterColShape += ShowNextMarker;
                var examPoint = new ExamPoint {
                    ColShape = shape,
                    NextPoint = nextPosition,
                    NextPointNumber = i + 1,
                };
                _examPoints.Add(examPoint);
            }
        }

        /// <summary>
        /// Возвращает позиции контрольных точек
        /// </summary>
        private static List<Vector3> GetPointPositions() {
            return new List<Vector3> {
                new Vector3(-6.56, -1568.73, 27.74),
                new Vector3(-86.23, -1540.59, 31.65),
                new Vector3(-180.21, -1467.11, 30.25),
                new Vector3(-269.99, -1393.67, 29.80),
                new Vector3(-270.49, -1173.12, 21.58),
                new Vector3(-195.27, -945.75, 27.74),
                new Vector3(-86.70, -935.66, 27.78),
                new Vector3(59.17, -994.16, 27.80),
                new Vector3(66.97, -1080.22, 27.86),
                new Vector3(57.16, -1293.46, 27.81),
                new Vector3(187.04, -1419.87, 27.81),
                new Vector3(282.02, -1495.99, 27.74),
                new Vector3(189.52, -1580.65, 27.77),
                new Vector3(88.14, -1523.39, 27.80),
                new Vector3(45.57, -1559.98, 27.79)
            };
        }

        /// <summary>
        /// Точки движения на экзамене
        /// </summary>
        private class ExamPoint {
            public ColShape ColShape { get; set; }
            public Vector3 NextPoint { get; set; }
            public int NextPointNumber { get; set; }
        }
    }
}