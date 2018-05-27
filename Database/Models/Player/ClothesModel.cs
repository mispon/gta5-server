using System.Collections.Generic;

namespace gta_mp_database.Models.Player {
    /// <summary>
    /// Модель одежды
    /// </summary>
    public class ClothesModel {
        /// <summary>
        /// Разновидность вещи
        /// </summary>
        public int Variation { get; set; }

        /// <summary>
        /// Часть тела
        /// </summary>
        public int Slot { get; set; }

        /// <summary>
        /// Отображение торса под вещь
        /// </summary>
        public int? Torso { get; set; }

        /// <summary>
        /// Нижняя майка
        /// </summary>
        public int? Undershirt { get; set; }

        /// <summary>
        /// Текущая раскраска
        /// </summary>
        public int Texture { get; set; }

        /// <summary>
        /// Полный список раскрасок
        /// </summary>
        public List<int> Textures { get; set; }

        /// <summary>
        /// Одета ли на игрока
        /// </summary>
        public bool OnPlayer { get; set; }

        /// <summary>
        /// Одежда или аксессуар
        /// </summary>
        public bool IsClothes { get; set; }
        
        /// <summary>
        /// Цена
        /// </summary>
        public int Price { get; set; }
    }
}