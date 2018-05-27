using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Work.Taxi.Interfaces {
    public interface ITaxiDriverManager {
        /// <summary>
        /// Обработчик выбранного клиентом маршрута
        /// </summary>
        void ProcessPassengerTarget(Client player, Vector3 target);
    }
}