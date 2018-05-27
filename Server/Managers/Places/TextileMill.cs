using gta_mp_server.Constant;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Managers.Work.Forklift;
using gta_mp_server.Managers.Work.Forklift.Interfaces;
using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using Ninject;
using Ninject.Syntax;

namespace gta_mp_server.Managers.Places {
    /// <summary>
    /// Текстильная фабрика
    /// Работа на погрузчике
    /// </summary>
    internal class TextileMill : Place {
        internal const string FORKLIFT_KEY = "Forklift";

        private readonly IPointCreator _pointCreator;
        private readonly IVehicleManager _vehicleManager;
        private readonly IForkliftManager _forkliftManager;

        public TextileMill(): this(ServerKernel.Kernel) {
            API.onPlayerEnterVehicle += OnLoaderEnterVehicle;
        }

        public TextileMill(IResolutionRoot kernel) {
            _pointCreator = kernel.Get<IPointCreator>();
            _vehicleManager = kernel.Get<IVehicleManager>();
            _forkliftManager = kernel.Get<IForkliftManager>();
        }

        /// <summary>
        /// Инизиализировать место
        /// </summary>
        public override void Initialize() {
            _pointCreator.CreateBlip(MainPosition.TextileMill, 50, 14, name: "Текстильная фабрика");
            _forkliftManager.Initialize();
            CreateForklifts();
        }

        /// <summary>
        /// Обработчик входа в погрузчик
        /// </summary>
        private void OnLoaderEnterVehicle(Client player, NetHandle vehicleHandle, int seat) {
            var vehicle = API.getEntityFromHandle<Vehicle>(vehicleHandle);
            if (!vehicle.hasData(FORKLIFT_KEY) || player.hasData(WorkData.IS_FORKLIFT)) {
                return;
            }
            API.sendNotificationToPlayer(player, "~r~Вы не работаете водителем погрузчика", true);
            player.warpOutOfVehicle();
        }

        /// <summary>
        /// Инициализировать погрузчики
        /// </summary>
        private void CreateForklifts() {
            foreach (var position in ForkliftDataGetter.ForkliftsPositions) {
                var vehicleInfo = new CommonVehicle {
                    Hash = VehicleHash.Forklift,
                    VehicleType = FORKLIFT_KEY,
                    SpawnPosition = position.Item1,
                    SpawnRotation = position.Item2,
                    Fuel = 100,
                    MaxFuel = 100
                };
                _vehicleManager.CreateVehicle(vehicleInfo);
            }
        }
    }
}