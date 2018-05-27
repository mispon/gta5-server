using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_server.Constant;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.IoC;
using gta_mp_server.Managers.DrivingSchool.Interfaces;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Ninject;
using Ninject.Syntax;
using Marker = gta_mp_server.Enums.Marker;

namespace gta_mp_server.Managers.Places {
    /// <summary>
    /// Автошкола
    /// </summary>
    internal class DrivingSchool : Place {
        private const float MAX_FUEL = 60;
        public const string OBJECT_KEY = "DrivingSchool";

        private readonly IPointCreator _pointCreator;
        private readonly IVehicleManager _vehicleManager;
        private readonly IDriverPracticeExamManager _practiceExamManager;

        public DrivingSchool() : this(ServerKernel.Kernel) {
            API.onPlayerEnterVehicle += OnEnterSchoolCar;
            API.onPlayerExitVehicle += OnExitSchoolCar;
        }

        public DrivingSchool(IResolutionRoot kernel) {
            _pointCreator = kernel.Get<IPointCreator>();
            _vehicleManager = kernel.Get<IVehicleManager>();
            _practiceExamManager = kernel.Get<IDriverPracticeExamManager>();
        }

        /// <summary>
        /// Инизиализировать место
        /// </summary>
        public override void Initialize() {
            API.requestIpl("bkr_biker_interior_placement_interior_6_biker_dlc_int_ware05_milo");
            CreateCars();
            _pointCreator.CreateBlip(MainPosition.DrivingSchool, 545, 3, name: "Автошкола");
            var enter = _pointCreator.CreateMarker(Marker.UpsideDownCone, MainPosition.DrivingSchool, Colors.Yellow, 1);
            enter.ColShape.onEntityEnterColShape += EnterIntoSchool;
            var exit = _pointCreator.CreateMarker(Marker.UpsideDownCone, new Vector3(1173.69, -3196.66, -39.01), Colors.Yellow, 1);
            exit.ColShape.onEntityEnterColShape += ExitFromSchool;
        }

        /// <summary>
        /// Вход в автошколу
        /// </summary>
        private void EnterIntoSchool(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.sendNativeToPlayer(player, Hash.DISPLAY_RADAR, false);
            API.setEntityPosition(player, new Vector3(1171.00, -3196.65, -39.01));
        }

        /// <summary>
        /// Выход из автошколы
        /// </summary>
        private void ExitFromSchool(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.sendNativeToPlayer(player, Hash.DISPLAY_RADAR, true);
            API.setEntityPosition(player, new Vector3(82.47, -1550.69, 29.60));
        }

        /// <summary>
        /// Обработчик события входа в машину
        /// </summary>
        private void OnEnterSchoolCar(Client player, NetHandle vehicle, int seat) {
            if (API.getEntityData(vehicle, OBJECT_KEY) == null) {
                return;
            }
            if (player.getData("OnPracticeExam") == null) {
                API.warpPlayerOutOfVehicle(player);
                API.sendNotificationToPlayer(player, "~r~Вы не можете сесть в ученическую машину", true);
                return;
            }
            API.setEntitySyncedData(vehicle, "SchoolCar", true);
            _practiceExamManager.Start(player);
        }

        /// <summary>
        /// Обработчик события выхода из машины
        /// </summary>
        private void OnExitSchoolCar(Client player, NetHandle vehicle, int seat) {
            if (API.getEntityData(vehicle, OBJECT_KEY) == null || API.getEntityData(player, "OnPracticeExam") == null) {
                return;
            }
            API.sendNotificationToPlayer(player, "~r~Учебная машина покинута во время экзамена", true);
            API.setEntitySyncedData(vehicle, "SchoolCar", false);
            _practiceExamManager.Finish(player, false);
        }

        /// <summary>
        /// Заспавнить машины
        /// </summary>
        private void CreateCars() {
            var cars = GetCarsInfo();
            foreach (var car in cars) {
                _vehicleManager.CreateVehicle(car);
            }
        }

        /// <summary>
        /// Возвращает информацию об ученических машинах
        /// </summary>
        private static IEnumerable<CommonVehicle> GetCarsInfo() {
            var positions = GetCarPositions();
            return positions.Select(position => new CommonVehicle {
                Hash = VehicleHash.Blista,
                VehicleType = OBJECT_KEY,
                SpawnPosition = position.Item1,
                SpawnRotation = position.Item2,
                MainColor = 64,
                SecondColor = 92,
                Fuel = MAX_FUEL,
                MaxFuel = MAX_FUEL
            }).ToList();
        }

        /// <summary>
        /// Возвращает позиции автомобилей на стоянке
        /// Item1 - позиция
        /// Item2 - поворот
        /// </summary>
        private static IEnumerable<Tuple<Vector3, Vector3>> GetCarPositions() {
            return new List<Tuple<Vector3, Vector3>> {
                new Tuple<Vector3, Vector3>(new Vector3(81.66, -1542.10, 28.90), new Vector3(-0.02, 0.01, -131.39)),
                new Tuple<Vector3, Vector3>(new Vector3(79.87, -1544.64, 28.90), new Vector3(-0.01, 0.01, -130.02)),
                new Tuple<Vector3, Vector3>(new Vector3(77.81, -1546.95, 28.90), new Vector3(-0.08, 0.01, -130.97)),
                new Tuple<Vector3, Vector3>(new Vector3(76.02, -1549.47, 28.90), new Vector3(-0.03, 0.00, -129.73)),
                new Tuple<Vector3, Vector3>(new Vector3(73.82, -1551.58, 28.90), new Vector3(-0.08, 0.01, -130.63)),
                new Tuple<Vector3, Vector3>(new Vector3(72.09, -1554.13, 28.90), new Vector3(-0.07, 0.01, -131.00)),
                new Tuple<Vector3, Vector3>(new Vector3(70.06, -1556.47, 28.90), new Vector3(-0.07, 0.10, -129.63)),
                new Tuple<Vector3, Vector3>(new Vector3(68.08, -1558.81, 28.90), new Vector3(-0.07, 0.10, -129.63)),
                //new Tuple<Vector3, Vector3>(new Vector3(66.12, -1561.24, 28.90), new Vector3(-0.07, 0.10, -129.63)),
                //new Tuple<Vector3, Vector3>(new Vector3(64.16, -1563.92, 28.90), new Vector3(-0.07, 0.10, -129.63)),
                //new Tuple<Vector3, Vector3>(new Vector3(71.48, -1532.78, 28.90), new Vector3(-0.13, 0.00, 48.97)),
                //new Tuple<Vector3, Vector3>(new Vector3(69.19, -1535.27, 28.90), new Vector3(-0.07, 0.00, 48.17)),
                //new Tuple<Vector3, Vector3>(new Vector3(67.31, -1537.70, 28.90), new Vector3(0.01, 0.04, 49.90)),
                //new Tuple<Vector3, Vector3>(new Vector3(65.35, -1540.16, 28.90), new Vector3(-0.07, 0.01, 49.35)),
                //new Tuple<Vector3, Vector3>(new Vector3(63.39, -1542.49, 28.90), new Vector3(-0.05, 0.03, 50.12)),
                //new Tuple<Vector3, Vector3>(new Vector3(61.35, -1544.75, 28.90), new Vector3(-0.05, 0.00, 48.94)),
                //new Tuple<Vector3, Vector3>(new Vector3(59.35, -1547.14, 28.90), new Vector3(-0.03, 0.00, 49.85)),
                //new Tuple<Vector3, Vector3>(new Vector3(57.24, -1549.39, 28.90), new Vector3(0.00, 0.00, 50.17)),
                //new Tuple<Vector3, Vector3>(new Vector3(55.22, -1551.62, 28.90), new Vector3(-0.09, 0.01, 50.00)),
                //new Tuple<Vector3, Vector3>(new Vector3(53.29, -1554.19, 28.90), new Vector3(-0.09, -0.03, 49.80))
            };
        }
    }
}