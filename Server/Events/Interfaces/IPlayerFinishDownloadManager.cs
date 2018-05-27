using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Events.Interfaces {
    public interface IPlayerFinishDownloadManager {
        /// <summary>
        /// Обработчик завершения загрузки данных игроком
        /// </summary>
        void OnFinishDownload(Client player);
    }
}