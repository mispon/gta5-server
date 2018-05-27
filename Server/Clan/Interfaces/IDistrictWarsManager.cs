namespace gta_mp_server.Clan.Interfaces {
    internal interface IDistrictWarsManager {
        /// <summary>
        /// Запускает войну за территорию
        /// </summary>
        void StartWar();

        /// <summary>
        /// Завершает войну
        /// </summary>
        void FinishWar();
    }
}