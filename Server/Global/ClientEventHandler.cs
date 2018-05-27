using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Global {
    /// <summary>
    /// Обработчик событий клиента
    /// </summary>
    internal class ClientEventHandler {
        /// <summary>
        /// Коллекция обработчиков событий
        /// </summary>
        private static readonly Dictionary<string, Action<Client, object[]>> _handlers = new Dictionary<string, Action<Client, object[]>>();

        /// <summary>
        /// Добавить обработчик событий
        /// </summary>
        public static void Add(string eventName, Action<Client, object[]> action) {
            if (_handlers.ContainsKey(eventName)) {
                return;
            }
            _handlers.Add(eventName, action);
        }

        /// <summary>
        /// Удалить обработчик событий
        /// </summary>
        public static void Remove(string eventName) {
            if (_handlers.ContainsKey(eventName)) {
                _handlers.Remove(eventName);
            }
        }

        /// <summary>
        /// Обработать событие
        /// </summary>
        public static void Process(Client player, string eventName, params object[] args) {
            var action = _handlers[eventName];
            action(player, args);
        }
    }
}