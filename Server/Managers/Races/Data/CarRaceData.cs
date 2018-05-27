using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Races.Data {
    /// <summary>
    /// Данные автогонок
    /// </summary>
    internal class CarRaceData {
        /// <summary>
        /// Точки маршрута трассы
        /// </summary>
        internal static List<Vector3> TrackPositions = new List<Vector3> {
            new Vector3(-156.68, -358.79, 32.73),
            new Vector3(-92.32, -141.47, 55.35),
            new Vector3(-29.82, 97.66, 76.21),
            new Vector3(0.98, 267.11, 107.80),
            new Vector3(-469.39, 250.80, 81.80),
            new Vector3(-535.72, 284.39, 81.69),
            new Vector3(-529.64, 434.93, 95.96),
            new Vector3(-474.87, 389.30, 100.03),
            new Vector3(-198.23, 422.40, 108.61),
            new Vector3(-120.71, 412.91, 111.72),
            new Vector3(24.65, 237.53, 108.25),
            new Vector3(-43.63, 45.57, 70.88),
            new Vector3(-85.01, -115.60, 56.52),
            new Vector3(-129.46, -270.48, 41.11),
            new Vector3(-194.45, -385.57, 30.90),
            new Vector3(-521.19, -388.69, 33.75),
            new Vector3(-523.71, -449.96, 32.91),
            new Vector3(-677.44, -437.49, 33.82),
            new Vector3(-792.40, -443.05, 35.06),
            new Vector3(-782.83, -408.34, 34.35),
            new Vector3(-843.25, -375.79, 38.04),
            new Vector3(-1034.41, -473.70, 35.54),
            new Vector3(-1198.36, -565.40, 25.99),
            new Vector3(-1307.79, -665.39, 25.07),
            new Vector3(-1227.64, -774.61, 17.01),
            new Vector3(-1159.24, -861.45, 12.78),
            new Vector3(-1089.98, -976.00, 2.85),
            new Vector3(-981.74, -1162.21, 2.53),
            new Vector3(-923.05, -1217.78, 3.73),
            new Vector3(-766.13, -1134.71, 9.34),
            new Vector3(-636.64, -1265.69, 9.39),
            new Vector3(-518.44, -1177.55, 18.33),
            new Vector3(-523.19, -1321.78, 27.27),
            new Vector3(-204.79, -1470.31, 29.88),
            new Vector3(-183.66, -1632.99, 31.99),
            new Vector3(-77.42, -1787.30, 26.68),
            new Vector3(207.80, -2031.17, 16.91),
            new Vector3(301.64, -1925.39, 24.18),
            new Vector3(366.65, -1853.47, 26.20),
            new Vector3(437.39, -1768.09, 27.53),
            new Vector3(505.45, -1675.09, 28.14),
            new Vector3(609.25, -1735.57, 28.07),
            new Vector3(927.07, -1762.89, 29.76)
        };

        /// <summary>
        /// Начальные позиции гонки
        /// Item1 - позиция
        /// Item2 - поворот
        /// </summary>
        internal static List<Tuple<Vector3, Vector3>> StartPositions = new List<Tuple<Vector3, Vector3>> {
            new Tuple<Vector3, Vector3>(new Vector3(-255.09, -619.34, 32.80), new Vector3(0.73, -3.43, -19.57)),
            new Tuple<Vector3, Vector3>(new Vector3(-249.90, -620.65, 33.00), new Vector3(0.85, -0.91, -21.51)),
            new Tuple<Vector3, Vector3>(new Vector3(-244.50, -622.17, 33.05), new Vector3(0.87, 0.00, -20.29)),
            new Tuple<Vector3, Vector3>(new Vector3(-239.57, -623.96, 33.03), new Vector3(0.76, 0.39, -19.80)),
            new Tuple<Vector3, Vector3>(new Vector3(-234.53, -625.62, 32.87), new Vector3(0.12, 3.38, -20.02)),
            new Tuple<Vector3, Vector3>(new Vector3(-257.18, -625.49, 32.71), new Vector3(0.78, -3.09, -19.24)),
            new Tuple<Vector3, Vector3>(new Vector3(-252.39, -627.45, 32.86), new Vector3(0.93, -0.91, -19.82)),
            new Tuple<Vector3, Vector3>(new Vector3(-247.44, -630.23, 32.89), new Vector3(1.05, 0.01, -21.26)),
            new Tuple<Vector3, Vector3>(new Vector3(-242.11, -631.71, 32.89), new Vector3(0.97, 0.11, -20.75)),
            new Tuple<Vector3, Vector3>(new Vector3(-237.72, -633.71, 32.77), new Vector3(0.72, 3.20, -19.43)),
        };

        /// <summary>
        /// Поворот финишной точки
        /// </summary>
        internal static Vector3 FinishPointRotation = new Vector3(0.93, 0.44, -94.78);
    }
}