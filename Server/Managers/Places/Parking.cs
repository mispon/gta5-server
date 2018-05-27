using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles;
using gta_mp_server.Managers.Vehicles.Interfaces;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using VehicleInfo = gta_mp_database.Entity.Vehicle;

namespace gta_mp_server.Managers.Places {
    /// <summary>
    /// Парковка
    /// </summary>
    internal class Parking : Place {
        private const string ON_PARKING = "VehicleOnParking";

        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IVehicleManager _vehicleManager;
        private readonly IVehicleInfoManager _vehicleInfoManager;
        private readonly IClanManager _clanManager;

        public Parking() {}
        public Parking(IPlayerInfoManager playerInfoManager, IVehicleManager vehicleManager,
            IVehicleInfoManager vehicleInfoManager, IClanManager clanManager) {
            _playerInfoManager = playerInfoManager;
            _vehicleManager = vehicleManager;
            _vehicleInfoManager = vehicleInfoManager;
            _clanManager = clanManager;
        }

        /// <summary>
        /// Инизиализировать парковку
        /// </summary>
        public override void Initialize() {
            ClientEventHandler.Add(ClientEvent.GET_VEHICLE_FROM_PARKING, GetVehicleFromParking);
            ClientEventHandler.Add(ClientEvent.PARK_VEHICLE, ParkVehicle);

            var parking = API.createSphereColShape(new Vector3(-316.40, -758.54, 33.97), 50f);
            parking.onEntityEnterColShape += (shape, entity) => ParkingAction(entity, vehicle => vehicle.setData(ON_PARKING, true));
            parking.onEntityExitColShape += (shape, entity) => ParkingAction(entity, vehicle => vehicle.resetData(ON_PARKING));
        }

        /// <summary>
        /// Обработчик въезда / выезда
        /// </summary>
        private void ParkingAction(NetHandle entity, Action<Vehicle> action) {
            var vehicle = API.getEntityFromHandle<Vehicle>(entity);
            if (vehicle != null) {
                action(vehicle);
            }
        }

        /// <summary>
        /// Взять транспорт с парковки
        /// </summary>
        private void GetVehicleFromParking(Client player, object[] args) {
            var vehicleInfo = JsonConvert.DeserializeObject<VehicleInfo>(args[0].ToString());
            if (!CanSpawn(player, vehicleInfo)) {
                return;
            }
            var freePlace = GetFreePlace();
            if (freePlace == null) {
                API.sendNotificationToPlayer(player, "~r~В данный момент на парковке нет свободных мест", true);
                return;
            }
            vehicleInfo.IsSpawned = true;
            _vehicleManager.CreateVehicle(vehicleInfo, freePlace.Item1, freePlace.Item2);
            _vehicleInfoManager.SetInfo(player, vehicleInfo);
            API.sendNotificationToPlayer(player, "~b~Ваш транспорт ожидает внутри парковки");
            API.triggerClientEvent(player, ServerEvent.HIDE_PARKING_MENU);
        }

        /// <summary>
        /// Возвращает свободное место на парковке
        /// </summary>
        private Tuple<Vector3, Vector3> GetFreePlace() {
            var occupiedPlaces = VehicleManager.GetAllVehicles()
                .Where(e => e.hasData(ON_PARKING))
                .Select(e => e.position).ToList();
            return _parkingPositions.FirstOrDefault(e => PlaceFree(occupiedPlaces, e.Item1));
        }

        /// <summary>
        /// Оставить транспорт на парковке
        /// </summary>
        private void ParkVehicle(Client player, object[] args) {
            var playerVehicles = _vehicleInfoManager.GetPlayerVehicles(player).Where(e => e.IsSpawned);
            var vehiclesOnParking = playerVehicles.Where(e => e.Instance != null && e.Instance.hasData(ON_PARKING)).ToList();
            if (!vehiclesOnParking.Any()) {
                API.sendNotificationToPlayer(player, "~r~Транспорт не найден на парковке", true);
                return;
            }
            var price = (int) args[0];
            if (!PayParking(player, price)) {
                return;
            }
            foreach (var playerVehicle in vehiclesOnParking) {
                playerVehicle.IsSpawned = false;
                playerVehicle.Instance.delete();
                _vehicleInfoManager.SetInfo(player, playerVehicle);
            }
            _clanManager.ReplenishClanBalance(1, price);
            API.sendNotificationToPlayer(player, "~b~Ваш транспорт успешно припаркован");
        }

        /// <summary>
        /// Проверяет, что транспорт можно спавнить
        /// </summary>
        private bool CanSpawn(Client player, VehicleInfo vehicle) {
            if (vehicle.IsSpawned) {
                API.sendNotificationToPlayer(player, "~r~Транспортное средство не припарковано", true);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Определяет, занято ли место
        /// </summary>
        public static bool PlaceFree(IEnumerable<Vector3> occupiedPlaces, Vector3 place) {
            foreach (var occupiedPlace in occupiedPlaces) {
                var distance = Vector3.Distance(occupiedPlace, place);
                if (distance <= 1) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Списание платы за парковку
        /// </summary>
        private bool PayParking(Client player, int price) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (playerInfo.Balance < price) {
                API.sendNotificationToPlayer(player, "~r~Недостаточно средств для оплаты парковки", true);
                return false;
            }
            playerInfo.Balance -= price;
            _playerInfoManager.RefreshUI(player, playerInfo);
            return true;
        }

        /// <summary>
        /// Парковочные места
        /// Item1 - позиция
        /// Item2 - поворот
        /// </summary>
        private static readonly List<Tuple<Vector3, Vector3>> _parkingPositions = new List<Tuple<Vector3, Vector3>> {
            new Tuple<Vector3, Vector3>(new Vector3(-342.05, -767.46, 33.28), new Vector3(-0.39, 0.01, -89.67)),
            new Tuple<Vector3, Vector3>(new Vector3(-341.85, -763.98, 33.28), new Vector3(-0.43, 0.00, -89.56)),
            new Tuple<Vector3, Vector3>(new Vector3(-342.15, -760.37, 33.28), new Vector3(-0.40, 0.00, -88.69)),
            new Tuple<Vector3, Vector3>(new Vector3(-342.25, -756.84, 33.28), new Vector3(-0.44, 0.00, -89.80)),
            new Tuple<Vector3, Vector3>(new Vector3(-342.21, -753.30, 33.28), new Vector3(-0.40, 0.02, -88.85)),
            new Tuple<Vector3, Vector3>(new Vector3(-342.60, -749.72, 33.28), new Vector3(-0.42, 0.05, -89.10)),
            new Tuple<Vector3, Vector3>(new Vector3(-337.52, -750.37, 33.28), new Vector3(-0.37, 0.08, -178.83)),
            new Tuple<Vector3, Vector3>(new Vector3(-334.71, -750.61, 33.28), new Vector3(-0.40, 0.03, -179.66)),
            new Tuple<Vector3, Vector3>(new Vector3(-331.80, -750.51, 33.28), new Vector3(-0.35, 0.00, -179.93)),
            new Tuple<Vector3, Vector3>(new Vector3(-328.96, -750.50, 33.28), new Vector3(-0.39, 0.01, -179.56)),
            new Tuple<Vector3, Vector3>(new Vector3(-323.31, -751.90, 33.28), new Vector3(-0.41, -0.01, 160.27)),
            new Tuple<Vector3, Vector3>(new Vector3(-320.32, -752.64, 33.28), new Vector3(-0.37, 0.00, 160.06)),
            new Tuple<Vector3, Vector3>(new Vector3(-317.48, -753.52, 33.28), new Vector3(-0.38, 0.01, 159.64)),
            new Tuple<Vector3, Vector3>(new Vector3(-314.76, -754.66, 33.28), new Vector3(-0.37, 0.01, 158.48)),
            new Tuple<Vector3, Vector3>(new Vector3(-310.76, -755.95, 33.28), new Vector3(-0.33, 0.00, 160.43)),
            new Tuple<Vector3, Vector3>(new Vector3(-307.94, -756.85, 33.28), new Vector3(-0.39, 0.04, 161.27)),
            new Tuple<Vector3, Vector3>(new Vector3(-305.07, -757.92, 33.28), new Vector3(-0.32, 0.00, 160.56)),
            new Tuple<Vector3, Vector3>(new Vector3(-302.20, -759.00, 33.28), new Vector3(-0.39, 0.02, 160.17)),
            new Tuple<Vector3, Vector3>(new Vector3(-298.29, -760.47, 33.28), new Vector3(-0.36, 0.07, 160.35)),
            new Tuple<Vector3, Vector3>(new Vector3(-295.36, -761.43, 33.28), new Vector3(-0.33, 0.00, 160.82)),
            new Tuple<Vector3, Vector3>(new Vector3(-292.55, -762.34, 33.28), new Vector3(-0.30, 0.00, 159.89)),
            new Tuple<Vector3, Vector3>(new Vector3(-289.66, -763.52, 33.28), new Vector3(-0.39, 0.02, 160.77)),
        };
    }
}