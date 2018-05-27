using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.Work.Bistro.Interfaces {
    internal interface IBistroManager {
        /// <summary>
        /// Создает точки доставки
        /// </summary>
        void CreatePoints();

        /// <summary>
        /// Выдать награду водителю фургона с едой
        /// </summary>
        void SetTruckDriverRevard(string driverName, int foodPrice, string clientName);

        /// <summary>
        /// Показать точку доставки еды
        /// </summary>
        void ShowNextDeliveryPoint(Client player);
    }
}