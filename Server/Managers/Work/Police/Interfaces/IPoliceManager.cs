using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.Work.Police.Interfaces {
    internal interface IPoliceManager {
        /// <summary>
        /// Инициализировать обработчик действий полизейских
        /// </summary>
        void Initialize();

        /// <summary>
        /// Возвращает арестованного игрока
        /// </summary>
        Client GetAttachedPlayer(Client player);

        /// <summary>
        /// Привязывает игрока к полицейскому
        /// </summary>
        void AttachPrisoner(Client policeman, Client prisoner, bool withData = false);

        /// <summary>
        /// Отвязывает игрока от полицейского
        /// </summary>
        void DetachPrisoner(Client policeman, Client prisoner, bool withData = false);
    }
}