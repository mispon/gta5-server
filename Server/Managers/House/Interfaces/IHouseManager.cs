using System.Collections.Generic;
using HouseInfo = gta_mp_database.Entity.House;


namespace gta_mp_server.Managers.House.Interfaces {
    internal interface IHouseManager {
        /// <summary>
        /// Загрузить дома
        /// </summary>
        void Initialize();

        /// <summary>
        /// Записать информацию о доме
        /// </summary>
        void SetHouse(HouseInfo house);

        /// <summary>
        /// Возвращает информацию о доме
        /// </summary>
        HouseInfo GetHouse(long houseId);

        /// <summary>
        /// Обновить иконку дома на карте
        /// </summary>
        void UpdateBlip(HouseInfo house, string playerName = "");

        /// <summary>
        /// Возвращает дома игрока
        /// </summary>
        List<HouseInfo> GetPlayerHouses(long playerId);
    }
}