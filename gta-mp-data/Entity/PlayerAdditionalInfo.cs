using System;
using LinqToDB.Mapping;

namespace gta_mp_data.Entity {
    /// <summary>
    /// Второстепенная информация игрока
    /// </summary>
    [Table(Name = "PlayersAdditionalInfo")]
    public class PlayerAdditionalInfo {
        /// <summary>
        /// Идентификатор аккаунта
        /// </summary>
        [PrimaryKey]
        [Column(Name = "AccountId")]
        public long AccountId { get; set; }

        /// <summary>
        /// Саб-ник
        /// </summary>
        [Column(Name = "TagName")]
        public string TagName { get; set; }

        /// <summary>
        /// Цвет саб-ника
        /// </summary>
        [Column(Name = "TagColor")]
        public string TagColor { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        [Column(Name = "PhoneNumber")]
        public int PhoneNumber { get; set; }

        /// <summary>
        /// Баланс телефона
        /// </summary>
        [Column(Name = "PhoneBalance")]
        public float PhoneBalance { get; set; }

        /// <summary>
        /// Время последнего телепорта домой
        /// </summary>
        [Column(Name = "LastHouseTp")]
        public DateTime LastTeleportToHouse { get; set; }
    }
}