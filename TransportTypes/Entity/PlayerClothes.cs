using LinqToDB.Mapping;

namespace gta_mp_data.Entity {
    /// <summary>
    /// Сущность таблицы с одеждами игроков
    /// </summary>
    [Table(Name = "PlayerClothes")]
    public class PlayerClothes {
        /// <summary>
        /// Идентификатор аккаунта
        /// </summary>
        [PrimaryKey]
        [Column(Name = "AccountId")]
        public long AccountId { get; set; }

        /// <summary>
        /// Разновидность вещи
        /// </summary>
        [PrimaryKey]
        [Column(Name = "Variation")]
        public int Variation { get; set; }

        /// <summary>
        /// Часть тела
        /// </summary>
        [PrimaryKey]
        [Column(Name = "Slot")]
        public int Slot { get; set; }

        /// <summary>
        /// Отображение торса под вещь
        /// </summary>
        [Column(Name = "Torso")]
        public int? Torso { get; set; }

        /// <summary>
        /// Нижняя майка
        /// </summary>
        [Column(Name = "Undershirt")]
        public int? Undershirt { get; set; }

        /// <summary>
        /// Текущая раскраска
        /// </summary>
        [Column(Name = "Texture")]
        public int Texture { get; set; }

        /// <summary>
        /// Полный список раскрасок
        /// </summary>
        [Column(Name = "Textures")]
        public string Textures { get; set; }

        /// <summary>
        /// Одета ли на игрока
        /// </summary>
        [Column(Name = "OnPlayer")]
        public bool OnPlayer { get; set; }

        /// <summary>
        /// Одежда или аксессуар
        /// </summary>
        [Column(Name = "IsClothes")]
        public bool IsClothes { get; set; }
    }
}