using gta_mp_database.Entity;

namespace gta_mp_database.Providers.Interfaces {
    public interface IDistrictsProvider {
        /// <summary>
        /// Возвращает улицу для сражения
        /// </summary>
        District GetNext();
    }
}