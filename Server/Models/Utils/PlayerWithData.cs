using gta_mp_database.Models.Player;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Models.Utils {
    /// <summary>
    /// Связка игрока с его данными
    /// </summary>
    internal class PlayerWithData {
        /// <summary>
        /// Инстанс игрока
        /// </summary>
        public Client Player { get; set; }

        /// <summary>
        /// Данные игрока
        /// </summary>
        public PlayerInfo PlayerInfo { get; set; }
    }
}