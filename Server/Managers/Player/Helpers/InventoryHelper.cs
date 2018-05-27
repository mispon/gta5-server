using System;
using System.Collections.Generic;
using gta_mp_data.Entity;
using gta_mp_data.Enums;
using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Shared;

namespace gta_mp_server.Managers.Player.Helpers {
    /// <summary>
    /// Вспомогательная логика инвентаря
    /// </summary>
    internal class InventoryHelper : IInventoryHelper {
        internal const int MAX_PLAYER_CARRYING = 100;

        /// <summary>
        /// Коэффициенты веса предметов по типу
        /// </summary>
        private static readonly Dictionary<InventoryType, float> _itemsWeight = new Dictionary<InventoryType, float> {
            [InventoryType.Money] = 0,
            [InventoryType.Food] = 1.5f,
            [InventoryType.WaterBottle] = 0.8f,
            [InventoryType.Medicine] = 3,
            [InventoryType.Canister] = 0.2f,
            [InventoryType.Marijuana] = 0.1f,
            [InventoryType.TempoSkin] = 0,
            [InventoryType.WeaponLicense] = 0,
            [InventoryType.DriverLicenceB] = 0,
            [InventoryType.DriverLicenceC] = 0
        };

        /// <summary>
        /// Вес оружия
        /// </summary>
        private static readonly Dictionary<WeaponType, float> _weaponsWeight = new Dictionary<WeaponType, float> {
            [WeaponType.Melee] = 3,
            [WeaponType.Handguns] = 5,
            [WeaponType.MachineGuns] = 7,
            [WeaponType.Shotguns] = 12,
            [WeaponType.AssaultRifles] = 10,
            [WeaponType.SniperRifles] = 15
        };

        /// <summary>
        /// Вес патронов
        /// </summary>
        private static readonly Dictionary<WeaponAmmoType, float> _ammosWeight = new Dictionary<WeaponAmmoType, float> {
            [WeaponAmmoType.Melee] = 0f,
            [WeaponAmmoType.Handguns] = 0.08f,
            [WeaponAmmoType.MachineGuns] = 0.1f,
            [WeaponAmmoType.Shotguns] = 0.25f,
            [WeaponAmmoType.AssaultRifles] = 0.2f,
            [WeaponAmmoType.SniperRifles] = 0.3f
        };

        /// <summary>
        /// Проверяет, что игрок / тс может столько унести / увезти
        /// </summary>
        public bool CanCarry(IEnumerable<InventoryItem> inventory, InventoryItem newItem, int count, int carrying = MAX_PLAYER_CARRYING) {
            var additionalWeight = (IsWeapon(newItem) ? GetWeaponWeight(newItem) : _itemsWeight[newItem.Type]) * count;
            if (Math.Abs(additionalWeight) < 0.1) {
                return true;
            }
            var currentWeight = CalculateWeight(inventory);
            return additionalWeight + currentWeight <= carrying;
        }

        /// <summary>
        /// Вычисляет текущий вес инвентаря
        /// </summary>
        internal static float CalculateWeight(IEnumerable<InventoryItem> inventory) {
            var result = 0f;
            foreach (var item in inventory) {
                var weight = IsWeapon(item) ? GetWeaponWeight(item) : _itemsWeight[item.Type];
                result += weight * item.Count;
            }
            return result;
        }

        /// <summary>
        /// Вычисляет вес оружия
        /// </summary>
        private static float GetWeaponWeight(InventoryItem item) {
            switch (item.Type) {
                case InventoryType.Weapon:
                    var weaponType = API.shared.getWeaponType((WeaponHash) item.Model);
                    return _weaponsWeight[weaponType];
                case InventoryType.Ammo:
                    var ammoType = (WeaponAmmoType) item.Model;
                    return _ammosWeight[ammoType];
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Проверяет, является ли предмет оружием
        /// </summary>
        private static bool IsWeapon(InventoryItem item) {
            return item.Type == InventoryType.Weapon || item.Type == InventoryType.Ammo;
        }
    }
}