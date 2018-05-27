using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.Auth.Interfaces {
    internal interface ICreatingCharManager {
        /// <summary>
        /// Показать окно создания персонажа
        /// </summary>
        void ShowCreator(Client player);
    }
}