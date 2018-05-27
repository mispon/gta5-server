using LinqToDB.Mapping;

namespace gta_mp_data.Entity {
    /// <summary>
    /// Основные данные игрока
    /// </summary>
    [Table(Name = "PlayersInfo")]
    public class PlayerInfo {
        /// <summary>
        /// Идентификатор аккаунта
        /// </summary>
        [PrimaryKey]
        [Column(Name = "AccountId")]
        public long AccountId { get; set; }

        /// <summary>
        /// Игровой ник
        /// </summary>
        [Column(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Уровень игрока
        /// </summary>
        [Column(Name = "Level")]
        public int Level { get; set; } = 1;

        /// <summary>
        /// Количество опыта
        /// </summary>
        [Column(Name = "Experience")]
        public int Experience { get; set; }

        /// <summary>
        /// Здоровье
        /// </summary>
        [Column(Name = "Health")]
        public int Health { get; set; }

        /// <summary>
        /// Сытость
        /// </summary>
        [Column(Name = "Satiety")]
        public float Satiety { get; set; }

        /// <summary>
        /// Последняя позиция до выхода
        /// </summary>
        [Column(Name = "LastPosition")]
        public string LastPosition { get; set; }

        /// <summary>
        /// Измерение
        /// </summary>
        [Column(Name = "Dimension")]
        public int Dimension { get; set; }
    }
}