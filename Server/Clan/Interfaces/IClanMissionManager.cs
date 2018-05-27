namespace gta_mp_server.Clan.Interfaces {
    internal interface IClanMissionManager {
        /// <summary>
        /// Игрок присоединяется к запуску миссии
        /// </summary>
        void VoteToStart(long clanId);

        /// <summary>
        /// Игрок отказывается от запуска миссии
        /// </summary>
        bool CancelVote(long clanId);
    }
}