using System.Collections.Generic;
using gta_mp_server.Helpers;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Work.Builder {
    /// <summary>
    /// Данные работы строителем
    /// </summary>
    internal class BuilderDataGetter {
        private static readonly string[] _scenarios = {"WORLD_HUMAN_WELDING", "WORLD_HUMAN_HAMMERING"};

        /// <summary>
        /// Рабочие точки
        /// </summary>
        public static List<WorkPoint> Points = new List<WorkPoint> {
            new WorkPoint {Position = new Vector3(-464.50, -879.45, 28.39), WorkerRotation = new Vector3(0.00, 0.00, 1.58)},
            new WorkPoint {Position = new Vector3(-446.70, -892.53, 28.39), WorkerRotation = new Vector3(0.00, 0.00, -72.42)},
            new WorkPoint {Position = new Vector3(-455.17, -918.31, 28.39), WorkerRotation = new Vector3(0.00, 0.00, 96.67)},
            new WorkPoint {Position = new Vector3(-457.74, -953.47, 28.39), WorkerRotation = new Vector3(0.00, 0.00, -88.32)},
            new WorkPoint {Position = new Vector3(-447.95, -957.20, 37.68), WorkerRotation = new Vector3(0.00, 0.00, -177.12)},
            new WorkPoint {Position = new Vector3(-454.61, -934.84, 37.68), WorkerRotation = new Vector3(0.00, 0.00, 90.44)},
            new WorkPoint {Position = new Vector3(-454.66, -901.99, 37.68), WorkerRotation = new Vector3(0.00, 0.00, 91.72)},
            new WorkPoint {Position = new Vector3(-447.84, -879.50, 37.68), WorkerRotation = new Vector3(0.00, 0.00, 5.65)},
            new WorkPoint {Position = new Vector3(-470.99, -879.20, 37.68), WorkerRotation = new Vector3(0.00, 0.00, 53.91)},
            new WorkPoint {Position = new Vector3(-454.50, -883.25, 46.98), WorkerRotation = new Vector3(0.00, 0.00, 90.43)},
            new WorkPoint {Position = new Vector3(-456.15, -903.62, 46.98), WorkerRotation = new Vector3(0.00, 0.00, 5.25)},
            new WorkPoint {Position = new Vector3(-454.38, -934.69, 46.98), WorkerRotation = new Vector3(0.00, 0.00, 96.44)},
            new WorkPoint {Position = new Vector3(-447.77, -946.97, 46.98), WorkerRotation = new Vector3(0.00, 0.00, 6.01)},
            new WorkPoint {Position = new Vector3(-486.96, -986.89, 51.48), WorkerRotation = new Vector3(0.00, 0.00, -91.33)},
            new WorkPoint {Position = new Vector3(-501.56, -986.74, 51.48), WorkerRotation = new Vector3(0.00, 0.00, 94.44)},
            new WorkPoint {Position = new Vector3(-496.07, -1006.83, 51.48), WorkerRotation = new Vector3(0.00, 0.00, -94.42)},
            new WorkPoint {Position = new Vector3(-490.61, -1032.77, 51.48), WorkerRotation = new Vector3(0.00, 0.00, 118.09)},
            new WorkPoint {Position = new Vector3(-470.27, -1052.46, 51.48), WorkerRotation = new Vector3(0.00, 0.00, 62.64)},
            new WorkPoint {Position = new Vector3(-452.64, -1055.91, 51.48), WorkerRotation = new Vector3(0.00, 0.00, -133.81)},
            new WorkPoint {Position = new Vector3(-496.08, -1007.02, 39.81), WorkerRotation = new Vector3(0.00, 0.00, -91.54)},
            new WorkPoint {Position = new Vector3(-486.93, -987.31, 39.81), WorkerRotation = new Vector3(0.00, 0.00, -92.35)},
            new WorkPoint {Position = new Vector3(-490.48, -1024.02, 39.81), WorkerRotation = new Vector3(0.00, 0.00, 90.29)},
            new WorkPoint {Position = new Vector3(-493.37, -1034.44, 39.81), WorkerRotation = new Vector3(0.00, 0.00, -72.41)},
            new WorkPoint {Position = new Vector3(-487.09, -1053.15, 39.81), WorkerRotation = new Vector3(0.00, 0.00, -126.04)},
            new WorkPoint {Position = new Vector3(-470.43, -1050.51, 39.81), WorkerRotation = new Vector3(0.00, 0.00, 158.03)},
            new WorkPoint {Position = new Vector3(-460.93, -1069.53, 39.81), WorkerRotation = new Vector3(0.00, 0.00, 139.20)},
            new WorkPoint {Position = new Vector3(-461.46, -1074.11, 34.29), WorkerRotation = new Vector3(0.00, 0.00, -5.98)},
            new WorkPoint {Position = new Vector3(-448.09, -1055.39, 28.13), WorkerRotation = new Vector3(0.00, 0.00, 145.05)},
            new WorkPoint {Position = new Vector3(-473.26, -1052.74, 28.13), WorkerRotation = new Vector3(0.00, 0.00, -38.69)},
            new WorkPoint {Position = new Vector3(-498.98, -1036.79, 28.13), WorkerRotation = new Vector3(0.00, 0.00, 119.30)},
            new WorkPoint {Position = new Vector3(-493.59, -1034.08, 28.13), WorkerRotation = new Vector3(0.00, 0.00, -62.89)},
            new WorkPoint {Position = new Vector3(-492.83, -1006.93, 28.13), WorkerRotation = new Vector3(0.00, 0.00, 81.22)},
            new WorkPoint {Position = new Vector3(-482.73, -985.03, 28.13), WorkerRotation = new Vector3(0.00, 0.00, 99.48)},
            new WorkPoint {Position = new Vector3(-505.46, -1017.46, 28.14), WorkerRotation = new Vector3(0.00, 0.00, -26.37)}
        };

        /// <summary>
        /// Возвращает случайную анимацию рабочего
        /// </summary>
        public static string GetScenario() {
            var index = ActionHelper.Random.Next(2);
            return _scenarios[index];
        }
    }
}