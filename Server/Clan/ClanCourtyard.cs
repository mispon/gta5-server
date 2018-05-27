using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Clan.Mission;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles;
using gta_mp_server.Models.Clan;
using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Object = GrandTheftMultiplayer.Server.Elements.Object;

namespace gta_mp_server.Clan {
    /// <summary>
    /// Внутренний дворик клана
    /// </summary>
    internal class ClanCourtyard : Script, IClanCourtyard {
        internal const string MISSION_BOOTY = "MissionBooty";
        internal const string BOOTY_IN_TRUNK = "BootyInTrunk";
        internal const string MISSION_VANS_KEY = "ClanMissionVans";
        private const string CLAN_ID_KEY = "ClanId";
        private const int MONEY_COEF = 8;

        private static readonly List<Marker> _markers = new List<Marker>();
        private static readonly List<TextLabel> _labels = new List<TextLabel>();
        private static readonly List<Blip> _vansBlips = new List<Blip>();

        private readonly IPointCreator _pointCreator;
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IClanManager _clanManager;

        public ClanCourtyard() {}
        public ClanCourtyard(IPointCreator pointCreator, IPlayerInfoManager playerInfoManager, IClanManager clanManager) {
            _pointCreator = pointCreator;
            _playerInfoManager = playerInfoManager;
            _clanManager = clanManager;
        }

        /// <summary>
        /// Проинициализировать дворик
        /// </summary>
        public void Initialize(long clanId, ClanCourtyardInfo courtyard) {
            CreateMissionEndPoint(clanId, courtyard.MissionEndPoint);
            CreateVansGarage(clanId, courtyard);
            var blip = _pointCreator.CreateBlip(courtyard.VansGarage, 357, 19, name: "Фургоны для миссии");
            API.setBlipTransparency(blip, 0);
            _vansBlips.Add(blip);
            ClientEventHandler.Add(ClientEvent.SPAWN_MISSION_VANS, OnSpawnMissionVans);
        }

        /// <summary>
        /// Отображать маркеры двориков
        /// </summary>
        public void ShowMarkers() {
            _markers.ForEach(e => API.setEntityTransparency(e, 150));
            _labels.ForEach(e => API.setEntityTransparency(e, 255));
            _vansBlips.ForEach(e => API.setBlipTransparency(e, 255));
        }

        /// <summary>
        /// Скрыть маркеры двориков
        /// </summary>
        public void HideMarkers() {
            _markers.ForEach(e => API.setEntityTransparency(e, 0));
            _labels.ForEach(e => API.setEntityTransparency(e, 0));
            _vansBlips.ForEach(e => API.setBlipTransparency(e, 0));
        }

        /// <summary>
        /// Создать точку сдачи добычи миссии
        /// </summary>
        private void CreateMissionEndPoint(long clanId, Vector3 position) {
            var endPoint = _pointCreator.CreateMarker(Enums.Marker.VerticalCylinder, position, GetClanColor(clanId), 3f);
            endPoint.ColShape.setData(CLAN_ID_KEY, clanId);
            endPoint.ColShape.onEntityEnterColShape += OnEnterEndPoint;
            endPoint.Marker.scale = new Vector3(3, 3, 1.2);
            API.setEntityTransparency(endPoint.Marker, 0);
            _markers.Add(endPoint.Marker);
            var label = API.createTextLabel("Сдача добычи", position.Add(new Vector3(0, 0, 2.5)), 10, 0.6F);
            API.setEntityTransparency(label, 0);
            _labels.Add(label);
        }

        /// <summary>
        /// Обработчик входа в точку сдачи добычи
        /// </summary>
        private void OnEnterEndPoint(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            var clanId = (long) shape.getData(CLAN_ID_KEY);
            if (!(PlayerHelper.PlayerCorrect(player, true) && ClanMissionManager.HasActiveMission() && PlayersClanCorrect(player, clanId))) {
                return;
            }
            var vehicle = API.getEntityFromHandle<Vehicle>(API.getPlayerVehicle(player));
            var missionBootyCount = GetMissionBooty(vehicle);
            if (missionBootyCount == 0) {
                API.sendNotificationToPlayer(player, "~r~У вас отсутствует добыча", true);
                return;
            }
            SetReward(player, clanId, missionBootyCount);
            vehicle.setData(MISSION_BOOTY, 0);
            ((List<Object>) vehicle.getData(BOOTY_IN_TRUNK)).ForEach(e => API.deleteEntity(e));
            vehicle.setData(BOOTY_IN_TRUNK, new List<Object>());
        }

        /// <summary>
        /// Выдать награду за добычу
        /// </summary>
        private void SetReward(Client player, long clanId, int bootyCount) {
            foreach (var member in GetMembers(player, clanId)) {
                var rank = (int) _playerInfoManager.GetInfo(player).Clan.Rank;
                _playerInfoManager.SetBalance(member, bootyCount * MONEY_COEF * rank);
                _clanManager.SetReputation(member, bootyCount / 2);
            }
        }

        /// <summary>
        /// Возвращает участников банды поблизости
        /// </summary>
        private IEnumerable<Client> GetMembers(Client player, long clanId) {
            return PlayerHelper.PlayerCorrect(player, true)
                ? API.getPlayersInRadiusOfPlayer(10f, player).Where(e => e != null && PlayersClanCorrect(e, clanId))
                : new List<Client>(0);
        }

        /// <summary>
        /// Возвращает количество добычи в фургоне
        /// </summary>
        private static int GetMissionBooty(Vehicle vehicle) {
            return vehicle.hasData(MISSION_BOOTY) ? (int) vehicle.getData(MISSION_BOOTY) : 0;
        }

        /// <summary>
        /// Создать точку спавна фургонов для миссий
        /// </summary>
        private void CreateVansGarage(long clanId, ClanCourtyardInfo courtyard) {
            var garage = _pointCreator.CreateMarker(Enums.Marker.VerticalCylinder, courtyard.VansGarage, GetClanColor(clanId), 1.5f);
            garage.ColShape.setData(CLAN_ID_KEY, clanId);
            garage.ColShape.onEntityEnterColShape += (shape, entity) => 
                OnEnterVansGarage(shape, entity, courtyard.VansSpawnPosition, courtyard.VansSpawnRotation);
            garage.ColShape.onEntityExitColShape += (shape, entity) =>
                PlayerHelper.ProcessAction(entity, player => API.triggerClientEvent(player, ServerEvent.HIDE_CLAN_VANS_MENU));
            API.setEntityTransparency(garage.Marker, 0);
            _markers.Add(garage.Marker);
        }

        /// <summary>
        /// Обработчик маркера гаража фургонов
        /// </summary>
        private void OnEnterVansGarage(ColShape shape, NetHandle entity, Vector3 spawnPosition, Vector3 spawnRotation) {
            var player = API.getPlayerFromHandle(entity);
            var clanId = (long) shape.getData(CLAN_ID_KEY);
            if (!(PlayerHelper.PlayerCorrect(player) && ClanMissionManager.HasActiveMission() && PlayersClanCorrect(player, clanId))) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.SHOW_CLAN_VANS_MENU, spawnPosition, spawnRotation);
        }

        /// <summary>
        /// Проверяет, что игрок состоит в нужном клане, чтоб пользоваться двориком
        /// </summary>
        private bool PlayersClanCorrect(Client player, long clanId) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (playerInfo.Clan == null || playerInfo.Clan.ClanId != clanId) {
                API.sendNotificationToPlayer(player, "~r~Вы не состоите в этом клане", true);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Обработчик спавна фургона банды
        /// </summary>
        private void OnSpawnMissionVans(Client player, object[] args) {
            var position = (Vector3) args[0];
            if (GarageBlocked(position)) {
                API.sendNotificationToPlayer(player, "~r~Выезд из гаража заблокирован", true);
                return;
            }
            var rotation = (Vector3) args[1];
            var vehicle = API.createVehicle(VehicleHash.Burrito3, position, rotation, 0, 0);
            API.setEntityPosition(vehicle, position);
            API.setEntityRotation(vehicle, rotation);
            API.setVehicleFuelLevel(vehicle, 150);
            vehicle.setData(VehicleManager.MAX_FUEL, 100);
            vehicle.setData(MISSION_BOOTY, 0);
            vehicle.setData(BOOTY_IN_TRUNK, new List<Object>());
            vehicle.setData(MISSION_VANS_KEY, true);
        }

        /// <summary>
        /// Проверяет, заблокирован ли выезд из гаража
        /// </summary>
        private bool GarageBlocked(Vector3 position) {
           return API.getAllVehicles().Any(e => {
                    var vehiclePosition = API.getEntityPosition(e);
                    return Vector3.Distance(vehiclePosition, position) <= 3;
                }
            );
        }

        /// <summary>
        /// Возвращает цвет для маркеров
        /// </summary>
        private static Color GetClanColor(long clanId) {
            switch (clanId) {
                case 1:
                    return Colors.Blue;
                case 2:
                    return Colors.Red;
                case 3:
                    return Colors.Green;
                default:
                    throw new ArgumentOutOfRangeException(nameof(clanId));
            }
        }
    }
}