using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Places;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Managers.Work.Wrecker.Interfaces;
using gta_mp_server.Models.Utils;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using Newtonsoft.Json;
using Ninject;
using Ninject.Syntax;

namespace gta_mp_server.Managers.Work.Wrecker {
    /// <summary>
    /// Логика работы эвакуаторщиком
    /// </summary>
    internal class WreckerManager : Script, IWreckerManager {
        internal const int AFK_MINUTES = 60;
        private const string LAST_TRAILER_KEY = "LastWreckersTrailer";
        private const string ON_DROP_ZONE = "OnDropZone";

        private static readonly Dictionary<int, WorkReward> _rewards = new Dictionary<int, WorkReward> {
            [1] = new WorkReward {Salary = 44, Exp = 26, WorkExp = 12},
            [2] = new WorkReward {Salary = 48, Exp = 28, WorkExp = 13},
            [3] = new WorkReward {Salary = 64, Exp = 30, WorkExp = 14},
            [4] = new WorkReward {Salary = 72, Exp = 32, WorkExp = 15},
            [5] = new WorkReward {Salary = 82, Exp = 34, WorkExp = 0}
        };
        private static readonly WorkReward _penalty = new WorkReward {Salary = -100, Exp = 0, WorkExp = 0};

        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IWorkInfoManager _workInfoManager;
        private readonly IVehicleInfoManager _vehicleInfoManager;
        private readonly IVehicleManager _vehicleManager;

        public WreckerManager() : this(ServerKernel.Kernel) {
            API.onVehicleTrailerChange += OnTrailerChange;
        }

        public WreckerManager(IResolutionRoot kernel) {
            _playerInfoManager = kernel.Get<IPlayerInfoManager>();
            _workInfoManager = kernel.Get<IWorkInfoManager>();
            _vehicleInfoManager = kernel.Get<IVehicleInfoManager>();
            _vehicleManager = kernel.Get<IVehicleManager>();
        }

        /// <summary>
        /// Проинициализировать работу эвакуаторщиком
        /// </summary>
        public void Initialize() {
            var dropZone = API.createSphereColShape(WreckerPositionsGetter.DropZonePosition, 4f);
            dropZone.onEntityEnterColShape += (shape, entity) => ProcessDropZone(entity, driver => {
                driver.setData(ON_DROP_ZONE, true);
                API.triggerClientEvent(driver, ServerEvent.SHOW_SUBTITLE, "Зажмите ~y~H~w~, чтобы припарковать машину");
            });
            dropZone.onEntityExitColShape += (shape, entity) => ProcessDropZone(entity, driver => {
                driver.resetData(ON_DROP_ZONE);
                API.triggerClientEvent(driver, ServerEvent.HIDE_HINT);
            });
        }

        /// <summary>
        /// Игрок прибыл на точку доставки груза
        /// </summary>
        private void ProcessDropZone(NetHandle entity, Action<Client> action) {
            var vehicle = API.getEntityFromHandle<Vehicle>(entity);
            var player = API.getVehicleDriver(vehicle) ?? API.getPlayerFromHandle(entity);
            if (player == null || !player.hasData(WorkData.IS_WRECKER)) {
                return;
            }
            action(player);
        }

        /// <summary>
        /// Обработчик событий с прицепом
        /// </summary>
        private void OnTrailerChange(NetHandle vehicleHandle, NetHandle trailerHandle) {
            var towTruck = API.getEntityFromHandle<Vehicle>(vehicleHandle);
            if (!towTruck.hasData(ParkingFine.TOW_TRUCK_KEY)) {
                return;
            }
            var wrecker = API.getVehicleDriver(towTruck);
            var trailer = GetTrailer(towTruck, trailerHandle);
            var canEvacuate = trailer.hasData(VehicleManager.OWNER_ID);
            if (trailerHandle.IsNull) {
                ProcessEvacuation(wrecker, trailer);
                API.triggerClientEvent(wrecker, ServerEvent.HIDE_DROP_ZONE);
            }
            else {
                if (!canEvacuate) {
                    API.sendNotificationToPlayer(wrecker, "~r~Эвакуация данного транспорта штрафуется", true);
                    return;
                }
                API.triggerClientEvent(wrecker, ServerEvent.SHOW_DROP_ZONE, JsonConvert.SerializeObject(WreckerPositionsGetter.DropZonePosition));
            }
            if (!canEvacuate) return;
            var eventName = GetEventName(trailerHandle, trailer);
            TriggerWreckersEvent(trailer, eventName);
        }

        /// <summary>
        /// Возвращает эвакуируемый транспорт
        /// </summary>
        private Vehicle GetTrailer(Vehicle towTruck, NetHandle trailerHandle) {
            Vehicle trailer;
            if (trailerHandle.IsNull) {
                trailer = (Vehicle) towTruck.getData(LAST_TRAILER_KEY);
                towTruck.resetData(LAST_TRAILER_KEY);
            }
            else {
                trailer = API.getEntityFromHandle<Vehicle>(trailerHandle);
                towTruck.setData(LAST_TRAILER_KEY, trailer);
            }
            return trailer;
        }

        /// <summary>
        /// Обработать эвакуацию
        /// </summary>
        private void ProcessEvacuation(Client player, Vehicle trailer) {
            if (!player.hasData(ON_DROP_ZONE)) return;
            var evacuationResult = CheckEvacuation(trailer);
            SetReward(player, evacuationResult);
            if (trailer.hasData(VehicleManager.COMMON_VEHICLE)) {
                API.sendNotificationToPlayer(player, "~r~Нельзя эвакуировать коммунальный транспорт");
                _vehicleManager.RestorePosition(trailer);
                return;
            }
            if (API.getVehicleDriver(trailer) != null) {
                API.sendNotificationToPlayer(player, "~r~Нельзя эвакуировать транспорт с водителем внутри");
                return;
            }
            var vehicleInfo = _vehicleInfoManager.GetInfoByHandle(trailer);
            if (vehicleInfo == null) {
                API.sendNotificationToPlayer(player, "~r~Нельзя эвакуировать данный транспорт");
                return;
            }
            if (evacuationResult) 
                vehicleInfo.OnParkingFine = true;
            else {
                API.sendNotificationToPlayer(player, "~r~Можно эвакуировать только брошенный транспорт!");
                vehicleInfo.OnParkingFine = false;
            }
            vehicleInfo.IsSpawned = false;
            trailer.delete();
            _vehicleInfoManager.SetInfo(vehicleInfo);
        }

        /// <summary>
        /// Выдает награду игроку
        /// </summary>
        private void SetReward(Client player, bool evacuationCorrect) {
            var workLevel = _workInfoManager.GetWorkInfo(player, WorkType.Wrecker).Level;
            var reward = evacuationCorrect ? _rewards[workLevel] : _penalty;
            _workInfoManager.SetSalary(player, WorkType.Wrecker, reward.Salary);
            _workInfoManager.SetExperience(player, WorkType.Wrecker, reward.WorkExp);
            _playerInfoManager.SetExperience(player, reward.Exp);
        }

        /// <summary>
        /// Возвращает имя вызываемого события
        /// </summary>
        private static string GetEventName(NetHandle trailerHandle, Vehicle trailer) {
            if (!CheckEvacuation(trailer)) {
                return string.Empty;
            }
            return trailerHandle.IsNull ? ServerEvent.ADD_AFK_VEHICLE : ServerEvent.REMOVE_AFK_VEHICLE;
        }

        /// <summary>
        /// Проверяет, что эвакуация транспорта разрешена
        /// </summary>
        private static bool CheckEvacuation(Vehicle vehicle) {
            return vehicle.hasData(VehicleManager.OWNER_ID) && VehicleManager.IsAfk(vehicle, AFK_MINUTES);
        }

        /// <summary>
        /// Разослать событие всем эвакуаторщикам при изменении состояния брошенного транспорта
        /// </summary>
        private void TriggerWreckersEvent(Vehicle trailer, string eventName) {
            if (string.IsNullOrEmpty(eventName)) {
                return;
            }
            var wreckers = API.getAllPlayers().Where(e => e.hasData(WorkData.IS_WRECKER));
            var afkVehicle = JsonConvert.SerializeObject(CreateAfkVehicleModel(trailer));
            foreach (var wrecker in wreckers) {
                API.triggerClientEvent(wrecker, eventName, afkVehicle);
            }
        }

        /// <summary>
        /// Создает модель брошенного транспорта
        /// </summary>
        private static AfkVehicle CreateAfkVehicleModel(Vehicle trailer) {
            return new AfkVehicle {
                Id = (long) trailer.getData(VehicleManager.VEHICLE_ID),
                Position = trailer.position
            };
        }
    }
}