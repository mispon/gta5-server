using System;
using System.Linq;
using gta_mp_data.Entity;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;

namespace gta_mp_server.Managers.Vehicles {
    /// <summary>
    /// Логика работы с багажником
    /// </summary>
    internal class VehicleTrunkManager : Script, IVehicleTrunkManager {
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IVehicleInfoManager _vehicleInfoManager;
        private readonly IInventoryManager _inventoryManager;
        private readonly IInventoryHelper _inventoryHelper;

        public VehicleTrunkManager() {}
        public VehicleTrunkManager(IPlayerInfoManager playerInfoManager, IVehicleInfoManager vehicleInfoManager,
            IInventoryManager inventoryManager, IInventoryHelper inventoryHelper) {
            _playerInfoManager = playerInfoManager;
            _vehicleInfoManager = vehicleInfoManager;
            _inventoryManager = inventoryManager;
            _inventoryHelper = inventoryHelper;
        }

        /// <summary>
        /// Положить предмет в багажник
        /// </summary>
        public bool PutInTrunk(Client player, Vehicle vehicle, InventoryItem item, int count) {
            if (!EnoughItems(player, item, count)) {
                return false;
            }
            var vehicleCarring = VehicleManager.GetCarrying((VehicleHash) vehicle.model);
            var vehicleInfo = _vehicleInfoManager.GetInfoByHandle(player, vehicle);
            var trunk = vehicleInfo.GetTrunk();
            if (!_inventoryHelper.CanCarry(trunk, item, count, vehicleCarring)) {
                API.sendColoredNotificationToPlayer(player, "Превышена грузоподъемность транспорта", 0, 6);
                return false;
            }
            var playerInfo = _playerInfoManager.GetInfo(player);
            playerInfo.Inventory.First(ItemPredicate(item)).DecreaseCount(count);
            var itemInTrunk = trunk.FirstOrDefault(ItemPredicate(item));
            if (itemInTrunk == null) {
                item.Count = count;
                trunk.Add(item);
            }
            else {
                itemInTrunk.Count += count;
            }
            vehicleInfo.SetTrunk(trunk);
            _vehicleInfoManager.SetInfo(player, vehicleInfo);
            _playerInfoManager.RefreshUI(player, playerInfo);
            _inventoryManager.EquipWeapon(player);
            return true;
        }

        /// <summary>
        /// Забрать предмет из багажника
        /// </summary>
        public bool TakeFromTrunk(Client player, Vehicle vehicle, InventoryItem item, int count) {
            if (!EnoughItems(player, item, count)) {
                return false;
            }
            var vehicleInfo = _vehicleInfoManager.GetInfoByHandle(player, vehicle);
            var playerInfo = _playerInfoManager.GetInfo(player);
            var trunk = vehicleInfo.GetTrunk();
            if (!_inventoryHelper.CanCarry(playerInfo.Inventory.Where(e => e.Count > 0), item, count)) {
                API.sendColoredNotificationToPlayer(player, "Превышена грузоподъемность персонажа", 0, 6);
                return false;
            }
            var itemInTrunk = trunk.First(ItemPredicate(item));
            if (itemInTrunk.Count - count > 0) {
                itemInTrunk.Count -= count;
            }
            else {
                trunk = trunk.Where(e => e.Id != item.Id).ToList();
            }
            playerInfo.Inventory.First(ItemPredicate(item)).Count += count;
            vehicleInfo.SetTrunk(trunk);
            _vehicleInfoManager.SetInfo(player, vehicleInfo);
            _playerInfoManager.RefreshUI(player, playerInfo);
            _inventoryManager.EquipWeapon(player);
            return true;
        }

        /// <summary>
        /// Возвращает предикат для выбранного предмета инвентаря
        /// </summary>
        private static Func<InventoryItem, bool> ItemPredicate(InventoryItem item) {
            return e => e.Type == item.Type && e.Model == item.Model;
        }

        /// <summary>
        /// Проверяет, достаточное ли количество предметов
        /// </summary>
        private bool EnoughItems(Client player, InventoryItem item, int count) {
            if (item.Count < count) {
                API.sendColoredNotificationToPlayer(player, "У вас нет столько предметов", 0, 6);
                return false;
            }
            return true;
        }
    }
}