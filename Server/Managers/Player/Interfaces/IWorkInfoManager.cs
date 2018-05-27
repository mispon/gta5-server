using gta_mp_data.Enums;
using gta_mp_database.Models.Work;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.Player.Interfaces {
    /// <summary>
    /// Логика работы с информацией о работах игрока
    /// </summary>
    internal interface IWorkInfoManager {
        /// <summary>
        /// Добавляет игроку работу, если до этого ее еще не было
        /// </summary>
        void CreateInfoIfNeed(Client player, WorkType type);

        /// <summary>
        /// Возвращает информацию о работе игрока
        /// </summary>
        WorkInfo GetWorkInfo(Client player, WorkType type);

        /// <summary>
        /// Записывает информацию о работе игрока
        /// </summary>
        void SetWorkInfo(Client player, WorkInfo info);

        /// <summary>
        /// Начислить зарплату
        /// </summary>
        void SetSalary(Client player, WorkType type, int salary);

        /// <summary>
        /// Устанавливает уровень работы
        /// </summary>
        void SetExperience(Client player, WorkType type, int exp);

        /// <summary>
        /// Выставляет активность работы
        /// </summary>
        void SetActivity(Client player, WorkType type, bool active);

        /// <summary>
        /// Возвращает активную работу
        /// </summary>
        WorkInfo GetActiveWork(Client player);

        /// <summary>
        /// Проверяет, достаточный ли уровень работы игрока
        /// </summary>
        bool WorkLevelEnough(Client player, WorkType type, int neededLevel);
    }
}