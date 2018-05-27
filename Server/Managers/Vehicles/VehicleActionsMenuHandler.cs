using System.Linq;
using gta_mp_data.Entity;
using gta_mp_server.Clan.Mission;
using gta_mp_server.Constant;
using gta_mp_server.Enums.Vehicles;
using gta_mp_server.Global;
using gta_mp_server.Managers.MenuHandlers;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player;
using gta_mp_server.Managers.Player.Helpers;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using Newtonsoft.Json;

namespace gta_mp_server.Managers.Vehicles {
    /// <summary>
    /// Действия с машиной
    /// </summary>
    internal class VehicleActionsMenuHandler : Script, IMenu {
        private const float SEARCH_RADIUS = 3.5f;

        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IVehicleManager _vehicleManager;
        private readonly IVehicleInfoManager _vehicleInfoManager;
        private readonly IVehicleTrunkManager _vehicleTrunkManager;

        public VehicleActionsMenuHandler() {}
        public VehicleActionsMenuHandler(IPlayerInfoManager playerInfoManager, IVehicleManager vehicleManager,
            IVehicleInfoManager vehicleInfoManager, IVehicleTrunkManager vehicleTrunkManager) {
            _playerInfoManager = playerInfoManager;
            _vehicleManager = vehicleManager;
            _vehicleInfoManager = vehicleInfoManager;
            _vehicleTrunkManager = vehicleTrunkManager;
        }

        /// <summary>
        /// Проинициализировать меню управления транспортом
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.TRIGGER_VEHICLE_ACTION_MENU, TriggerVehicleMenu);
            ClientEventHandler.Add(ClientEvent.TURN_ENGINE, TriggerEngine);
            ClientEventHandler.Add(ClientEvent.LOCK_VEHICLE, OnLockTrigger);
            ClientEventHandler.Add(ClientEvent.CHANGE_DOOR_STATE, ChangeDoorState);
            ClientEventHandler.Add(ClientEvent.PUT_IN_TRUNK, PutInTrunk);
            ClientEventHandler.Add(ClientEvent.TAKE_FROM_TRUNK, TakeFromTrunk);
            ClientEventHandler.Add(ClientEvent.TRIGGER_VANS_TRUNK, TriggerVansTrunk);
        }

        /// <summary>
        /// Открыть меню управления транспорта
        /// </summary>
        private void TriggerVehicleMenu(Client player, object[] args) {
            var isOpen = (bool) args[0];
            if (!isOpen) {
                API.triggerClientEvent(player, ServerEvent.HIDE_VEHICLE_ACTION_MENU);
                return;
            }
            var vehicle = _vehicleManager.GetNearestVehicle(player, SEARCH_RADIUS);
            if (vehicle == null) {
                API.sendNotificationToPlayer(player, "~r~Поблизости нет транспорта", true);
                return;
            }
            if (IsVehicleOwner(player, vehicle) && !player.isInVehicle && !RentOfScootersMenuHandler.IsScooter(vehicle.model)) {
                ShowMenuWithTrunk(player, vehicle);
            }
            else {
                API.triggerClientEvent(player, ServerEvent.SHOW_VEHICLE_ACTION_MENU, (VehicleHash) vehicle.model == VehicleHash.Burrito3);
            }
        }

        /// <summary>
        /// Загрузить инвентарь и багажник
        /// </summary>
        private void ShowMenuWithTrunk(Client player, Vehicle vehicle) {
            var inventory = _playerInfoManager.GetInfo(player).Inventory.Where(e => e.Count > 0);
            var trunk = _vehicleInfoManager.GetInfoByHandle(player, vehicle).GetTrunk().Where(e => e.Count > 0);
            var weight = InventoryHelper.CalculateWeight(trunk);
            var carrying = VehicleManager.GetCarrying((VehicleHash) vehicle.model);
            API.triggerClientEvent(
                player, ServerEvent.SHOW_VEHICLE_ACTION_MENU, JsonConvert.SerializeObject(inventory), 
                JsonConvert.SerializeObject(trunk), (int) weight, carrying
            );
        }

        /// <summary>
        /// Управление двигателем
        /// </summary>
        private void TriggerEngine(Client player, object[] args) {
            if (!player.isInVehicle) {
                API.sendNotificationToPlayer(player, "~r~Вы не находитесь в транспортe", true);
                return;
            }
            var vehicle = API.getPlayerVehicle(player);
            var vehicleFuel = API.getVehicleFuelLevel(vehicle);
            if (vehicleFuel <= 0f) {
                API.sendNotificationToPlayer(player, "~r~Бензобак пуст");
                return;
            }
            var status = API.getVehicleEngineStatus(vehicle);
            API.setVehicleEngineStatus(vehicle, !status);
        }

        /// <summary>
        /// Обработчик открытия / закрытия машины
        /// </summary>
        private void OnLockTrigger(Client player, object[] args) {
            var vehicle = _vehicleManager.GetNearestVehicle(player, SEARCH_RADIUS, VehicleManager.OWNER_ID);
            if (vehicle == null) {
                API.sendNotificationToPlayer(player, "~r~Рядом нет доступного транспорта", true);
                return;
            }
            if (player.isInVehicle) {
                API.sendNotificationToPlayer(player, "~r~Необходимо покинуть транспорт", true);
                return;
            }
            var unlocked = API.getVehicleLocked(vehicle);
            var actionName = unlocked ? "открыт" : "закрыт";
            if (!IsVehicleOwner(player, vehicle)) {
                API.sendNotificationToPlayer(player, $"~r~Нельзя {actionName}ь чужой транспорт", true);
                return;
            }
            API.setVehicleLocked(vehicle, !unlocked);
            API.sendNotificationToPlayer(player, $"Транспорт ~b~{actionName}");
            if (!API.isVehicleABicycle((VehicleHash) vehicle.model)) {
                API.triggerClientEvent(player, ClientEvent.CAR_LOCK_SOUND);
            }
        }

        /// <summary>
        /// Изменяет состояние двери
        /// </summary>
        private void ChangeDoorState(Client player, object[] args) {
            var door = (int) args[0];
            var needOpen = (bool) args[1];
            var vehicle = _vehicleManager.GetNearestVehicle(player, SEARCH_RADIUS, VehicleManager.OWNER_ID);
            if (vehicle == null) {
                return;
            }
            if (needOpen)
                vehicle.openDoor(door);
            else
                vehicle.closeDoor(door);
        }

        /// <summary>
        /// Положить вещи в багажник
        /// </summary>
        private void PutInTrunk(Client player, object[] args) {
            var vehicle = _vehicleManager.GetNearestVehicle(player, SEARCH_RADIUS, VehicleManager.OWNER_ID);
            if (vehicle == null || !IsVehicleOwner(player, vehicle)) {
                API.sendColoredNotificationToPlayer(player, "Рядом нет доступного транспорта", 0, 6);
                return;
            }
            var item = JsonConvert.DeserializeObject<InventoryItem>(args[0].ToString());
            var count = (int) args[1];
            var result = _vehicleTrunkManager.PutInTrunk(player, vehicle, item, count);
            if (result) {
                vehicle.closeDoor((int)VehicleDoor.Trunk);
                API.sendColoredNotificationToPlayer(player, "Вы положили предметы в багажник", 0, 116);
                ShowMenuWithTrunk(player, vehicle);
            }
        }

        /// <summary>
        /// Положить вещи в багажник
        /// </summary>
        private void TakeFromTrunk(Client player, object[] args) {
            var vehicle = _vehicleManager.GetNearestVehicle(player, SEARCH_RADIUS, VehicleManager.OWNER_ID);
            if (vehicle == null || !IsVehicleOwner(player, vehicle)) {
                API.sendColoredNotificationToPlayer(player, "Рядом нет доступного транспорта", 0, 6);
                return;
            }
            var item = JsonConvert.DeserializeObject<InventoryItem>(args[0].ToString());
            var count = (int) args[1];
            var result = _vehicleTrunkManager.TakeFromTrunk(player, vehicle, item, count);
            if (result) {
                vehicle.closeDoor((int) VehicleDoor.Trunk);
                API.sendColoredNotificationToPlayer(player, "Вы забрали предметы из багажника", 0, 116);
                ShowMenuWithTrunk(player, vehicle);
            }
        }

        /// <summary>
        /// Обработчик багажника фургона банд
        /// </summary>
        private void TriggerVansTrunk(Client player, object[] args) {
            var vehicle = _vehicleManager.GetNearestVehicle(player, SEARCH_RADIUS);
            if (vehicle == null || (VehicleHash) vehicle.model != VehicleHash.Burrito3) {
                return;
            }
            if (player.isInVehicle) {
                API.sendNotificationToPlayer(player, "~r~Необходимо выйти из фургона", true);
                return;
            }
            var isOpen = vehicle.isDoorOpen((int) VehicleDoor.BackLeft) || vehicle.isDoorOpen((int) VehicleDoor.BackRight);
            if (isOpen)
                CloseVansDoor(vehicle);
            else
                OpenVansDoor(vehicle);
        }

        /// <summary>
        /// Закрывает дверь фургона
        /// </summary>
        private static void CloseVansDoor(Vehicle vehicle) {
            vehicle.closeDoor((int) VehicleDoor.BackLeft);
            vehicle.closeDoor((int) VehicleDoor.BackRight);
            if (vehicle.hasData(ClanMission.VANS_SHAPE_KEY)) {
                var shape = (ColShape)vehicle.getData(ClanMission.VANS_SHAPE_KEY);
                API.shared.deleteColShape(shape);
                vehicle.resetData(ClanMission.VANS_SHAPE_KEY);
            }
        }

        /// <summary>
        /// Открывает дверь фургона
        /// </summary>
        private static void OpenVansDoor(Vehicle vehicle) {
            vehicle.openDoor((int) VehicleDoor.BackLeft);
            vehicle.openDoor((int) VehicleDoor.BackRight);
            var shape = ClanMission.CreateVansShape();
            shape.attachToEntity(vehicle);
            vehicle.setData(ClanMission.VANS_SHAPE_KEY, shape);
            shape.setData(ClanMission.VANS_SHAPE_KEY, vehicle);
        }

        /// <summary>
        /// Проверяет, что игрок является владельцем транспорта
        /// </summary>
        private static bool IsVehicleOwner(Client player, Vehicle vehicle) {
            if (!vehicle.hasData(VehicleManager.OWNER_ID)) {
                return false;
            }
            var accountId = (long) player.getData(PlayerInfoManager.ID_KEY);
            var vehicleOwnerId = (long) vehicle.getData(VehicleManager.OWNER_ID);
            return accountId == vehicleOwnerId;
        }
    }
}