using System.Linq;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.IoC;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles;
using gta_mp_server.Quests.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using Ninject;

namespace gta_mp_server.Clan {
    /// <summary>
    /// Обработчик меню клана
    /// </summary>
    internal class ClanMenuHandler : Script, IMenu {
        private const int MIN_LEVEL = 10;

        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IClanManager _clanManager;

        public ClanMenuHandler() {}
        public ClanMenuHandler(IPlayerInfoManager playerInfoManager, IClanManager clanManager) {
            _playerInfoManager = playerInfoManager;
            _clanManager = clanManager;
        }

        /// <summary>
        /// Инициализировать обработчик меню
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.JOIN_CLAN, JoinClan);
            ClientEventHandler.Add(ClientEvent.LEFT_CLAN, LeftClan);
            ClientEventHandler.Add(ClientEvent.ACCEPT_CLAN_QUEST, AcceptClanQuest);
        }

        /// <summary>
        /// Вступить в клан
        /// </summary>
        private void JoinClan(Client player, object[] args) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (playerInfo.Clan != null) {
                API.sendNotificationToPlayer(player, "~r~Вы уже состоите в банде", true);
                return;
            }
            if (playerInfo.Level < MIN_LEVEL) {
                API.sendNotificationToPlayer(player, $"~r~Необходимо достигнуть {MIN_LEVEL}-го уровня", true);
                return;
            }
            var clanId = (int) args[0];
            if (_clanManager.ClanIsFull(clanId)) {
                API.sendNotificationToPlayer(player, "~r~В банде слишком много участников");
                API.sendNotificationToPlayer(player, "~r~Попробуйте позже или вступите в другую");
                return;
            }
            _clanManager.JoinClan(player, playerInfo, clanId);
            _playerInfoManager.RefreshUI(player, playerInfo);
            API.triggerClientEvent(player, ServerEvent.HIDE_CLAN_MENU);
        }

        /// <summary>
        /// Покинуть клан
        /// </summary>
        private void LeftClan(Client player, object[] args) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (playerInfo.Clan == null) {
                API.sendNotificationToPlayer(player, "~r~Вы не состоите в банде", true);
                return;
            }
            _clanManager.LeftClan(player, playerInfo);
            _playerInfoManager.RefreshUI(player, playerInfo);
            player.resetData(PlayerData.CLAN_QUEST);
            API.triggerClientEvent(player, ServerEvent.HIDE_CLAN_MENU);
            API.triggerClientEvent(player, ServerEvent.HIDE_CLAN_QUEST_POINTS);
        }

        /// <summary>
        /// Взять клановое задание
        /// </summary>
        private void AcceptClanQuest(Client player, object[] args) {
            if (player.hasData(PlayerData.CLAN_QUEST)) {
                API.sendColoredNotificationToPlayer(player, "Вы уже взяли задание", 0, 6, true);
                return;
            }
            var clanQuests = ServerKernel.Kernel.Get<ClanQuest[]>();
            var selectedQuest = clanQuests[ActionHelper.Random.Next(clanQuests.Length)];
            selectedQuest.ShowTarget(player);
            API.sendColoredNotificationToPlayer(player, "Вы начали задание", 0, 134);
            API.triggerClientEvent(player, ServerEvent.HIDE_CLAN_LEADER_MENU);
        }
    }
}