using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models {
    /// <summary>
    /// Модель инкапсулирует позиции внутри дома
    /// </summary>
    public class HousesInnerPositions {
        /// <summary>
        /// Позиция после входа, прихожая
        /// </summary>
        public Vector3 Hallway { get; set; }

        /// <summary>
        /// Кладовая
        /// </summary>
        public Vector3 Storage { get; set; }

        /// <summary>
        /// Гардероб
        /// </summary>
        public Vector3 Wardrobe { get; set; }

        /// <summary>
        /// Выход
        /// </summary>
        public Vector3 Exit { get; set; }
    }
}