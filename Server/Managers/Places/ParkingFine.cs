using System;
using System.Linq;
using gta_mp_server.Constant;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Vehicles;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Managers.Work.Wrecker;
using gta_mp_server.Managers.Work.Wrecker.Interfaces;
using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using Ninject;
using Ninject.Syntax;

namespace gta_mp_server.Managers.Places {
    /// <summary>
    /// Штрафстоянка
    /// </summary>
    internal class ParkingFine : Place {
        internal const string ON_PARKING_FINE = "VehicleOnParkingFine";
        internal const string TOW_TRUCK_KEY = "TowTruck";
        private const int MAX_FUEL = 90;

        private readonly IPointCreator _pointCreator;
        private readonly IVehicleManager _vehicleManager;
        private readonly IWreckerManager _wreckerManager;

        public ParkingFine() : this(ServerKernel.Kernel) {
            API.onPlayerEnterVehicle += PlayerEnterVehicle;
            API.onPlayerExitVehicle += PlayerExitVehicle;
        }

        public ParkingFine(IResolutionRoot kernel) {
            _pointCreator = kernel.Get<IPointCreator>();
            _vehicleManager = kernel.Get<IVehicleManager>();
            _wreckerManager = kernel.Get<IWreckerManager>();
        }

        /// <summary>
        /// Инизиализировать штрафстоянку
        /// </summary>
        public override void Initialize() {
            _wreckerManager.Initialize();
            _pointCreator.CreateBlip(MainPosition.ParkingFine, 68, 6, name: "Штрафстоянка (Эвакуаторщик)");
            CreateTowTrucks();
            var parkingFine = API.createSphereColShape(new Vector3(409.83, -1637.81, 29.29), 5.5f);
            parkingFine.onEntityEnterColShape += (shape, entity) => ParkingFineAction(entity, vehicle => vehicle.setData(ON_PARKING_FINE, true));
            parkingFine.onEntityExitColShape += (shape, entity) => ParkingFineAction(entity, vehicle => vehicle.resetData(ON_PARKING_FINE));
        }

        /// <summary>
        /// Обработчик въезда / выезда
        /// </summary>
        private void ParkingFineAction(NetHandle entity, Action<Vehicle> action) {
            var vehicle = API.getEntityFromHandle<Vehicle>(entity);
            if (vehicle != null) {
                action(vehicle);
            }
        }

        /// <summary>
        /// Обработка входа в эвакуатор
        /// </summary>
        private void PlayerEnterVehicle(Client player, NetHandle vehicle, int seat) {
            if (!API.hasEntityData(vehicle, TOW_TRUCK_KEY)) {
                return;
            }
            if (!player.hasData(WorkData.IS_WRECKER)) {
                API.sendNotificationToPlayer(player, "~r~Вы не работаете на эвакуаторе");
                player.warpOutOfVehicle();
                return;
            }
            var afkVehicles = _vehicleManager.GetAfkVehicles(WreckerManager.AFK_MINUTES)
                .Where(e => e.hasData(VehicleManager.OWNER_ID) && e.hasData(VehicleManager.VEHICLE_ID) && !e.hasData(VehicleManager.COMMON_VEHICLE))
                .Select(e => new {
                    Id = (long) e.getData(VehicleManager.VEHICLE_ID),
                    Position = e.position
                });
            API.triggerClientEvent(player, ServerEvent.SHOW_AFK_VEHICLES, JsonConvert.SerializeObject(afkVehicles));
        }

        /// <summary>
        /// Обработка выхода из эвакуатора
        /// </summary>
        private void PlayerExitVehicle(Client player, NetHandle vehicle, int seat) {
            if (!API.hasEntityData(vehicle, TOW_TRUCK_KEY)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_AFK_VEHICLES);
        }

        /// <summary>
        /// Создать эвакуаторы
        /// </summary>
        private void CreateTowTrucks() {
            foreach (var position in WreckerPositionsGetter.TowTrucksPositions) {
                var truck = new CommonVehicle {
                    Hash = VehicleHash.TowTruck2,
                    VehicleType = TOW_TRUCK_KEY,
                    SpawnPosition = position.Item1,
                    SpawnRotation = position.Item2,
                    Fuel = MAX_FUEL,
                    MaxFuel = MAX_FUEL,
                    MainColor = 33,
                    SecondColor = 1
                };
                _vehicleManager.CreateVehicle(truck);
            }
        }
    }
}