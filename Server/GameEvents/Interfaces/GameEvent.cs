using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Interfaces;
using gta_mp_server.Models;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Ninject;
using PlayerInfo = gta_mp_database.Models.Player.PlayerInfo;

namespace gta_mp_server.GameEvents.Interfaces {
    internal abstract class GameEvent : Script {
        private static EventInfo _eventInfo;
        private static List<Client> _members;
        private static List<Client> _redTeam;
        private static List<Client> _blueTeam;
        private static ColShape _eventZone;
        protected static EventTeam Winners = EventTeam.None;

        protected readonly IPlayerInfoManager PlayerInfoManager;
        protected readonly IGtaCharacter GtaCharacter;
        private readonly IInventoryManager _inventoryManager;
        private readonly IWorkEquipmentManager _workEquipmentManager;

        protected GameEvent() {
            PlayerInfoManager = ServerKernel.Kernel.Get<IPlayerInfoManager>();
            GtaCharacter = ServerKernel.Kernel.Get<IGtaCharacter>();
            _inventoryManager = ServerKernel.Kernel.Get<IInventoryManager>();
            _workEquipmentManager = ServerKernel.Kernel.Get<IWorkEquipmentManager>();
        }

        /// <summary>
        /// Возвращает название эвента
        /// </summary>
        protected abstract EventInfo GetEventInfo();

        /// <summary>
        /// Создает зону эвента
        /// </summary>
        protected abstract ColShape CreateEventZone();

        /// <summary>
        /// Устанавливает параметры игрока перед эвентом
        /// </summary>
        protected abstract void SetEventData(Client player, EventTeam team, int index);

        /// <summary>
        /// Сообщение о завершении эвента
        /// </summary>
        protected abstract void SendFinishNotify(EventTeam winners);

        /// <summary>
        /// Выдает награду победителю
        /// </summary>
        protected abstract void SetReward(Client player);

        /// <summary>
        /// Подготовка эвента
        /// </summary>
        public void Initialize() {
            _eventInfo = GetEventInfo();
            _members = new List<Client>(_eventInfo.MaxMembers);
            _redTeam = new List<Client>(_eventInfo.MaxMembers / 2);
            _blueTeam = new List<Client>(_eventInfo.MaxMembers / 2);
            API.sendChatMessageToAll($"~g~Открыта регистрация на эвент \"{_eventInfo.Name}\".");
            ClientEventHandler.Add(ClientEvent.EVENT_PARTICIPATION, ChangeEventParticipation);
            ClientEventHandler.Add(ClientEvent.START_EVENT, (player, args) => player.freeze(false));
            _eventZone = CreateEventZone();
            _eventZone.onEntityEnterColShape += OnPlayerEnterEventZone;
            _eventZone.onEntityExitColShape += OnPlayerLeaveEventZone;
        }

        /// <summary>
        /// Обработка события попадания игрока в зону эвента
        /// </summary>
        private void OnPlayerEnterEventZone(ColShape shape, NetHandle entity) {
            PlayerHelper.ProcessAction(entity, player => {
                if (!player.hasData(PlayerData.LEAVE_EVENT)) {
                    return;
                }
                player.resetData(PlayerData.LEAVE_EVENT);
                API.sendChatMessageToPlayer(player, "~g~Вы вернулись в зону эвента");
                ActionHelper.CancelAction(player);
            }, true);
        }

        /// <summary>
        /// Обработка события выхода игрока из зоны эвента
        /// </summary>
        private void OnPlayerLeaveEventZone(ColShape shape, NetHandle entity) {
            PlayerHelper.ProcessAction(entity, player => {
                if (player.hasData(PlayerData.WAS_KILLED)) {
                    return;
                }
                player.setData(PlayerData.LEAVE_EVENT, true);
                API.sendChatMessageToPlayer(player, "~r~Вы покинули зону эвента! Чтобы не умереть, вернитесь в течении 30 сек.");
                ActionHelper.SetAction(player, 30000, player.kill);
            }, true);
        }

        /// <summary>
        /// Запускает эвент
        /// </summary>
        public void Start() {
            if (!CanStart()) {
                return;
            }
            FillTeams();
            foreach (var member in _members) {
                API.setPlayerHealth(member, PlayerInfo.MAX_VALUE);
                member.setData(PlayerData.ON_EVENT, _eventInfo.Name);
                member.setData(PlayerData.LAST_POSITION, member.position);
                member.setData(PlayerData.LAST_DIMENSION, member.dimension);
                member.freeze(true);
                API.triggerClientEvent(member, ServerEvent.SET_TIMER, 10, ClientEvent.START_EVENT);
            }
            MovePlayersToEvent(_redTeam, EventTeam.Red);
            MovePlayersToEvent(_blueTeam, EventTeam.Blue);
        }

        /// <summary>
        /// Проверка условий перед началом эвента
        /// </summary>
        private static bool CanStart() {
            _members = _members.Where(e => API.shared.isPlayerConnected(e)).ToList();
            if (_members.Count > 1) {
                return true;
            }
            Dispose();
            return false;
        }

        /// <summary>
        /// Разбивает игроков по командам
        /// </summary>
        private static void FillTeams() {
            for (var i = 0; i < _members.Count; i++) {
                if (i % 2 == 0)
                    _redTeam.Add(_members[i]);
                else
                    _blueTeam.Add(_members[i]);
            }
        }

        /// <summary>
        /// Перемещает игроков на эвент
        /// </summary>
        private void MovePlayersToEvent(IReadOnlyList<Client> teamMembers, EventTeam team) {
            for (var i = 0; i < teamMembers.Count; i++) {
                var member = teamMembers[i];
                API.removeAllPlayerWeapons(member);
                SetEventData(member, team, i);
            }
        }

        /// <summary>
        /// Отправляет сообщение участникам эвента
        /// </summary>
        protected void SendMessageForMembers(string message) {
            foreach (var member in _members) {
                API.sendChatMessageToPlayer(member, message);
            }
        }

        /// <summary>
        /// Возвращает команду победителей, если она определена
        /// </summary>
        protected static EventTeam GetWinnerTeam() {
            if (_redTeam.All(e => e.hasData(PlayerData.WAS_KILLED))) {
                return EventTeam.Blue;
            }
            if (_blueTeam.All(e => e.hasData(PlayerData.WAS_KILLED))) {
                return EventTeam.Red;
            }
            return EventTeam.None;
        }

        /// <summary>
        /// Завершает эвент
        /// </summary>
        protected void Finish() {
            SendFinishNotify(Winners);
            SetWinnersReward();
            StopSpectatingPlayers();
            ResetEventData();
            Dispose();
        }

        /// <summary>
        /// Выдать награду победителям
        /// </summary>
        private void SetWinnersReward() {
            var winners = Winners == EventTeam.Red ? _redTeam : _blueTeam;
            foreach (var winner in winners) {
                SetReward(winner);
            }
        }

        /// <summary>
        /// Остановка режима наблюдения убитых игроков
        /// </summary>
        private static void StopSpectatingPlayers() {
            // задержка после окончания эвента
            Thread.Sleep(29600);
            foreach (var member in _members) {
                if (member.spectating) {
                    member.stopSpectating();
                    API.shared.setEntityInvincible(member, false);
                }
            }
            // note: необходима задержка, т.к. по предположению, гта не успевает отреагировать на сл. инструкции
            Thread.Sleep(400);
        }

        /// <summary>
        /// Очищает данные эвента у игрока
        /// </summary>
        private void ResetEventData() {
            foreach (var player in _members) {
                RestorePosition(player);
                API.setPlayerHealth(player, PlayerInfo.MAX_VALUE);
                PlayerInfoManager.SetPlayerClothes(player, true);
                player.resetData(PlayerData.ON_EVENT);
                player.resetData(PlayerData.WAS_KILLED);
                player.resetData(PlayerData.KILLER);
                player.resetSyncedData(PlayerData.IS_REGISTERED);
                _inventoryManager.EquipWeapon(player);
                _workEquipmentManager.SetEquipment(player);
            }
        }

        /// <summary>
        /// Перемещает игрока на последнюю позицию после эвента
        /// </summary>
        private static void RestorePosition(Client player) {
            PlayerHelper.RestorePosition(player);
            player.freeze(false);
        }

        /// <summary>
        /// Воскрешение игрока
        /// </summary>
        protected void OnPlayerRespawn(Client player, Vector3 position) {
            API.setEntityPosition(player, position);
            API.setEntityInvincible(player, true);
            player.freeze(true);
            Client observable;
            if (player.hasData(PlayerData.KILLER)) {
                observable = (Client) player.getData(PlayerData.KILLER);
            }
            else {
                observable = _members.FirstOrDefault(e => !e.hasData(PlayerData.WAS_KILLED));
            }
            if (observable != null) {
                player.spectate(observable);
            }
        }

        /// <summary>
        /// Обработчик смерти игрока
        /// </summary>
        protected void OnPlayerDeath(Client player, NetHandle enityKiller) {
            player.setData(PlayerData.WAS_KILLED, true);
            var killer = GetKiller(player, enityKiller);
            SendKillMessage(player, killer);
            if ((Winners = GetWinnerTeam()) != EventTeam.None) {
                Task.Run(() => Finish());
            }
        }

        /// <summary>
        /// Возвращает убийцу игрока
        /// </summary>
        private Client GetKiller(Client player, NetHandle entityHandle) {
            Client result = null;
            if (!entityHandle.IsNull) {
                result = API.getPlayerFromHandle(entityHandle);
                player.setData(PlayerData.KILLER, result);
            }
            return result;
        }

        /// <summary>
        /// Оповестить участников о смерти игрока
        /// </summary>
        private void SendKillMessage(Client player, Client killer) {
            var playerName = _redTeam.Contains(player) ? $"~r~{player.name}" : $"~b~{player.name}";
            var message = $"{playerName} ~w~погиб";
            if (killer != null) {
                var killerName = _redTeam.Contains(killer) ? $"~r~{killer.name}" : $"~b~{killer.name}";
                message = $"{killerName} ~w~убил {playerName}";
            }
            SendMessageForMembers(message);
        }

        /// <summary>
        /// Обработчик выхода игрока с сервера
        /// </summary>
        protected static void OnPlayerDisconnected(Client player, string reason) {
            if (!player.hasData(PlayerData.ON_EVENT)) {
                return;
            }
            _members.Remove(player);
            if (_redTeam.Contains(player))
                _redTeam.Remove(player);
            else
                _blueTeam.Remove(player);
        }

        /// <summary>
        /// Обработчик регистрации игроков в эвенте
        /// </summary>
        private static void ChangeEventParticipation(Client player, object[] args) {
            var join = (bool) args[0];
            if (join && !_members.Contains(player)) {
                _members.Add(player);
                player.setSyncedData(PlayerData.IS_REGISTERED, true);
            }
            if (!join && _members.Contains(player)) {
                _members.Remove(player);
                player.resetSyncedData(PlayerData.IS_REGISTERED);
            }
        }

        /// <summary>
        /// Возвращает информацию об эвенте
        /// </summary>
        public EventInfo GetInfo(Client player) {
            _eventInfo.TotalMembers = _members.Count;
            _eventInfo.IsMember = _members.Contains(player);
            return _eventInfo;
        }

        /// <summary>
        /// Очистить ресурсы
        /// </summary>
        private static void Dispose() {
            _members.Clear();
            _redTeam.Clear();
            _blueTeam.Clear();
            ClientEventHandler.Remove(ClientEvent.EVENT_PARTICIPATION);
            ClientEventHandler.Remove(ClientEvent.START_EVENT);
            Winners = EventTeam.None;
            API.shared.deleteColShape(_eventZone);
        }
    }
}