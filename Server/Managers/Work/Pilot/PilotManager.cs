using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_data.Enums;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Global;
using gta_mp_server.Managers.MenuHandlers.Work;
using gta_mp_server.Managers.Places.AirPorts;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Pilot.Interfaces;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;

namespace gta_mp_server.Managers.Work.Pilot {
    /// <summary>
    /// Логика работы пилотом
    /// </summary>
    internal class PilotManager : Script, IPilotManager {
        private const string POINT_CONTRACT_TYPE = "PointContractType";
        private const string ON_TARGET = "OnPilotTarget";

        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IWorkInfoManager _workInfoManager;
        private readonly IClanManager _clanManager;

        private static readonly Dictionary<int, WorkReward> _rewards = new Dictionary<int, WorkReward> {
            [1] = new WorkReward { Salary = 30, Exp = 28, WorkExp = 14 },
            [2] = new WorkReward { Salary = 40, Exp = 30, WorkExp = 15 },
            [3] = new WorkReward { Salary = 60, Exp = 46, WorkExp = 16 },
            [4] = new WorkReward { Salary = 80, Exp = 48, WorkExp = 17 },
            [5] = new WorkReward { Salary = 120, Exp = 50, WorkExp = 0 }
        };

        public PilotManager() {}
        public PilotManager(IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager, IClanManager clanManager) {
            _playerInfoManager = playerInfoManager;
            _workInfoManager = workInfoManager;
            _clanManager = clanManager;
        }

        /// <summary>
        /// Проинициализировать работу пилотов
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.PROCESS_PILOT_DELIVERY, ProcessDelivery);
            foreach (var contract in AirPort.Contracts) {
                var point = API.createSphereColShape(contract.TargetPosition, 8f);
                point.setData(POINT_CONTRACT_TYPE, contract.Type);
                point.onEntityEnterColShape += (shape, entity) => 
                    ProcessTargetColShape(entity, pilot => OnPlayerComeToPoint(pilot, shape));
                point.onEntityExitColShape += (shape, entity) =>
                    ProcessTargetColShape(entity, pilot => pilot.resetData(ON_TARGET));
            }
        }

        /// <summary>
        /// Игрок прибыл на точку доставки груза
        /// </summary>
        private void ProcessTargetColShape(NetHandle entity, Action<Client> action) {
            var vehicle = API.getEntityFromHandle<Vehicle>(entity);
            var pilot = API.getVehicleDriver(vehicle);
            if (pilot == null || !pilot.hasSyncedData(WorkData.IS_PILOT)) {
                return;
            }
            action(pilot);
        }

        /// <summary>
        /// Игрок прибыл на точку доставки
        /// </summary>
        private void OnPlayerComeToPoint(Client pilot, ColShape shape) {
            if (pilot == null || !pilot.hasSyncedData(WorkData.IS_PILOT)) {
                return;
            }
            var pointType = (DeliveryContractType) shape.getData(POINT_CONTRACT_TYPE);
            if (!pilot.hasData(WorkData.DELIVERY_CONTRACT)) {
                return;
            }
            var contract = (DeliveryContract) pilot.getData(WorkData.DELIVERY_CONTRACT);
            if (pointType != contract.Type) {
                return;
            }
            pilot.setData(ON_TARGET, true);
            API.triggerClientEvent(pilot, ServerEvent.SHOW_SUBTITLE, "Нажмите ~y~E~w~, чтобы завершить контракт");
        }

        /// <summary>
        /// Обработчик доставки
        /// </summary>
        private void ProcessDelivery(Client player, object[] args) {
            if (!player.hasData(ON_TARGET)) {
                return;
            }
            var workLevel = _workInfoManager.GetWorkInfo(player, WorkType.Pilot).Level;
            var contract = (DeliveryContract) player.getData(WorkData.DELIVERY_CONTRACT);
            var reward = _rewards[workLevel];
            _playerInfoManager.SetExperience(player, reward.Exp);
            _workInfoManager.SetSalary(player, WorkType.Pilot, contract.Reward + reward.Salary);
            _workInfoManager.SetExperience(player, WorkType.Pilot, reward.WorkExp);
            API.sendNotificationToPlayer(player, $"~g~Бонус от уровня работы: {reward.Salary}$");
            var reputation = GetClanReputation(contract);
            if (reputation > 0) {
                _clanManager.SetReputation(player, reputation);
            }
            player.resetData(WorkData.DELIVERY_CONTRACT);
            player.resetData(ON_TARGET);
            API.triggerClientEvent(player, ServerEvent.HIDE_PILOT_TARGET_POINT);
        }

        /// <summary>
        /// Возвращает репутацию клана за выполненный контракт
        /// </summary>
        private static int GetClanReputation(DeliveryContract contract) {
            var reputation = 0;
            if (PilotMenuHandler.ClanFarmContracts.Contains(contract.Type)) {
                reputation = 5;
            }
            if (PilotMenuHandler.ClanMilitaryContracts.Contains(contract.Type)) {
                reputation = 10;
            }
            return reputation;
        }
    }
}