using System.Collections.Generic;
using System.Linq;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Managers.Work.Police;
using gta_mp_server.Managers.Work.Police.Data;
using gta_mp_server.Managers.Work.Police.Interfaces;
using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using Ninject;
using Ninject.Syntax;

namespace gta_mp_server.Managers.Places {
    /// <summary>
    /// Департамент полиции
    /// </summary>
    internal class PoliceDepartment : Place {
        private const float MAX_FUEL = 70;
        internal const string POLICE_VEHICLE_KEY = "PoliceVehicle";

        private readonly IPointCreator _pointCreator;
        private readonly IVehicleManager _vehicleManager;
        private readonly IWorkInfoManager _workInfoManager;
        private readonly IPoliceManager _policeManager;
        private readonly IJailManager _jailManager;

        public PoliceDepartment() : this(ServerKernel.Kernel) {
            API.onPlayerEnterVehicle += OnEnterPoliceVehicle;
            API.onPlayerExitVehicle += OnExitPoliceVehicle;
        }

        public PoliceDepartment(IResolutionRoot kernel) {
            _pointCreator = kernel.Get<IPointCreator>();
            _vehicleManager = kernel.Get<IVehicleManager>();
            _workInfoManager = kernel.Get<IWorkInfoManager>();
            _policeManager = kernel.Get<IPoliceManager>();
            _jailManager = kernel.Get<IJailManager>();
        }

        /// <summary>
        /// Инизиализировать место
        /// </summary>
        public override void Initialize() {
            CreateVehicles();
            _pointCreator.CreateBlip(MainPosition.PoliceDepartment, 526, 53, name: "Департамент полиции");
            _pointCreator.CreateBlip(MainPosition.Jail, 188, 53, name: "Тюрьма");
            _policeManager.Initialize();
            _jailManager.Initialize();
        }

        /// <summary>
        /// Обработчик входа в полицейскую машину
        /// </summary>
        private void OnEnterPoliceVehicle(Client player, NetHandle vehicle, int seat) {
            if (!API.hasEntityData(vehicle, POLICE_VEHICLE_KEY)) {
                return;
            }
            if (seat < 0 && !PlayerCanEnter(player, vehicle)) {
                API.warpPlayerOutOfVehicle(player);
            }
        }

        /// <summary>
        /// Обработчик выхода из полицейской машины
        /// </summary>
        private void OnExitPoliceVehicle(Client player, NetHandle vehicle, int seat) {
            if (!API.hasEntityData(vehicle, POLICE_VEHICLE_KEY) || player.hasData(WorkData.IS_POLICEMAN)) {
                return;
            }
            if (seat > 0 && !PrisonerCanExit(player)) {
                player.setIntoVehicle(vehicle, seat);
                return;
            }
            player.resetData(PoliceManager.PULL_OUT);
            API.setEntityInvincible(player, false);
        }

        /// <summary>
        /// Проверяет, что задержанный игрок может выйти из машины
        /// </summary>
        private bool PrisonerCanExit(Client prisoner) {
            return _policeManager.GetAttachedPlayer(prisoner) == null || PlayerHelper.GetData(prisoner, PoliceManager.PULL_OUT, false);
        }

        /// <summary>
        /// Проверка, что игрок может ездить на такой машине
        /// </summary>
        private bool PlayerCanEnter(Client player, NetHandle vehicle) {
            const int police3MinLvl = 3;
            const int police2MinLvl = 4;
            if (!PlayerHelper.GetData(player, WorkData.IS_POLICEMAN, false)) {
                API.sendNotificationToPlayer(player, "~r~Вы не работаете в полиции", true);
                return false;
            }
            var workInfo = _workInfoManager.GetWorkInfo(player, WorkType.Police);
            if (VehicleHash.Police3 == (VehicleHash) API.getEntityModel(vehicle) && workInfo.Level < police3MinLvl) {
                API.sendNotificationToPlayer(player, $"~r~Необходим {police3MinLvl}-й уровень работы и выше", true);
                return false;
            }
            if (VehicleHash.Police2 == (VehicleHash) API.getEntityModel(vehicle) && workInfo.Level < police2MinLvl) {
                API.sendNotificationToPlayer(player, $"~r~Необходим {police2MinLvl}-й уровень работы и выше", true);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Создает машины полиции
        /// </summary>
        private void CreateVehicles() {
            var vehicles = GetVehiclesInfo();
            foreach (var vehicle in vehicles) {
                _vehicleManager.CreateVehicle(vehicle);
            }
        }

        /// <summary>
        /// Возвращает информацию об машинах полиции
        /// </summary>
        private static IEnumerable<CommonVehicle> GetVehiclesInfo() {
            var infos = PoliceDataGetter.GetVehiclePositions();
            return infos.Select(info => new CommonVehicle {
                Hash = info.Hash,
                VehicleType = POLICE_VEHICLE_KEY,
                SpawnPosition = info.Position,
                SpawnRotation = info.Rotation,
                Fuel = MAX_FUEL,
                MaxFuel = MAX_FUEL,
                MainColor = 111
            }).ToList();
        }
    }
}