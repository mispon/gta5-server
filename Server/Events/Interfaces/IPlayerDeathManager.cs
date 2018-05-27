using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;

namespace gta_mp_server.Events.Interfaces {
    internal interface IPlayerDeathManager {
        /// <summary>
        /// Обработчик смерти игрока
        /// </summary>
        void OnPlayerDeath(Client player, NetHandle handle, int weapon);
    }
}