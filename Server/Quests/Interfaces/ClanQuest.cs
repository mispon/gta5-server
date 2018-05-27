using gta_mp_server.Clan;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Quests.Interfaces {
    internal abstract class ClanQuest : Script {
        protected const string CLAN_QUEST_TIMER = "ClanQuestTimer";
        private const int REP_REWARD = 20;
        private const int MONEY_REWARD = 400;
        private const int EXP_REWARD = 40;
        private const int CLAN_AUTHORITY = 30;
        private const int FLAG = (int) (AnimationFlags.OnlyAnimateUpperBody | AnimationFlags.AllowPlayerControl);

        protected readonly IPlayerInfoManager PlayerInfoManager;
        private readonly IClanManager _clanManager;

        protected ClanQuest() {}
        protected ClanQuest(IPlayerInfoManager playerInfoManager, IClanManager clanManager) {
            PlayerInfoManager = playerInfoManager;
            _clanManager = clanManager;
        }

        /// <summary>
        /// Инициализирует квест
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Направляет на место задания
        /// </summary>
        public abstract void ShowTarget(Client player);

        /// <summary>
        /// Выдает награду за сдачу квеста
        /// </summary>
        protected void SetQuestReward(Client player) {
            var playerInfo = PlayerInfoManager.GetInfo(player);
            var clanRank = (int) playerInfo.Clan.Rank;
            var money = clanRank * MONEY_REWARD;
            var experience = clanRank * EXP_REWARD;
            PlayerInfoManager.SetBalance(player, money);
            PlayerInfoManager.SetExperience(player, experience);
            _clanManager.SetReputation(player, REP_REWARD, playerInfo);
            ClanManager.SetAuthority(playerInfo.Clan.ClanId, CLAN_AUTHORITY);
            player.resetData(PlayerData.CLAN_QUEST);
        }

        /// <summary>
        /// Анимация действия в квесте
        /// </summary>
        protected void PlayActionAnimation(Client player) {
            API.playPlayerAnimation(player, FLAG, "mp_common", "givetake2_a");
        }
    }
}