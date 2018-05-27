using gta_mp_server.Constant;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Managers.Work.Fisherman;
using gta_mp_server.Managers.Work.Fisherman.Interfaces;
using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using Ninject;
using Ninject.Syntax;

namespace gta_mp_server.Managers.Places {
    /// <summary>
    /// Рыбацкое поселение
    /// </summary>
    internal class FishingVillage : Place {
        private const string BOAT_KEY = "Boat";

        private readonly IPointCreator _pointCreator;
        private readonly IVehicleManager _vehicleManager;
        private readonly IFishermanManager _fishermanManager;

        public FishingVillage() : this(ServerKernel.Kernel) {
            API.onPlayerEnterVehicle += OnPlayerEnterVehicle;
        }

        public FishingVillage(IResolutionRoot kernel) {
            _pointCreator = kernel.Get<IPointCreator>();
            _vehicleManager = kernel.Get<IVehicleManager>();
            _fishermanManager = kernel.Get<IFishermanManager>();
        }

        /// <summary>
        /// Инизиализировать место
        /// </summary>
        public override void Initialize() {
            _pointCreator.CreateBlip(MainPosition.FishingVillage, 410, 63, name: "Рыбацкое поселение");
            _fishermanManager.Initialize();
            CreateBoats();
        }

        /// <summary>
        /// Обработчик входа в лодку
        /// </summary>
        private void OnPlayerEnterVehicle(Client player, NetHandle vehicleHandle, int seat) {
            var vehicle = API.getEntityFromHandle<Vehicle>(vehicleHandle);
            if (!vehicle.hasData(BOAT_KEY)) {
                return;
            }
            if (!player.hasSyncedData(WorkData.IS_FISHERMAN_ON_BOAT)) {
                API.sendNotificationToPlayer(player, "~r~Вы не работаете рыбаком на лодке");
                player.warpOutOfVehicle();
                return;
            }
            API.setEntityPositionFrozen(vehicle, false);
        }

        /// <summary>
        /// Создает лодки
        /// </summary>
        private void CreateBoats() {
            foreach (var boat in FishermanDataGetter.Boats) {
                var info = new CommonVehicle {
                    Hash = VehicleHash.Dinghy2,
                    SpawnPosition = boat.Position,
                    SpawnRotation = boat.Rotation,
                    VehicleType = BOAT_KEY,
                    Fuel = 100,
                    MaxFuel = 100,
                    MainColor = 0,
                    SecondColor = 0
                };
                var vehicle = _vehicleManager.CreateVehicle(info);
                API.setEntityPositionFrozen(vehicle, true);
            }
        }
    }
}