namespace gta_mp_server.Enums {
    /// <summary>
    /// Состояние игрока по отношению к дому
    /// </summary>
    internal enum HouseState {
        /// <summary>
        /// Свободный дом
        /// </summary>
        FreeHouse = 1,

        /// <summary>
        /// Вход в свой дом
        /// </summary>
        OwnerEnter = 2,

        /// <summary>
        /// Вход в чужой дом
        /// </summary>
        AnotherEnter = 3,

        /// <summary>
        /// Выход из дома
        /// </summary>
        Exit = 4,

        /// <summary>
        /// Вход в гараж
        /// </summary>
        GarageEnter = 5,

        /// <summary>
        /// Выход из гаража
        /// </summary>
        GarageExit = 6
    }
}