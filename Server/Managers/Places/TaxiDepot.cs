using System.Collections.Generic;
using System.Linq;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Managers.Work.Taxi;
using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Shared;

namespace gta_mp_server.Managers.Places {
    /// <summary>
    /// Автостоянка такси
    /// </summary>
    internal class TaxiDepot : Place {
        private const float MAX_FUEL = 60f;
        internal const string TAXI_KEY = "Taxi";

        private readonly IVehicleManager _vehicleManager;

        public TaxiDepot() {}
        public TaxiDepot(IVehicleManager vehicleManager) {
            _vehicleManager = vehicleManager;
        }

        /// <summary>
        /// Инизиализировать автобусное депо
        /// </summary>
        public override void Initialize() {
            CreateTaxis();
        }

        /// <summary>
        /// Создает автобусы
        /// </summary>
        private void CreateTaxis() {
            var taxis = GetTaxisInfo();
            foreach (var taxi in taxis) {
                _vehicleManager.CreateVehicle(taxi);
            }
        }

        /// <summary>
        /// Возвращает информацию о такси
        /// </summary>
        private static IEnumerable<CommonVehicle> GetTaxisInfo() {
            var positions = TaxiPositionsGetter.GetCarPositions();
            return positions.Select(position => new CommonVehicle {
                Hash = VehicleHash.Taxi,
                VehicleType = TAXI_KEY,
                SpawnPosition = position.Item1,
                SpawnRotation = position.Item2,
                MainColor = 42,
                SecondColor = 42,
                Fuel = MAX_FUEL,
                MaxFuel = MAX_FUEL
            }).ToList();
        }
    }
}