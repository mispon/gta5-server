using System;
using System.Linq;
using gta_mp_database.Entity;
using gta_mp_database.Providers.Interfaces;
using LinqToDB;

namespace gta_mp_database.Providers {
    /// <summary>
    /// Провайдер к данным таблицы районов
    /// </summary>
    public class DistrictsProvider : IDistrictsProvider {
        /// <summary>
        /// Возвращает район для сражения
        /// </summary>
        public District GetNext() {
            using (var db = new Database()) {
                var district = db.Districts.OrderBy(e => e.LastWarTime).First();
                district.LastWarTime = DateTime.Now;
                db.Update(district);
                return district;
            }
        }
    }
}