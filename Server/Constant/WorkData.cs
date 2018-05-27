namespace gta_mp_server.Constant {
    /// <summary>
    /// Константы установки данных игрока для различных работ
    /// </summary>
    internal class WorkData {
        /// <summary>
        /// Грузчик
        /// </summary>
        public const string IS_LOADER = "IsLoader";

        /// <summary>
        /// Водитель погрузчика
        /// </summary>
        public const string IS_FORKLIFT = "IsForklift";

        /// <summary>
        /// Строитель
        /// </summary>
        public const string IS_BUILDER = "IsBuilder";

        /// <summary>
        /// Водитель автобуса
        /// </summary>
        public const string IS_BUS_DRIVER = "IsBusDriver";
        public const string RED_SHAPE_VALUE = "red_bus_stop_{0}";
        public const string BLUE_SHAPE_VALUE = "blue_bus_stop_{0}";
        public const string YELLOW_SHAPE_VALUE = "yellow_bus_stop_{0}";
        public const string GREEN_SHAPE_VALUE = "green_bus_stop_{0}";

        /// <summary>
        /// Таксист
        /// </summary>
        public const string IS_TAXI_DIVER = "IsTaxiDriver";

        /// <summary>
        /// Полицейский
        /// </summary>
        public const string IS_POLICEMAN = "IsPoliceman";
        public const string ALERT_ZONE_KEY = "PoliceAlertZone";

        /// <summary>
        /// Закусочная
        /// </summary>
        public const string IS_FOOD_TRUCK_DRIVER = "IsFoodTruckDriver";
        public const string IS_FOOD_DELIVERYMAN = "IsFoodDeliveryman";

        /// <summary>
        /// Дальнобойщик
        /// </summary>
        public const string IS_TRUCKER = "IsTrucker";

        /// <summary>
        /// Эвакуаторщик
        /// </summary>
        public const string IS_WRECKER = "IsWrecker";

        /// <summary>
        /// Пилот
        /// </summary>
        public const string IS_PILOT = "IsPilot";

        /// <summary>
        /// Рыбак
        /// </summary>
        public const string IS_FISHERMAN = "IsFisherman";
        public const string IS_FISHERMAN_ON_BOAT = "IsFishermanOnBoat";

        /// <summary>
        /// Контракт на доставку груза
        /// </summary>
        public const string DELIVERY_CONTRACT = "DeliveryContract";

        /// <summary>
        /// Ферма
        /// </summary>
        public const string IS_FARMER = "IsFarmer";
        public const string IS_TRACTOR_DRIVER = "IsTractorDriver";
    }
}