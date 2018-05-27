using System;
using LinqToDB.Mapping;

namespace gta_mp_data.Entity {
    /// <summary>
    /// Сущность таблице с информацией о водительской лицензии игроков
    /// </summary>
    [Table(Name = "DriversInfo")]
    public class DriverInfo {
        /// <summary>
        /// Идентификатор аккаунта
        /// </summary>
        [PrimaryKey]
        [Column(Name = "AccountId")]
        public long AccountId { get; set; }

        /// <summary>
        /// Оплачен ли теоретический экзамен
        /// </summary>
        [Column(Name = "TheoryExamPaid")]
        public bool TheoryExamPaid { get; set; }

        /// <summary>
        /// Сдана ли теория
        /// </summary>
        [Column(Name = "PassedTheory")]
        public bool PassedTheory { get; set; }

        /// <summary>
        /// Оплачен ли теоретический экзамен
        /// </summary>
        [Column(Name = "PracticeExamPaid")]
        public bool PracticeExamPaid { get; set; }

        /// <summary>
        /// Пройден ли практический экзамен на легковом автомобиле
        /// </summary>
        [Column(Name = "PassedPracticeB")]
        public bool PassedPracticeB { get; set; }

        /// <summary>
        /// Пройден ли практический экзамен на грузовом автомобиле
        /// </summary>
        [Column(Name = "PassedPracticeC")]
        public bool PassedPracticeC { get; set; }

        /// <summary>
        /// Время, когда можно следующий раз сдать на права после неудачи
        /// </summary>
        [Column(Name = "TimeToNextTry")]
        public DateTime TimeToNextTry { get; set; }
    }
}