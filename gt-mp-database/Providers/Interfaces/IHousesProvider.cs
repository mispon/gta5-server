using System.Collections.Generic;
using gta_mp_database.Entity;

namespace gta_mp_database.Providers.Interfaces {
    public interface IHousesProvider {
        /// <summary>
        /// Возвращает список инфармации о домах
        /// </summary>
        List<House> GetHouses();

        /// <summary>
        /// Обновить информацию о домах
        /// </summary>
        void UpdateHouses(IEnumerable<House> houses);
    }
}