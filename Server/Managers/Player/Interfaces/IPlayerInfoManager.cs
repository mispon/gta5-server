using System;
using System.Collections.Generic;
using gta_mp_database.Models.Player;
using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Server.Elements;
// ReSharper disable All

namespace gta_mp_server.Managers.Player.Interfaces {
    /// <summary>
    /// Логика работы с общими данными игрока
    /// </summary>
    internal interface IPlayerInfoManager {
        /// <summary>
        /// Добавляет игрока в список игроков онлайн
        /// </summary>
        void Add(Client player, PlayerInfo playerInfo);

        /// <summary>
        /// Возвращает игрока по идентификатору
        /// Item1 - игрок
        /// Item2 - его данные
        /// </summary>
        PlayerWithData GetWithData(long accountId, bool needFromDb = true);

        /// <summary>
        /// Записать данные игрока
        /// </summary>
        void Set(PlayerInfo playerInfo);

        /// <summary>
        /// Удаляет игрока из списка
        /// </summary>
        void Remove(Client player);

        /// <summary>
        /// Возвращает данные игрока из кэша
        /// </summary>
        PlayerInfo GetInfo(Client player);

        /// <summary>
        /// Возвращает данные игрока по его идентификатору
        /// </summary>
        PlayerInfo Get(long accontId);

        /// <summary>
        /// Возвращает всех игроков, соответствующих условию
        /// </summary>
        Dictionary<Client, PlayerInfo> GetWhere(Func<PlayerInfo, bool> predicate);

        /// <summary>
        /// Обновляет данные игрока
        /// </summary>
        void RefreshUI(Client player, PlayerInfo info);

        /// <summary>
        /// Обновляет опыт
        /// </summary>
        void SetExperience(Client player, int exp);

        /// <summary>
        /// Обновляет баланс
        /// </summary>
        void SetBalance(Client player, int balance, bool considerPremium = false);

        /// <summary>
        /// Очистить уровень розыска игрока
        /// </summary>
        void ClearWanted(Client player);

        /// <summary>
        /// Проверяет пол игрока
        /// </summary>
        bool IsMale(Client player);

        /// <summary>
        /// Записывает измерение игрока
        /// </summary>
        void SetDimension(Client player, int dimension);

        /// <summary>
        /// Одевает одежду игрока
        /// </summary>
        void SetPlayerClothes(Client player, bool withAppearance = false);

        /// <summary>
        /// Возвращает игрока по номеру телефона
        /// </summary>
        Client GetByNumber(int number);

        /// <summary>
        /// Записывает номер телефона
        /// </summary>
        void SetPhoneNumber(Client player);
    }
}