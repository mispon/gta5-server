using System;
using System.Collections.Generic;
using System.Timers;
using gta_mp_server.Managers.Races.Interfaces;
using gta_mp_server.Managers.Races.TrackHandlers.Interfaces;

namespace gta_mp_server.Managers.Races {
    /// <summary>
    /// Отвечает за запуск гонок
    /// </summary>
    internal class RaceScheduler : IScheduler {
        private readonly IEnumerable<Race> _races;
        private readonly IEnumerable<TrackHandler> _trackHandlers;

        public RaceScheduler(IEnumerable<Race> races, IEnumerable<TrackHandler> trackHandlers) {
            _races = races;
            _trackHandlers = trackHandlers;
        } 

        /// <summary>
        /// Инициализация шедалера
        /// </summary>
        public void Initialize() {
            foreach (var handler in _trackHandlers) {
                handler.CreateTrack();
            }
            var timer = new Timer(50000);
            timer.Elapsed += Schedule;
            timer.Start();
        }

        /// <summary>
        /// Запускает гонки каждые полчаса
        /// </summary>
        private void Schedule(object sender, ElapsedEventArgs elapsedEventArgs) {
            if (DateTime.Now.Minute == 0 || DateTime.Now.Minute == 30) {
                StartRaces();
            }
            if (DateTime.Now.Minute == 10 || DateTime.Now.Minute == 40) {
                FinishRaces();
            }
        }

        /// <summary>
        /// Запустить гонки
        /// </summary>
        private void StartRaces() {
            foreach (var race in _races) {
                if (race.InProgress) {
                    continue;
                }
                race.Start();
            }
        }

        /// <summary>
        /// Принудительно завершить гонки
        /// </summary>
        private void FinishRaces() {
            foreach (var race in _races) {
                if (race.InProgress) {
                    race.Finish();
                }
            }
        }
    }
}