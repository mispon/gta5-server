using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;

namespace gta_mp_server.Events.Interfaces {
    internal interface IVehicleEnterManager {
        /// <summary>
        /// Вход игрока в тс
        /// </summary>
        void OnPlayerEnterVehicle(Client player, NetHandle handle, int seat);

        /// <summary>
        /// Выход игрока из тс
        /// </summary>
        void OnPlayerExitVehicle(Client player, NetHandle vehicle, int seat);
    }
}