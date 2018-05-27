using System.Linq;
using gta_mp_data.Entity;
using gta_mp_data.Enums;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Places;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using PlayerInfo = gta_mp_database.Models.Player.PlayerInfo;
using Vehicle = GrandTheftMultiplayer.Server.Elements.Vehicle;

namespace gta_mp_server.Managers.MenuHandlers {
    /// <summary>
    /// Обработчик меню заправки
    /// </summary>
    internal class FillingMenuHandler : Script, IMenu {
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IClanManager _clanManager;

        public FillingMenuHandler() {}
        public FillingMenuHandler(IPlayerInfoManager playerInfoManager, IClanManager clanManager) {
            _playerInfoManager = playerInfoManager;
            _clanManager = clanManager;
        }

        /// <summary>
        /// Инициализировать обработчик
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.FILL_VEHICLE, FillVehicle);
            ClientEventHandler.Add(ClientEvent.FILL_CANISTER, FillCanister);
            ClientEventHandler.Add(ClientEvent.BUY_CANISTER, BuyCanister);
        }

        /// <summary>
        /// Заправить машину игрока
        /// </summary>
        private void FillVehicle(Client player, object[] args) {
            var stationKey = string.Format(FillingStation.STATION_KEY, args[0]);
            var vehicle = PlayerHelper.GetData<Vehicle>(player, stationKey, null);
            if (vehicle == null) {
                API.sendColoredNotificationToPlayer(player, "Транспорт не находится рядом с бензоколонкой", 0, 6);
                return;
            }
            var currentFuel = API.getVehicleFuelLevel(vehicle);
            var maxFuel = (int) vehicle.getData(VehicleManager.MAX_FUEL);
            var fillingAmount = (int) args[1]; 
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (!CanFillVehicle(player, playerInfo, maxFuel, currentFuel, fillingAmount)) {
                return;
            }
            API.setVehicleFuelLevel(vehicle, currentFuel + fillingAmount);
            playerInfo.Balance -= fillingAmount;
            _playerInfoManager.RefreshUI(player, playerInfo);
            var district = (int) args[2];
            _clanManager.ReplenishClanBalance(district, fillingAmount);
            API.sendNotificationToPlayer(player, $"Приобретено ~b~{fillingAmount}л. ~w~бензина");
            API.triggerClientEvent(player, ServerEvent.HIDE_FILLING_MENU);
        }

        /// <summary>
        /// Проверяет корректность покупки
        /// </summary>
        private bool CanFillVehicle(Client player, PlayerInfo playerInfo, int maxFuel, float currentFuel, float fillingAmount) {
            if (!HasMoney(player, playerInfo.Balance, (int) fillingAmount)) {
                return false;
            }
            var newFuelCount = currentFuel + fillingAmount;
            if (newFuelCount > maxFuel) {
                API.sendColoredNotificationToPlayer(player, "Количество топлива превышает вместимость бака", 0, 6);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Обработчик заправки канистры
        /// </summary>
        private void FillCanister(Client player, object[] args) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            var canister = playerInfo.Inventory.FirstOrDefault(e => e.Type == InventoryType.Canister);
            if (canister == null) {
                API.sendColoredNotificationToPlayer(player, "У вас не приобретена канистра", 0, 6);
                return;
            }
            var liters = (int) args[0];
            if (!HasMoney(player, playerInfo.Balance, liters)) {
                return;
            }
            if (canister.Count + liters > 50) {
                API.sendColoredNotificationToPlayer(player, "Количество литров превышает вместимость канистры", 0, 6);
                return;
            }
            canister.Count += liters;
            _playerInfoManager.RefreshUI(player, playerInfo);
            var street = (int) args[1];
            _clanManager.ReplenishClanBalance(street, liters);
            API.sendNotificationToPlayer(player, $"Списано ~b~{liters}$");
            API.triggerClientEvent(player, ServerEvent.HIDE_FILLING_MENU);
        }

        /// <summary>
        /// Обработчик покупки канистры
        /// </summary>
        private void BuyCanister(Client player, object[] args) {
            var price = (int) args[0];
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (!HasMoney(player, playerInfo.Balance, price)) {
                return;
            }
            if (playerInfo.Inventory.Any(e => e.Type == InventoryType.Canister)) {
                API.sendColoredNotificationToPlayer(player, "У вас уже есть канистра", 0, 6);
                return;
            }
            var item = CreateCanister(playerInfo.AccountId);
            playerInfo.Inventory.Add(item);
            playerInfo.Balance -= price;
            _playerInfoManager.RefreshUI(player, playerInfo);
            var street = (int) args[1];
            _clanManager.ReplenishClanBalance(street, price);
            API.sendNotificationToPlayer(player, $"Списано ~b~{price}$");
        }

        /// <summary>
        /// Создает объект новой канистры
        /// </summary>
        private static InventoryItem CreateCanister(long accountId) {
            var item = new InventoryItem {
                OwnerId = accountId,
                Name = InventoryType.Canister.GetDescription(),
                Type = InventoryType.Canister,
                Count = 0,
                CountInHouse = 0,
                Model = -1
            };
            return item;
        }

        /// <summary>
        /// Проверяет, достаточно ли денег
        /// </summary>
        private bool HasMoney(Client player, int balance, int price) {
            if (balance < price) {
                API.sendColoredNotificationToPlayer(player, "У вас недостаточно средств", 0, 6);
                return false;
            }
            return true;
        }
    }
}