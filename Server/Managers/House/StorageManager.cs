using System;
using System.Linq;
using gta_mp_data.Entity;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Managers.House.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using Newtonsoft.Json;
using PlayerInfo = gta_mp_database.Models.Player.PlayerInfo;

namespace gta_mp_server.Managers.House {
    /// <summary>
    /// Логика работы с гардеробом
    /// </summary>
    internal class StorageManager : Script, IStorageManager {
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IInventoryManager _inventoryManager;
        private readonly IInventoryHelper _inventoryHelper;

        public StorageManager() {}
        public StorageManager(IPlayerInfoManager playerInfoManager, IInventoryManager inventoryManager, IInventoryHelper inventoryHelper) {
            _playerInfoManager = playerInfoManager;
            _inventoryManager = inventoryManager;
            _inventoryHelper = inventoryHelper;
        }

        /// <summary>
        /// Проинициализировать обработчик гардероба
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.PUT_ITEM_TO_STORAGE, (player, args) => ProcessStorage(player, args, PutItem));
            ClientEventHandler.Add(ClientEvent.TAKE_ITEM_TO_STORAGE, (player, args) => ProcessStorage(player, args, TakeItem));
        }

        /// <summary>
        /// Выполняет действие с хранилищем
        /// </summary>
        private void ProcessStorage(Client player, object[] args, Action<Client, PlayerInfo, long, int> action) {
            var item = JsonConvert.DeserializeObject<InventoryItem>(args[0].ToString());
            var count = (int) args[1];
            var playerInfo = _playerInfoManager.GetInfo(player);
            action(player, playerInfo, item.Id, count);
            if (item.Type == InventoryType.Weapon || item.Type == InventoryType.Ammo) {
                _inventoryManager.EquipWeapon(player);
            }
            _playerInfoManager.RefreshUI(player, playerInfo);
            API.sendNotificationToPlayer(player, "~b~Предметы успешно перемещены");
            API.triggerClientEvent(player, ServerEvent.SHOW_HOUSE_STORAGE_MENU, JsonConvert.SerializeObject(playerInfo.Inventory));
        }

        /// <summary>
        /// Положить в хранилище
        /// </summary>
        private static void PutItem(Client player, PlayerInfo playerInfo, long id, int count) {
            var item = playerInfo.Inventory.First(e => e.Id == id);
            if (item.Count < count) {
                API.shared.sendNotificationToPlayer(player, "~r~Попытка переместить неверное количество");
                return;
            }
            item.Count -= count;
            item.CountInHouse += count;
        }

        /// <summary>
        /// Забрать из хранилища
        /// </summary>
        private void TakeItem(Client player, PlayerInfo playerInfo, long id, int count) {
            var item = playerInfo.Inventory.First(e => e.Id == id);
            if (!_inventoryHelper.CanCarry(playerInfo.Inventory, item, count)) {
                API.sendColoredNotificationToPlayer(player, "Превышена грузоподъемность персонажа", 0, 6);
                return;
            }
            if (item.CountInHouse < count) {
                API.sendNotificationToPlayer(player, "~r~Попытка переместить неверное количество");
                return;
            }
            item.CountInHouse -= count;
            item.Count += count;
        }
    }
}