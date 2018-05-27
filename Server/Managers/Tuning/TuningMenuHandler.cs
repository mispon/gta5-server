using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_server.Constant;
using gta_mp_server.Enums.Vehicles;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles;
using gta_mp_server.Managers.Vehicles.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using VehicleInfo = gta_mp_database.Entity.Vehicle;

namespace gta_mp_server.Managers.Tuning {
    /// <summary>
    /// Обработчик меню тюнинга
    /// </summary>
    internal class TuningMenuHandler : Script, IMenu {
        private const int DEFAULT = -1;
        private const string EXCESS_STEP_MESSAGE = "~r~У вас не приобретены более ранние версии";
        private const string SUCCESS_MESSAGE = "~b~Тюнинг успешно установлен";
        private readonly byte[] _defaultNeon = {0 , 0, 0};

        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IVehicleInfoManager _vehicleInfoManager;

        /// <summary>
        /// Детали тюнинга, открывающиеся последовательно
        /// </summary>
        private static readonly HashSet<int> _slotsWithUpgrade = new HashSet<int> {
            (int) VehicleMod.Engine,
            (int) VehicleMod.Brakes,
            (int) VehicleMod.Transmission,
            (int) VehicleMod.Suspension,
            (int) VehicleMod.Armor,
        };

        public TuningMenuHandler() {}
        public TuningMenuHandler(IPlayerInfoManager playerInfoManager, IVehicleInfoManager vehicleInfoManager) {
            _playerInfoManager = playerInfoManager;
            _vehicleInfoManager = vehicleInfoManager;
        }

        /// <summary>
        /// Инициализировать обработчик меню
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.SET_VEHICLE_MOD, SelectOrBuyVehicleMod);
            ClientEventHandler.Add(ClientEvent.SET_NEON, SetNeon);
            ClientEventHandler.Add(ClientEvent.SET_ENGINE_POWER, SetEnginePower);
            ClientEventHandler.Add(ClientEvent.SET_VEHICLE_COLOR, SetVehicleColor);
            ClientEventHandler.Add(ClientEvent.REPAIR_VEHICLE, RepairVehicle);
            ClientEventHandler.Add(ClientEvent.EXIT_FROM_TUNING_GARAGE, ExitFromGarage);
        }

        /// <summary>
        /// Обработчик покупки тюнинга значение тюнинга детали
        /// </summary>
        private void SelectOrBuyVehicleMod(Client player, object[] args) {
            var price = (int) args[0];
            var slot = (int) args[1];
            var value = (int) args[2];
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (!PlayerHelper.EnoughMoney(player, playerInfo, price)) {
                return;
            }
            var vehicleInfo = GetVehicleInfo(player);
            var mods = vehicleInfo.Tuning.GetMods();
            if (mods.ContainsKey(slot)) {
                SetMod(player, mods, slot, value);
            }
            else {
                AddMod(player, mods, slot, value);
            }
            if (value != DEFAULT) {
                playerInfo.Balance -= price;
            }
            vehicleInfo.Tuning.SetMods(mods);
            _vehicleInfoManager.SetInfo(player, vehicleInfo);
            _playerInfoManager.RefreshUI(player, playerInfo);
        }

        /// <summary>
        /// Перезаписывает существующий тюнинг
        /// </summary>
        private void SetMod(Client player, IDictionary<int, int> mods, int slot, int value) {
            if (_slotsWithUpgrade.Contains(slot) && !ValueAcceptable(player, mods[slot], value)) {
                return;
            }
            mods[slot] = value;
            API.sendNotificationToPlayer(player, SUCCESS_MESSAGE);
        }

        /// <summary>
        /// Проверяет что новое значение допустимо
        /// </summary>
        private bool ValueAcceptable(Client player, int currentValue, int newValue, int step = 1) {
            if (currentValue >= newValue) {
                API.sendNotificationToPlayer(player, "~r~Выбранная или более мощная версия уже установлена", true);
                return false;
            }
            if (newValue >= currentValue + 2 * step) {
                API.sendNotificationToPlayer(player, EXCESS_STEP_MESSAGE, true);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Добавляет новый тюнинг
        /// </summary>
        private void AddMod(Client player, IDictionary<int, int> mods, int slot, int value) {
            if (_slotsWithUpgrade.Contains(slot) && value > 0) {
                API.sendNotificationToPlayer(player, EXCESS_STEP_MESSAGE, true);
                return;
            }
            mods.Add(slot, value);
            API.sendNotificationToPlayer(player, SUCCESS_MESSAGE);
        }

        /// <summary>
        /// Устанавливает неоновую подсветку
        /// </summary>
        private void SetNeon(Client player, object[] args) {
            var price = (int) args[0];
            var color = ParseColor(args);
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (!PlayerHelper.EnoughMoney(player, playerInfo, price)) {
                return;
            }
            var vehicleInfo = GetVehicleInfo(player);
            var currentColor = vehicleInfo.Tuning.GetNeonColor();
            if (NeonColorsEquals(currentColor, color)) {
                API.sendNotificationToPlayer(player, "~r~Выбранный цвет уже установлен", true);
                return;
            }
            if (!_defaultNeon.SequenceEqual(color)) {
                playerInfo.Balance -= price;
            }
            vehicleInfo.Tuning.SetNeonColor(color);
            _vehicleInfoManager.SetInfo(player, vehicleInfo);
            _playerInfoManager.RefreshUI(player, playerInfo);
            API.sendNotificationToPlayer(player, "~b~Неоновая подсветка успешно установлена");
        }

        /// <summary>
        /// Парсит rgb-цвет в массив
        /// </summary>
        private static byte[] ParseColor(object[] args) {
            return new[] {Convert.ToByte(args[1]), Convert.ToByte(args[2]), Convert.ToByte(args[3])};
        }

        /// <summary>
        /// Сравнивает значения цветов неона
        /// </summary>
        private static bool NeonColorsEquals(byte[] currentColor, byte[] newColor) {
            if (currentColor.Length == 0) {
                return false;
            }
            return currentColor.SequenceEqual(newColor);
        }

        /// <summary>
        /// Устанавливает мощность двигателя
        /// </summary>
        private void SetEnginePower(Client player, object[] args) {
            var price = (int) args[0];
            var power = (int) args[1];
            var playerInfo = _playerInfoManager.GetInfo(player);
            var vehicle = API.getPlayerVehicle(player);
            var currentPower = API.getVehicleEnginePowerMultiplier(vehicle);
            if (!PlayerHelper.EnoughMoney(player, playerInfo, price) || !ValueAcceptable(player, (int) currentPower, power, 10)) {
                return;
            }
            if (power <= currentPower) {
                API.sendNotificationToPlayer(player, "~r~Двигатель уже разогнан до выбранной или большей мощности", true);
                return;
            }
            playerInfo.Balance -= price;
            var vehicleInfo = GetVehicleInfo(player);
            vehicleInfo.Tuning.EnginePower = power;
            _vehicleInfoManager.SetInfo(player, vehicleInfo);
            _playerInfoManager.RefreshUI(player, playerInfo);
            API.setVehicleEnginePowerMultiplier(vehicle, power);
            API.sendNotificationToPlayer(player, "~b~Двигатель успешно разогнан");
        }

        /// <summary>
        /// Устанавливает цвет
        /// </summary>
        private void SetVehicleColor(Client player, object[] args) {
            var price = (int) args[0];
            var color = (int) args[1];
            var isMain = (bool) args[2];
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (!PlayerHelper.EnoughMoney(player, playerInfo, price)) {
                return;
            }
            var vehicleInfo = GetVehicleInfo(player);
            var currentColor = isMain ? vehicleInfo.Tuning.PrimaryColor : vehicleInfo.Tuning.SecondColor;
            if (currentColor == color) {
                API.sendNotificationToPlayer(player, "~r~Транспорт уже окрашен в выбранный цвет", true);
                return;
            }
            if (isMain) {
                vehicleInfo.Tuning.PrimaryColor = color;
            }
            else {
                vehicleInfo.Tuning.SecondColor = color;
            }
            playerInfo.Balance -= price;
            _vehicleInfoManager.SetInfo(player, vehicleInfo);
            _playerInfoManager.RefreshUI(player, playerInfo);
            API.sendNotificationToPlayer(player, "~b~Транспорт перекрашен в выбранный цвет");
        }

        /// <summary>
        /// Ремонтирует транспорт
        /// </summary>
        private void RepairVehicle(Client player, object[] args) {
            var price = (int) args[0];
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (!PlayerHelper.EnoughMoney(player, playerInfo, price)) {
                return;
            }
            playerInfo.Balance -= price;
            var vehicle = API.getPlayerVehicle(player);
            API.repairVehicle(vehicle);
            _playerInfoManager.RefreshUI(player, playerInfo);
            API.sendNotificationToPlayer(player, "~b~Транспорт отремонтирован");
        }

        /// <summary>
        /// Восстанавливает внешний вид транспорта
        /// </summary>
        private void ExitFromGarage(Client player, object[] args) {
            var vehicle = API.getEntityFromHandle<Vehicle>(API.getPlayerVehicle(player));
            while (vehicle.freezePosition) vehicle.freezePosition = false;
            var vehicleId = (long) vehicle.getData(VehicleManager.VEHICLE_ID);
            var vehicleInfo = _vehicleInfoManager.GetInfoById(player, vehicleId);
            VehicleManager.SetVehicleTuning(vehicle, vehicleInfo.Tuning);
            API.setEntityDimension(player, 0);
            API.setEntityDimension(vehicle, 0);
            API.setEntityPosition(vehicle, TuningGarage.AfterExitPosition);
            API.setEntityRotation(vehicle, TuningGarage.AfterExitRotation);
            API.sendNativeToPlayer(player, Hash.DISPLAY_RADAR, true);
            API.triggerClientEvent(player, ServerEvent.HIDE_HINT);
            player.resetSyncedData(TuningGarage.IN_TUNING_GARAGE);
        }

        /// <summary>
        /// Возвращает информацию транспорта
        /// </summary>
        private VehicleInfo GetVehicleInfo(Client player) {
            var vehicle = API.getPlayerVehicle(player);
            return _vehicleInfoManager.GetInfoByHandle(player, vehicle);
        }
    }
}