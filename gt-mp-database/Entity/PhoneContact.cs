using LinqToDB.Mapping;

namespace gta_mp_database.Entity {
    /// <summary>
    /// Сущность записи в телефоне игрока
    /// </summary>
    [Table(Name = "PhoneContacts")]
    public class PhoneContact {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [PrimaryKey, Identity, Column(Name = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Идентификатор игрока
        /// </summary>
        [Column(Name = "AccountId")]
        public long AccountId { get; set; }

        /// <summary>
        /// Имя контакта
        /// </summary>
        [Column(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Телефонный номер
        /// </summary>
        [Column(Name = "Number")]
        public int Number { get; set; }
    }
}