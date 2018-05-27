using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using gta_mp_database.Providers.Interfaces;
using gta_mp_server.Global.Interfaces;
using gta_mp_server.Helpers;

namespace gta_mp_server.Global {
    /// <summary>
    /// Синхронизация кэша с базой данных
    /// </summary>
    internal class DatabaseSynchronizer : IDatabaseSynchronizer {
        private const int SYNC_TIMEOUT = 300000; // 5 мин.

        private readonly IPlayersProvider _playersProvider;
        private readonly IHousesProvider _housesProvider;
        private readonly IVehiclesProvider _vehiclesProvider;
        private readonly IClanProvider _clanProvider;

        public DatabaseSynchronizer(IPlayersProvider playersProvider, IHousesProvider housesProvider,
            IVehiclesProvider vehiclesProvider, IClanProvider clanProvider) {
            _playersProvider = playersProvider;
            _housesProvider = housesProvider;
            _vehiclesProvider = vehiclesProvider;
            _clanProvider = clanProvider;
        }

        /// <summary>
        /// Запустить синхронайзер
        /// </summary>
        public void Start() {
            var timer = new Timer {Enabled = true, AutoReset = true, Interval = SYNC_TIMEOUT};
            timer.Elapsed += (sender, args) => Task.Run(() => Synchronize());
        }

        /// <summary>
        /// Синхронизировать кэш с бд
        /// </summary>
        private void Synchronize() {
            UpdatePlayersData();
            var playersInfos = ServerState.Players.Values.ToList();
            _playersProvider.UpdatePlayersInfos(playersInfos);
            var houses = ServerState.Houses.Values.ToList();
            _housesProvider.UpdateHouses(houses);
            _clanProvider.SaveClans(ServerState.Clans);
            foreach (var player in playersInfos) {
                _vehiclesProvider.Update(player.Vehicles.Values);
            }
        }

        /// <summary>
        /// Обновить данные о метонахождении игроков
        /// </summary>
        private static void UpdatePlayersData() {
            foreach (var playerInfo in ServerState.Players) {
                var position = playerInfo.Key.position;
                playerInfo.Value.LastPosition = PositionConverter.VectorToString(position);
            }
        }
    }
}