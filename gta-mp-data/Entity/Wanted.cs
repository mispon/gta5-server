using LinqToDB.Mapping;

namespace gta_mp_data.Entity {
    /// <summary>
    /// Сущность таблицы с информацией о правонарушениях игрока
    /// </summary>
    [Table(Name = "Wanted")]
    public class Wanted {
        /// <summary>
        /// Идентификатор аккаунта
        /// </summary>
        [PrimaryKey]
        [Column(Name = "AccountId")]
        public long AccountId { get; set; }

        /// <summary>
        /// Количество избиений других игроков
        /// </summary>
        [Column(Name = "Beatings")]
        public int Beatings { get; set; }

        /// <summary>
        /// Безнаказанное количество убийств
        /// </summary>
        [Column(Name = "Kills")]
        public int Kills { get; set; }

        /// <summary>
        /// Срок до освобождения
        /// </summary>
        [Column(Name = "JailTime")]
        public int JailTime { get; set; }

        /// <summary>
        /// Общее количество тюремных заключений
        /// </summary>
        [Column(Name = "Jails")]
        public int Jails { get; set; }

        /// <summary>
        /// Общее количество убийств
        /// </summary>
        [Column(Name = "TotalKills")]
        public int TotalKills { get; set; }

        /// <summary>
        /// Текущий уровень розыска
        /// </summary>
        public int WantedLevel => CalculateWantedLevel();

        /// <summary>
        /// Рассчитывает уровень розыска
        /// </summary>
        private int CalculateWantedLevel() {
            var result = (int) (Beatings * 0.05f + Kills * 0.3f);
            if (result == 0 && (Beatings > 0 || Kills > 0)) {
                result += 1;
            }
            return result;
        }
    }
}