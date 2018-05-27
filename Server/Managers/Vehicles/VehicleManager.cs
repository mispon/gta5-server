using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_database.Entity;
using gta_mp_server.Events;
using gta_mp_server.Helpers;
using gta_mp_server.IoC;
using gta_mp_server.Managers.MenuHandlers;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Gta.Vehicle;
using GrandTheftMultiplayer.Shared.Math;
using Ninject;
using Ninject.Syntax;
using Vehicle = GrandTheftMultiplayer.Server.Elements.Vehicle;
using VehicleInfo = gta_mp_database.Entity.Vehicle;

namespace gta_mp_server.Managers.Vehicles {
    internal class VehicleManager : Script, IVehicleManager {
        internal const float ONE_KILOMETER = 500f;
        internal const string VEHICLE_ID = "VehicleId";
        internal const string OWNER_ID = "OwnerId";
        internal const string DONT_RESTORE = "DontRestorePosition";
        internal const string MAX_FUEL = "VehicleMaxFuel";
        internal const string COMMON_VEHICLE = "CommonVehicle";
        private const string VEHICLE_INFO_KEY = "CommonVehicleInfo";
        private const string POSITION_KEY = "VehiclePosition";
        private const float FUEL_CONSUMPTION = 0.8f;
        private const float PASSIVE_CONSUMPTION = 0.025f;
        private const float MIN_SPAWN_FAULT = 3f;
        private const int MAX_AFK_MINUTES = 300;

        private readonly IVehicleInfoManager _vehicleInfoManager;

        /// <summary>
        /// Транспорт, которому не нужно показывать спидометр и бензин
        /// </summary>
        internal static readonly HashSet<VehicleHash> NotUpdates = new HashSet<VehicleHash> {
            VehicleHash.Forklift,
            VehicleHash.Duster,
            VehicleHash.Mammatus,
            VehicleHash.Seabreeze,
            VehicleHash.Vestra,
            VehicleHash.Nimbus,
            VehicleHash.Dinghy2,
            VehicleHash.Tractor
        };

        public VehicleManager() : this(ServerKernel.Kernel) {
            ActionHelper.StartTimer(900000, () => {
                CleanAfkVehicles();
                RestorePositions();
            });
        }

        public VehicleManager(IResolutionRoot kernel) {
            _vehicleInfoManager = kernel.Get<IVehicleInfoManager>();
        }

        /// <summary>
        /// Проверяет, что транспорт брошен
        /// </summary>
        public static bool IsAfk(Vehicle vehicle, int maxAfkMinutes) {
            if (!vehicle.hasData(VehicleEventsManager.AFK_KEY)) {
                return false;
            }
            var afkPeriod = DateTime.Now - (DateTime) vehicle.getData(VehicleEventsManager.AFK_KEY);
            return afkPeriod.TotalMinutes >= maxAfkMinutes;
        }

        /// <summary>
        /// Удаляет брошенный транспорт с карты
        /// </summary>
        private void CleanAfkVehicles() {
            var vehicles = GetAllVehicles().Where(e => !e.hasData(COMMON_VEHICLE)).ToList();
            foreach (var vehicle in vehicles) {
                if (IsAfk(vehicle, MAX_AFK_MINUTES) || vehicle.health <= 0) {
                    RemoveVehicle(vehicle);
                }
            }
        }

        /// <summary>
        /// Обновление состояния работающих машин
        /// </summary>
        public void UpdateVehicles() {
            var activeVehicles = API.getAllVehicles().Where(e => API.getVehicleEngineStatus(e));
            foreach (var vehicle in activeVehicles) {
                var hash = (VehicleHash) API.getEntityModel(vehicle);
                if (!NotUpdates.Contains(hash)) {
                    UpdateFuel(vehicle);
                }
            }
        }

        /// <summary>
        /// Обновить количество топлива в баке
        /// </summary>
        private void UpdateFuel(NetHandle vehicleHandle) {
            var vehicle = API.getEntityFromHandle<Vehicle>(vehicleHandle);
            var distance = Vector3.Distance(vehicle.position, (Vector3) API.getEntityData(vehicle, POSITION_KEY));
            vehicle.setData(POSITION_KEY, vehicle.position);
            var vehicleFuel = API.getVehicleFuelLevel(vehicle);
            vehicleFuel -= PASSIVE_CONSUMPTION;
            vehicleFuel -= distance / ONE_KILOMETER * FUEL_CONSUMPTION;
            API.setVehicleFuelLevel(vehicle, vehicleFuel);
            if (vehicleFuel <= 0f) {
                API.setVehicleEngineStatus(vehicle, false);
            }
        }

        /// <summary>
        /// Создать коммунальный транспорт
        /// </summary>
        public Vehicle CreateVehicle(CommonVehicle info) {
            var vehicle = API.createVehicle(info.Hash, info.SpawnPosition, info.SpawnRotation, info.MainColor, info.SecondColor);
            vehicle.setData(info.VehicleType, true);
            vehicle.setData(COMMON_VEHICLE, true);
            vehicle.setData(VEHICLE_INFO_KEY, info);
            vehicle.setData(MAX_FUEL, info.Fuel);
            vehicle.setData(POSITION_KEY, info.SpawnPosition);
            API.setVehicleEngineStatus(vehicle, false);
            API.setVehicleFuelLevel(vehicle, info.Fuel);
            SetAdditionalSettings(vehicle);
            return vehicle;
        }

        /// <summary>
        /// Устанавливает дополнительные настройки для некоторого транспорта
        /// </summary>
        private void SetAdditionalSettings(Vehicle vehicle) {
            var hash = (VehicleHash) API.getEntityModel(vehicle);
            if (hash == VehicleHash.Dinghy2) {
                API.setEntityPositionFrozen(vehicle, true);
            }
            if (NotUpdates.Contains(hash)) {
                API.setEntityInvincible(vehicle, true);
            }
        }

        /// <summary>
        /// Создать транспорт игрока
        /// </summary>
        public Vehicle CreateVehicle(VehicleInfo vehicleInfo, Vector3 position, Vector3 rotation, int dimension = 0) {
            const int minFuel = 5;
            var vehicle = API.createVehicle(vehicleInfo.Hash, position, rotation, 0, 0, dimension);
            vehicle.setData(VEHICLE_ID, vehicleInfo.Id);
            vehicle.setData(OWNER_ID, vehicleInfo.OwnerId);
            vehicle.setData(MAX_FUEL, vehicleInfo.MaxFuel);
            var fuelLevel = vehicleInfo.Fuel < minFuel ? minFuel : vehicleInfo.Fuel;
            vehicleInfo.Instance = vehicle;
            API.setVehicleFuelLevel(vehicle, fuelLevel);
            API.setVehicleEngineStatus(vehicle, false);
            SetVehicleTuning(vehicle, vehicleInfo.Tuning);
            return vehicle;
        }

        /// <summary>
        /// Устанавливает настройки тюнинга
        /// </summary>
        internal static void SetVehicleTuning(Vehicle vehicle, VehicleTuning tuning) {
            ResetTuning(vehicle);
            API.shared.setVehiclePrimaryColor(vehicle, tuning.PrimaryColor);
            API.shared.setVehicleSecondaryColor(vehicle, tuning.SecondColor);
            API.shared.setVehicleEnginePowerMultiplier(vehicle, tuning.EnginePower);
            var neonColor = tuning.GetNeonColor();
            if (neonColor.Length > 0) {
                TurnNeonState(vehicle, true);
                API.shared.setVehicleNeonColor(vehicle, neonColor[0], neonColor[1], neonColor[2]);
            }
            foreach (var mod in tuning.GetMods()) {
                API.shared.setVehicleMod(vehicle, mod.Key, mod.Value);
            }
        }

        /// <summary>
        /// Выставляет стандартные значения тюнинга
        /// </summary>
        private static void ResetTuning(Vehicle vehicle) {
            const int defaultValue = -1;
            var vehicleHash = (VehicleHash) API.shared.getEntityModel(vehicle);
            var allMods = API.shared.getVehicleValidMods(vehicleHash).Keys;
            foreach (var mod in allMods) {
                API.shared.setVehicleMod(vehicle, mod, defaultValue);
            }
            TurnNeonState(vehicle, false);
            API.shared.setVehicleEnginePowerMultiplier(vehicle, 0);
        }

        /// <summary>
        /// Управляет неоновой подсветкой транспорта
        /// </summary>
        private static void TurnNeonState(Vehicle vehicle, bool turnOn) {
            for (var i = 0; i < 4; i++) {
                API.shared.setVehicleNeonState(vehicle, i, turnOn);
            }
        }

        /// <summary>
        /// Возвращает список всего транспорта на сервере
        /// </summary>
        public static List<Vehicle> GetAllVehicles(string vehicleKey = null) {
            var result = API.shared.getAllVehicles().Select(e => API.shared.getEntityFromHandle<Vehicle>(e));
            if (vehicleKey != null) {
                result = result.Where(e => e.hasData(vehicleKey));
            }
            return result.ToList();
        }

        /// <summary>
        /// Возвращает транспорт по идентификатору
        /// </summary>
        public Vehicle GetInstanceById(long vehicleId) {
            var vehiclesWithIds = GetAllVehicles().Where(e => e.hasData(VEHICLE_ID)).ToList();
            return vehiclesWithIds.FirstOrDefault(e => (long) e.getData(VEHICLE_ID) == vehicleId);
        }

        /// <summary>
        /// Возвращает брошенный транспорт
        /// </summary>
        public List<Vehicle> GetAfkVehicles(int afkMinutes) {
            return GetAllVehicles().Where(e => IsAfk(e, afkMinutes)).ToList();
        }

        /// <summary>
        /// Восстановить начальную позицию транспорта
        /// </summary>
        public void RestorePosition(Vehicle vehicle) {
            var info = (CommonVehicle) vehicle.getData(VEHICLE_INFO_KEY);
            var driver = API.getVehicleDriver(vehicle);
            if (vehicle.hasData(DONT_RESTORE) || OnRespawnAndCorrect(vehicle, info.SpawnPosition) || driver != null) {
                return;
            }
            if (vehicle.hasData(VehicleEventsManager.AFK_KEY) && !IsAfk(vehicle, 20)) {
                return;
            }
            vehicle.delete();
            CreateVehicle(info);
        }

        /// <summary>
        /// Возвращает ближайшую машину
        /// </summary>
        public Vehicle GetNearestVehicle(Client player, float range, string key = null) {
            var playerPosition = API.getEntityPosition(player);
            var vehicles = GetAllVehicles().Where(e => Vector3.Distance(e.position, playerPosition) < 10f);
            if (key != null) {
                vehicles = vehicles.Where(e => API.getEntityData(e, key) != null).ToList();
            }
            Vehicle result = null;
            var nearestDistance = float.MaxValue;
            foreach (var vehicle in vehicles) {
                var position = API.getEntityPosition(vehicle);
                var distance = Vector3.Distance(position, playerPosition);
                if (distance > range || distance >= nearestDistance) {
                    continue;
                }
                nearestDistance = distance;
                result = API.getEntityFromHandle<Vehicle>(vehicle);
            }
            return result;
        }

        /// <summary>
        /// Восстановить начальные позиции транспорта
        /// </summary>
        private void RestorePositions() {
            foreach (var vehicle in GetAllVehicles(COMMON_VEHICLE)) {
                RestorePosition(vehicle);
            }
        }

        /// <summary>
        /// Проверить, что транспорт находится на респауне в корректном состоянии
        /// </summary>
        private bool OnRespawnAndCorrect(Vehicle vehicle, Vector3 respawnPosition) {
            var distance = Vector3.Distance(respawnPosition, vehicle.position);
            var fuel = API.getVehicleFuelLevel(vehicle);
            return distance <= MIN_SPAWN_FAULT && vehicle.health > 0 && fuel > 10;
        }

        /// <summary>
        /// Удалить транспорт игрока и отправить на штрафстоянку
        /// </summary>
        private void RemoveVehicle(Vehicle vehicle) {
            if (vehicle.hasData(OWNER_ID) && !RentOfScootersMenuHandler.IsScooter(vehicle.model)) {
                var vehicleInfo = _vehicleInfoManager.GetInfoByHandle(vehicle);
                if (vehicleInfo != null) {
                    vehicleInfo.IsSpawned = false;
                    vehicleInfo.OnParkingFine = true;
                    _vehicleInfoManager.SetInfo(vehicleInfo);
                }
            }
            vehicle.delete();
        }

        /// <summary>
        /// Возвращает грузоподъемность транспорта
        /// </summary>
        internal static int GetCarrying(VehicleHash hash) {
            var vehicleClass = (VehicleClass) API.shared.getVehicleClass(hash);
            switch (vehicleClass) {
                case VehicleClass.Motorcycles:
                    return 100;
                case VehicleClass.Compacts:
                case VehicleClass.Super:
                    return 300;
                case VehicleClass.Sports:
                case VehicleClass.Sports2:
                    return 400;
                case VehicleClass.Coupes:
                case VehicleClass.Muscle:
                case VehicleClass.Sedans:
                    return 500;
                case VehicleClass.OffRoad:
                case VehicleClass.SUVs:
                    return 1200;
                case VehicleClass.Vans:
                    return 2000;
                default:
                    return 0;
            }
        }
    }
}