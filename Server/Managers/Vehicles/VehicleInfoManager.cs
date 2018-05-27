using System.Collections.Generic;
using System.Linq;
using gta_mp_database.Providers.Interfaces;
using gta_mp_server.Managers.Player;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using Vehicle = GrandTheftMultiplayer.Server.Elements.Vehicle;
using VehicleInfo = gta_mp_database.Entity.Vehicle;

namespace gta_mp_server.Managers.Vehicles {
    /// <summary>
    /// Логика работы с данными транспорта
    /// </summary>
    internal class VehicleInfoManager: Script, IVehicleInfoManager {
        private readonly IVehiclesProvider _vehiclesProvider;
        private readonly IPlayerInfoManager _playerInfoManager;

        public VehicleInfoManager() {}
        public VehicleInfoManager(IVehiclesProvider vehiclesProvider, IPlayerInfoManager playerInfoManager) {
            _vehiclesProvider = vehiclesProvider;
            _playerInfoManager = playerInfoManager;
        }

        /// <summary>
        /// Возвращает информацию о транспорте игрока по идентификатору
        /// </summary>
        public VehicleInfo GetInfoById(Client player, long vehicleId) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            playerInfo.Vehicles.TryGetValue(vehicleId, out var result);
            return result;
        }

        /// <summary>
        /// Возвращает информацию о транспорте игрока по инстансу
        /// </summary>
        public VehicleInfo GetInfoByHandle(Client player, NetHandle handle) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            var vehicle = API.getEntityFromHandle<Vehicle>(handle);
            return playerInfo.Vehicles.Values.FirstOrDefault(e => e.Instance == vehicle);
        }

        /// <summary>
        /// Возвращает информацию о транспорте по инстансу
        /// </summary>
        public VehicleInfo GetInfoByHandle(NetHandle handle) {
            var vehicle = API.getEntityFromHandle<Vehicle>(handle);
            if (!vehicle.hasData(VehicleManager.OWNER_ID)) {
                return null;
            }
            var ownerId = (long) vehicle.getData(VehicleManager.OWNER_ID);
            var vehicleId = (long) vehicle.getData(VehicleManager.VEHICLE_ID);
            var playerData = _playerInfoManager.GetWithData(ownerId, false);
            VehicleInfo result;
            if (playerData.Player == null) {
                result = _vehiclesProvider.Get(vehicleId);
            }
            else {
                playerData.PlayerInfo.Vehicles.TryGetValue(vehicleId, out result);
            }
            return result;
        }

        /// <summary>
        /// Возвращает транспорт игрока
        /// </summary>
        public List<VehicleInfo> GetPlayerVehicles(Client player) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            return playerInfo.Vehicles.Values.ToList();
        }

        /// <summary>
        /// Возвращает транспорт игрока
        /// </summary>
        public List<VehicleInfo> GetPlayerVehicles(long accountId) {
            var vehicles = _vehiclesProvider.GetVehicles(accountId);
            return vehicles.ToList();
        }

        /// <summary>
        /// Записывает информацию о транспорте игрока
        /// </summary>
        public void SetInfo(Client player, VehicleInfo vehicleInfo) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (playerInfo.Vehicles.ContainsKey(vehicleInfo.Id)) {
                playerInfo.Vehicles[vehicleInfo.Id] = vehicleInfo;
            }
            else {
                playerInfo.Vehicles.Add(vehicleInfo.Id, vehicleInfo);
            }
        }

        /// <summary>
        /// Записывает информацию о транспорте игрока
        /// </summary>
        public void SetInfo(VehicleInfo vehicleInfo) {
            var player = API.getAllPlayers().FirstOrDefault(e => e != null && vehicleInfo.OwnerId == (long) e.getData(PlayerInfoManager.ID_KEY));
            if (player != null) {
               SetInfo(player, vehicleInfo);
            }
            else {
                _vehiclesProvider.Update(new List<VehicleInfo> { vehicleInfo });
            }
        }

        /// <summary>
        /// Удалить транспорт игрока
        /// </summary>
        public void RemoveVehicle(Client player, long vehicleId) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            var vehicle = playerInfo.Vehicles[vehicleId].Instance;
            if (vehicle != null && vehicle.exists) {
                API.deleteEntity(vehicle);
            }
            playerInfo.Vehicles.Remove(vehicleId);
            _vehiclesProvider.Remove(vehicleId);
        }

        /// <summary>
        /// Припарковать весь транспорт
        /// </summary>
        public void ParkAllVehicles() {
            _vehiclesProvider.ParkAll();
        }
    }
}