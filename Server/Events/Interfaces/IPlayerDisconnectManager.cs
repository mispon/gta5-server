using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Events.Interfaces {
    internal interface IPlayerDisconnectManager {
        /// <summary>
        /// Обработчик выхода игрока
        /// </summary>
        void OnPlayerDisconnect(Client player, string reason);
    }
}