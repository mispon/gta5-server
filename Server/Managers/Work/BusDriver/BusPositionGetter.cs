using System.Collections.Generic;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Work.BusDriver {
    /// <summary>
    /// Позиции работы водителя автобуса
    /// </summary>
    internal class BusPositionGetter {
        /// <summary>
        /// Позиции автобусов
        /// </summary>
        public static List<Vector3> GetBusPositions() {
            return new List<Vector3> {
                new Vector3(38.66262, -885.7814, 30.05629),
                new Vector3(45.47654, -887.486, 30.0478),
                new Vector3(52.08877, -889.8126, 30.03715),
                new Vector3(52.48101, -868.479, 30.34551),
                new Vector3(45.56001, -866.905, 30.35408),
                new Vector3(59.1962, -869.7655, 30.3412),
                new Vector3(62.50557, -852.6475, 30.60876),
                new Vector3(55.83046, -850.2854, 30.62629),
                new Vector3(39.95807, -844.549, 30.67337),
                new Vector3(33.06276, -862.6744, 30.37846),
                new Vector3(26.56431, -881.1082, 30.07878),
                new Vector3(33.46073, -842.2034, 30.69547),
                new Vector3(26.55771, -860.0689, 30.40243),
                new Vector3(19.74788, -879.0526, 30.08735)
            };
        }

        /// <summary>
        /// Позиции красной ветки
        /// </summary>
        public static List<Vector3> GetRedRoute() {
            return new List<Vector3> {
                new Vector3(96.28, -636.77, 30.38),
                new Vector3(843.33, 66.99, 66.16),
                new Vector3(1522.47, 817.93, 76.29),
                new Vector3(2430.38, 2894.69, 39.08),
                new Vector3(2983.45, 4611.95, 51.96),
                new Vector3(2254.66, 5193.83, 59.11),
                new Vector3(1661.42, 4856.50, 40.75),
                new Vector3(2103.04, 4717.74, 40.01),
                new Vector3(2672.44, 4347.29, 44.44),
                new Vector3(2679.17, 3251.03, 54.07),
                new Vector3(2096.45, 1373.31, 74.20),
                new Vector3(2527.77, 392.29, 110.96),
                new Vector3(11.66, -1126.39, 27.41),
                new Vector3(-44.69, -1018.57, 27.60)
            };
        }

        /// <summary>
        /// Позиции синей ветки
        /// </summary>
        public static List<Vector3> GetBlueRoute() {
            return new List<Vector3> {
                new Vector3(-191.01, -672.70, 32.74),
                new Vector3(-1336.99, -434.54, 33.02),
                new Vector3(-1554.68, 1385.06, 125.07),
                new Vector3(-1093.85, 2674.65, 18.08),
                new Vector3(175.60, 2904.49, 45.23),
                new Vector3(1483.60, 4503.94, 51.30),
                new Vector3(1850.76, 3666.54, 32.88),
                new Vector3(1794.64, 3320.03, 40.55),
                new Vector3(542.48, 2693.27, 41.15),
                new Vector3(-73.51, 1848.07, 198.92),
                new Vector3(220.01, 274.51, 104.28),
                new Vector3(207.27, -262.22, 50.22),
                new Vector3(253.63, -568.75, 42.01),
                new Vector3(115.73, -784.27, 30.12)
            };
        }

        /// <summary>
        /// Позиции зеленой ветки
        /// </summary>
        public static List<Vector3> GetGreenRoute() {
            return new List<Vector3> {
                new Vector3(-435.62, -829.04, 29.71),
                new Vector3(-1181.59, -1269.64, 4.96),
                new Vector3(-2974.62, 480.90, 14.08),
                new Vector3(-3097.25, 1140.48, 19.25),
                new Vector3(-2217.65, 4279.83, 46.19),
                new Vector3(-152.56, 6211.02, 30.03),
                new Vector3(66.53, 6623.95, 30.34),
                new Vector3(-351.52, 6325.47, 28.71),
                new Vector3(-747.87, 5520.83, 34.69),
                new Vector3(-3231.95, 1034.72, 10.51),
                new Vector3(-1850.08, -596.11, 10.29),
                new Vector3(-455.73, -538.77, 23.99),
                new Vector3(145.56, -582.34, 42.76),
                new Vector3(-78.22, -919.13, 28.05)
            };
        }

        /// <summary>
        /// Позиции желтой ветки
        /// </summary>
        public static List<Vector3> GetYellowRoute() {
            return new List<Vector3> {
                new Vector3(62.26, -206.88, 52.02),
                new Vector3(-317.51, 264.39, 85.85),
                new Vector3(-1747.33, -417.86, 42.43),
                new Vector3(-1212.55, -1218.68, 6.38),
                new Vector3(96.97, -2050.75, 17.05),
                new Vector3(828.21, -1863.91, 28.01),
                new Vector3(807.90, -1351.94, 25.06),
                new Vector3(460.04, 115.50, 97.49),
                new Vector3(-145.80, -301.51, 38.08),
                new Vector3(-155.96, -1515.97, 32.60),
                new Vector3(360.07, -1784.03, 27.81),
                new Vector3(284.20, -1292.19, 28.48),
                new Vector3(40.29, -962.65, 28.09)
            };
        }
    }
}