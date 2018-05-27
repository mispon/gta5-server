using GrandTheftMultiplayer.Server.API;

namespace gta_mp_server.Managers.Places.Interfaces {
    /// <summary>
    /// Базовый класс любого игрового места
    /// </summary>
    public abstract class Place : Script {
        /// <summary>
        /// Инизиализировать место
        /// </summary>
        public abstract void Initialize();
    }
}