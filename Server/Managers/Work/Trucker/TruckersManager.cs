using System;
using System.Collections.Generic;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Helpers;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Places;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Managers.Work.Trucker.Interfaces;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using Ninject;
using Ninject.Syntax;

namespace gta_mp_server.Managers.Work.Trucker {
    /// <summary>
    /// Логика работы дальнобойщиком
    /// </summary>
    internal class TruckersManager : Script, ITruckersManager {
        internal const string TRUCKER_CONTRACT_TYPE = "TruckerPointType";
        private const string ON_TARGET = "OnTruckerTarget";
        private const string TRAILER_KEY = "TruckTrailer";

        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IWorkInfoManager _workInfoManager;
        private readonly IVehicleManager _vehicleManager;

        private static readonly Dictionary<int, WorkReward> _rewards = new Dictionary<int, WorkReward> {
            [1] = new WorkReward {Salary = 20, Exp = 32, WorkExp = 22},
            [2] = new WorkReward {Salary = 40, Exp = 34, WorkExp = 23},
            [3] = new WorkReward {Salary = 60, Exp = 36, WorkExp = 24},
            [4] = new WorkReward {Salary = 80, Exp = 38, WorkExp = 25},
            [5] = new WorkReward {Salary = 100, Exp = 40, WorkExp = 0}
        };

        public TruckersManager() : this(ServerKernel.Kernel) {
            API.onVehicleTrailerChange += OnTrailerChange;
        }

        public TruckersManager(IResolutionRoot kernel) {
            _playerInfoManager = kernel.Get<IPlayerInfoManager>();
            _workInfoManager = kernel.Get<IWorkInfoManager>();
            _vehicleManager = kernel.Get<IVehicleManager>();
        }

        /// <summary>
        /// Инициализация менеджера работы дальнобойщиком
        /// </summary>
        public void Initialize() {
            foreach (var contract in Port.TruckerContracts) {
                var point = API.createSphereColShape(contract.TargetPosition, 4f);
                point.setData(TRUCKER_CONTRACT_TYPE, contract.Type);
                point.onEntityEnterColShape += (shape, entity) => ProcessTargetColShape(entity, driver => {
                    var pointType = (DeliveryContractType) shape.getData(TRUCKER_CONTRACT_TYPE);
                    if (!driver.hasData(TRUCKER_CONTRACT_TYPE)) {
                        return;
                    }
                    var driverContractType = (DeliveryContractType) driver.getData(TRUCKER_CONTRACT_TYPE);
                    if (pointType != driverContractType) {
                        return;
                    }
                    driver.setData(ON_TARGET, true);
                    API.triggerClientEvent(driver, ServerEvent.SHOW_SUBTITLE, "Зажмите ~y~H~w~, чтобы сдать груз");
                });
                point.onEntityExitColShape += (shape, entity) => ProcessTargetColShape(entity, driver => driver.resetData(ON_TARGET));
            }
        }

        /// <summary>
        /// Игрок прибыл на точку доставки груза
        /// </summary>
        private void ProcessTargetColShape(NetHandle entity, Action<Client> action) {
            var vehicle = API.getEntityFromHandle<Vehicle>(entity);
            var driver = API.getVehicleDriver(vehicle);
            if (driver == null || !driver.hasSyncedData(WorkData.IS_TRUCKER)) {
                return;
            }
            action(driver);
        }

        /// <summary>
        /// Прицепление / отцепление фургона
        /// </summary>
        private void OnTrailerChange(NetHandle vehicleHandle, NetHandle trailer) {
            if (!API.hasEntityData(vehicleHandle, Port.TRUCK_KEY)) {
                return;
            }
            var truck = API.getEntityFromHandle<Vehicle>(vehicleHandle);
            var player = API.getVehicleDriver(truck);
            if (player == null) return;
            if (trailer.IsNull) {
                API.triggerClientEvent(player, ServerEvent.HIDE_TRUCKER_TARGET_POINT);
                if (!player.hasData(ON_TARGET)) {
                    return;
                }
                ProcessDelivery(player);
                player.resetData(TRAILER_KEY);
                player.resetData(TRUCKER_CONTRACT_TYPE);
            }
            else {
                ShowTargetPoint(player);
                player.setData(TRAILER_KEY, trailer);
                API.setEntityData(trailer, VehicleManager.DONT_RESTORE, true);
            }
        }

        /// <summary>
        /// Показать точку доставки груза
        /// </summary>
        private void ShowTargetPoint(Client player) {
            if (!player.hasData(WorkData.DELIVERY_CONTRACT)) {
                API.sendNotificationToPlayer(player, "~r~Необходимо заключить контракт", true);
                return;
            }
            var contract = (DeliveryContract) player.getData(WorkData.DELIVERY_CONTRACT);
            API.triggerClientEvent(player, ServerEvent.SHOW_TRUCKER_TARGET_POINT, contract.TargetPosition);
        }

        /// <summary>
        /// Обработка доставки груза
        /// </summary>
        private void ProcessDelivery(Client player) {
            var trailerHandle = (NetHandle) player.getData(TRAILER_KEY);
            API.resetEntityData(trailerHandle, VehicleManager.DONT_RESTORE);
            if (!player.hasData(ON_TARGET)) {
                API.sendNotificationToPlayer(player, "~b~Вы бросили груз");
                return;
            }
            SetReward(player);
            player.resetData(WorkData.DELIVERY_CONTRACT);
            var trailer = API.getEntityFromHandle<Vehicle>(trailerHandle);
            if (trailer.hasData(VehicleManager.COMMON_VEHICLE)) {
                _vehicleManager.RestorePosition(trailer);
            }
        }

        /// <summary>
        /// Выдает награду за доставку груза
        /// </summary>
        private void SetReward(Client player) {
            var workLevel = _workInfoManager.GetWorkInfo(player, WorkType.Trucker).Level;
            var contract = PlayerHelper.GetData<DeliveryContract>(player, WorkData.DELIVERY_CONTRACT, null);
            var reward = _rewards[workLevel];
            _workInfoManager.SetSalary(player, WorkType.Trucker, contract.Reward + reward.Salary);
            _workInfoManager.SetExperience(player, WorkType.Trucker, reward.WorkExp);
            _playerInfoManager.SetExperience(player, reward.Exp);
            API.sendNotificationToPlayer(player, $"~g~Бонус от уровня работы: {reward.Salary}$");
        }
    }
}