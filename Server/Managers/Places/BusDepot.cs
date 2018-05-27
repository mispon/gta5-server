using System.Collections.Generic;
using System.Linq;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Managers.Work.BusDriver;
using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Places {
    /// <summary>
    /// Автобусный парк
    /// </summary>
    internal class BusDepot : Place {
        private const float MAX_FUEL = 100;
        internal const string BUS_KEY = "Bus";

        private readonly IVehicleManager _vehicleManager;

        public BusDepot() {}
        public BusDepot(IVehicleManager vehicleManager) {
            _vehicleManager = vehicleManager;
        }

        /// <summary>
        /// Инизиализировать автобусное депо
        /// </summary>
        public override void Initialize() {
            CreateBuses();
        }

        /// <summary>
        /// Создает автобусы
        /// </summary>
        private void CreateBuses() {
            var buses = GetBusesInfo();
            foreach (var bus in buses) {
                _vehicleManager.CreateVehicle(bus);
            }
        }

        /// <summary>
        /// Возвращает информацию об автобусах
        /// </summary>
        private static IEnumerable<CommonVehicle> GetBusesInfo() {
            var positions = BusPositionGetter.GetBusPositions();
            var rotation = new Vector3(0, 0, -20);
            return positions.Select(position => new CommonVehicle {
                Hash = VehicleHash.Coach,
                VehicleType = BUS_KEY,
                SpawnPosition = position,
                SpawnRotation = rotation,
                Fuel = MAX_FUEL,
                MaxFuel = MAX_FUEL
            }).ToList();
        }
    }
}