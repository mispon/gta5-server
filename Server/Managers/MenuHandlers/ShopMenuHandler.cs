using System.Linq;
using gta_mp_data.Entity;
using gta_mp_data.Enums;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using PlayerInfo = gta_mp_database.Models.Player.PlayerInfo;

namespace gta_mp_server.Managers.MenuHandlers {
    internal class ShopMenuHandler : Script, IMenu {
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IClanManager _clanManager;
        private readonly IInventoryHelper _inventoryHelper;

        public ShopMenuHandler() {}
        public ShopMenuHandler(IPlayerInfoManager playerInfoManager, IClanManager clanManager, IInventoryHelper inventoryHelper) {
            _playerInfoManager = playerInfoManager;
            _clanManager = clanManager;
            _inventoryHelper = inventoryHelper;
        }

        /// <summary>
        /// Инициализировать меню
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.BUY_FOOD, OnBuyFood);
            ClientEventHandler.Add(ClientEvent.BUY_THING, OnBuyThing);
            ClientEventHandler.Add(ClientEvent.REPLENISH_PHONE_BALANCE, ReplenishPhoneBalance);
            ClientEventHandler.Add(ClientEvent.BUY_PHONE, BuyPhone);
        }

        /// <summary>
        /// Обработчик покупки еды в магазине
        /// </summary>
        private void OnBuyFood(Client player, object[] args) {
            var price = (int) args[1];
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (!PlayerHelper.EnoughMoney(player, playerInfo, price)) {
                return;
            }
            playerInfo.Balance -= price;
            var satietyCount = (int) args[0];
            var newSatiety = playerInfo.Satiety + satietyCount;
            playerInfo.Satiety = newSatiety <= PlayerInfo.MAX_VALUE ? newSatiety : PlayerInfo.MAX_VALUE;
            _playerInfoManager.RefreshUI(player, playerInfo);
            PlayerHelper.PlayEatAnimation(player);
            var district = (int) args[2];
            _clanManager.ReplenishClanBalance(district, price);
            API.sendNotificationToPlayer(player, $"~b~Списано {price}$");
        }

        /// <summary>
        /// Обработчик покупки вещей
        /// </summary>
        private void OnBuyThing(Client player, object[] args) {
            var cost = (int) args[1];
            var count = (int) args[2];
            var price = cost * count;
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (!PlayerHelper.EnoughMoney(player, playerInfo, price)) {
                return;
            }
            var type = (InventoryType) args[0];
            if (!AddItemToInventory(playerInfo, type, count)) {
                API.sendColoredNotificationToPlayer(player, "Превышена грузоподъемность персонажа", 0, 6);
                return;
            }
            playerInfo.Balance -= price;
            _playerInfoManager.RefreshUI(player, playerInfo);
            var street = (int) args[3];
            _clanManager.ReplenishClanBalance(street, price);
            API.sendNotificationToPlayer(player, $"~b~Списано {price}$");
        }

        /// <summary>
        /// Добавляет покупку в инвентарь
        /// </summary>
        private bool AddItemToInventory(PlayerInfo playerInfo, InventoryType type, int count) {
            var item = playerInfo.Inventory.FirstOrDefault(e => e.Type == type) ?? CreateItem(playerInfo.AccountId, type, count);
            if (!_inventoryHelper.CanCarry(playerInfo.Inventory, item, count)) {
                return false;
            }
            if (playerInfo.Inventory.Any(e => e.Type == type)) {
                item.Count += count;
            }
            else {
                playerInfo.Inventory.Add(item);
            }
            return true;
        }

        /// <summary>
        /// Создает новый предмет в инвентаре
        /// </summary>
        private static InventoryItem CreateItem(long ownerId, InventoryType type, int count) {
            return new InventoryItem {
                OwnerId = ownerId,
                Name = type.GetDescription(),
                Type = type,
                Count = count,
                CountInHouse = 0,
                Model = (int) Validator.INVALID_ID
            };
        }

        /// <summary>
        /// Пополнение баланса телефона
        /// </summary>
        private void ReplenishPhoneBalance(Client player, object[] args) {
            var amount = (int) args[0];
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (!PlayerHelper.EnoughMoney(player, playerInfo, amount)) {
                return;
            }
            if (playerInfo.PhoneNumber == 0) {
                API.sendNotificationToPlayer(player, "~r~У вас нет телефона", true);
                return;
            }
            playerInfo.Balance -= amount;
            playerInfo.PhoneBalance += amount;
            API.sendNotificationToPlayer(player, $"~b~Баланс моб. телефона пополнен на {amount}$");
            _playerInfoManager.RefreshUI(player, playerInfo);
        }

        /// <summary>
        /// Покупка телефона
        /// </summary>
        private void BuyPhone(Client player, object[] args) {
            var price = (int) args[0];
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (!PlayerHelper.EnoughMoney(player, playerInfo, price)) {
                return;
            }
            if (playerInfo.PhoneNumber != 0) {
                API.sendNotificationToPlayer(player, "~r~У вас уже есть телефон", true);
                return;
            }
            playerInfo.Balance -= price;
            _playerInfoManager.SetPhoneNumber(player);
            player.setSyncedData("HasPhone", true);
            API.sendNotificationToPlayer(player, $"~b~Приобретен моб. телефон. Списано {price}$");
            _playerInfoManager.RefreshUI(player, playerInfo);
        }
    }
}