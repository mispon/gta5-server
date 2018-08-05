using System;
using LinqToDB.Mapping;

namespace gta_mp_data.Entity {
    /// <summary>
    /// Аккаунт игрока на сервере
    /// </summary>
    [Table(Name = "Accounts")]
    public class Account {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [PrimaryKey, Identity]
        [Column(Name = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Почта
        /// </summary>
        [Column(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Column(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Баланс аккаунта
        /// </summary>
        [Column(Name = "Balance")]
        public int Balance { get; set; }

        /// <summary>
        /// Дата окончания премиума
        /// </summary>
        [Column(Name = "PremiumEnd")]
        public DateTime PremiumEnd { get; set; }

        /// <summary>
        /// Время последнего входа
        /// </summary>
        [Column(Name = "LastLogin")]
        public DateTime LastLogin { get; set; }

        /// <summary>
        /// Общее время в игре
        /// </summary>
        [Column(Name = "TotalTime")]
        public long TotalTime { get; set; }

        /// <summary>
        /// Рефералы игрока
        /// </summary>
        [Column(Name = "FriendReferal")]
        public string FriendReferal { get; set; }

        /// <summary>
        /// Количество дней входа в игру подряд
        /// </summary>
        [Column(Name = "DaysInRow")]
        public int DaysInRow { get; set; }
    }
}