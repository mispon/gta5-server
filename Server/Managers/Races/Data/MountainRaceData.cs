using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Races.Data {
    /// <summary>
    /// Данные гонки "Горный трек"
    /// </summary>
    internal class MountainRaceData {
        /// <summary>
        /// Точки маршрута трассы
        /// </summary>
        internal static List<Vector3> TrackPositions = new List<Vector3> {
            new Vector3(-552.97, 5421.20, 61.59),
            new Vector3(-570.24, 5054.49, 131.28),
            new Vector3(-281.63, 4947.45, 262.13),
            new Vector3(38.42, 5051.68, 456.23),
            new Vector3(342.99, 5442.40, 671.91),
            new Vector3(768.69, 5696.83, 694.27),
            new Vector3(1169.36, 5576.51, 535.55),
            new Vector3(1854.39, 5405.40, 227.76),
            new Vector3(2622.83, 5110.99, 43.32)
        };

        /// <summary>
        /// Начальные позиции гонки
        /// Item1 - позиция
        /// Item2 - поворот
        /// </summary>
        internal static List<Tuple<Vector3, Vector3>> StartPositions = new List<Tuple<Vector3, Vector3>> {
            new Tuple<Vector3, Vector3>(new Vector3(-907.79, 5413.33, 35.79), new Vector3(-2.70, -11.00, -73.88)),
            new Tuple<Vector3, Vector3>(new Vector3(-908.38, 5414.98, 35.79), new Vector3(-2.68, -12.86, -75.02)),
            new Tuple<Vector3, Vector3>(new Vector3(-909.15, 5417.14, 35.78), new Vector3(-2.70, -11.39, -75.81)),
            new Tuple<Vector3, Vector3>(new Vector3(-909.52, 5419.96, 35.75), new Vector3(-2.57, -13.01, -76.70)),
            new Tuple<Vector3, Vector3>(new Vector3(-910.48, 5422.37, 35.77), new Vector3(-2.42, -9.88, -72.26)),
            new Tuple<Vector3, Vector3>(new Vector3(-913.66, 5421.35, 35.91), new Vector3(-2.56, -9.17, -72.25)),
            new Tuple<Vector3, Vector3>(new Vector3(-912.41, 5418.50, 35.89), new Vector3(-2.15, -13.49, -73.49)),
            new Tuple<Vector3, Vector3>(new Vector3(-912.04, 5416.23, 35.92), new Vector3(-2.68, -11.63, -77.83)),
            new Tuple<Vector3, Vector3>(new Vector3(-911.49, 5414.38, 35.94), new Vector3(-2.66, -10.74, -76.26)),
            new Tuple<Vector3, Vector3>(new Vector3(-911.29, 5411.92, 35.96), new Vector3(-2.11, -10.30, -73.88))
        };

        /// <summary>
        /// Поворот финишной точки
        /// </summary>
        internal static Vector3 FinishPointRotation = new Vector3(1.66, -8.62, -78.66);
    }
}