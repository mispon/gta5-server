using System;
using System.Linq;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Places;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Managers.Work.Interfaces;
using gta_mp_server.Managers.Work.Wrecker;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using Vehicle = gta_mp_database.Entity.Vehicle;

namespace gta_mp_server.Managers.MenuHandlers.Work {
    /// <summary>
    /// Обработчик меню штрафстоянки
    /// </summary>
    internal class ParkingFineMenuHandler : BaseWorkMenu {
        private const int MIN_LEVEL = 6;

        private readonly IWorkEquipmentManager _workEquipmentManager;
        private readonly IVehicleManager _vehicleManager;
        private readonly IVehicleInfoManager _vehicleInfoManager;

        public ParkingFineMenuHandler() {}
        public ParkingFineMenuHandler(IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager, IWorkEquipmentManager workEquipmentManager,
            IVehicleManager vehicleManager, IVehicleInfoManager vehicleInfoManager) : base(playerInfoManager, workInfoManager) {
            _workEquipmentManager = workEquipmentManager;
            _vehicleManager = vehicleManager;
            _vehicleInfoManager = vehicleInfoManager;
        }

        /// <summary>
        /// Инициализировать обработчик меню работы
        /// </summary>
        public override void Initialize() {
            ClientEventHandler.Add(ClientEvent.GET_VEHICLE_FROM_PARKING_FINE, GetVehicleFromParkingFine);
            ClientEventHandler.Add(ClientEvent.WRECKER, WorkAsWrecker);
            ClientEventHandler.Add(ClientEvent.WRECKER_SALARY, GetSalary);
        }

        /// <summary>
        /// Забрать машину со штрафстоянки
        /// </summary>
        private void GetVehicleFromParkingFine(Client player, object[] args) {
            var freePlace = GetFreePlace();
            if (freePlace == null) {
                API.sendNotificationToPlayer(player, "~r~В данный момент на стоянке нет свободных мест", true);
                return;
            }
            var price = (int) args[1];
            var playerInfo = PlayerInfoManager.GetInfo(player);
            if (playerInfo.Balance < price) {
                API.sendNotificationToPlayer(player, "~r~Недостаточно средств", true);
                return;
            }
            playerInfo.Balance -= price;
            var vehicleInfo = JsonConvert.DeserializeObject<Vehicle>(args[0].ToString());
            vehicleInfo.IsSpawned = true;
            vehicleInfo.OnParkingFine = false;
            _vehicleManager.CreateVehicle(vehicleInfo, freePlace.Item1, freePlace.Item2);
            _vehicleInfoManager.SetInfo(player, vehicleInfo);
            API.triggerClientEvent(player, ServerEvent.HIDE_PARKING_FINE_MENU);
        }

        /// <summary>
        /// Возвращает свободное место на парковке
        /// </summary>
        private Tuple<Vector3, Vector3> GetFreePlace() {
            var occupiedPlaces = VehicleManager.GetAllVehicles()
                .Where(e => e.hasData(ParkingFine.ON_PARKING_FINE))
                .Select(e => e.position).ToList();
            return WreckerPositionsGetter.SpawnPlaces.FirstOrDefault(e => Parking.PlaceFree(occupiedPlaces, e.Item1));
        }

        /// <summary>
        /// Работать эвакуаторщиком
        /// </summary>
        private void WorkAsWrecker(Client player, object[] args) {
            WorkInfoManager.CreateInfoIfNeed(player, WorkType.Wrecker);
            if (!CanWork(player, MIN_LEVEL) || HasActiveWork(player)) {
                return;
            }
            player.setData(WorkData.IS_WRECKER, true);
            WorkInfoManager.SetActivity(player, WorkType.Wrecker, true);
            _workEquipmentManager.SetEquipment(player);
            API.sendNotificationToPlayer(player, "Вы устроились ~b~водителем эвакуатора");
            API.triggerClientEvent(player, ServerEvent.SHOW_SUBTITLE, "Чтобы начать работу, садитесь в ~y~эвакуатор");
            API.triggerClientEvent(player, ServerEvent.HIDE_PARKING_FINE_MENU);
        }

        /// <summary>
        /// Получить зарплату
        /// </summary>
        private void GetSalary(Client player, object[] args) {
            var activeWork = WorkInfoManager.GetActiveWork(player);
            if (!WorkIsCorrect(player, activeWork, () => activeWork.Type == WorkType.Wrecker)) {
                return;
            }
            WorkInfoManager.SetActivity(player, WorkType.Wrecker, false);
            player.resetData(WorkData.IS_WRECKER);
            PayOut(player, activeWork);
            PlayerInfoManager.SetPlayerClothes(player);
            API.triggerClientEvent(player, ServerEvent.HIDE_PARKING_FINE_MENU);
        }
    }
}