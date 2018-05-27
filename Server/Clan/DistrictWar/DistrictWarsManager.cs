using gta_mp_database.Entity;
using gta_mp_database.Providers.Interfaces;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Marker = gta_mp_server.Enums.Marker;

namespace gta_mp_server.Clan.DistrictWar {
    /// <summary>
    /// Логика сражения за территорию
    /// </summary>
    internal class DistrictWarsManager: Script, IDistrictWarsManager {
        private const int REP_FOR_CAPTURE = 50;

        private static long _ownerId = Validator.INVALID_ID;
        private static District _district;
        private static Blip _blip;
        private static PointResult _capturePoint;

        private readonly IDistrictsProvider _districtsProvider;
        private readonly IPointCreator _pointCreator;
        private readonly IClanManager _clanManager;
        private readonly IPlayerInfoManager _playerInfoManager;

        public DistrictWarsManager() {}
        public DistrictWarsManager(IDistrictsProvider districtsProvider, IPointCreator pointCreator,
            IClanManager clanManager, IPlayerInfoManager playerInfoManager) {
            _districtsProvider = districtsProvider;
            _pointCreator = pointCreator;
            _clanManager = clanManager;
            _playerInfoManager = playerInfoManager;
        }

        /// <summary>
        /// Запускает войну за район
        /// </summary>
        public void StartWar() {
            ClientEventHandler.Add(ClientEvent.CAPTURE_DISTRICT, CaptureStreet);
            _district = _districtsProvider.GetNext();
            _clanManager.RemoveDistrict(_district.Id);
            var position = PositionConverter.ToVector3(_district.Position);
            _blip = _pointCreator.CreateBlip(position, 439, 4, scale: 2f, name: "Война за район");
            _capturePoint = _pointCreator.CreateMarker(Marker.VerticalCylinder, position, Colors.White, 3.7f, "Точка захвата района");
            _capturePoint.ColShape.onEntityEnterColShape += PlayerComeToCapturePoint;
            _capturePoint.ColShape.onEntityExitColShape += PlayerAwayFromCapturePoint;
            API.setMarkerScale(_capturePoint.Marker, new Vector3(4, 4, 4));
            API.setEntityPosition(_capturePoint.Label, _capturePoint.Label.position.Add(new Vector3(0, 0, 2)));
            API.sendChatMessageToAll($"~b~[Банды] Началось сражение за район \"{_district.Name}\"!");
        }

        /// <summary>
        /// Игрок зашел на точку захвата
        /// </summary>
        private void PlayerComeToCapturePoint(ColShape shape, NetHandle entity) {
            PlayerHelper.ProcessAction(entity, player => {
                var playerInfo = _playerInfoManager.GetInfo(player);
                if (playerInfo.Clan == null || playerInfo.Clan.ClanId == _ownerId) {
                    return;
                }
                API.triggerClientEvent(player, ServerEvent.SET_PROGRESS_ACTION, 60, ClientEvent.CAPTURE_DISTRICT);
            });
        }

        /// <summary>
        /// Игрок покинул точку захвата
        /// </summary>
        private void PlayerAwayFromCapturePoint(ColShape shape, NetHandle entity) {
            PlayerHelper.ProcessAction(entity, player => API.triggerClientEvent(player, ServerEvent.STOP_PROGRESS_ACTION));
        }

        /// <summary>
        /// Захват района
        /// </summary>
        private void CaptureStreet(Client player, object[] args) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (playerInfo.Clan.ClanId == _ownerId) {
                return;
            }
            _ownerId = playerInfo.Clan.ClanId;
            var color = ClanManager.GetClanColor(_ownerId);
            API.setMarkerColor(_capturePoint.Marker, color.Bright, color.Red, color.Green, color.Blue);
            API.setBlipColor(_blip, GetBlipColor(playerInfo.Clan.ClanId));
            _clanManager.SetReputation(player, REP_FOR_CAPTURE, playerInfo);
            API.sendChatMessageToAll($"~b~[Банды] Район \"{_district.Name}\" захвачен бандой \"{ClanManager.GetClanName(playerInfo.Clan.ClanId)}\"!");
        }

        /// <summary>
        /// Возвращает цвет игонки в зависимости от клана
        /// </summary>
        private static int GetBlipColor(long clanId) {
            switch (clanId) {
                case 1:
                    return 3;
                case 2:
                    return 1;
                default:
                    return 2;
            }
        }

        /// <summary>
        /// Завершает войну
        /// </summary>
        public void FinishWar() {
            ClientEventHandler.Remove(ClientEvent.CAPTURE_DISTRICT);
            _blip.delete();
            _capturePoint.Marker.delete();
            _capturePoint.Label.delete();
            API.deleteColShape(_capturePoint.ColShape);
            if (_ownerId == Validator.INVALID_ID) {
                API.sendChatMessageToAll($"~b~[Банды] Район \"{_district.Name}\" остался не захвачен!");
            }
            else {
                _clanManager.AddDistrict(_ownerId, _district.Id);
                API.sendChatMessageToAll($"~b~[Банды] Банда \"{ClanManager.GetClanName(_ownerId)}\" становятся владельцами района \"{_district.Name}\"!");
            }
            _ownerId = Validator.INVALID_ID;
        }
    }
}