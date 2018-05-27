using System.Collections.Generic;
using System.Linq;
using gta_mp_database.Entity;
using gta_mp_database.Providers.Interfaces;
using LinqToDB;
using LinqToDB.Data;

namespace gta_mp_database.Providers {
    /// <summary>
    /// Провайдер к базе Houses
    /// </summary>
    public class HousesProvider : IHousesProvider {
        public HousesProvider() {
            DataConnection.DefaultSettings = new AppSettings();
        }

        /// <summary>
        /// Возвращает список инфармации о домах
        /// </summary>
        public List<House> GetHouses() {
            using (var db = new Database()) {
                return db.Houses.ToList();
            }
        }

        /// <summary>
        /// Обновить информацию о домах
        /// </summary>
        public void UpdateHouses(IEnumerable<House> houses) {
            using (var db = new Database()) {
                foreach (var entity in houses) {
                    db.Update(entity);
                }
            }
        }
    }
}