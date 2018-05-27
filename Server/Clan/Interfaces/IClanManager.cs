using gta_mp_database.Models.Player;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Clan.Interfaces {
    internal interface IClanManager {
        /// <summary>
        /// Загружает кланы в кэш
        /// </summary>
        void Initialize();

        /// <summary>
        /// Вступление в клан
        /// </summary>
        void JoinClan(Client player, PlayerInfo playerInfo, int clanId);

        /// <summary>
        /// Выход из клана
        /// </summary>
        void LeftClan(Client player, PlayerInfo playerInfo);

        /// <summary>
        /// Устанавливает ранг
        /// </summary>
        void SetReputation(Client player, int value, PlayerInfo playerInfo = null);

        /// <summary>
        /// Пополнить баланс клана от покупки
        /// </summary>
        void ReplenishClanBalance(int districtId, int amount);

        /// <summary>
        /// Добавить улицу клану
        /// </summary>
        void AddDistrict(long clanId, int districtId);

        /// <summary>
        /// Удалает улицу из кланового списка контрольных улиц
        /// </summary>
        void RemoveDistrict(int districtId);

        /// <summary>
        /// Проверяет, что в выбранном клане нет перекоса 
        /// по количеству участников относительно других кланов
        /// </summary>
        bool ClanIsFull(long clanId);
    }
}