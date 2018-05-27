using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Global.Interfaces {
    internal interface IChatHandler {
        /// <summary>
        /// Обработчик сообщения
        /// </summary>
        void OnChatMessage(Client player, string message, CancelEventArgs clancel);
    }
}