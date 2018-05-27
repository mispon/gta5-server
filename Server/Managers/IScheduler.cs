namespace gta_mp_server.Managers {
    /// <summary>
    /// Интерфейс реализуют все планировщики
    /// </summary>
    internal interface IScheduler {
        /// <summary>
        /// Инициализация
        /// </summary>
        void Initialize();
    }
}