using System.Collections.Generic;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Managers.Work.Pilot.Interfaces;
using gta_mp_server.Models.Utils;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using Ninject;
using Ninject.Syntax;
using Marker = gta_mp_server.Enums.Marker;

namespace gta_mp_server.Managers.Places.AirPorts {
    /// <summary>
    /// Аэропорт
    /// </summary>
    internal class AirPort: Place {
        private const string PLANE_KEY = "AirPortPlane";
        private const string WAS_WARPED = "WasWarped";

        private readonly IPointCreator _pointCreator;
        private readonly IVehicleManager _vehicleManager;
        private readonly IWorkInfoManager _workInfoManager;
        private readonly IPilotManager _pilotManager;

        /// <summary>
        /// Контракты, в порядке "из города" - "в город"
        /// </summary>
        internal static List<DeliveryContract> Contracts = new List<DeliveryContract> {
            new DeliveryContract(350, 450) {Type = DeliveryContractType.MechanicalDrawings, Reward = 400, TargetPosition = new Vector3(1705.22, 3254.04, 39.01)},
            new DeliveryContract(350, 450) {Type = DeliveryContractType.CollectedPrototypes, Reward = 400, TargetPosition = new Vector3(-1579.52, -2808.01, 11.45)},
            new DeliveryContract(450, 550) {Type = DeliveryContractType.Fertilizer, Reward = 500, TargetPosition = new Vector3(2103.87, 4793.69, 39.34)},
            new DeliveryContract(450, 550) {Type = DeliveryContractType.Crop, Reward = 500, TargetPosition = new Vector3(-1579.52, -2808.01, 11.45)},
            new DeliveryContract(550, 650) {Type = DeliveryContractType.MilitaryEquipment, Reward = 600, TargetPosition = new Vector3(-2014.16, 2862.59, 31.24)},
            new DeliveryContract(550, 650) {Type = DeliveryContractType.MilitaryReports, Reward = 600, TargetPosition = new Vector3(-1579.52, -2808.01, 11.45)},
            new DeliveryContract(750, 850) {Type = DeliveryContractType.MoneyToFarm, Reward = 800, TargetPosition = new Vector3(2103.87, 4793.69, 39.34)},
            new DeliveryContract(750, 850) {Type = DeliveryContractType.Marijuana, Reward = 800, TargetPosition = new Vector3(-1579.52, -2808.01, 11.45)},
            new DeliveryContract(900, 1100) {Type = DeliveryContractType.MoneyToMilitary, Reward = 1000, TargetPosition = new Vector3(-2014.16, 2862.59, 31.24)},
            new DeliveryContract(900, 1100) {Type = DeliveryContractType.SmugglingWeapon, Reward = 1000, TargetPosition = new Vector3(-1579.52, -2808.01, 11.45)}
        };

        public AirPort() : this(ServerKernel.Kernel) {
            API.onPlayerEnterVehicle += OnPlayerEnterVehicle;
            API.onPlayerExitVehicle += OnPlayerExitVehicle;
        }

        public AirPort(IResolutionRoot kernel) {
            _pointCreator = kernel.Get<IPointCreator>();
            _vehicleManager = kernel.Get<IVehicleManager>();
            _workInfoManager = kernel.Get<IWorkInfoManager>();
            _pilotManager = kernel.Get<IPilotManager>();
        }

        /// <summary>
        /// Инизиализировать место
        /// </summary>
        public override void Initialize() {
            CreateNpcs();
            _pointCreator.CreateBlip(MainPosition.AirPort, 359, 63, name: "Аэропорт");
            var enter = _pointCreator.CreateMarker(Marker.UpsideDownCone, MainPosition.AirPort, Colors.Yellow, 1f);
            enter.ColShape.onEntityEnterColShape += (shape, entity) => 
                PlayerHelper.ProcessAction(entity, player => API.setEntityPosition(player, AirPortData.AfterEnter));
            AirPortData.Exits.ForEach(exitPosition => {
                var exit = _pointCreator.CreateMarker(Marker.UpsideDownCone, exitPosition, Colors.Yellow, 1f);
                exit.ColShape.onEntityEnterColShape += (shape, entity) =>
                    PlayerHelper.ProcessAction(entity, player => API.setEntityPosition(player, AirPortData.AfterExit));
            });
            CreatePlanes();
            _pilotManager.Initialize();
        }

        /// <summary>
        /// Создает нпс во всех аэропортах
        /// </summary>
        private void CreateNpcs() {
            foreach (var npcData in AirPortData.Npcs) {
                _pointCreator.CreateBlip(npcData.Position, 572, 53, name: "Работа лётчиком");
                var npc = _pointCreator.CreatePed(npcData.Hash, npcData.Name, npcData.Position, npcData.Rotation, npcData.MarkerPosition, Colors.VividCyan);
                npc.ColShape.onEntityEnterColShape += (shape, entity) => PlayerComeToNpc(entity, npcData.Contracts);
                npc.ColShape.onEntityExitColShape += (shape, entity) =>
                    PlayerHelper.ProcessAction(entity, player => API.triggerClientEvent(player, ServerEvent.HIDE_PILOT_MENU));
            }
        }

        /// <summary>
        /// Игрок подошел к нпс
        /// </summary>
        private void PlayerComeToNpc(NetHandle entity, ICollection<DeliveryContract> contracts) {
            PlayerHelper.ProcessAction(entity, player => {
                API.triggerClientEvent(player, ServerEvent.SHOW_PILOT_MENU, JsonConvert.SerializeObject(contracts));
            });
        }
        
        /// <summary>
        /// Создает самолеты
        /// </summary>
        private void CreatePlanes() {
            foreach (var planeInfo in AirPortData.Planes) {
                var plane = new CommonVehicle {
                    Hash = planeInfo.Hash,
                    SpawnPosition = planeInfo.Position,
                    SpawnRotation = planeInfo.Rotation,
                    Fuel = 150,
                    MaxFuel = 150,
                    VehicleType = PLANE_KEY,
                    MainColor = 111,
                    SecondColor = 74
                };
                _vehicleManager.CreateVehicle(plane);
            }
        }

        /// <summary>
        /// Обработчик входа игрока в самолет
        /// </summary>
        private void OnPlayerEnterVehicle(Client player, NetHandle vehicleHandle, int seat) {
            var vehicle = API.getEntityFromHandle<Vehicle>(vehicleHandle);
            if (!vehicle.hasData(PLANE_KEY)) {
                return;
            }
            var activeWork = _workInfoManager.GetActiveWork(player);
            if (activeWork == null || activeWork.Type != WorkType.Pilot) {
                WarpOut(player, "Вы не работаете летчиком");
                return;
            }
            if (!AllowToFlyPlane(vehicle, activeWork.Level)) {
                player.setData(WAS_WARPED, true);
                WarpOut(player, "Недостаточный уровень работы для данного самолета");
                return;
            }
            var contract = PlayerHelper.GetData<DeliveryContract>(player, WorkData.DELIVERY_CONTRACT, null);
            if (contract == null) {
                API.sendNotificationToPlayer(player, "~r~Необходимо заключить контракт", true);
                return;
            }
            API.triggerClientEvent(player, ServerEvent.SHOW_PILOT_TARGET_POINT, contract.TargetPosition);
        }

        /// <summary>
        /// Обработчик выхода игрока из самолета
        /// </summary>
        private void OnPlayerExitVehicle(Client player, NetHandle vehicleHandle, int seat) {
            var vehicle = API.getEntityFromHandle<Vehicle>(vehicleHandle);
            if (!vehicle.hasData(PLANE_KEY) || player.hasData(WAS_WARPED)) {
                player.resetData(WAS_WARPED);
                return;
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_PILOT_TARGET_POINT);
        }

        /// <summary>
        /// Проверяет, разрешено ли игроку управлять выбранным самолетом
        /// </summary>
        private bool AllowToFlyPlane(Vehicle plane, int workLevel) {
            var hash = (VehicleHash) API.getEntityModel(plane);
            switch (hash) {
                case VehicleHash.Duster:
                    return true;
                case VehicleHash.Mammatus:
                    return workLevel > 1;
                case VehicleHash.Seabreeze:
                    return workLevel > 2;
                case VehicleHash.Vestra:
                    return workLevel > 3;
                case VehicleHash.Nimbus:
                    return workLevel > 4;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Выкидывает игрока из самолета
        /// </summary>
        private void WarpOut(Client player, string message) {
            API.sendNotificationToPlayer(player, $"~r~{message}", true);
            player.warpOutOfVehicle();
        }
    }
}