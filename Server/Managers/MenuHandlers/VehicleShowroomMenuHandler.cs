using System.Collections.Generic;
using System.Linq;
using gta_mp_database.Entity;
using gta_mp_database.Providers.Interfaces;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.House.Interfaces;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Models.Shops;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using Newtonsoft.Json;
using Vehicle = gta_mp_database.Entity.Vehicle;
using PlayerHouse = gta_mp_database.Entity.House;

namespace gta_mp_server.Managers.MenuHandlers {
    internal class VehicleShowroomMenuHandler : Script, IMenu {
        internal const string IN_SHOWROOM_PREVIEW = "InShowroomPreview";

        private readonly IVehiclesProvider _vehiclesProvider;
        private readonly IVehicleInfoManager _vehicleInfoManager;
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IHouseManager _houseManager;
        private readonly IClanManager _clanManager;

        public VehicleShowroomMenuHandler() {}
        public VehicleShowroomMenuHandler(IVehiclesProvider vehiclesProvider, IVehicleInfoManager vehicleInfoManager,
            IPlayerInfoManager playerInfoManager, IHouseManager houseManager, IClanManager clanManager) {
            _vehiclesProvider = vehiclesProvider;
            _vehicleInfoManager = vehicleInfoManager;
            _playerInfoManager = playerInfoManager;
            _houseManager = houseManager;
            _clanManager = clanManager;
        }

        /// <summary>
        /// Инициализировать обработчик меню
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.ENTER_TO_VEHICLE_PREVIEW, EnterToPreview);
            ClientEventHandler.Add(ClientEvent.EXIT_FROM_VEHICLE_PREVIEW, ExitFromPreview);
            ClientEventHandler.Add(ClientEvent.BUY_VEHICLE, BuyVehicle);
            ClientEventHandler.Add(ClientEvent.SELL_VEHICLE, SellVehicle);
        }

        /// <summary>
        /// Перенести игрока в просмотр тс
        /// </summary>
        private void EnterToPreview(Client player, object[] args) {
            player.setSyncedData(IN_SHOWROOM_PREVIEW, true);
            player.freeze(true);
            var accountId = _playerInfoManager.GetInfo(player).AccountId;
            API.setEntityDimension(player, (int) -accountId);
            API.triggerClientEvent(player, ServerEvent.SHOW_HINT, "Если случайно закрылось меню, нажмите О, чтобы снова открыть", 120);
        }

        /// <summary>
        /// Перенести игрока из просмотра тс
        /// </summary>
        private void ExitFromPreview(Client player, object[] args) {
            API.setEntityDimension(player, 0);
            player.freeze(false);
            player.resetSyncedData(IN_SHOWROOM_PREVIEW);
            API.triggerClientEvent(player, ServerEvent.HIDE_HINT);
        }

        /// <summary>
        /// Обработка покупки тс
        /// </summary>
        private void BuyVehicle(Client player, object[] args) {
            var vehicleInfo = JsonConvert.DeserializeObject<ShowroomVehicle>(args[0].ToString());
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (playerInfo.Balance < vehicleInfo.Price) {
                API.sendNotificationToPlayer(player, "~r~Недостаточно средств!", true);
                return;
            }
            playerInfo.Balance -= vehicleInfo.Price;
            var primaryColor = (int) args[1];
            var secondColor = (int) args[2];
            var toHouse = (bool) args[3];
            var vehicle = CreateVehicle(vehicleInfo, playerInfo.AccountId, primaryColor, secondColor);
            var targetMessage = toHouse && SetVehicleInHouse(player, vehicle, playerInfo.AccountId) ? "в гараж" : "на главную парковку";
            API.sendNotificationToPlayer(player, $"~g~Машина приобретена и отправлена {targetMessage}");
            _vehiclesProvider.Add(vehicle);
            _vehicleInfoManager.SetInfo(player, vehicle);
            var district = (int) args[4];
            if (district != Validator.INVALID_ID) {
                _clanManager.ReplenishClanBalance(district, vehicleInfo.Price);
            }
            _playerInfoManager.RefreshUI(player, playerInfo);
            API.triggerClientEvent(player, ServerEvent.HIDE_SHOWROOM_MENU);
        }

        /// <summary>
        /// Поместить машину в свободный гараж
        /// </summary>
        private bool SetVehicleInHouse(Client player, Vehicle vehicle, long accountId) {
            var houses = _houseManager.GetPlayerHouses(accountId);
            if (!houses.Any()) {
                API.sendNotificationToPlayer(player, "~r~У вас не арендовано ни одного дома", true);
                return false;
            }
            var house = GetFreeHouse(player, houses);
            if (house == null) {
                API.sendNotificationToPlayer(player, "~r~Нет свободных мест в гараже", true);
                return false;
            }
            vehicle.HouseId = house.Id;
            _houseManager.SetHouse(house);
            return true;
        }

        /// <summary>
        /// Возвращает дом со свободным гаражем
        /// </summary>
        private PlayerHouse GetFreeHouse(Client player, IEnumerable<PlayerHouse> houses) {
            var vehicles = _playerInfoManager.GetInfo(player).Vehicles.Values;
            foreach (var house in houses) {
                var vehiclesInGarage = vehicles.Count(e => e.HouseId == house.Id);
                if (!HouseHelper.GarageIsFull(house.Type, vehiclesInGarage)) {
                    return house;
                }
            }
            return null;
        }

        /// <summary>
        /// Продать транспорт
        /// </summary>
        private void SellVehicle(Client player, object[] args) {
            var vehicleId = (int) args[0];
            var price = (int) args[1];
            _playerInfoManager.SetBalance(player, price, true);
            _vehicleInfoManager.RemoveVehicle(player, vehicleId);
            API.sendNotificationToPlayer(player, "~b~Транспорт успешно продан");
        }

        /// <summary>
        /// Создает купленный транспорт
        /// </summary>
        private static Vehicle CreateVehicle(ShowroomVehicle vehicleInfo, long accountId, int primaryColor, int secondColor) {
            return new Vehicle {
                OwnerId = accountId,
                Hash = vehicleInfo.Hash,
                Fuel = vehicleInfo.MaxFuel,
                HouseId = Validator.INVALID_ID,
                MaxFuel = vehicleInfo.MaxFuel,
                OnParkingFine = false,
                Tuning = new VehicleTuning {
                    PrimaryColor = primaryColor,
                    SecondColor = secondColor
                }
            };
        }
    }
}