using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Races.Data {
    /// <summary>
    /// Данные мотогонки
    /// </summary>
    internal class MotoRaceData {
        /// <summary>
        /// Точки маршрута трассы
        /// </summary>
        internal static List<Vector3> TrackPositions = new List<Vector3> {
            new Vector3(-63.68, -540.45, 30.71),
            new Vector3(6.20, -626.17, 14.51),
            new Vector3(31.59, -700.32, 14.89),
            new Vector3(72.32, -771.44, 16.20),
            new Vector3(162.57, -587.41, 17.40),
            new Vector3(407.22, -540.29, 7.66),
            new Vector3(435.13, -580.76, 14.27),
            new Vector3(526.75, -894.73, 14.79),
            new Vector3(530.86, -1140.76, 27.77),
            new Vector3(519.06, -1299.23, 28.20),
            new Vector3(564.99, -1210.46, 8.75),
            new Vector3(637.40, -853.87, 11.90),
            new Vector3(776.25, -874.36, 24.02),
            new Vector3(763.11, -939.32, 23.26),
            new Vector3(668.32, -1000.61, 21.11),
            new Vector3(668.81, -1098.51, 22.35),
            new Vector3(675.35, -1290.09, 23.28),
            new Vector3(792.01, -1562.86, 19.52),
            new Vector3(808.79, -1872.61, 19.06),
            new Vector3(730.32, -2489.83, 9.44),
            new Vector3(534.37, -2615.54, 11.13),
            new Vector3(208.47, -2579.18, 5.09)
        };

        /// <summary>
        /// Начальные позиции гонки
        /// Item1 - позиция
        /// Item2 - поворот
        /// </summary>
        internal static List<Tuple<Vector3, Vector3>> StartPositions = new List<Tuple<Vector3, Vector3>> {
            new Tuple<Vector3, Vector3>(new Vector3(-192.15, -516.38, 26.39), new Vector3(1.74, -9.32, -92.26)),
            new Tuple<Vector3, Vector3>(new Vector3(-192.14, -519.96, 26.46), new Vector3(3.26, -10.54, -88.61)),
            new Tuple<Vector3, Vector3>(new Vector3(-192.13, -523.06, 26.53), new Vector3(1.60, -8.80, -92.21)),
            new Tuple<Vector3, Vector3>(new Vector3(-192.06, -525.76, 26.54), new Vector3(1.61, -7.31, -90.67)),
            new Tuple<Vector3, Vector3>(new Vector3(-192.21, -528.57, 26.54), new Vector3(1.61, -7.62, -92.16)),
            new Tuple<Vector3, Vector3>(new Vector3(-192.14, -532.10, 26.54), new Vector3(1.60, -8.61, -91.03)),
            new Tuple<Vector3, Vector3>(new Vector3(-192.15, -534.29, 26.50), new Vector3(1.55, -7.15, -90.59)),
            new Tuple<Vector3, Vector3>(new Vector3(-192.30, -537.58, 26.37), new Vector3(1.51, -8.31, -91.24)),
            new Tuple<Vector3, Vector3>(new Vector3(-197.68, -530.22, 26.36), new Vector3(1.59, -9.97, -88.65)),
            new Tuple<Vector3, Vector3>(new Vector3(-197.90, -524.16, 26.35), new Vector3(1.57, -10.06, -88.06))
        };

        /// <summary>
        /// Поворот финишной точки
        /// </summary>
        internal static Vector3 FinishPointRotation = new Vector3(0.31, -8.21, 57.73);
    }
}