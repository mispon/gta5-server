using System;
using System.Timers;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Managers;

namespace gta_mp_server.Clan.DistrictWar {
    /// <summary>
    /// Планировщик запуска уличных войн
    /// </summary>
    internal class DistrictWarsScheduler : IScheduler {
        private static bool _warInProgress;

        private readonly IDistrictWarsManager _districtWarsManager;

        public DistrictWarsScheduler(IDistrictWarsManager districtWarsManager) {
            _districtWarsManager = districtWarsManager;
        }

        /// <summary>
        /// Инициализировать таймеры
        /// </summary>
        public void Initialize() {
            var timer = new Timer(55000);
            timer.Elapsed += ProcessWar;
            timer.Start();
        }

        /// <summary>
        /// Запустить войну за территорию
        /// </summary>
        private void ProcessWar(object sender, ElapsedEventArgs elapsedEventArgs) {
            switch (DateTime.Now.Hour) {
                case 20 when DateTime.Now.Minute == 0:
                    if (!_warInProgress) {
                        _districtWarsManager.StartWar();
                        _warInProgress = true;
                    }
                    break;
                case 21 when DateTime.Now.Minute == 0:
                    if (_warInProgress) {
                        _districtWarsManager.FinishWar();
                        _warInProgress = false;
                    }
                    break;
            }
        }
    }
}