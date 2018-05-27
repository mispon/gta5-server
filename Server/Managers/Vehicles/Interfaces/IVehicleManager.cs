using System.Collections.Generic;
using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using VehicleInfo = gta_mp_database.Entity.Vehicle;

namespace gta_mp_server.Managers.Vehicles.Interfaces {
    internal interface IVehicleManager {
        /// <summary>
        /// Создать коммунальный транспорт
        /// </summary>
        Vehicle CreateVehicle(CommonVehicle info);

        /// <summary>
        /// Создать транспорт игрока
        /// </summary>
        Vehicle CreateVehicle(VehicleInfo vehicleInfo, Vector3 position, Vector3 rotation, int dimension = 0);

        /// <summary>
        /// Найти транспорт по идентификатору владельца
        /// </summary>
        Vehicle GetInstanceById(long vehicleId);

        /// <summary>
        /// Обновление состояния работающих машин
        /// </summary>
        void UpdateVehicles();

        /// <summary>
        /// Восстановить начальную позицию транспорта
        /// </summary>
        void RestorePosition(Vehicle vehicle);

        /// <summary>
        /// Возвращает ближайшую машину
        /// </summary>
        Vehicle GetNearestVehicle(Client player, float range, string key = null);

        /// <summary>
        /// Возвращает брошенный транспорт
        /// </summary>
        List<Vehicle> GetAfkVehicles(int afkMinutes);
    }
}