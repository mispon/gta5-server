using System.Linq;
using gta_mp_database.Converters;
using gta_mp_database.Models.Player;
using gta_mp_database.Providers.Interfaces;
using LinqToDB;
using LinqToDB.Data;

namespace gta_mp_database.Providers {
    /// <summary>
    /// Провайдер к таблице с настройками внешнего вида игроков
    /// </summary>
    public class PlayersAppearanceProvider : IPlayersAppearanceProvider {
        public PlayersAppearanceProvider() {
            DataConnection.DefaultSettings = new AppSettings();
        }

        /// <summary>
        /// Сохраняет настройки внешнего вида игрока
        /// </summary>
        public void Save(long accountId, PlayerAppearance model) {
            var entity = PlayerAppearanceConverter.ConverToEntity(accountId, model);
            using (var db = new Database()) {
                if (db.PlayersAppearance.FirstOrDefault(e => e.AccountId == accountId) == null) {
                    db.Insert(entity);
                }
                else {
                    db.Update(entity);
                }
            }
        }

        /// <summary>
        /// Возвращает настройки внешнего вида игрока
        /// </summary>
        public PlayerAppearance Get(long accountId) {
            using (var db = new Database()) {
                var entity = db.PlayersAppearance.FirstOrDefault(e => e.AccountId == accountId);
                return PlayerAppearanceConverter.ConvertToModel(entity);
            }
        }
    }
}