using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using gta_mp_data.Entity;
using gta_mp_data.Enums;
using gta_mp_database.Providers.Interfaces;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Managers.Player;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using PlayerInfo = gta_mp_database.Models.Player.PlayerInfo;

namespace gta_mp_server.Clan {
    /// <summary>
    /// Общая логика клана
    /// </summary>
    internal class ClanManager : Script, IClanManager {
        private const int LOW = 700;
        private const int MIDDLE = 1500;
        private const int HIGH = 2800;
        private const int HIGHER = 4500;
        private const float CLAN_SHARE = 0.7f;

        private readonly IAccountsProvider _accountsProvider;
        private readonly IClanProvider _clanProvider;
        private readonly IPlayerInfoManager _playerInfoManager;

        private static readonly Dictionary<int, string> _clanTags = new Dictionary<int, string> {
            [1] = "[Семья] ", [2] = "[Картель] ", [3] = "[Короли] "
        };

        public ClanManager() {}
        public ClanManager(IAccountsProvider accountsProvider, IClanProvider clanProvider, IPlayerInfoManager playerInfoManager) {
            _accountsProvider = accountsProvider;
            _clanProvider = clanProvider;
            _playerInfoManager = playerInfoManager;
        }

        /// <summary>
        /// Загружает кланы в кэш
        /// </summary>
        public void Initialize() {
            ServerState.Clans = _clanProvider.LoadClans();
            var timer = new Timer(14400000); // каждые 4 часа
            timer.Elapsed += (sender, args) => Task.Run(() => PayOut());
            timer.Start();
        }

        /// <summary>
        /// Пополнить баланс клана от покупки
        /// </summary>
        public void ReplenishClanBalance(int districtId, int amount) {
            var clan = ServerState.Clans.FirstOrDefault(e => e.GetDistricts().Contains(districtId));
            if (clan != null) {
                clan.Balance += (int) (amount * CLAN_SHARE);
            }
        }

        /// <summary>
        /// Вступление в клан
        /// </summary>
        public void JoinClan(Client player, PlayerInfo playerInfo, int clanId) {
            var clanInfo = new PlayerClanInfo {
                AccountId = playerInfo.AccountId,
                ClanId = clanId,
                Rank = ClanRank.Lowest,
                Reputation = 0
            };
            playerInfo.Clan = clanInfo;
            playerInfo.TagName = _clanTags[clanId];
            PlayerManager.SetPlayerName(player, playerInfo);
            API.sendNotificationToPlayer(player, $"Вы присоединились к банде ~b~\"{GetClanName(clanId)}\"");
        }

        /// <summary>
        /// Выход из клана
        /// </summary>
        public void LeftClan(Client player, PlayerInfo playerInfo) {
            API.sendNotificationToPlayer(player, $"Вы покинули банду ~b~\"{GetClanName(playerInfo.Clan.ClanId)}\"");
            _clanProvider.RemoveClanInfo(playerInfo.AccountId);
            playerInfo.Clan = null;
            playerInfo.TagName = string.Empty;
            PlayerManager.SetPlayerName(player, playerInfo);
        }

        /// <summary>
        /// Устанавливает ранг
        /// </summary>
        public void SetReputation(Client player, int value, PlayerInfo playerInfo = null) {
            if (playerInfo == null) {
                playerInfo = _playerInfoManager.GetInfo(player);
            }
            if (value > 0 && playerInfo.IsPremium()) {
                value += (int) (value * 0.3);
            }
            playerInfo.Clan.Reputation += value;
            var prefix = value > 0 ? "~b~Получено" : "~w~Потеряно";
            API.sendNotificationToPlayer(player, $"{prefix} {Math.Abs(value)} очков репутации банды");
            var rank = GetRank(playerInfo.Clan.Reputation);
            if (rank != playerInfo.Clan.Rank) {
                playerInfo.Clan.Rank = rank;
                API.sendNotificationToPlayer(player, $"~g~Изменение ранга. Новый ранг: {rank.GetDescription()}");
            }
        }

        /// <summary>
        /// Устанавлевает значение авторитета клана
        /// </summary>
        internal static void SetAuthority(long clanId, int value) {
            var clanInfo = GetInfo(clanId);
            clanInfo.Authority += value;
        }

        /// <summary>
        /// Проверяет, достаточно ли авторитета у клана
        /// </summary>
        internal static int GetAuthority(long clanId) {
            return GetInfo(clanId).Authority;
        }

        /// <summary>
        /// Возвращает название клана
        /// </summary>
        internal static string GetClanName(long clanId) {
            return GetInfo(clanId).Name;
        }

        /// <summary>
        /// Добавить улицу клану
        /// </summary>
        public void AddDistrict(long clanId, int districtId) {
            var clan = ServerState.Clans.First(e => e.Id == clanId);
            var districts = clan.GetDistricts();
            districts.Add(districtId);
            clan.SetDistricts(districts);
            _clanProvider.SaveClans(ServerState.Clans);
        }

        /// <summary>
        /// Удалает улицу из кланового списка контрольных улиц
        /// </summary>
        public void RemoveDistrict(int districtId) {
            foreach (var clan in ServerState.Clans) {
                var districts = clan.GetDistricts();
                if (districts.Contains(districtId)) {
                    districts.Remove(districtId);
                    clan.SetDistricts(districts);
                }
            }
        }

        /// <summary>
        /// Проверяет, что в выбранном клане нет перекоса 
        /// по количеству участников относительно других кланов
        /// </summary>
        public bool ClanIsFull(long clanId) {
            const int maxOverweight = 10;
            var clanMembers = _clanProvider.GetMembersCountByClans();
            if (!clanMembers.ContainsKey(clanId)) {
                return false;
            }
            var membersInSelectedClan = clanMembers[clanId];
            foreach (var clanMember in clanMembers.Where(e => e.Key != clanId)) {
                if (membersInSelectedClan - clanMember.Value > maxOverweight) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Выплачивает доли участникам клана
        /// </summary>
        private void PayOut() {
            var activeAccounts = _accountsProvider.GetActive();
            var membersByClan = activeAccounts
                .Select(e => _playerInfoManager.GetWithData(e.Id))
                .Where(e => e.PlayerInfo.Clan != null)
                .GroupBy(e => e.PlayerInfo.Clan.ClanId)
                .ToDictionary(e => e.Key, e => e.ToList());
            foreach (var clan in membersByClan) {
                PayOutByClan(clan.Key, clan.Value);
            }
        }

        /// <summary>
        /// Выплатить доли в зависимости от рангов
        /// </summary>
        private void PayOutByClan(long clanId, IReadOnlyCollection<PlayerWithData> players) {
            var clan = ServerState.Clans.First(e => e.Id == clanId);
            for (var rank = Enum.GetNames(typeof(ClanRank)).Length; rank >= 1; rank--) {
                var clanRank = (ClanRank) rank;
                var members = players.Where(e => e.PlayerInfo.Clan.Rank == clanRank).ToList();
                if (members.Count == 0) continue;
                var rankBalance = clanRank != ClanRank.Lowest ? (int) (clan.Balance * 0.2) : clan.Balance;
                SetRewardToMembers(members, rankBalance);
                clan.Balance -= rankBalance;
            }
        }

        /// <summary>
        /// Выдать награду каждому участнику
        /// </summary>
        private void SetRewardToMembers(IReadOnlyCollection<PlayerWithData> members, int rankBalance) {
            var memberReward = rankBalance / members.Count;
            if (memberReward == 0) {
                return;
            }
            foreach (var member in members) {
                if (member.PlayerInfo.IsPremium()) {
                    memberReward += (int) (memberReward * 0.25);
                }
                member.PlayerInfo.Balance += memberReward;
                if (member.Player != null) {
                    API.sendNotificationToPlayer(member.Player, $"~b~Зачислена доля от банды в размере {memberReward}$");
                    _playerInfoManager.RefreshUI(member.Player, member.PlayerInfo);
                }
                else {
                    _playerInfoManager.Set(member.PlayerInfo);
                }
            }
        }

        /// <summary>
        /// Возвращает данные клана
        /// </summary>
        private static gta_mp_database.Entity.Clan GetInfo(long clanId) {
            return ServerState.Clans.First(e => e.Id == clanId);
        }

        /// <summary>
        /// Возвращает ранг в клане
        /// </summary>
        private static ClanRank GetRank(int reputation) {
            if (reputation < LOW) return ClanRank.Lowest;
            if (LOW <= reputation && reputation < MIDDLE) return ClanRank.Low;
            if (MIDDLE <= reputation && reputation < HIGH) return ClanRank.Middle;
            if (HIGH <= reputation && reputation <= HIGHER) return ClanRank.High;
            return ClanRank.Highest;
        }

        /// <summary>
        /// Возвращает цвет клана
        /// </summary>
        internal static Color GetClanColor(long clanId) {
            switch (clanId) {
                case 1:
                    return Colors.Blue;
                case 2:
                    return Colors.Red;
                case 3:
                    return Colors.Green;
                default:
                    throw new ArgumentException("Неизвестный идентификатор клана");
            }
        }
    }
}