using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models.Places {
    /// <summary>
    /// Модель заправки
    /// </summary>
    internal class FillingModel {
        /// <summary>
        /// Идентификатор заправки
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Расположение заправки
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Позиции продавца
        /// </summary>
        public string NpcPositions { get; set; }

        /// <summary>
        /// Позиции бензоколонок
        /// </summary>
        public string FillingPoints { get; set; }

        /// <summary>
        /// Улица, на которой находится заправка
        /// </summary>
        public int District { get; set; }
    }
}