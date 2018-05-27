namespace gta_mp_server.Enums {
    /// <summary>
    /// Состояния полицейских
    /// </summary>
    public enum PolicemanState {
        /// <summary>
        /// Нет задержанных
        /// </summary>
        NoPrisoner = 1,

        /// <summary>
        /// Есть задержанный
        /// </summary>
        WithPrisoner = 2,

        /// <summary>
        /// Задержанный в машине
        /// </summary>
        PrisonerInCar = 3,

        /// <summary>
        /// У полицейского нет состояния
        /// </summary>
        NoData = -1
    }
}