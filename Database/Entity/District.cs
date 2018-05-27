using System;
using LinqToDB.Mapping;

namespace gta_mp_database.Entity {
    /// <summary>
    /// Сущность района, за который сражаются кланы
    /// </summary>
    [Table(Name = "Districts")]
    public class District {
        /// <summary>
        /// Идентификатор района
        /// </summary>
        [PrimaryKey, Column(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        [Column(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Центральная позиция, место захвата района
        /// </summary>
        [Column(Name = "Position")]
        public string Position { get; set; }

        /// <summary>
        /// Дата последнего сражения за район
        /// </summary>
        [Column(Name = "LastWarTime")]
        public DateTime LastWarTime { get; set; }
    }
}