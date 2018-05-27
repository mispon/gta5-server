using System;
using System.Linq;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.House;
using gta_mp_server.Managers.House.Interfaces;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using PlayerInfo = gta_mp_database.Models.Player.PlayerInfo;
using Vehicle = gta_mp_database.Entity.Vehicle;
using HouseInfo = gta_mp_database.Entity.House;

namespace gta_mp_server.Managers.MenuHandlers {
    internal class HouseMenuHandler : Script, IMenu {
        internal const string IN_HOUSE = "PlayerInHouse";

        private readonly IHouseManager _houseManager;
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IVehicleInfoManager _vehicleInfoManager;
        private readonly IVehicleManager _vehicleManager;

        public HouseMenuHandler() {}
        public HouseMenuHandler(IHouseManager houseManager, IPlayerInfoManager playerInfoManager,
            IVehicleInfoManager vehicleInfoManager, IVehicleManager vehicleManager) {
            _houseManager = houseManager;
            _playerInfoManager = playerInfoManager;
            _vehicleInfoManager = vehicleInfoManager;
            _vehicleManager = vehicleManager;
        }

        /// <summary>
        /// Инициализировать меню
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.GET_HOUSE_RENT, GetHouseRent);
            ClientEventHandler.Add(ClientEvent.ENTER_HOUSE, EnterHouse);
            ClientEventHandler.Add(ClientEvent.EXIT_HOUSE, ExitHouse);
            ClientEventHandler.Add(ClientEvent.LOCK_HOUSE, LockHouse);
            ClientEventHandler.Add(ClientEvent.ENTER_GARAGE, EnterGarage);
            ClientEventHandler.Add(ClientEvent.EXIT_GARAGE, ExitGarage);
        }

        /// <summary>
        /// Снять дом в аренду
        /// </summary>
        private void GetHouseRent(Client player, object[] args) {
            var house = GetHouse(args[0]);
            var daysRent = (int) args[1];
            var rentCost = daysRent * house.DailyRent;
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (rentCost > playerInfo.Balance) {
                API.sendNotificationToPlayer(player, "~r~У вас не хватает денег на аренду", true);
                return;
            }
            playerInfo.Balance -= rentCost;
            _playerInfoManager.RefreshUI(player, playerInfo);
            SetRent(player, house, playerInfo, daysRent);
            API.triggerClientEvent(player, ServerEvent.SHOW_HOUSE_MENU, (int) HouseState.OwnerEnter, JsonConvert.SerializeObject(house));
        }

        /// <summary>
        /// Установить аренду дома
        /// </summary>
        private void SetRent(Client player, HouseInfo house, PlayerInfo playerInfo, int daysRent) {
            var endOfRenting = DateTime.Now.AddDays(daysRent);
            var isRentExt = Validator.IsValid(house.EndOfRenting);
            house.EndOfRenting = isRentExt ? house.EndOfRenting.AddDays(daysRent) : endOfRenting;
            house.OwnerId = playerInfo.AccountId;
            _houseManager.SetHouse(house);
            _houseManager.UpdateBlip(house, playerInfo.Name);
            var actionName = isRentExt ? "продлена" : "оплачена";
            API.sendNotificationToPlayer(player, $"~g~Аренда {actionName} до {house.EndOfRenting:g}");
        }

        /// <summary>
        /// Войти в дом
        /// </summary>
        private void EnterHouse(Client player, object[] args) {
            var house = GetHouse(args[0]);
            if (IsLocked(player, house)) {
                return;
            }
            var hallway = HousesPositionsGetter.InnerPositions[house.Type].Hallway;
            SetPlayerPosition(player, hallway, (int) house.Id);
            player.setData(IN_HOUSE, house.Id);
            API.sendNativeToPlayer(player, Hash.DISPLAY_RADAR, false);
            API.triggerClientEvent(player, ServerEvent.HIDE_HOUSE_MENU);
        }

        /// <summary>
        /// Выйти из дома
        /// </summary>
        private void ExitHouse(Client player, object[] args) {
            var house = GetHouse(args[0]);
            if (IsLocked(player, house)) {
                return;
            }
            var position = PositionConverter.ToVector3(house.Position);
            SetPlayerPosition(player, position, 0);
            player.resetData(IN_HOUSE);
            API.sendNativeToPlayer(player, Hash.DISPLAY_RADAR, true);
            API.triggerClientEvent(player, ServerEvent.HIDE_HOUSE_MENU);
        }

        /// <summary>
        /// Переместить игрока в / из дома
        /// </summary>
        private void SetPlayerPosition(Client player, Vector3 position, int dimension) {
            API.setEntityPosition(player, position);
            _playerInfoManager.SetDimension(player, -dimension);
        }

        /// <summary>
        /// Проверка, что дверь заперта
        /// </summary>
        private bool IsLocked(Client player, HouseInfo house) {
            if (house.Lock) {
                API.sendNotificationToPlayer(player, "~r~Дверь заперта");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Отпереть / запереть дом
        /// </summary>
        private void LockHouse(Client player, object[] args) {
            var house = GetHouse(args[0]);
            house.Lock = !house.Lock;
            _houseManager.SetHouse(house);
            var state = player.getData(IN_HOUSE) == null ? HouseState.OwnerEnter : HouseState.Exit;
            API.triggerClientEvent(player, ServerEvent.SHOW_HOUSE_MENU, (int) state, JsonConvert.SerializeObject(house));
        }

        /// <summary>
        /// Войти в гараж
        /// </summary>
        private void EnterGarage(Client player, object[] args) {
            var house = GetHouse(args[0]);
            if (player.isInVehicle) {
                var vehicle = API.getPlayerVehicle(player);
                var vehicleInfo = _vehicleInfoManager.GetInfoByHandle(player, vehicle);
                if (!CanParking(player, house, vehicleInfo)) {
                    return;
                }
                EnterGarage(player, house);
                ParkVehicle(player, house, vehicleInfo);
            }
            else {
                EnterGarage(player, house);
            }
        }

        /// <summary>
        /// Может ли игрок парковаться в гараже
        /// </summary>
        private bool CanParking(Client player, HouseInfo house, Vehicle vehicleInfo) {
            var vehiclesInGarage = _playerInfoManager.GetInfo(player).Vehicles.Values.Count(e => e.HouseId == house.Id);
            if (HouseHelper.GarageIsFull(house.Type, vehiclesInGarage)) {
                API.sendNotificationToPlayer(player, "~r~В гараже больше нет мест", true);
                return false;
            }
            if (vehicleInfo == null) {
                API.sendNotificationToPlayer(player, "~r~Нельзя парковать чужой транспорт в гараж", true);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Войти в гараж
        /// </summary>
        private void EnterGarage(Client player, HouseInfo house) {
            LoadVehicles(player, house);
            var garage = HousesPositionsGetter.GetGarageInnerPositions(house.Type);
            SetGaragePosition(player, garage.AfterEnter, garage.EnterRotation, (int) house.Id);
            API.sendNativeToPlayer(player, Hash.DISPLAY_RADAR, false);
            API.triggerClientEvent(player, ServerEvent.HIDE_HOUSE_MENU);
        }

        /// <summary>
        /// Выйти из гаража
        /// </summary>
        private void ExitGarage(Client player, object[] args) {
            var house = GetHouse(args[0]);
            if (player.isInVehicle) {
                UnparkVehicle(player, house);
            }
            var garageExit = PositionConverter.ToVector3(house.GaragePosition);
            var rotationAfterExit = PositionConverter.ToVector3(house.RotationAfterExit);
            SetGaragePosition(player, garageExit, rotationAfterExit, 0);
            RemoveVehicles(player, house);
            API.sendNativeToPlayer(player, Hash.DISPLAY_RADAR, true);
            API.triggerClientEvent(player, ServerEvent.HIDE_HOUSE_MENU);
        }

        /// <summary>
        /// Припарковать транспорт
        /// </summary>
        private void ParkVehicle(Client player, HouseInfo house, Vehicle vehicleInfo) {
            vehicleInfo.HouseId = house.Id;
            _houseManager.SetHouse(house);
            _vehicleInfoManager.SetInfo(player, vehicleInfo);
        }

        /// <summary>
        /// Удаляет транспорт из гаража
        /// </summary>
        private void UnparkVehicle(Client player, HouseInfo house) {
            var vehicle = API.getPlayerVehicle(player);
            var vehicleInfo = _vehicleInfoManager.GetInfoByHandle(player, vehicle);
            vehicleInfo.HouseId = Validator.INVALID_ID;
            _houseManager.SetHouse(house);
            _vehicleInfoManager.SetInfo(player, vehicleInfo);
        }

        /// <summary>
        /// Переместить игрока в гараж с машиной или без
        /// </summary>
        private void SetGaragePosition(Client player, Vector3 position, Vector3 rotation, int dimention) {
            _playerInfoManager.SetDimension(player, -dimention);
            API.setEntityPosition(player, position);
            if (player.isInVehicle) {
                var vehicle = API.getPlayerVehicle(player);
                API.setEntityDimension(vehicle, -dimention);
                API.setEntityPosition(vehicle, position);
                API.setEntityRotation(vehicle, rotation);
                player.setIntoVehicle(vehicle, -1);
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_HOUSE_MENU);
        }

        /// <summary>
        /// Загрузить машины в гараж
        /// </summary>
        private void LoadVehicles(Client player, HouseInfo house) {
            var vehicles = _vehicleInfoManager.GetPlayerVehicles(player);
            var parkingPositions = HousesPositionsGetter.GetGarageInnerPositions(house.Type).Positions;
            var positionIndex = 0;
            foreach (var vehicle in vehicles) {
                if (vehicle.HouseId != house.Id) {
                    continue;
                }
                var position = parkingPositions[positionIndex];
                vehicle.IsSpawned = true;
                _vehicleManager.CreateVehicle(vehicle, position.Item1, position.Item2, (int) -house.Id);
                _vehicleInfoManager.SetInfo(player, vehicle);
                positionIndex++;
            }
        }

        /// <summary>
        /// Удалить транспорт из гаража при выходе игрока
        /// </summary>
        private void RemoveVehicles(Client player, HouseInfo house) {
            var vehicles = _vehicleInfoManager.GetPlayerVehicles(player);
            foreach (var vehicle in vehicles) {
                if (vehicle.HouseId != house.Id || vehicle.Instance == null) {
                    continue;
                }
                vehicle.IsSpawned = false;
                vehicle.Instance.delete();
                _vehicleInfoManager.SetInfo(player, vehicle);
            }
        }

        /// <summary>
        /// Десериализовать данные дома
        /// </summary>
        private static HouseInfo GetHouse(object arg) {
            return JsonConvert.DeserializeObject<HouseInfo>((string) arg);
        }
    }
}