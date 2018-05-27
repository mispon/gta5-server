using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Helpers.Interfaces {
    public interface IDoormanager {
        /// <summary>
        /// Зарегистрировать дверь
        /// </summary>
        int Register(int modelHash, Vector3 position);

        /// <summary>
        /// Установить состояние двери
        /// </summary>
        void SetDoorState(int doorId, bool locked, float heading);
    }
}