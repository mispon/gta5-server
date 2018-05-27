using System;
using System.Collections.Generic;
using gta_mp_server.Clan.Data;
using gta_mp_server.Constant;
using gta_mp_server.Enums.Clan;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.IoC;
using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared.Math;
using Ninject;
using Marker = gta_mp_server.Enums.Marker;
using Object = GrandTheftMultiplayer.Server.Elements.Object;

namespace gta_mp_server.Clan.Mission {
    /// <summary>
    /// Миссия клана
    /// </summary>
    internal class ClanMission : Script {
        internal const string VANS_SHAPE_KEY = "VansTrunkShape";
        internal const string BOOTY_OBJECT = "BootyObject";
        internal const string PLACE_KEY = "PlaceKey";
        private const int MAX_BOOTY_COUNT = 100;
        private const int TAKE_COUNT = 5;

        private PointResult _bootyPoint;
        private Blip _blip;

        private readonly IPointCreator _pointCreator;

        public ClanMission() {
            _pointCreator = ServerKernel.Kernel.Get<IPointCreator>();
        }

        /// <summary>
        /// Количество проголосовавших за запуск
        /// </summary>
        public int Members { get; set; } = 1;

        /// <summary>
        /// Место, где проходит миссия
        /// </summary>
        public MissionPlace Place { get; set; } = MissionPlace.Unknown;

        /// <summary>
        /// Признак активности миссии
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Время запуска миссии
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Количество добычи
        /// </summary>
        private int BootyCount { get; set; } = 700;

        /// <summary>
        /// Запускает миссию
        /// </summary>
        public void Start() {
            Active = true;
            StartTime = DateTime.Now;
            var position = MissionDataGetter.GetMissionPosition(Place);
            _bootyPoint = _pointCreator.CreateMarker(Marker.VerticalCylinder, position, Colors.DarkBlue, 1.5f);
            _bootyPoint.Marker.scale = new Vector3(3, 3, 1);
            _bootyPoint.ColShape.onEntityEnterColShape += (shape, entity) => PlayerHelper.ProcessAction(entity, OnPlayerEnterBootyShape);
            _blip = _pointCreator.CreateBlip(position, 78, 19, name: "Миссия банды");
        }

        /// <summary>
        /// Завершает миссию
        /// </summary>
        public void Finish() {
            Active = false;
            API.deleteEntity(_blip);
            API.deleteColShape(_bootyPoint.ColShape);
            API.deleteEntity(_bootyPoint.Marker);
            _bootyPoint = null;
        }

        /// <summary>
        /// Возвращает обратно добычу, которая была удалена у игрока
        /// </summary>
        public void RestoreBootyCount() {
            BootyCount += TAKE_COUNT;
        }

        /// <summary>
        /// Обработчик входа в точку получения добычи
        /// </summary>
        private void OnPlayerEnterBootyShape(Client player) {
            if (player.hasData(BOOTY_OBJECT) || BootyCount == 0) return;
            var objectParams = MissionDataGetter.BootyParams[Place];
            var bootyObject = API.createObject(objectParams.Model, player.position, player.rotation);
            API.attachEntityToEntity(bootyObject, player, "SKEL_R_HAND", objectParams.PositionOffset, objectParams.RotationOffset);
            API.playPlayerAnimation(player, PlayerHelper.LOADER_FLAGS, "anim@heists@box_carry@", "run");
            bootyObject.setData(PLACE_KEY, Place);
            player.setData(BOOTY_OBJECT, bootyObject);
            BootyCount -= TAKE_COUNT;
            if (BootyCount == 0) {
                API.setEntityTransparency(_bootyPoint.Marker, 0);
                API.setEntityTransparency(_blip, 0);
            }
        }

        /// <summary>
        /// Создает обработчик фургона
        /// </summary>
        public static SphereColShape CreateVansShape() {
            var shape = API.shared.createSphereColShape(new Vector3(), 4f);
            shape.onEntityEnterColShape += (colshape, entity) => PlayerHelper.ProcessAction(entity, player => EnterVansShape(player, colshape));
            return shape;
        }

        /// <summary>
        /// Обработчик погрузки в фургон
        /// </summary>
        private static void EnterVansShape(Client player, ColShape shape) {
            if (!player.hasData(BOOTY_OBJECT)) {
                return;
            }
            var vehicle = (Vehicle) shape.getData(VANS_SHAPE_KEY);
            var bootyCount = (int) vehicle.getData(ClanCourtyard.MISSION_BOOTY);
            if (bootyCount == MAX_BOOTY_COUNT) {
                API.shared.sendNotificationToPlayer(player, "~r~Фургон полностью заполнен", true);
                return;
            }
            bootyCount += TAKE_COUNT;
            var bootyObject = (Object) player.getData(BOOTY_OBJECT);
            DetachBooty(vehicle, bootyObject, bootyCount);
            player.resetData(BOOTY_OBJECT);
            API.shared.stopPlayerAnimation(player);
            vehicle.setData(ClanCourtyard.MISSION_BOOTY, bootyCount);
            API.shared.sendColoredNotificationToPlayer(player, $"Заполнено {bootyCount} из {MAX_BOOTY_COUNT}", 0, 18);
        }

        /// <summary>
        /// Перемещает груз в фургон
        /// </summary>
        private static void DetachBooty(Vehicle vehicle, Object bootyObject, int bootyCount) {
            var place = (MissionPlace) bootyObject.getData(PLACE_KEY);
            var offsets = MissionDataGetter.VansOffsets[place];
            if (offsets.ContainsKey(bootyCount)) {
                bootyObject.detach();
                bootyObject.position = vehicle.position;
                bootyObject.attachTo(vehicle, null, offsets[bootyCount], new Vector3(0.0, 0.0, 0.15));
                ((List<Object>) vehicle.getData(ClanCourtyard.BOOTY_IN_TRUNK)).Add(bootyObject);
            }
            else {
                API.shared.deleteEntity(bootyObject);
            }
        }
    }
}