using gta_mp_data.Enums;
using LinqToDB.Mapping;

namespace gta_mp_data.Entity {
    /// <summary>
    /// Сущность таблицы работы игроков
    /// </summary>
    [Table(Name = "PlayerWorks")]
    public class PlayerWork {
        /// <summary>
        /// Идентификатор аккаунта
        /// </summary>
        [PrimaryKey]
        [Column(Name = "AccountId")]
        public long AccountId { get; set; }

        /// <summary>
        /// Уровень работы
        /// </summary>
        [Column(Name = "Level")]
        public int Level { get; set; }

        /// <summary>
        /// Опыт работы
        /// </summary>
        [Column(Name = "Experience")]
        public int Experience { get; set; }

        /// <summary>
        /// Тип работы
        /// </summary>
        [PrimaryKey]
        [Column(Name = "Type")]
        public WorkType Type { get; set; }

        /// <summary>
        /// Количество денег к выплате
        /// </summary>
        [Column(Name = "Salary")]
        public int Salary { get; set; }

        /// <summary>
        /// Является ли работа активной в данный момент
        /// </summary>
        [Column(Name = "Active")]
        public bool Active { get; set; }
    }
}