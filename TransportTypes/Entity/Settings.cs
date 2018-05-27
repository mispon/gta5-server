using LinqToDB.Mapping;

namespace gta_mp_data.Entity {
    /// <summary>
    /// Сущность настроек игрока
    /// </summary>
    [Table(Name = "Settings")]
    public class Settings {
        /// <summary>
        /// Идентификатор аккаунта
        /// </summary>
        [PrimaryKey]
        [Column(Name = "AccountId")]
        public long AccountId { get; set; }

        /// <summary>
        /// Использавать ли svg-спидометр
        /// </summary>
        [Column(Name = "SvgSpeedometer")]
        public bool SvgSpeedometer { get; set; }

        /// <summary>
        /// Показывать ли игроку подсказки
        /// </summary>
        [Column(Name = "ShowHints")]
        public bool ShowHints { get; set; }
    }
}