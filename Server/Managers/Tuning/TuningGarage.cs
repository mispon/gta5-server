using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using gta_mp_server.Constant;
using gta_mp_server.Enums.Vehicles;
using gta_mp_server.Global;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Models.Shops;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using Ninject;
using Ninject.Syntax;
using Marker = gta_mp_server.Enums.Marker;

namespace gta_mp_server.Managers.Tuning {
    /// <summary>
    /// Гараж тюнинга
    /// </summary>
    internal class TuningGarage : Place {
        internal const string IN_TUNING_GARAGE = "InTuningGarage";

        private readonly IPointCreator _pointCreator;
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IVehicleInfoManager _vehicleInfoManager;

        internal static readonly Vector3 AfterExitPosition = new Vector3(-360.15, -128.26, 38.16);
        internal static readonly Vector3 AfterExitRotation = new Vector3(-0.01, 0.00, 70.16);
        private static readonly Vector3 _afterEnterPosition = new Vector3(201.10, -997.93, -99.40);
        private static readonly Vector3 _afterEnterRotation = new Vector3(-0.08, 0.00, 93.09);

        public TuningGarage() : this(ServerKernel.Kernel) {
            API.onPlayerExitVehicle += (player, vehicle, seat) => {
                if (player.hasSyncedData(IN_TUNING_GARAGE)) {
                    player.setIntoVehicle(vehicle, seat);
                }
            };
        }

        public TuningGarage(IResolutionRoot kernel) {
            _pointCreator = kernel.Get<IPointCreator>();
            _playerInfoManager = kernel.Get<IPlayerInfoManager>();
            _vehicleInfoManager = kernel.Get<IVehicleInfoManager>();
        }

        /// <summary>
        /// Инизиализировать место
        /// </summary>
        public override void Initialize() {
            _pointCreator.CreateBlip(MainPosition.Tuning, 446, 47, name: "Автомастерская");
            var enter = _pointCreator.CreateMarker(Marker.VerticalCylinder, MainPosition.Tuning, Colors.Blue, 3f);
            enter.Marker.scale = new Vector3(3, 3, 1.5);
            enter.ColShape.onEntityEnterColShape += EnterToTuningGarage;
        }

        /// <summary>
        /// Обработчик входа в гараж тюнинга
        /// </summary>
        private void EnterToTuningGarage(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            Vehicle vehicle = null;
            if (!CanEnter(player, ref vehicle)) {
                return;
            }
            player.setSyncedData(IN_TUNING_GARAGE, true);
            var accountId = (int) _playerInfoManager.GetInfo(player).AccountId;
            API.setEntityDimension(player, -accountId);
            API.setEntityDimension(vehicle, -accountId);
            API.setEntityPosition(vehicle, _afterEnterPosition);
            API.setEntityRotation(vehicle, _afterEnterRotation);
            while (!vehicle.freezePosition) vehicle.freezePosition = true;
            var model = (VehicleHash) API.getEntityModel(vehicle);
            API.sendNativeToPlayer(player, Hash.DISPLAY_RADAR, false);
            API.triggerClientEvent(player, ServerEvent.SHOW_HINT, "Если случайно закрылось меню, нажмите О, чтобы снова открыть", 120);
            API.triggerClientEvent(player, ServerEvent.SHOW_TUNING_MENU, JsonConvert.SerializeObject(GetParsedMods(vehicle)), API.isVehicleACar(model));
        }

        /// <summary>
        /// Проверяет, что игрок может въехать в гараж
        /// </summary>
        private bool CanEnter(Client player, ref Vehicle vehicle) {
            if (player == null || !player.isInVehicle) {
                return false;
            }
            if (player.hasData(PlayerData.IS_REGISTERED)) {
                API.sendNotificationToPlayer(player, "~r~Вы зарегистрированы в эвенте", true);
                return false;
            }
            vehicle = API.getEntityFromHandle<Vehicle>(API.getPlayerVehicle(player));
            var vehicleInfo = _vehicleInfoManager.GetInfoByHandle(player, vehicle);
            if (vehicleInfo == null) {
                API.sendNotificationToPlayer(player, "~r~Нельзя тюнинговать данный транспорт", true);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Возвращает модель доступных тюнингов для транспорта
        /// </summary>
        private List<TuningInfo> GetParsedMods(NetHandle vehicle) {
            var result = new List<TuningInfo>();
            var vehicleHash = (VehicleHash) API.getEntityModel(vehicle);
            var vehicleName = API.getVehicleDisplayName(vehicleHash);
            var mods = API.getVehicleValidMods(vehicleHash);
            foreach (var mod in mods) {
                if (!Enum.IsDefined(typeof(VehicleMod), mod.Key)) {
                    File.AppendAllText("mods.txt", $"{mod.Key} - {vehicleName}\r\n");
                    continue;
                }
                var modType = (VehicleMod) mod.Key;
                var name = modType.GetDescription();
                var info = new TuningInfo {
                    Name = name,
                    Slot = mod.Key,
                    Price = TuningData.GetPrice(modType),
                    Values = mod.Value.Select(e => e.Key).ToList()
                };
                result.Add(info);
            }
            return result;
        }
    }
}