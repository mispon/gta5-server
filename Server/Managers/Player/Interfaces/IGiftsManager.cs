using gta_mp_data.Entity;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.Player.Interfaces {
    internal interface IGiftsManager {
        /// <summary>
        /// Инициализация
        /// </summary>
        void Initialize();

        /// <summary>
        /// Награда за каждодневный вход в игру
        /// </summary>
        void ProcessDaysGift(Client player, Account account);

        /// <summary>
        /// Запустить таймер выдачи награды за нахождение в игре
        /// </summary>
        void StartGiftsTimer(Client player);
    }
}