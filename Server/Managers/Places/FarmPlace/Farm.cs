using gta_mp_server.Constant;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Managers.Work.Farmer.Interfaces;
using gta_mp_server.Models.Places.Instances;
using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Shared;

namespace gta_mp_server.Managers.Places.FarmPlace {
    /// <summary>
    /// Ферма
    /// </summary>
    internal class Farm : Place {
        internal const string TRACTOR_KEY = "Tractor";

        private readonly IPointCreator _pointCreator;
        private readonly IFarmerManager _farmerManager;
        private readonly ITractorDriverManager _tractorDriverManager;
        private readonly IVehicleManager _vehicleManager;

        internal static FarmInstance Instance = new FarmInstance();

        public Farm() {}
        public Farm(IPointCreator pointCreator, IFarmerManager farmerManager,
            ITractorDriverManager tractorDriverManager, IVehicleManager vehicleManager) {
            _pointCreator = pointCreator;
            _farmerManager = farmerManager;
            _tractorDriverManager = tractorDriverManager;
            _vehicleManager = vehicleManager;
        }

        /// <summary>
        /// Инизиализировать место
        /// </summary>
        public override void Initialize() {
            _pointCreator.CreateBlip(MainPosition.Farm, 88, 43, name: "Ферма");
            _farmerManager.Initialize();
            _tractorDriverManager.Initialize();
            CreateTractors();
        }

        /// <summary>
        /// Создать тракторы и их прицепы
        /// </summary>
        private void CreateTractors() {
            foreach (var tractorInfo in FarmDataGetter.Tractors) {
                var info = new CommonVehicle {
                    Hash = tractorInfo.Hash,
                    SpawnPosition = tractorInfo.Position,
                    SpawnRotation = tractorInfo.Rotation,
                    VehicleType = TRACTOR_KEY,
                    Fuel = 100,
                    MaxFuel = 100,
                    MainColor = tractorInfo.Hash == VehicleHash.Tractor2 ? 64 : 5
                };
                _vehicleManager.CreateVehicle(info);
            }
        }
    }
}