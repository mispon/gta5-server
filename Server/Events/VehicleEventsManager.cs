using System;
using gta_mp_server.Constant;
using gta_mp_server.Events.Interfaces;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;

namespace gta_mp_server.Events {
    /// <summary>
    /// Обработчик событий входа / выхода из транспорта
    /// </summary>
    internal class VehicleEventsManager : Script, IVehicleEnterManager {
        internal const string AFK_KEY = "VehicleAfk";
        private const int DRIVER_SEAT = -1;

        private readonly IPlayerInfoManager _playerInfoManager;

        public VehicleEventsManager() {}
        public VehicleEventsManager(IPlayerInfoManager playerInfoManager) {
            _playerInfoManager = playerInfoManager;
        }

        /// <summary>
        /// Вход игрока в тс
        /// </summary>
        public void OnPlayerEnterVehicle(Client player, NetHandle vehicle, int seat) {
            API.triggerClientEvent(player, ServerEvent.HIDE_SPEEDOMETER);

            var hash = (VehicleHash) API.getEntityModel(vehicle);
            if (seat == DRIVER_SEAT && CanShowSpeedometer(hash)) {
                var settings = _playerInfoManager.GetInfo(player).Settings;
                API.triggerClientEvent(player, ServerEvent.SHOW_SPEEDOMETER, settings.SvgSpeedometer);
                ActionHelper.SetAction(player, 5000, () => API.triggerClientEvent(player, ServerEvent.HIDE_HINT));
            }
            API.resetEntityData(vehicle, AFK_KEY);
            if (!API.isVehicleABicycle(hash)) {
                API.triggerClientEvent(player, ServerEvent.SHOW_SUBTITLE, "~y~F2~w~ - меню управления транспортом");
            }
        }

        /// <summary>
        /// Выход игрока из тс
        /// </summary>
        public void OnPlayerExitVehicle(Client player, NetHandle vehicle, int seat) {
            if (seat == DRIVER_SEAT) {
                API.triggerClientEvent(player, ServerEvent.HIDE_SPEEDOMETER);
            }
            API.setEntityData(vehicle, AFK_KEY, DateTime.Now);
        }

        /// <summary>
        /// Проверяет, что у тс есть спидометр
        /// </summary>
        private bool CanShowSpeedometer(VehicleHash hash) {
            return !VehicleManager.NotUpdates.Contains(hash) && !API.isVehicleABicycle(hash) && !API.isVehicleATrain(hash);
        }
    }
}