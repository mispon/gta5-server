using System;
using System.Threading.Tasks;
using gta_mp_server.Events.Interfaces;
using gta_mp_server.Global;
using gta_mp_server.Global.Interfaces;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using GrandTheftMultiplayer.Server.API;
using Ninject;
using Ninject.Syntax;

namespace gta_mp_server {
    internal class Main : Script {
        private const int UPDATE_DELAY = 10;

        private readonly IWorldInitializer _worldInitializer;
        private readonly IDatabaseSynchronizer _databaseSynchronizer;
        private readonly IPlayerFinishDownloadManager _finishDownloadManager;
        private readonly IPlayerConnectManager _playerConnectManager;
        private readonly IPlayerDisconnectManager _playerDisconnectManager;
        private readonly IPlayerRespawnManager _playerRespawn;
        private readonly IPlayerDamagedManager _playerDamagedManager;
        private readonly IPlayerDeathManager _playerDeathManager;
        private readonly IVehicleEnterManager _vehicleEnterManager;
        private readonly IChatHandler _chatHandler;
        private readonly IPlayerManager _playerManager;
        private readonly IVehicleManager _vehicleManager;
        private readonly IVehicleInfoManager _vehicleInfoManager;
        private readonly IGiftsManager _giftsManager;

        /// <summary>
        /// Конструктор с обработчиками
        /// </summary>
        public Main() : this(new StandardKernel(new DatabaseCoreModule(), new ServerCoreModule())) {
            API.onResourceStart += OnStart;
            API.onUpdate += OnUpdate;
            API.onPlayerFinishedDownload += _finishDownloadManager.OnFinishDownload;
            API.onPlayerConnected += _playerConnectManager.OnPlayerConnected;
            API.onPlayerRespawn += _playerRespawn.OnPlayerRespawn;
            API.onPlayerDisconnected += _playerDisconnectManager.OnPlayerDisconnect;
            API.onPlayerDamaged += _playerDamagedManager.OnPlayerDamaged;
            API.onPlayerDeath += _playerDeathManager.OnPlayerDeath;
            API.onPlayerEnterVehicle += _vehicleEnterManager.OnPlayerEnterVehicle;
            API.onPlayerExitVehicle += _vehicleEnterManager.OnPlayerExitVehicle;
            API.onChatMessage += (sender, message, cancel) => _chatHandler.OnChatMessage(sender, message, cancel);
            API.onClientEventTrigger += ClientEventHandler.Process;
            API.onResourceStop += OnStop;
        }

        /// <summary>
        /// Конструктор с инициализацией полей
        /// </summary>
        public Main(IResolutionRoot kernel) {
            ServerKernel.Kernel = kernel;
            _worldInitializer = kernel.Get<IWorldInitializer>();
            _databaseSynchronizer = kernel.Get<IDatabaseSynchronizer>();
            _finishDownloadManager = kernel.Get<IPlayerFinishDownloadManager>();
            _playerConnectManager = kernel.Get<IPlayerConnectManager>();
            _playerDisconnectManager = kernel.Get<IPlayerDisconnectManager>();
            _playerRespawn = kernel.Get<IPlayerRespawnManager>();
            _playerDamagedManager = kernel.Get<IPlayerDamagedManager>();
            _playerDeathManager = kernel.Get<IPlayerDeathManager>();
            _vehicleEnterManager = kernel.Get<IVehicleEnterManager>();
            _chatHandler = kernel.Get<IChatHandler>();
            _playerManager = kernel.Get<IPlayerManager>();
            _vehicleManager = kernel.Get<IVehicleManager>();
            _vehicleInfoManager = kernel.Get<IVehicleInfoManager>();
            _giftsManager = kernel.Get<IGiftsManager>();
        }

        /// <summary>
        /// Старт сервера 
        /// </summary>
        private void OnStart() {
            _giftsManager.Initialize();
            _worldInitializer.Initialize();
            _databaseSynchronizer.Start();
            WeatherManager.Start();
            API.setGamemodeName("gtagrime.ru");
            API.consoleOutput($"{DateTime.Now:T}: Server started!");
        }

        /// <summary>
        /// Остановка сервера
        /// </summary>
        private void OnStop() {
            _vehicleInfoManager.ParkAllVehicles();
        }

        /// <summary>
        /// Обновление состояния сервера
        /// </summary>
        private DateTime _lastUpDateTime;
        private void OnUpdate() {
            if (DateTime.Now.Subtract(_lastUpDateTime).TotalSeconds <= UPDATE_DELAY) {
                return;
            }
            _lastUpDateTime = DateTime.Now;
            Task.Run(() => _playerManager.UpdatePlayers());
            Task.Run(() => _vehicleManager.UpdateVehicles());
            API.setTime(DateTime.Now.Hour, DateTime.Now.Minute);
        }
    }
}