using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Events.Interfaces {
    internal interface IPlayerDamagedManager {
        /// <summary>
        /// Обработчик получения повреждений
        /// </summary>
        void OnPlayerDamaged(Client player, Client enemy, int cause, int id);
    }
}