using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;

namespace gta_mp_server.Managers.Work.Loader.Interfaces {
    internal interface ILoaderEventHandler {
        /// <summary>
        /// Обработчик получения предмета
        /// </summary>
        void OnTakeThing(ColShape shape, NetHandle entity);

        /// <summary>
        /// Обработчик сдачи предмета
        /// </summary>
        bool OnPutThing(ColShape shape, Client player);
    }
}