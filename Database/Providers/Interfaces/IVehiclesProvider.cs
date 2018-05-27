using System.Collections.Generic;
using gta_mp_database.Entity;

namespace gta_mp_database.Providers.Interfaces {
    public interface IVehiclesProvider {
        /// <summary>
        /// Получить информацию о транспорте по идентификатору
        /// </summary>
        Vehicle Get(long vehicleId);

        /// <summary>
        /// Получить транспортные средства игрока
        /// </summary>
        IEnumerable<Vehicle> GetVehicles(long accountId);

        /// <summary>
        /// Добавляет запись о новом транспортном средстве
        /// </summary>
        void Add(Vehicle vehicle);

        /// <summary>
        /// Обновить данные транспортных средств
        /// </summary>
        void Update(IEnumerable<Vehicle> vehicles);

        /// <summary>
        /// Обновить состояние спавна
        /// </summary>
        void SetSpawn(long vehicleId, bool isSpawned);

        /// <summary>
        /// Удалить транспорт
        /// </summary>
        void Remove(long vehicleId);

        /// <summary>
        /// Припарковать весь вызванный транспорт
        /// </summary>
        void ParkAll();
    }
}