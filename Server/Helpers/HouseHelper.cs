using System;
using gta_mp_database.Enums;
using gta_mp_server.Enums;

namespace gta_mp_server.Helpers {
    /// <summary>
    /// Вспомогательные методы для домов
    /// </summary>
    internal class HouseHelper {
        /// <summary>
        /// Возвращает статус дома по отношению к игроку
        /// </summary>
        public static HouseState GetHouseState(long houseOwnerId, long playerId) {
            if (houseOwnerId == Validator.INVALID_ID) {
                return HouseState.FreeHouse;
            }
            return houseOwnerId == playerId ? HouseState.OwnerEnter : HouseState.AnotherEnter;
        }

        /// <summary>
        /// Проверка, что есть места в гараже
        /// </summary>
        public static bool GarageIsFull(HouseType houseType, int vehiclesInGatage) {
            var maxCount = GetMaxPlacesInGarage(houseType);
            return vehiclesInGatage >= maxCount;
        }

        /// <summary>
        /// Возвращает шанс взлома по типу дома
        /// </summary>
        public static int GetBreakChance(HouseType type) {
            switch (type) {
                case HouseType.Eco:
                case HouseType.Eco2:
                case HouseType.Eco3:
                    return 30;
                case HouseType.Standart:
                case HouseType.Standart2:
                    return 20;
                case HouseType.Premium:
                case HouseType.Premium2:
                case HouseType.Premium3:
                    return 10;
                case HouseType.Elite:
                case HouseType.Elite2:
                    return 3;
                default:
                    throw new ArgumentException("Неизвестный тип дома!");
            }
        }

        /// <summary>
        /// Возвращает лейбл дома на миникарте по его типу
        /// </summary>
        public static string GetHouseLabel(HouseType type, string playerName) {
            string houseName;
            switch (type) {
                case HouseType.Eco:
                case HouseType.Eco2:
                case HouseType.Eco3:
                    houseName = "Экономный";
                    break;
                case HouseType.Standart:
                case HouseType.Standart2:
                    houseName = "Стандартный";
                    break;
                case HouseType.Premium:
                case HouseType.Premium2:
                case HouseType.Premium3:
                    houseName = "Премиум";
                    break;
                case HouseType.Elite:
                case HouseType.Elite2:
                    houseName = "Элитный";
                    break;
                default:
                    throw new ArgumentException("Неизвестный тип дома!");
            }
            return GetOwnerName(houseName, playerName);
        }

        /// <summary>
        /// Форматирует имя для текста метки
        /// </summary>
        private static string GetOwnerName(string houseName, string playerName) {
            return string.IsNullOrEmpty(playerName) 
                ? houseName 
                : $"{houseName} ({playerName})";
        }

        /// <summary>
        /// Возвращает максимально допустимое количество машин в гараже
        /// </summary>
        private static int GetMaxPlacesInGarage(HouseType type) {
            switch (type) {
                case HouseType.Eco:
                case HouseType.Eco2:
                case HouseType.Eco3:
                case HouseType.Standart:
                case HouseType.Standart2:
                    return 2;
                case HouseType.Premium:
                case HouseType.Premium2:
                case HouseType.Premium3:
                    return 4;
                case HouseType.Elite:
                case HouseType.Elite2:
                    return 10;
                default:
                    throw new ArgumentException("Неизвестный тип дома!");
            }
        }
    }
}