using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models.Utils {
    /// <summary>
    /// Агрегированная информация о брошенном транспорте
    /// </summary>
    internal class AfkVehicle {
        /// <summary>
        /// Идентификатор транспорта
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Текущая позиция
        /// </summary>
        public Vector3 Position { get; set; }
    }
}