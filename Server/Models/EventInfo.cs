namespace gta_mp_server.Models {
    /// <summary>
    /// Информация об активном эвенте
    /// </summary>
    internal class EventInfo {
        /// <summary>
        /// Имя эвента
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип эвента
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Количество участников
        /// </summary>
        public int TotalMembers { get; set; }

        /// <summary>
        /// Максимальное количество участников
        /// </summary>
        public int MaxMembers { get; set; }

        /// <summary>
        /// Является ли игрок участником
        /// </summary>
        public bool IsMember { get; set; }
    }
}