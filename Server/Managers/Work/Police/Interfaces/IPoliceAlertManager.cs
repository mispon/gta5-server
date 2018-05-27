using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Work.Police.Interfaces {
    internal interface IPoliceAlertManager {
        /// <summary>
        /// Запускает таймер, генерящий события патрулирования
        /// </summary>
        void RunAlertsGenerator();

        /// <summary>
        /// Показывает игроку все вызовы на карте
        /// </summary>
        void ShowAllAlerts(Client player);

        /// <summary>
        /// Скрывает все вызовы с карты
        /// </summary>
        void HideAllAlerts(Client player);

        /// <summary>
        /// Создать новый вызов
        /// </summary>
        void SendAlert(Vector3 position, string name);

        /// <summary>
        /// Завершить вызов полицейского
        /// </summary>
        void FinishAlert(int alertId);
    }
}