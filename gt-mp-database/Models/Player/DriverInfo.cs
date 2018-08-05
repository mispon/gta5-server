using System;

namespace gta_mp_database.Models.Player {
    /// <summary>
    /// Информация о водительской лицензии
    /// </summary>
    public class DriverInfo {
        /// <summary>
        /// Был ли оплачен теоретический экзамен
        /// </summary>
        public bool TheoryExamPaid { get; set; }

        /// <summary>
        /// Сдал теорию
        /// </summary>
        public bool PassedTheory { get; set; }

        /// <summary>
        /// Был ли оплачен практический экзамен
        /// </summary>
        public bool PracticeExamPaid { get; set; }

        /// <summary>
        /// Сдал практику легковых автомобилей
        /// </summary>
        public bool PassedPracticeB { get; set; }

        /// <summary>
        /// Сдал практику легковых автомобилей
        /// </summary>
        public bool PassedPracticeC { get; set; }

        /// <summary>
        /// Время, когда можно будет сдавать экзамен в сл. раз
        /// </summary>
        public DateTime TimeToNextTry { get; set; }

        /// <summary>
        /// Может ли игрок водить легковые авто
        /// </summary>
        public bool CanDriveB => PassedTheory && PassedPracticeB;

        /// <summary>
        /// Может ли игрок водить грузовые авто
        /// </summary>
        public bool CanDriveС => PassedTheory && PassedPracticeC;
    }
}