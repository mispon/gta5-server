using gta_mp_server.Models.Clan;

namespace gta_mp_server.Clan.Interfaces {
    internal interface IClanCourtyard {
        /// <summary>
        /// Проинициализировать дворик
        /// </summary>
        void Initialize(long clanId, ClanCourtyardInfo courtyard);

        /// <summary>
        /// Отображать маркеры двориков
        /// </summary>
        void ShowMarkers();

        /// <summary>
        /// Скрыть маркеры двориков
        /// </summary>
        void HideMarkers();
    }
}