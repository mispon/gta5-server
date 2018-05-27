using gta_mp_data.Enums;
using LinqToDB.Mapping;

namespace gta_mp_data.Entity {
    /// <summary>
    /// Информация о клане игрока
    /// </summary>
    [Table(Name = "PlayerClanInfo")]
    public class PlayerClanInfo {
        /// <summary>
        /// Идентификатор аккаунта
        /// </summary>
        [PrimaryKey, Column(Name = "AccountId")]
        public long AccountId { get; set; }

        /// <summary>
        /// Идентификатор клана
        /// </summary>
        [Column(Name = "ClanId")]
        public long ClanId { get; set; }

        /// <summary>
        /// Репутация
        /// </summary>
        [Column(Name = "Reputation")]
        public int Reputation { get; set; }

        /// <summary>
        /// Ранг
        /// </summary>
        [Column(Name = "Rank")]
        public ClanRank Rank { get; set; }
    }
}