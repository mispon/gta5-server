using System.Collections.Generic;
using gta_mp_database.Entity;

namespace gta_mp_database.Providers.Interfaces {
    public interface IClanProvider {
        /// <summary>
        /// Загрузить кланы
        /// </summary>
        List<Clan> LoadClans();

        /// <summary>
        /// Сохранить кланы
        /// </summary>
        void SaveClans(IEnumerable<Clan> clans);

        /// <summary>
        /// Удалить информацию о клане игрока
        /// </summary>
        void RemoveClanInfo(long accountId);

        /// <summary>
        /// Возвращает количество участников в каждом клане
        /// </summary>
        Dictionary<long, int> GetMembersCountByClans();
    }
}