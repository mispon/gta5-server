using System.Collections.Generic;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Managers.Work.Trucker;
using gta_mp_server.Managers.Work.Trucker.Interfaces;
using gta_mp_server.Models.Utils;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using Ninject;
using Ninject.Syntax;

namespace gta_mp_server.Managers.Places {
    internal class Port : Place {
        internal const string TRUCK_KEY = "Truck";
        private const int MAX_FUEL = 200;

        internal static readonly List<DeliveryContract> TruckerContracts = new List<DeliveryContract> {
            new DeliveryContract {Type = DeliveryContractType.Corn, TargetPosition = new Vector3(2842.14, 4702.57, 45.94)},
            new DeliveryContract {Type = DeliveryContractType.Crude, TargetPosition = new Vector3(2687.37, 1367.84, 23.52)},
            new DeliveryContract {Type = DeliveryContractType.Tools, TargetPosition = new Vector3(-593.64, 5298.67, 69.21)},
            new DeliveryContract {Type = DeliveryContractType.WorkshopWood, TargetPosition = new Vector3(1379.25, -2065.66, 51.49)},
            new DeliveryContract {Type = DeliveryContractType.BuildingsWood, TargetPosition = new Vector3(-494.59, -956.44, 22.45)},
            new DeliveryContract {Type = DeliveryContractType.Dyes, TargetPosition = new Vector3(749.23, -1353.32, 25.86)},
            new DeliveryContract {Type = DeliveryContractType.Fuel, TargetPosition = new Vector3(-68.84, -1747.88, 27.97)},
            new DeliveryContract {Type = DeliveryContractType.Products, TargetPosition = new Vector3(158.92, -1465.83, 27.64)},
            new DeliveryContract {Type = DeliveryContractType.ScrapMetal, TargetPosition = new Vector3(-456.83, -1715.34, 18.14)}
        };

        private readonly IPointCreator _pointCreator;
        private readonly IVehicleManager _vehicleManager;
        private readonly ITruckersManager _truckersManager;

        public Port() : this(ServerKernel.Kernel) {
            API.onPlayerEnterVehicle += OnEnterTruck;
        }

        public Port(IResolutionRoot kernel) {
            _pointCreator = kernel.Get<IPointCreator>();
            _vehicleManager = kernel.Get<IVehicleManager>();
            _truckersManager = kernel.Get<ITruckersManager>();

            ActionHelper.StartTimer(600000, RefreshContracts);
        }

        /// <summary>
        /// Инизиализировать морской торговый порт
        /// </summary>
        public override void Initialize() {
            foreach (var truckerNpc in TruckersDataGetter.TruckerNpcs) {
                _pointCreator.CreateBlip(truckerNpc.Position, 477, 21, name: "Грузоперевозка");
                var ped = _pointCreator.CreatePed(
                    truckerNpc.Hash, truckerNpc.Name, truckerNpc.Position,
                    truckerNpc.Rotation, truckerNpc.MarkerPosition, Colors.VividCyan
                );
                ped.ColShape.onEntityEnterColShape += (shape, entity) => PlayerComeToNpc(entity, truckerNpc.Contracts);
                ped.ColShape.onEntityExitColShape += (shape, entity) => PlayerAwayFromNpc(entity);
            }
            CreateTrucks();
            _truckersManager.Initialize();
        }

        /// <summary>
        /// Обработка входа в грузових
        /// </summary>
        private void OnEnterTruck(Client player, NetHandle vehicle, int seat) {
            if (!API.hasEntityData(vehicle, TRUCK_KEY)) {
                return;
            }
            if (player.hasSyncedData(WorkData.IS_TRUCKER)) {
                ActionHelper.SetAction(player, 6000, () => 
                    API.triggerClientEvent(player, ServerEvent.SHOW_SUBTITLE, "~y~O~w~ - восстановить маршрут, ~y~H~w~ (зажать) - прицеп", 8000)
                );
                return;
            }
            API.sendNotificationToPlayer(player, "~r~Вы не работаете дальнобойщиком", true);
            player.warpOutOfVehicle();
        }

        /// <summary>
        /// Создает грузовые машины для дальнобойщиков
        /// </summary>
        private void CreateTrucks() {
            foreach (var truckInfo in TruckersDataGetter.TruckerPositions) {
                var truck = new CommonVehicle {
                    Hash = truckInfo.Hash,
                    VehicleType = TRUCK_KEY,
                    SpawnPosition = truckInfo.Position,
                    SpawnRotation = truckInfo.Rotation,
                    Fuel = MAX_FUEL,
                    MaxFuel = MAX_FUEL,
                    MainColor = truckInfo.IsTrailer ? 24 : 70,
                    SecondColor = 1
                };
                _vehicleManager.CreateVehicle(truck);
            }
        }

        /// <summary>
        /// Игрок подошел к нпс
        /// </summary>
        private void PlayerComeToNpc(NetHandle entity, ICollection<DeliveryContract> contracts) {
            PlayerHelper.ProcessAction(entity, player => {
                API.triggerClientEvent(player, ServerEvent.SHOW_TRUCKERS_MENU, JsonConvert.SerializeObject(contracts));
            });
        }

        /// <summary>
        /// Игрок отошел от Npc
        /// </summary>
        private void PlayerAwayFromNpc(NetHandle entity) {
            PlayerHelper.ProcessAction(entity, player => {
                API.triggerClientEvent(player, ServerEvent.HIDE_TRUCKERS_MENU);
            });
        }

        /// <summary>
        /// Обновляет награды контрактов
        /// </summary>
        private static void RefreshContracts() {
            foreach (var contract in TruckerContracts) {
                contract.ChangeReward();
            }
        }
    }
}