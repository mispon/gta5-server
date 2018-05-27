using gta_mp_server.Events.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Events {
    /// <summary>
    /// Обработка входа игрока на сервер
    /// </summary>
    internal class PlayerConnectManager : Script, IPlayerConnectManager {
        /// <summary>
        /// Обработать событие коннекта игрока
        /// </summary>
        public void OnPlayerConnected(Client player) {
            // todo: проверить, что игрок не в бане
            API.setPlayerSkin(player, PedHash.FreemodeMale01);
            API.setEntityPosition(player, new Vector3(-236.81, -835.26, 125.26));
            player.freeze(true);
        }
    }
}