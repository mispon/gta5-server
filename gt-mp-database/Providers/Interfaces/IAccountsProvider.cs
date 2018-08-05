using System.Collections.Generic;
using gta_mp_data.Entity;

namespace gta_mp_database.Providers.Interfaces {
    public interface IAccountsProvider {
        /// <summary>
        /// Создает новый аккаунт
        /// </summary>
        bool Create(string email, string password, string friendReferal = null);

        /// <summary>
        /// Возвращает аккаунт игрока по почте
        /// </summary>
        Account Get(string email);

        /// <summary>
        /// Возвращает аккаунт игрока при удачном логине
        /// </summary>
        Account Get(string login, string password);

        /// <summary>
        /// Возвращает список активных за последнее время аккаунтов
        /// </summary>
        List<Account> GetActive(int daysAgo = 3);

        /// <summary>
        /// Обновить последнюю дату входа игрока
        /// </summary>
        void Update(Account account);

        /// <summary>
        /// Обновить общее время в игре
        /// </summary>
        void UpdateTotalTime(long accountId);

        /// <summary>
        /// Возвращает рефералов игрока
        /// </summary>
        string GetFriendReferal(long accountId);
    }
}