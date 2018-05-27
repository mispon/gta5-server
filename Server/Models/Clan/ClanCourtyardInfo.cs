using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models.Clan {
    /// <summary>
    /// Модель данных внутреннего дворика клана
    /// </summary>
    internal class ClanCourtyardInfo {
        /// <summary>
        /// Точка сдачи добычи миссии
        /// </summary>
        public Vector3 MissionEndPoint { get; set; }

        /// <summary>
        /// Позиция гаража с фургонами для миссии
        /// </summary>
        public Vector3 VansGarage { get; set; }

        /// <summary>
        /// Позиция спавна фургона
        /// </summary>
        public Vector3 VansSpawnPosition { get; set; }

        /// <summary>
        /// Поворот спавна фургона
        /// </summary>
        public Vector3 VansSpawnRotation { get; set; }
    }
}