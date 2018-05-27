using gta_mp_data.Enums;
using gta_mp_database.Models.Player;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Clan.Mission;
using gta_mp_server.Constant;
using gta_mp_server.Events.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Police;
using gta_mp_server.Managers.Work.Police.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;

namespace gta_mp_server.Events {
    /// <summary>
    /// Обработка смерти игрока
    /// </summary>
    internal class PlayerDeathManager : Script, IPlayerDeathManager {
        private const int CLAN_WAR_EXP = 100;
        private const int POLICEMAN_PENALTY = -500;

        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IPoliceManager _policeManager;
        private readonly IPoliceAlertManager _policeAlertManager;
        private readonly IClanManager _clanManager;
        private readonly IWorkInfoManager _workInfoManager;
        private readonly IInventoryManager _inventoryManager;

        public PlayerDeathManager() {}
        public PlayerDeathManager(IPlayerInfoManager playerInfoManager, IPoliceManager policeManager, IPoliceAlertManager policeAlertManager,
            IClanManager clanManager, IWorkInfoManager workInfoManager, IInventoryManager inventoryManager) {
            _playerInfoManager = playerInfoManager;
            _policeManager = policeManager;
            _policeAlertManager = policeAlertManager;
            _clanManager = clanManager;
            _workInfoManager = workInfoManager;
            _inventoryManager = inventoryManager;
        }

        /// <summary>
        /// Обработчик смерти игрока
        /// </summary>
        public void OnPlayerDeath(Client player, NetHandle handle, int weapon) {
            if (player.hasData(PlayerData.ON_EVENT) || player.hasData(PlayerData.ON_RACE) || player.hasData(PlayerData.FIGHTER)) {
                return;
            }
            if (player.hasData(ClanMission.BOOTY_OBJECT)) {
                ClanMissionManager.DetachBooty(player);
            }
            var playerInfo = _playerInfoManager.GetInfo(player);
            _inventoryManager.RefreshAmmo(player, playerInfo);
            ProcessKiller(player, handle, playerInfo);
            ProcessPoliceman(player);
            SetDeathPenalty(player, playerInfo);
        }

        /// <summary>
        /// Обработчик убийцы
        /// </summary>
        private void ProcessKiller(Client player, NetHandle handle, PlayerInfo deathPlayerInfo) {
            var killer = API.getPlayerFromHandle(handle);
            if (killer == null || player == killer) {
                return;
            }
            if (killer.hasData(WorkData.IS_POLICEMAN)) {
                var playerInfo = _playerInfoManager.GetInfo(player);
                if (playerInfo.Wanted.WantedLevel > 0) {
                    player.setData(PoliceManager.PRISONER_KEY, killer);
                }
                else {
                    API.sendNotificationToPlayer(killer, "~b~Постарайтесь не причинять вред игрокам");
                    _workInfoManager.SetSalary(killer, WorkType.Police, POLICEMAN_PENALTY);
                }
                return;
            }
            _policeAlertManager.SendAlert(killer.position, PoliceAlertManager.MURDER_ALERT);
            var killerInfo = _playerInfoManager.GetInfo(killer);
            var oldWanted = killerInfo.Wanted.WantedLevel;
            killerInfo.Wanted.Kills += 1;
            killerInfo.Wanted.TotalKills += 1;
            if (BothHasClan(deathPlayerInfo, killerInfo)) {
                var reputation = deathPlayerInfo.Clan.ClanId != killerInfo.Clan.ClanId ? 3 : -5;
                _clanManager.SetReputation(killer, reputation, killerInfo);
                _playerInfoManager.SetExperience(killer, CLAN_WAR_EXP);
            }
            _playerInfoManager.RefreshUI(killer, killerInfo);
            if (oldWanted != killerInfo.Wanted.WantedLevel) {
                API.sendNotificationToPlayer(killer, $"~m~Уровень розыска увеличен до: {killerInfo.Wanted.WantedLevel}");
            }
        }

        /// <summary>
        /// Проверяет, что оба игрока в кланах
        /// </summary>
        private static bool BothHasClan(PlayerInfo deathPlayer, PlayerInfo enemyPlayer) {
            return deathPlayer.Clan != null && enemyPlayer.Clan != null;
        }

        /// <summary>
        /// Штраф за смерть
        /// </summary>
        private void SetDeathPenalty(Client player, PlayerInfo playerInfo) {
            if (player.hasData(WorkData.IS_POLICEMAN) || playerInfo.IsPremium()) {
                return;
            }
            const int balanceCoef = 10;
            const int expCoef = 100;
            var balancePenalty = balanceCoef * playerInfo.Level;
            var expPenalty = expCoef * playerInfo.Level;
            if (balancePenalty <= playerInfo.Balance) {
                playerInfo.Balance -= balancePenalty;
                API.sendNotificationToPlayer(player, $"~b~Вы заплатили {balancePenalty}$ за лечение");
            }
            else {
                // note: экспа может уйти в минус, т.е. в текущей реализации игрок не теряет уровень
                playerInfo.Experience -= expPenalty;
            }
            _playerInfoManager.RefreshUI(player, playerInfo);
        }

        /// <summary>
        /// Обработка смерти полицейского
        /// </summary>
        private void ProcessPoliceman(Client player) {
            if (!player.hasData(WorkData.IS_POLICEMAN)) {
                return;
            }
            var prisoner = _policeManager.GetAttachedPlayer(player);
            if (prisoner != null) {
                _policeManager.DetachPrisoner(player, prisoner, true);
            }
        }
    }
}