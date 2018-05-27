using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Races.Data {
    /// <summary>
    /// Данные ралли
    /// </summary>
    internal class RallyRaceData {
        /// <summary>
        /// Точки маршрута трассы
        /// </summary>
        internal static List<Vector3> TrackPositions = new List<Vector3> {
            new Vector3(888.88, 2453.53, 49.52),
            new Vector3(1019.52, 2441.60, 43.37),
            new Vector3(1159.01, 2468.00, 52.82),
            new Vector3(1150.95, 2255.04, 48.30),
            new Vector3(1121.19, 2382.45, 49.49),
            new Vector3(990.59, 2403.22, 50.69),
            new Vector3(978.93, 2341.61, 47.42),
            new Vector3(1033.34, 2258.35, 42.88),
            new Vector3(1117.41, 2245.03, 48.25),
            new Vector3(1140.39, 2156.09, 52.41),
            new Vector3(1081.76, 2213.26, 46.85),
            new Vector3(913.90, 2265.11, 43.99),
            new Vector3(895.38, 2408.41, 48.60),
            new Vector3(957.32, 2475.67, 48.56),
            new Vector3(1067.63, 2438.61, 48.77),
            new Vector3(1162.52, 2444.41, 54.17),
            new Vector3(1146.12, 2251.60, 48.46),
            new Vector3(1139.34, 2345.34, 53.21),
            new Vector3(1119.27, 2454.25, 50.65),
            new Vector3(962.55, 2392.42, 49.45),
            new Vector3(983.70, 2335.90, 47.48),
            new Vector3(1003.46, 2249.99, 45.94),
            new Vector3(1077.49, 2245.68, 43.37),
            new Vector3(1166.64, 2197.74, 49.10),
            new Vector3(1089.49, 2171.98, 52.29),
            new Vector3(1033.74, 2213.67, 48.00),
            new Vector3(900.16, 2299.32, 44.88),
            new Vector3(892.42, 2407.46, 48.67)
        };

        /// <summary>
        /// Начальные позиции гонки
        /// Item1 - позиция
        /// Item2 - поворот
        /// </summary>
        internal static List<Tuple<Vector3, Vector3>> StartPositions = new List<Tuple<Vector3, Vector3>> {
            new Tuple<Vector3, Vector3>(new Vector3(863.83, 2407.34, 51.22), new Vector3(-3.85, 9.42, -90.17)),
            new Tuple<Vector3, Vector3>(new Vector3(864.91, 2402.00, 51.08), new Vector3(-4.30, -2.01, -91.48)),
            new Tuple<Vector3, Vector3>(new Vector3(854.93, 2407.26, 52.04), new Vector3(-6.54, 8.54, -88.59)),
            new Tuple<Vector3, Vector3>(new Vector3(853.37, 2402.19, 52.02), new Vector3(-4.38, -0.62, -91.13)),
            new Tuple<Vector3, Vector3>(new Vector3(845.41, 2406.86, 52.79), new Vector3(-4.63, 2.20, -89.69)),
            new Tuple<Vector3, Vector3>(new Vector3(844.50, 2401.95, 52.83), new Vector3(-4.95, -4.99, -96.07)),
            new Tuple<Vector3, Vector3>(new Vector3(830.93, 2400.25, 53.75), new Vector3(-1.50, 1.39, -66.61)),
            new Tuple<Vector3, Vector3>(new Vector3(826.74, 2403.94, 53.99), new Vector3(-3.11, 1.86, -77.94)),
            new Tuple<Vector3, Vector3>(new Vector3(825.85, 2409.35, 54.24), new Vector3(-4.57, 1.76, -109.76)),
            new Tuple<Vector3, Vector3>(new Vector3(829.35, 2415.81, 54.50), new Vector3(-5.47, 1.05, -145.61))
        };

        /// <summary>
        /// Поворот финишной точки
        /// </summary>
        internal static Vector3 FinishPointRotation = new Vector3(0.13, 1.24, 6.66);
    }
}