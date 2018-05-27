using gta_mp_server.Constant;
using gta_mp_server.Events.Interfaces;
using gta_mp_server.Managers.Auth;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Events {
    internal class PlayerFinishDownloadManager : Script, IPlayerFinishDownloadManager {
        /// <summary>
        /// Обработчик завершения загрузки данных игроком
        /// </summary>
        public void OnFinishDownload(Client player) {
            player.setSyncedData(LoginManager.DISABLE_HOTKEYS, true);
            API.sendNativeToPlayer(player, Hash.DISPLAY_RADAR, false);
            API.triggerClientEvent(player, ServerEvent.SHOW_LOGIN);
        }
    }
}