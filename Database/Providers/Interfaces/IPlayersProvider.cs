using System.Collections.Generic;
using gta_mp_data.Entity;
using PlayerInfo = gta_mp_database.Models.Player.PlayerInfo;

namespace gta_mp_database.Providers.Interfaces {
    public interface IPlayersProvider {
        /// <summary>
        /// Добавить данные нового игрока
        /// </summary>
        void Add(Account account);

        /// <summary>
        /// Возвращает данные игрока
        /// </summary>
        PlayerInfo GetInfo(long accountId);

        /// <summary>
        /// Записать данные игрока
        /// </summary>
        void SetInfo(PlayerInfo playerInfo);

        /// <summary>
        /// Обновить данные игроков
        /// </summary>
        void UpdatePlayersInfos(List<PlayerInfo> playersInfos);

        /// <summary>
        /// Записывает имя игрока
        /// </summary>
        bool SetName(long accountId, string name);

        /// <summary>
        /// Установить измерение игрока
        /// </summary>
        void SetDimension(long accountId, int dimension);

        /// <summary>
        /// Зачисляет награду за реферала
        /// </summary>
        void SetReferalReward(string name, int value);

        /// <summary>
        /// Возвращает следующий номер телефона
        /// </summary>
        int GetPhoneNumber();
    }
}