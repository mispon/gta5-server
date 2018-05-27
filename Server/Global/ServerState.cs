using System.Collections.Concurrent;
using System.Collections.Generic;
using gta_mp_database.Entity;
using GrandTheftMultiplayer.Server.Elements;
using PlayerInfo = gta_mp_database.Models.Player.PlayerInfo;

namespace gta_mp_server.Global {
    /// <summary>
    /// Объекты сервера в глобальной видимости / Кэш
    /// </summary>
    internal class ServerState {
        /// <summary>
        /// Игроки на сервере
        /// NOTE: Работа с коллекцией производится через менеджеры
        /// </summary>
        public static ConcurrentDictionary<Client, PlayerInfo> Players = new ConcurrentDictionary<Client, PlayerInfo>();

        /// <summary>
        /// Дома игроков
        /// </summary>
        public static Dictionary<long, House> Houses = new Dictionary<long, House>();

        /// <summary>
        /// Кланы
        /// </summary>
        public static List<gta_mp_database.Entity.Clan> Clans = new List<gta_mp_database.Entity.Clan>();
    }
}