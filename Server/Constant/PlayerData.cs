namespace gta_mp_server.Constant {
    /// <summary>
    /// Флаги, специфичные для игроков
    /// </summary>
    internal class PlayerData {
        /// <summary>
        /// Последняя возиция игрока
        /// </summary>
        internal const string LAST_POSITION = "LastPosition";

        /// <summary>
        /// Последнее измерение игрока
        /// </summary>
        internal const string LAST_DIMENSION = "LastDimension";

        /// <summary>
        /// Флаг того, что игрок зарегистрирован на эвент или гонку
        /// </summary>
        internal const string IS_REGISTERED = "IsRegistered";

        /// <summary>
        /// Игрок принимает участие в эвенте
        /// </summary>
        internal const string ON_EVENT = "PlayerOnEvent";

        /// <summary>
        /// Игрок принимает участие в гонках
        /// </summary>
        internal const string ON_RACE = "OnRace";

        /// <summary>
        /// Тип гонок, в которых принимает участие игрок
        /// </summary>
        internal const string RACE_TYPE = "RaceType";

        /// <summary>
        /// Игрок был убит на эвенте
        /// </summary>
        internal const string WAS_KILLED = "PlayerWasKilled";

        /// <summary>
        /// Флаг, что игрок покинул зону эвента
        /// </summary>
        internal const string LEAVE_EVENT = "LeaveEvent";

        /// <summary>
        /// Флаг убийцы игрока
        /// </summary>
        internal const string KILLER = "Killer";

        /// <summary>
        /// Флаг участника уличной драки
        /// </summary>
        internal const string FIGHTER = "Fighter";

        /// <summary>
        /// Время последнего крика
        /// </summary>
        internal const string CHAT_SCREAM = "ChatScreamTime";

        /// <summary>
        /// Текущий интервал награды
        /// </summary>
        internal const string GIFT_INTERVAL = "GiftInterval";

        /// <summary>
        /// Задание банды
        /// </summary>
        internal const string CLAN_QUEST = "ClanQuest";
    }
}