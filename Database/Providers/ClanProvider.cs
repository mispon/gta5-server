using System.Collections.Generic;
using System.Linq;
using gta_mp_database.Entity;
using gta_mp_database.Providers.Interfaces;
using LinqToDB;

namespace gta_mp_database.Providers {
    /// <summary>
    /// Провайдер к данным кланов
    /// </summary>
    public class ClanProvider : IClanProvider {
        /// <summary>
        /// Загрузить кланы
        /// </summary>
        public List<Clan> LoadClans() {
            using (var db = new Database()) {
                return db.Clans.ToList();
            }
        }

        /// <summary>
        /// Сохранить кланы
        /// </summary>
        public void SaveClans(IEnumerable<Clan> clans) {
            using (var db = new Database()) {
                foreach (var clan in clans) {
                    db.Update(clan);
                }
            }
        }

        /// <summary>
        /// Удалить информацию о клане игрока
        /// </summary>
        public void RemoveClanInfo(long accountId) {
            using (var db = new Database()) {
                db.PlayerClanInfos.Where(e => e.AccountId == accountId).Delete();
            }
        }

        /// <summary>
        /// Возвращает количество участников в каждом клане
        /// </summary>
        public Dictionary<long, int> GetMembersCountByClans() {
            using (var db = new Database()) {
                return db.PlayerClanInfos.GroupBy(e => e.ClanId).ToDictionary(e => e.Key, e => e.Count());
            }
        }
    }
}