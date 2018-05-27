using gta_mp_database.Enums;
using GrandTheftMultiplayer.Shared;

namespace gta_mp_server.Managers.House.Interfaces {
    internal interface IHouseEventManager {
        /// <summary>
        /// Игрок подошел ко входу
        /// </summary>
        void OnPlayerWentToEnter(NetHandle entity, long houseId);

        /// <summary>
        /// Игрок отошел от входа / выхода
        /// </summary>
        void OnPlayerAway(NetHandle entity);

        /// <summary>
        /// Обработчик гардероба
        /// </summary>
        void OnPlayerEnterWardrobe(NetHandle entity, HouseType type);

        /// <summary>
        /// Обработчик хранилища
        /// </summary>
        void OnPlayerEnterStorage(NetHandle entity, HouseType type);

        /// <summary>
        /// Обработчик выхода из дома
        /// </summary>
        void OnPlayerExit(NetHandle entity, long houseId);

        /// <summary>
        /// Игрок подошел ко входу в гараж
        /// </summary>
        void OnPlayerWentToGarageEnter(NetHandle entity, long houseId);

        /// <summary>
        /// Обработчик выхода из гаража
        /// </summary>
        void OnPlayerExitGarage(NetHandle entity, long houseId);
    }
}