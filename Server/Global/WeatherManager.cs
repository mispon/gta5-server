using System.Collections.Generic;
using gta_mp_server.Helpers;
using GrandTheftMultiplayer.Server.API;

namespace gta_mp_server.Global {
    /// <summary>
    /// Управление погодой на сервере
    /// </summary>
    internal class WeatherManager : Script {
        private static int _currentWeather;

        /// <summary>
        /// Дерево переходов погоды
        /// </summary>
        private static readonly Dictionary<int, List<int>> _weatherTree = new Dictionary<int, List<int>> {
            [0] = new List<int> {0, 1, 2},
            [1] = new List<int> {0, 1, 2, 4, 8},
            [2] = new List<int> {0, 1, 2, 4, 5},
            [3] = new List<int> {1, 2, 4, 5, 9},
            [4] = new List<int> {1, 2, 3, 4, 5, 6},
            [5] = new List<int> {2, 3, 4, 5, 6},
            [6] = new List<int> {2, 3, 4, 5, 6, 7},
            [7] = new List<int> {1, 2, 3, 4, 5},
            [8] = new List<int> {0, 1, 2, 4, 8},
            [9] = new List<int> {1, 2, 3, 4, 5, 9},
            //[10] = new List<int> {10, 11, 12, 13},
            //[11] = new List<int> {10, 11, 12, 13},
            //[12] = new List<int> {10, 11, 12, 13},
            //[13] = new List<int> {10, 11, 12, 13}, // до зимы
            //[14] = new List<int> { }, // halloween
        };

        /// <summary>
        /// Запустить управление погодой
        /// </summary>
        internal static void Start() {
            ActionHelper.StartTimer(3600000, () => {
                var weathersChoise = _weatherTree[_currentWeather];
                _currentWeather = weathersChoise[ActionHelper.Random.Next(weathersChoise.Count)];
                API.shared.setWeather(_currentWeather);
            });
        }
    }
}