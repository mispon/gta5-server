using System;
using gta_mp_data.Enums;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Clan.Mission {
    /// <summary>
    /// Обработчик меню лидера клана
    /// </summary>
    internal class MissionMenuHandler : Script, IMenu {
        internal const int NEEDED_AUTHORITY = 500;
        internal const string MISSION_START_VOTE = "MissionStartVote";

        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IClanMissionManager _clanMissionManager;

        public MissionMenuHandler() {}
        public MissionMenuHandler(IPlayerInfoManager playerInfoManager, IClanMissionManager clanMissionManager) {
            _playerInfoManager = playerInfoManager;
            _clanMissionManager = clanMissionManager;
        }

        /// <summary>
        /// Инициализировать обработчик меню
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.JOIN_CLAN_MISSION, JoinClanMission);
            ClientEventHandler.Add(ClientEvent.LEFT_CLAN_MISSION, LeftClanMission);
        }

        /// <summary>
        /// Проголосовать за запуск миссии
        /// </summary>
        private void JoinClanMission(Client player, object[] args) {
            if (player.hasData(MISSION_START_VOTE)) {
                API.sendColoredNotificationToPlayer(player, "Вы уже проголосовали за запуск миссии", 0, 6, true);
                return;
            }
            if (!PlayerCanVote(player)) return;
            var clanId = Convert.ToInt64(args[0]);
            if (ClanManager.GetAuthority(clanId) < NEEDED_AUTHORITY) {
                API.sendColoredNotificationToPlayer(player, $"Банде требуется {NEEDED_AUTHORITY} очков авторитета", 0, 6, true);
                return;
            }
            _clanMissionManager.VoteToStart(clanId);
            API.sendColoredNotificationToPlayer(player, "Вы проголосовали за запуск миссии", 0, 18);
        }

        /// <summary>
        /// Отменить свой голос за запуск миссии
        /// </summary>
        private void LeftClanMission(Client player, object[] args) {
            if (!player.hasData(MISSION_START_VOTE)) {
                API.sendColoredNotificationToPlayer(player, "Вы еще не голосовали за запуск миссии", 0, 6, true);
                return;
            }
            if (!PlayerCanVote(player)) return;
            var clanId = Convert.ToInt64(args[0]);
            if (ClanMissionManager.Missions[clanId].Active) {
                API.sendColoredNotificationToPlayer(player, "Миссия уже запущена", 0, 6, true);
                return;
            }
            var result = _clanMissionManager.CancelVote(clanId);
            API.sendColoredNotificationToPlayer(player, result ? "Вы отказались от запуска миссии" : "Голосование не запущено", 0, 5);
        }

        /// <summary>
        /// Проверяет, что у игрока достаточный ранг для голосования
        /// </summary>
        private bool PlayerCanVote(Client player) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (playerInfo.Clan.Rank < ClanRank.Middle) {
                API.sendColoredNotificationToPlayer(player, $"Для голосования требуется ранг \"{ClanRank.Middle.GetDescription()}\" и выше", 0, 6, true);
                return false;
            }
            return true;
        }
    }
}