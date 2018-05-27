using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using Vehicle = gta_mp_database.Entity.Vehicle;

namespace gta_mp_server.Managers.Vehicles.Interfaces {
    internal interface IVehicleInfoManager {
        /// <summary>
        /// Возвращает информацию о транспорте игрока по идентификатору
        /// </summary>
        Vehicle GetInfoById(Client player, long vehicleId);

        /// <summary>
        /// Возвращает информацию о транспорте игрока по инстансу
        /// </summary>
        Vehicle GetInfoByHandle(Client player, NetHandle handle);

        /// <summary>
        /// Возвращает информацию о транспорте по инстансу
        /// </summary>
        Vehicle GetInfoByHandle(NetHandle handle);

        /// <summary>
        /// Возвращает транспорт игрока
        /// </summary>
        List<Vehicle> GetPlayerVehicles(Client player);

        /// <summary>
        /// Возвращает транспорт игрока
        /// </summary>
        List<Vehicle> GetPlayerVehicles(long accountId);

        /// <summary>
        /// Записывает информацию о транспорте игрока
        /// </summary>
        void SetInfo(Client player, Vehicle vehicleInfo);

        /// <summary>
        /// Записывает информацию о транспорте игрока
        /// </summary>
        void SetInfo(Vehicle vehicleInfo);

        /// <summary>
        /// Удалить транспорт игрока
        /// </summary>
        void RemoveVehicle(Client player, long vehicleId);

        /// <summary>
        /// Припарковать весь транспорт
        /// </summary>
        void ParkAllVehicles();
    }
}