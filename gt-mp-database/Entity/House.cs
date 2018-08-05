using System;
using gta_mp_database.Enums;
using LinqToDB.Mapping;

namespace gta_mp_database.Entity {
    /// <summary>
    /// Сущность дома
    /// </summary>
    [Table(Name = "Houses")]
    public class House {
        /// <summary>
        /// Идентификатор дома
        /// </summary>
        [PrimaryKey, Identity]
        [Column(Name = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Владелец
        /// </summary>
        [Column(Name = "OwnerId")]
        public long OwnerId { get; set; }

        /// <summary>
        /// Тип дома
        /// </summary>
        [Column(Name = "Type")]
        public HouseType Type { get; set; }

        /// <summary>
        /// Дверной замок
        /// </summary>
        [Column(Name = "Lock")]
        public bool Lock { get; set; }

        /// <summary>
        /// Стоимость аренды за один день
        /// </summary>
        [Column(Name = "DailyRent")]
        public int DailyRent { get; set; }

        /// <summary>
        /// Дата окончания аренды
        /// </summary>
        [Column(Name = "EndOfRenting")]
        public DateTime EndOfRenting { get; set; }

        /// <summary>
        /// Расположение дома
        /// </summary>
        [Column(Name = "Position")]
        public string Position { get; set; }

        /// <summary>
        /// Позиция гаража
        /// </summary>
        [Column(Name = "GaragePosition")]
        public string GaragePosition { get; set; }

        /// <summary>
        /// Поворот после выхода из гаража
        /// </summary>
        [Column(Name = "RotationAfterExit")]
        public string RotationAfterExit { get; set; }
    }
}