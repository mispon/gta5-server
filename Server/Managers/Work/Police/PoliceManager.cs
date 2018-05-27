using System.Collections.Generic;
using System.Linq;
using gta_mp_database.Models.Player;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.Places;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Managers.Work.Police.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Work.Police {
    /// <summary>
    /// Менеджер действий полицейских
    /// </summary>
    internal class PoliceManager : Script, IPoliceManager {
        internal const string PRISONER_KEY = "prisoner";
        internal const string STATE_KEY = "policemanState";
        internal const string PULL_OUT = "prisonerPullOut";
        private const float MAX_RANGE = 2.5f;
        private const string NO_PLAYERS_NEARBY = "~r~Нет игроков поблизости";
        private const string NO_PRISONER = "~r~С вами нет задержанного";
        private const int ARREST_ANIMATION = (int) (AnimationFlags.Loop | AnimationFlags.OnlyAnimateUpperBody | AnimationFlags.AllowPlayerControl);

        private readonly IPoliceRewardManager _policeRewardManager;
        private readonly IPoliceAlertManager _policeAlertManager;
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IVehicleManager _vehicleManager;

        public PoliceManager() {}
        public PoliceManager(IPoliceRewardManager policeRewardManager, IPoliceAlertManager policeAlertManager,
            IPlayerInfoManager playerInfoManager, IVehicleManager vehicleManager) {
            _policeRewardManager = policeRewardManager;
            _policeAlertManager = policeAlertManager;
            _playerInfoManager = playerInfoManager;
            _vehicleManager = vehicleManager;
        }

        /// <summary>
        /// Инициализировать обработчик действий полицейских
        /// </summary>
        public void Initialize() {
            _policeAlertManager.RunAlertsGenerator();
            ClientEventHandler.Add(ClientEvent.CHECK_PLAYER, OnCheckPlayer);
            ClientEventHandler.Add(ClientEvent.ARREST_PLAYER, OnArrestPlayer);
            ClientEventHandler.Add(ClientEvent.GIVE_PENALTY, OnGivePenalty);
            ClientEventHandler.Add(ClientEvent.FINISH_POLICE_ALERT, OnFinishAlert);
            ClientEventHandler.Add(ClientEvent.PUT_IN_CAR, PutPrisonerInCar);
            ClientEventHandler.Add(ClientEvent.TAKE_FROM_CAR, TakePrisonerFromCar);
            ClientEventHandler.Add(ClientEvent.RELEASE_PLAYER, OnReleasePlayer);
            ClientEventHandler.Add(ClientEvent.GET_POLICE_MENU, OnGetPoliceMenu);
        }

        /// <summary>
        /// Возвращает арестованного игрока
        /// </summary>
        public Client GetAttachedPlayer(Client player) {
            return PlayerHelper.GetData<Client>(player, PRISONER_KEY, null);
        }

        /// <summary>
        /// Привязывает игрока к полицейскому
        /// </summary>
        public void AttachPrisoner(Client policeman, Client prisoner, bool withData = false) {
            API.attachEntityToEntity(prisoner, policeman, "SKEL_ROOT", new Vector3(-0.2, 0.5, 0.1), new Vector3());
            prisoner.freeze(true);
            API.playPlayerAnimation(prisoner, ARREST_ANIMATION, "mp_arresting", "idle");
            if (withData) {
                policeman.setData(PRISONER_KEY, prisoner);
                prisoner.setData(PRISONER_KEY, policeman);
                var state = prisoner.isInVehicle ? PolicemanState.PrisonerInCar : PolicemanState.WithPrisoner;
                policeman.setData(STATE_KEY, state);
            }
        }

        /// <summary>
        /// Привязывает игрока к полицейскому
        /// </summary>
        public void DetachPrisoner(Client policeman, Client prisoner, bool withData = false) {
            prisoner.detach();
            prisoner.freeze(false);
            if (withData) {
                policeman.resetData(PRISONER_KEY);
                prisoner.resetData(PRISONER_KEY);
                policeman.setData(STATE_KEY, PolicemanState.NoPrisoner);
                API.stopPlayerAnimation(prisoner);
            }
        }

        /// <summary>
        /// Проверить игрока
        /// </summary>
        private void OnCheckPlayer(Client policeman, object[] args) {
            var player = PlayerHelper.GetNearestPlayer(policeman, MAX_RANGE);
            if (!HasNearby(policeman, player, NO_PLAYERS_NEARBY)) {
                return;
            }
            var wantedLevel = _playerInfoManager.GetInfo(player).Wanted.WantedLevel;
            API.sendNotificationToPlayer(policeman, $"~b~Уровень розыска игрока: {wantedLevel}");
            API.triggerClientEvent(policeman, ServerEvent.HIDE_POLICEMAN_MENU);
        }

        /// <summary>
        /// Задержать игрока
        /// </summary>
        private void OnArrestPlayer(Client policeman, object[] args) {
            var player = PlayerHelper.GetNearestPlayer(policeman, MAX_RANGE);
            if (policeman.isInVehicle || !(HasNearby(policeman, player, NO_PLAYERS_NEARBY) && IsWanted(policeman, player))) {
                return;
            }
            AttachPrisoner(policeman, player, true);
            API.triggerClientEvent(policeman, ServerEvent.HIDE_POLICEMAN_MENU);
        }

        /// <summary>
        /// Оштрафовать
        /// </summary>
        private void OnGivePenalty(Client policeman, object[] args) {
            var prisoner = PlayerHelper.GetNearestPlayer(policeman, MAX_RANGE);
            if (!(HasNearby(policeman, prisoner, NO_PLAYERS_NEARBY) && IsWanted(policeman, prisoner))) {
                return;
            }
            var prisonerInfo = _playerInfoManager.GetInfo(prisoner);
            var penalty = CalculatePenalty(prisonerInfo);
            if (!PlayerHasMoney(policeman, prisoner, penalty)) {
                return;
            }
            prisonerInfo.Balance -= penalty;
            _playerInfoManager.RefreshUI(prisoner, prisonerInfo);
            _policeRewardManager.SetEffortReward(policeman, prisoner);
            _playerInfoManager.ClearWanted(prisoner);
            SendPenaltyNotify(policeman, prisoner, penalty);
            DetachPrisoner(policeman, prisoner, true);
            API.triggerClientEvent(policeman, ServerEvent.HIDE_POLICEMAN_MENU);
        }

        /// <summary>
        /// Обработчик завершения патрулирования
        /// </summary>
        private void OnFinishAlert(Client player, object[] args) {
            if (!player.hasData(WorkData.ALERT_ZONE_KEY)) {
                API.sendNotificationToPlayer(player, "~r~Вы не находитесь в зоне партрулирования", true);
                return;
            }
            var alertId = (int) player.getData(WorkData.ALERT_ZONE_KEY);
            if (PlayerHelper.PlayerCorrect(player, true)) {
                var patrolGroup = API.getPlayersInRadiusOfPlayer(10f, player).Where(e => e != null && e.hasData(WorkData.IS_POLICEMAN)).ToList();
                foreach (var policeman in patrolGroup) {
                    _policeRewardManager.SetPatrolReward(policeman);
                }
            }
            _policeAlertManager.FinishAlert(alertId);
            player.resetData(WorkData.ALERT_ZONE_KEY);
            API.triggerClientEvent(player, ServerEvent.HIDE_POLICEMAN_MENU);
        }

        /// <summary>
        /// Посадить задержанного игрока в машину 
        /// </summary>
        private void PutPrisonerInCar(Client policeman, object[] args) {
            var vehicle = _vehicleManager.GetNearestVehicle(policeman, MAX_RANGE, PoliceDepartment.POLICE_VEHICLE_KEY);
            var model = (VehicleHash) API.getEntityModel(vehicle);
            if (vehicle == null || !API.isVehicleACar(model)) {
                API.sendNotificationToPlayer(policeman, "~r~Рядом нет полицейской машины", true);
                return;
            }
            var freeSeats = GetFreeSeats(vehicle);
            if (!freeSeats.Any()) {
                API.sendNotificationToPlayer(policeman, "~r~В этой машине нет свободных мест");
                return;
            }
            var prisoner = GetAttachedPlayer(policeman);
            if (!HasNearby(policeman, prisoner, NO_PRISONER)) {
                return;
            }
            DetachPrisoner(policeman, prisoner);
            prisoner.setIntoVehicle(vehicle, freeSeats.First());
            policeman.setData(STATE_KEY, PolicemanState.PrisonerInCar);
            API.setEntityInvincible(prisoner, true);
            API.triggerClientEvent(policeman, ServerEvent.HIDE_POLICEMAN_MENU);
        }

        /// <summary>
        /// Посадить задержанного игрока в машину 
        /// </summary>
        private void TakePrisonerFromCar(Client policeman, object[] args) {
            var prisoner = GetAttachedPlayer(policeman);
            prisoner.setData(PULL_OUT, true);
            API.warpPlayerOutOfVehicle(prisoner);
            AttachPrisoner(policeman, prisoner);
            policeman.setData(STATE_KEY, PolicemanState.WithPrisoner);
            API.triggerClientEvent(policeman, ServerEvent.HIDE_POLICEMAN_MENU);
        }

        /// <summary>
        /// Отпустить игрока
        /// </summary>
        private void OnReleasePlayer(Client policeman, object[] args) {
            var prisoner = GetAttachedPlayer(policeman);
            if (!HasNearby(policeman, prisoner, NO_PRISONER)) {
                return;
            }
            DetachPrisoner(policeman, prisoner, true);
            API.sendNotificationToPlayer(prisoner, "~b~Полицейский освободил вас");
            API.triggerClientEvent(policeman, ServerEvent.HIDE_POLICEMAN_MENU);
        }

        /// <summary>
        /// Обработчик открытия меню полицейского
        /// </summary>
        private void OnGetPoliceMenu(Client player, object[] args) {
            var isOpen = (bool) args[0];
            if (isOpen) {
                API.triggerClientEvent(player, ServerEvent.TRIGGER_POLICE_ACTION_MENU, 0);
                return;
            }
            var state = PlayerHelper.GetData(player, STATE_KEY, PolicemanState.NoData);
            if (state != PolicemanState.NoData) {
                API.triggerClientEvent(player, ServerEvent.TRIGGER_POLICE_ACTION_MENU, (int) state);
            }
        }

        /// <summary>
        /// Проверяет, что по близости есть игрок
        /// </summary>
        private bool HasNearby(Client policeman, Client player, string msg) {
            if (player == null) {
                API.sendNotificationToPlayer(policeman, msg);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Проверяет, что игрок находится в розыске
        /// </summary>
        private bool IsWanted(Client policeman, Client player) {
            var wantedLevel = _playerInfoManager.GetInfo(player).Wanted.WantedLevel;
            if (wantedLevel == 0) {
                API.sendNotificationToPlayer(policeman, "~r~Игрок не в розыске", true);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Проверяет, что игрок может заплатить штраф
        /// </summary>
        private bool PlayerHasMoney(Client policeman, Client player, int penalty) {
            var balance = _playerInfoManager.GetInfo(player).Balance;
            var result = balance >= penalty;
            if (!result) {
                API.sendNotificationToPlayer(policeman, "~r~У игрока не хватает денег", true);
            }
            return result;
        }

        /// <summary>
        /// Возвращает свободные места в машине
        /// </summary>
        private HashSet<int> GetFreeSeats(Vehicle vehicle) {
            var result = new HashSet<int> {1, 2};
            var playersInside = API.getVehicleOccupants(vehicle);
            foreach (var player in playersInside) {
                var seat = API.getPlayerVehicleSeat(player);
                result.Remove(seat);
            }
            return result;
        }

        /// <summary>
        /// Рассчитывает размер штрафа / награды
        /// </summary>
        internal static int CalculatePenalty(PlayerInfo playerInfo) {
            return playerInfo.Wanted.WantedLevel * playerInfo.Level;
        }

        /// <summary>
        /// Оповестить игроков о штрафе
        /// </summary>
        private void SendPenaltyNotify(Client policeman, Client player, int penalty) {
            API.sendNotificationToPlayer(player, $"~b~Вас оштрафовал {policeman.name}");
            API.sendNotificationToPlayer(player, $"~b~Списано: {penalty}$");
            API.sendNotificationToPlayer(policeman, $"~b~Вы оштрафовали {player.name} на {penalty}$");
        }
    }
}