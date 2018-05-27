using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_server.Constant;
using gta_mp_server.Global.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Global {
    /// <summary>
    /// Обработчик чата
    /// </summary>
    internal class ChatHandler : Script, IChatHandler {
        private const string SCREAM_PREF = "!к";
        private const string CLAN_PREF = "!б";
        private const string POLICE_PREF = "!п";
        private const string PM_PREF = "\"";

        private readonly IPlayerInfoManager _playerInfoManager;

        public ChatHandler() {}
        public ChatHandler(IPlayerInfoManager playerInfoManager) {
            _playerInfoManager = playerInfoManager;
        }

        /// <summary>
        /// Обработчик сообщения
        /// </summary>
        public void OnChatMessage(Client player, string message, CancelEventArgs cancel) {
            cancel.Cancel = true;
            if (message.Length <= 2) {
                return;
            }
            if (message.StartsWith(SCREAM_PREF)) {
                ProcessScream(player, message);
            }
            else if (message.StartsWith(CLAN_PREF)) {
                ProcessClanMessage(player, message);
            }
            else if (message.StartsWith(POLICE_PREF)) {
                ProcessPoliceMessage(player, message);
            }
            else if (message.StartsWith(PM_PREF)) {
                ProcessPrivateMessage(player, message);
            }
            else {
                var players = API.getPlayersInRadiusOfPlayer(20f, player).Where(e => e != null);
                SendMessage(players, $"{player.name}: {message}");
            }
        }

        /// <summary>
        /// Обработчик глобального крика
        /// </summary>
        private void ProcessScream(Client player, string message) {
            if (player.hasData(PlayerData.CHAT_SCREAM) && (DateTime.Now - (DateTime) player.getData(PlayerData.CHAT_SCREAM)).TotalSeconds < 30) {
                API.sendNotificationToPlayer(player, "~r~Кричать можно не чаще, чем раз в 30 секунд", true);
                return;
            }
            player.setData(PlayerData.CHAT_SCREAM, DateTime.Now);
            message = message.Substring(2);
            SendMessage(API.getAllPlayers(), $"~o~[Кричит] ~w~{player.name}: {message}");
        }

        /// <summary>
        /// Обработчик клановых сообщений
        /// </summary>
        private void ProcessClanMessage(Client player, string message) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (playerInfo.Clan == null) {
                API.sendNotificationToPlayer(player, "~r~Вы не состоите в банде", true);
            }
            message = message.Substring(2);
            var players = _playerInfoManager.GetWhere(info => info.Clan != null && info.Clan.ClanId == playerInfo.Clan.ClanId);
            SendMessage(players.Keys, $"~p~[Банда] ~w~{player.name}: {message}");
        }

        /// <summary>
        /// Обработчик сообщений полиции
        /// </summary>
        private void ProcessPoliceMessage(Client player, string message) {
            if (!player.hasData(WorkData.IS_POLICEMAN)) {
                API.sendNotificationToPlayer(player, "~r~Вы не работаете в полиции", true);
                return;
            }
            message = message.Substring(2);
            var players = API.getAllPlayers().Where(e => e.hasData(WorkData.IS_POLICEMAN));
            SendMessage(players, $"~b~[Полиция] ~w~{player.name}: {message}");
        }

        /// <summary>
        /// Обрабатывает личное сообщение
        /// </summary>
        private void ProcessPrivateMessage(Client player, string message) {
            var messageData = message.Split(' ');
            if (messageData.Length < 3) {
                API.sendNotificationToPlayer(player, "~r~Правильный формат: \"Имя Игрока сообщение", true);
                return;
            }
            var name = messageData[0].Substring(1).Trim();
            var surname = messageData[1].Trim();
            message = string.Empty;
            for (var i = 2; i < messageData.Length; i++) {
                message += $"{messageData[i]} ";
            }
            var targetPlayer = API.getPlayerFromName($"{name} {surname}");
            if (targetPlayer == null) {
                API.sendNotificationToPlayer(player, "~r~Игрок не в сети", true);
                return;
            }
            if (targetPlayer == player) {
                API.sendNotificationToPlayer(player, "~r~Нельзя писать самому себе", true);
                return;
            }
            API.sendChatMessageToPlayer(player, $"~q~[Шепчет] ~w~Вы: {message}");
            API.sendChatMessageToPlayer(targetPlayer, $"~q~[Шепчет] ~w~{player.name}: {message}");
        }

        /// <summary>
        /// Отправить сообщение игрокам
        /// </summary>
        private void SendMessage(IEnumerable<Client> players, string message) {
            foreach (var player in players) {
                API.sendChatMessageToPlayer(player, message);
            }
        }
    }
}