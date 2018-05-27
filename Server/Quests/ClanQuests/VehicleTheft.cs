using System;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Events;
using gta_mp_server.Helpers;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Quests.Interfaces;
using gta_mp_server.Quests.QuestsData;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Ninject;
using Ninject.Syntax;

namespace gta_mp_server.Quests.ClanQuests {
    /// <summary>
    /// Задание банды на угон авто
    /// </summary>
    internal class VehicleTheft : ClanQuest {
        internal const string THIEFT_VEHICLE = "ThieftVehicle";
        private const string THIEF_NAME = "VehicleThiefName";

        public VehicleTheft() : this(ServerKernel.Kernel) {
            API.onPlayerEnterVehicle += OnEnterHijackedVehicle;
            API.onPlayerExitVehicle += OnExitHijackedVehicle;
        }

        public VehicleTheft(IResolutionRoot kernel) : base(kernel.Get<IPlayerInfoManager>(), kernel.Get<IClanManager>()) {}

        /// <summary>
        /// Инициализирует квест
        /// </summary>
        public override void Initialize() {
            foreach (var position in VehicleTheftData.EndPointPositions) {
                CreateQuestEndPoint(position.Key, position.Value);
            }
        }

        /// <summary>
        /// Создает точку сдачи угнаной машины
        /// </summary>
        private void CreateQuestEndPoint(long clanId, Vector3 position) {
            var endPoint = API.createCylinderColShape(position, 4f, 3f);
            endPoint.onEntityEnterColShape += (shape, entity) => PlayerHelper.ProcessAction(entity, player => {
                var playerClanId = PlayerInfoManager.GetInfo(player).Clan?.ClanId ?? Validator.INVALID_ID;
                if (clanId != playerClanId) return;
                var vehicle = API.getPlayerVehicle(player);
                if (vehicle.IsNull || !API.hasEntityData(vehicle, THIEFT_VEHICLE)) return;
                API.deleteEntity(vehicle);
                SetQuestReward(player);
                ActionHelper.CancelAction(player, CLAN_QUEST_TIMER);
                API.triggerClientEvent(player, ServerEvent.HIDE_CLAN_QUEST_POINTS);
            }, true);
        }

        /// <summary>
        /// Направляет на место задания
        /// </summary>
        public override void ShowTarget(Client player) {
            var vehicle = CreateHijackedVehicle();
            player.setData(PlayerData.CLAN_QUEST, true);
            vehicle.setData(THIEF_NAME, player.name);
            ActionHelper.SetAction(player, 1200000, () => player.resetData(PlayerData.CLAN_QUEST), CLAN_QUEST_TIMER);
            API.triggerClientEvent(player, ServerEvent.SHOW_CLAN_QUEST_TARGET, vehicle.position, false);
            API.triggerClientEvent(player, ServerEvent.SHOW_SUBTITLE, "Угоните ~y~суперкар");
        }

        /// <summary>
        /// Создает транспорт для угона
        /// </summary>
        private Vehicle CreateHijackedVehicle() {
            var vehicleHash = VehicleTheftData.GetVehicleHash();
            var position = VehicleTheftData.GetHijackedPosition();
            var vehicle = API.createVehicle(
                vehicleHash, position.Item1, position.Item2, 
                ActionHelper.Random.Next(159), ActionHelper.Random.Next(159)
            );
            vehicle.setData(THIEFT_VEHICLE, true);
            vehicle.engineStatus = false;
            SetVehicleAfk(vehicle);
            return vehicle;
        }

        /// <summary>
        /// Обработчик входа в угоняемую машину
        /// </summary>
        private void OnEnterHijackedVehicle(Client player, NetHandle vehHandle, int seat) {
            var vehicle = API.getEntityFromHandle<Vehicle>(vehHandle);
            if (!vehicle.hasData(THIEFT_VEHICLE)) return;
            var thiefName = vehicle.hasData(THIEF_NAME) ? (string) vehicle.getData(THIEF_NAME) : string.Empty;
            if (thiefName == player.name) {
                var playerClanId = PlayerInfoManager.GetInfo(player).Clan.ClanId;
                API.triggerClientEvent(player, ServerEvent.SHOW_CLAN_QUEST_END_POINT, VehicleTheftData.EndPointPositions[playerClanId]);
            }
            else {
                player.warpOutOfVehicle();
            }
        }

        /// <summary>
        /// Обработчик выхода из угоняемой машины
        /// </summary>
        private void OnExitHijackedVehicle(Client player, NetHandle vehHandle, int seat) {
            if (!API.hasEntityData(vehHandle, THIEFT_VEHICLE)) return;
            SetVehicleAfk(vehHandle);
            API.triggerClientEvent(player, ServerEvent.SHOW_CLAN_QUEST_TARGET, player.position);
        }

        /// <summary>
        /// Устанавливает флаг афк на воруемый транспорт
        /// </summary>
        private void SetVehicleAfk(NetHandle vehicle) {
            API.setEntityData(vehicle, VehicleEventsManager.AFK_KEY, DateTime.Now.AddMinutes(-280));
        }
    }
}