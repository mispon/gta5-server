using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_data.Entity;
using gta_mp_database.Providers.Interfaces;
using LinqToDB;
using LinqToDB.Data;

namespace gta_mp_database.Providers {
    /// <summary>
    /// Провайдер к данным аккаунтов игроков
    /// </summary>
    public class AccountsProvider : IAccountsProvider {
        public AccountsProvider() {
            DataConnection.DefaultSettings = new AppSettings();
        }

        /// <summary>
        /// Создает новый аккаунт
        /// </summary>
        public bool Create(string email, string password, string friendReferal = null) {
            using (var db = new Database()) {
                var exist = Get(email);
                if (exist != null) {
                    return false;
                }
                var account = new Account {Email = email, Password = password, LastLogin = DateTime.Now, FriendReferal = friendReferal};
                db.Insert(account);
                return true;
            }
        }

        /// <summary>
        /// Возвращает аккаунт игрока по почте
        /// </summary>
        public Account Get(string email) {
            using (var db = new Database()) {
                return db.Accounts.FirstOrDefault(e => e.Email == email);
            }
        }

        /// <summary>
        /// Возвращает аккаунт игрока по почте и паролю
        /// </summary>
        public Account Get(string email, string password) {
            using (var db = new Database()) {
                return db.Accounts.FirstOrDefault(e => e.Email == email && e.Password == password);
            }
        }

        /// <summary>
        /// Возвращает список активных за последнее время аккаунтов
        /// </summary>
        public List<Account> GetActive(int daysAgo = 3) {
            using (var db = new Database()) {
                return db.Accounts.Where(e => (DateTime.Now - e.LastLogin).TotalDays < daysAgo).ToList();
            }
        }

        /// <summary>
        /// Обновить последнюю дату входа игрока
        /// </summary>
        public void Update(Account account) {
            using (var db = new Database()) {
                db.Update(account);
            }
        }

        /// <summary>
        /// Обновить общее время в игре
        /// </summary>
        public void UpdateTotalTime(long accountId) {
            using (var db = new Database()) {
                var account = db.Accounts.First(e => e.Id == accountId);
                var totalTime = account.TotalTime;
                var lastLogin = account.LastLogin;
                totalTime += (DateTime.Now - lastLogin).Ticks;
                db.Accounts
                    .Where(e => e.Id == accountId)
                    .Set(e => e.TotalTime, totalTime)
                    .Update();
            }
        }

        /// <summary>
        /// Возвращает рефералов игрока
        /// </summary>
        public string GetFriendReferal(long accountId) {
            using (var db = new Database()) {
                return db.Accounts.First(e => e.Id == accountId).FriendReferal;
            }
        }
    }
}