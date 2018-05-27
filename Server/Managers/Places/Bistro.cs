using gta_mp_server.Constant;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Managers.Work.Bistro;
using gta_mp_server.Managers.Work.Bistro.Interfaces;
using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using Ninject;
using Ninject.Syntax;

namespace gta_mp_server.Managers.Places {
    /// <summary>
    /// Закусочная
    /// </summary>
    internal class Bistro : Place {
        private const string BISTRO_TRUCK_KEY = "BistroTruck";
        private const string BISTRO_SCOOTER_KEY = "BistroScooter";
        private const string SELL_SHAPE = "SellShape";
        private const string TRUCK_DRIVER_KEY = "TrunkDriver";

        private readonly IPointCreator _pointCreator;
        private readonly IVehicleManager _vehicleManager;
        private readonly IBistroManager _bistroManager;

        public Bistro() : this(ServerKernel.Kernel) {
            API.onPlayerEnterVehicle += OnEnterBistroVehicle;
            API.onPlayerExitVehicle += OnExitBistroVehicle;
        }

        public Bistro(IResolutionRoot kernel) {
            _pointCreator = kernel.Get<IPointCreator>();
            _vehicleManager = kernel.Get<IVehicleManager>();
            _bistroManager = kernel.Get<IBistroManager>();
        }

        /// <summary>
        /// Инизиализировать закусочную
        /// </summary>
        public override void Initialize() {
            _pointCreator.CreateBlip(MainPosition.Bistro, 106, 49, scale: 0.8f, name: "Закусочная");
            CreateVehicles();
            _bistroManager.CreatePoints();
        }

        /// <summary>
        /// Обработчик входа в фургон закусочной
        /// </summary>
        private void OnEnterBistroVehicle(Client player, NetHandle vehicleHandle, int seat) {
            var vehicle = API.getEntityFromHandle<Vehicle>(vehicleHandle);
            if (!(vehicle.hasData(BISTRO_SCOOTER_KEY) || vehicle.hasData(BISTRO_TRUCK_KEY))) {
                return;
            }
            if (vehicle.hasData(BISTRO_SCOOTER_KEY) && player.hasData(WorkData.IS_FOOD_DELIVERYMAN)) {
                if (!player.hasSyncedData(BistroManager.POINT_NUMBER_KEY)) {
                    _bistroManager.ShowNextDeliveryPoint(player);
                }
                API.triggerClientEvent(player, ServerEvent.SHOW_HINT, "Если сбился маршрут, нажмите на O, чтобы восстановить его", 110);
                ActionHelper.SetAction(player, 10000, () => API.triggerClientEvent(player, ServerEvent.HIDE_HINT));
                return;
            }
            if (vehicle.hasData(BISTRO_TRUCK_KEY) && player.hasData(WorkData.IS_FOOD_TRUCK_DRIVER)) {
                CreateTruckShape(player, vehicle);
                return;
            }
            API.sendNotificationToPlayer(player, "~r~Вы не работаете в закусочной", true);
            player.warpOutOfVehicle();
        }

        /// <summary>
        /// Обработчик выхода их фургона закусочной
        /// </summary>
        private void OnExitBistroVehicle(Client player, NetHandle vehicleHandle, int seat) {
            var vehicle = API.getEntityFromHandle<Vehicle>(vehicleHandle);
            if (vehicle.hasData(BISTRO_TRUCK_KEY) && player.hasData(WorkData.IS_FOOD_TRUCK_DRIVER)) {
                var sellShape = (ColShape) vehicle.getData(SELL_SHAPE);
                API.deleteColShape(sellShape);
            }
        }

        /// <summary>
        /// Создает обработчик событий для фургона
        /// </summary>
        private void CreateTruckShape(Client player, Vehicle vehicle) {
            var sellShape = API.createSphereColShape(vehicle.position, 3f);
            sellShape.onEntityEnterColShape += PlayerComeToFoodTruck;
            sellShape.onEntityExitColShape += PlayerAwayFromFoodTruck;
            sellShape.setData(TRUCK_DRIVER_KEY, player.socialClubName);
            sellShape.attachToEntity(vehicle);
            vehicle.setData(SELL_SHAPE, sellShape);
        }

        /// <summary>
        /// Игрок подошел к фургону с едой
        /// </summary>
        private void PlayerComeToFoodTruck(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            var trunkDriver = shape.getData(TRUCK_DRIVER_KEY) as string;
            API.triggerClientEvent(player, ServerEvent.SHOW_BISTRO_FOOD_MENU, trunkDriver);
        }

        /// <summary>
        /// Игрок отошел от фургона с едой
        /// </summary>
        private void PlayerAwayFromFoodTruck(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_BISTRO_FOOD_MENU);
        }

        /// <summary>
        /// Создает транспорт бистро
        /// </summary>
        private void CreateVehicles() {
            foreach (var info in BistroPositionsGetter.GetVehiclesInfo()) {
                var commonVehicle = new CommonVehicle {
                    Hash = info.IsTruck ? VehicleHash.Taco : VehicleHash.Faggio,
                    VehicleType = info.IsTruck ? BISTRO_TRUCK_KEY : BISTRO_SCOOTER_KEY,
                    SpawnPosition = info.Position,
                    SpawnRotation = info.Rotation,
                    Fuel = info.IsTruck ? 100 : 50,
                    MaxFuel = info.IsTruck ? 100 : 50,
                    MainColor = 38,
                    SecondColor = 32
                };
                _vehicleManager.CreateVehicle(commonVehicle);
            }
        }
    }
}