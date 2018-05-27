using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models.Places {
    /// <summary>
    /// Модель магазина
    /// </summary>
    internal class ShopModel {
        /// <summary>
        /// Позиция входа в магазин
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Позиции нпс
        /// </summary>
        public string NpcPositions { get; set; }

        /// <summary>
        /// Улица, к которой относится магазин
        /// </summary>
        public int District { get; set; }
    }
}