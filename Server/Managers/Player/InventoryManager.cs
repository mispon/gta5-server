using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_data.Entity;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.Player.Helpers;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles;
using gta_mp_server.Managers.Vehicles.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using Newtonsoft.Json;
using PlayerInfo = gta_mp_database.Models.Player.PlayerInfo;

namespace gta_mp_server.Managers.Player {
    /// <summary>
    /// Логика работы с инвентарем
    /// </summary>
    internal class InventoryManager : Script, IInventoryManager {
        private const string SKIN_RESET_ACTION = "ResetPlayerSkin";

        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IVehicleManager _vehicleManager;

        public InventoryManager() {}
        public InventoryManager(IPlayerInfoManager playerInfoManager, IVehicleManager vehicleManager) {
            _playerInfoManager = playerInfoManager;
            _vehicleManager = vehicleManager;
        }

        /// <summary>
        /// Проинициализировать инвентарь
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.SHOW_INVENTORY, ShowInventory);
            ClientEventHandler.Add(ClientEvent.USE_INVENTORY_ITEM, UseInventoryItem);
        }

        /// <summary>
        /// Одеть оружие на игрока
        /// </summary>
        public void EquipWeapon(Client player) {
            API.removeAllPlayerWeapons(player);
            var playerInfo = _playerInfoManager.GetInfo(player);
            var weapons = playerInfo.Inventory.Where(e => e.Type == InventoryType.Weapon && e.Count > 0);
            foreach (var weapon in weapons) {
                var ammo = GetAmmo(playerInfo, weapon.Model)?.Count ?? 0;
                API.givePlayerWeapon(player, (WeaponHash) weapon.Model, ammo, false);
            }
        }

        /// <summary>
        /// Обновить патроны игрока
        /// </summary>
        public void RefreshAmmo(Client player, PlayerInfo playerInfo = null) {
            if (playerInfo == null) {
                playerInfo = _playerInfoManager.GetInfo(player);
            }
            var weapons = API.getPlayerWeapons(player);
            foreach (var weapon in weapons) {
                var ammoCount = API.getPlayerWeaponAmmo(player, weapon);
                var ammoType = API.getWeaponAmmoType(weapon);
                var ammoItem = playerInfo.Inventory.FirstOrDefault(e => e.Type == InventoryType.Ammo && e.Model == (int) ammoType);
                if (ammoItem != null) {
                    ammoItem.Count = ammoCount;
                }
            }
        }

        /// <summary>
        /// Возвращает количество патронов
        /// </summary>
        internal static InventoryItem GetAmmo(PlayerInfo playerInfo, int weaponHash) {
            var ammoType = (int) API.shared.getWeaponAmmoType((WeaponHash) weaponHash);
            return playerInfo.Inventory.FirstOrDefault(e => e.Type == InventoryType.Ammo && e.Model == ammoType);
        }

        /// <summary>
        /// Открывает инвентарь игрока
        /// </summary>
        private void ShowInventory(Client player, object[] args) {
            var inventory = GetInventory(player);
            var weight = InventoryHelper.CalculateWeight(inventory);
            API.triggerClientEvent(player, ServerEvent.SHOW_INVENTORY, JsonConvert.SerializeObject(inventory), (int) weight);
        }

        /// <summary>
        /// Использовать предмет из инвентаря
        /// </summary>
        private void UseInventoryItem(Client player, object[] args) {
            var item = JsonConvert.DeserializeObject<InventoryItem>(args[0].ToString());
            var countToUse = (int) args[1];
            var playerInfo = _playerInfoManager.GetInfo(player);
            switch (item.Type) {
                case InventoryType.Medicine:
                    UseMedicine(player, playerInfo, countToUse);
                    break;
                case InventoryType.Food:
                    UsePackedLunch(player, playerInfo, countToUse);
                    break;
                case InventoryType.Canister:
                    FillVehicle(player, playerInfo, countToUse);
                    break;
                case InventoryType.TempoSkin:
                    SetRandomSkin(player, playerInfo);
                    break;
                default:
                    API.sendNotificationToPlayer(player, $"~r~Нельзя применить \"{item.Name}\"");
                    break;
            }
            _playerInfoManager.RefreshUI(player, playerInfo);
            API.triggerClientEvent(player, ServerEvent.SHOW_INVENTORY, JsonConvert.SerializeObject(GetInventory(player)));
        }

        /// <summary>
        /// Использовать аптечку
        /// </summary>
        private void UseMedicine(Client player, PlayerInfo playerInfo, int count) {
            const int healCount = 30;
            var health = player.health + healCount * count;
            API.setPlayerHealth(player, health);
            playerInfo.Health = health;
            playerInfo.Inventory.First(e => e.Type == InventoryType.Medicine).DecreaseCount(count);
        }

        /// <summary>
        /// Использовать сухой паёк
        /// </summary>
        private static void UsePackedLunch(Client player, PlayerInfo playerInfo, int count) {
            const int satietyCount = 25;
            var newSatiety = playerInfo.Satiety + satietyCount * count;
            playerInfo.Satiety = newSatiety > PlayerInfo.MAX_VALUE ? PlayerInfo.MAX_VALUE : newSatiety;
            playerInfo.Inventory.First(e => e.Type == InventoryType.Food).DecreaseCount(count);
            PlayerHelper.PlayEatAnimation(player);
        }

        /// <summary>
        /// Заправить транспорт из канистры
        /// </summary>
        private void FillVehicle(Client player, PlayerInfo playerInfo, int liters) {
            if (player.isInVehicle) {
                API.sendNotificationToPlayer(player, "~r~Чтобы заправить транспорт, необходимо выйти из нее", true);
                return;
            }
            var vehicle = _vehicleManager.GetNearestVehicle(player, 3.5f);
            if (vehicle == null) {
                API.sendNotificationToPlayer(player, "~r~Подойдите ближе к транспорту", true);
                return;
            }
            if (!vehicle.hasData(VehicleManager.MAX_FUEL)) {
                API.sendNotificationToPlayer(player, "~r~Можно заправить только личный транспорт", true);
                return;
            }
            var maxFuel = (int) vehicle.getData(VehicleManager.MAX_FUEL);
            var newFuel = API.getVehicleFuelLevel(vehicle) + liters;
            if (maxFuel < newFuel) {
                API.sendNotificationToPlayer(player, "~r~Количество бензина превышает вместимость бака", true);
                return;
            }
            API.setVehicleFuelLevel(vehicle, newFuel);
            API.sendNotificationToPlayer(player, $"~b~Залито {liters} литров");
            playerInfo.Inventory.First(e => e.Type == InventoryType.Canister).DecreaseCount(liters);
        }

        /// <summary>
        /// Применить рандомный скин
        /// </summary>
        private void SetRandomSkin(Client player, PlayerInfo playerInfo) {
            ActionHelper.CancelAction(player, SKIN_RESET_ACTION);
            var skins = Enum.GetValues(typeof(TemporarySkin));
            var hash = (PedHash) skins.GetValue(ActionHelper.Random.Next(skins.Length));
            API.setPlayerSkin(player, hash);
            ActionHelper.SetAction(player, 3600000, () => _playerInfoManager.SetPlayerClothes(player, true), SKIN_RESET_ACTION);
            playerInfo.Inventory.First(e => e.Type == InventoryType.TempoSkin).DecreaseCount();
            EquipWeapon(player);
            API.sendColoredNotificationToPlayer(player, "Действие скина закончится через час", 0, 21);
        }

        /// <summary>
        /// Возвращает инвентарь игрока для отображения
        /// </summary>
        private IEnumerable<InventoryItem> GetInventory(Client player) {
            return _playerInfoManager.GetInfo(player).Inventory.Where(e => e.Count > 0);
        }
    }
}