using gta_mp_data.Enums;

namespace gta_mp_database.Models.Work {
    /// <summary>
    /// Информация о прогрессе работы игрока
    /// </summary>
    public class WorkInfo {
        /// <summary>
        /// Тип работы
        /// </summary>
        public WorkType Type { get; set; }

        /// <summary>
        /// Уровень работника
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Кол-во опыта
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        /// Зарплата
        /// </summary>
        public int Salary { get; set; }

        /// <summary>
        /// Активность работы в данный момент
        /// </summary>
        public bool Active { get; set; }
    }
}
