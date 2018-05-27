using System;
using System.Collections.Generic;
using System.Timers;
using gta_mp_server.GameEvents.Interfaces;
using gta_mp_server.Managers;

namespace gta_mp_server.GameEvents {
    /// <summary>
    /// Логика запусков эвентов
    /// </summary>
    internal class EventsScheduler : IScheduler {
        internal static GameEvent ActiveEvent;
        private static readonly Queue<GameEvent> _events = new Queue<GameEvent>();
        private int _lastHour;

        public EventsScheduler(IEnumerable<GameEvent> events) {
            foreach (var gameEvent in events) {
                _events.Enqueue(gameEvent);
            }
        }

        /// <summary>
        /// Запуск шедалера
        /// </summary>
        public void Initialize() {
            _lastHour = DateTime.Now.Hour;
            var timer = new Timer(60000);
            timer.Elapsed += StartGameEvent;
            timer.Start();
        }

        /// <summary>
        /// Запускает игровой эвент, если пришло время
        /// </summary>
        private void StartGameEvent(object sender, ElapsedEventArgs elapsedEventArgs) {
            if (ActiveEvent == null && (_lastHour < DateTime.Now.Hour || _lastHour == 23 && DateTime.Now.Hour == 0)) {
                _lastHour = DateTime.Now.Hour;
                ActiveEvent = _events.Dequeue();
                ActiveEvent.Initialize();
                _events.Enqueue(ActiveEvent);
            }
            if (ActiveEvent != null && DateTime.Now.Minute == 10) {
                ActiveEvent.Start();
                ActiveEvent = null;
            }
        }
    }
}