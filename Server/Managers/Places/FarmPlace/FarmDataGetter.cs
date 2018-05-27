using System.Collections.Generic;
using gta_mp_server.Models.Places;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Places.FarmPlace {
    /// <summary>
    /// Данные работы фермером
    /// </summary>
    internal class FarmDataGetter {
        /// <summary>
        /// Точка сдачи урожая
        /// </summary>
        internal static Vector3 FarmEndPoint = new Vector3(2936.32, 4637.32, 47.54);

        /// <summary>
        /// Позиция доставки урожая
        /// </summary>
        internal static Vector3 HarvestDeliveryPoint = new Vector3(2775.68, 4753.38, 45.06);

        /// <summary>
        /// Точки сбора урожая
        /// </summary>
        internal static List<Vector3> FarmPoints = new List<Vector3> {
            new Vector3(2890.59, 4637.93, 47.73),
            new Vector3(2893.63, 4618.88, 47.42),
            new Vector3(2893.44, 4603.31, 47.09),
            new Vector3(2888.78, 4614.20, 47.20),
            new Vector3(2882.71, 4629.33, 47.91),
            new Vector3(2884.50, 4613.86, 47.25),
            new Vector3(2886.48, 4598.38, 47.03),
            new Vector3(2879.11, 4610.87, 47.20),
            new Vector3(2875.49, 4625.79, 47.86),
            new Vector3(2869.02, 4627.12, 47.93),
            new Vector3(2870.51, 4611.66, 47.20),
            new Vector3(2871.12, 4594.42, 46.87),
            new Vector3(2867.33, 4582.04, 46.35),
            new Vector3(2860.73, 4592.31, 46.81),
            new Vector3(2854.46, 4616.62, 47.57),
            new Vector3(2844.61, 4621.98, 47.71),
            new Vector3(2852.41, 4599.50, 46.95),
            new Vector3(2849.22, 4580.35, 46.33),
            new Vector3(2839.69, 4593.20, 46.43),
            new Vector3(2827.09, 4601.99, 45.57),
            new Vector3(2816.18, 4612.46, 44.84),
            new Vector3(2945.59, 4674.73, 48.30),
            new Vector3(2941.01, 4686.64, 49.89),
            new Vector3(2936.23, 4679.48, 49.05),
            new Vector3(2929.40, 4673.57, 48.19),
            new Vector3(2928.70, 4686.02, 49.48),
            new Vector3(2918.23, 4687.02, 48.88),
            new Vector3(2914.57, 4675.08, 48.25),
            new Vector3(2910.62, 4664.35, 48.18),
            new Vector3(2906.23, 4656.61, 48.24),
            new Vector3(2900.21, 4676.34, 48.11),
            new Vector3(2879.71, 4672.31, 47.34),
            new Vector3(2881.30, 4657.52, 47.53),
            new Vector3(2873.39, 4665.07, 47.28),
            new Vector3(2874.56, 4652.12, 47.50),
            new Vector3(2869.56, 4640.91, 47.70),
            new Vector3(2864.39, 4651.79, 47.41),
            new Vector3(2856.75, 4665.69, 46.90),
            new Vector3(2846.97, 4660.10, 46.94),
            new Vector3(2843.70, 4641.29, 47.51),
            new Vector3(2834.25, 4654.16, 46.09),
            new Vector3(2823.92, 4654.30, 45.45),
            new Vector3(2828.77, 4643.00, 45.41),
            new Vector3(2821.65, 4631.24, 45.14),
            new Vector3(2815.22, 4639.23, 44.42),
            new Vector3(2807.32, 4645.58, 44.14),
            new Vector3(2808.49, 4632.54, 44.21)
        };

        /// <summary>
        /// Данные тракторов
        /// </summary>
        internal static List<SpawnVehicleInfo> Tractors = new List<SpawnVehicleInfo> {
            new SpawnVehicleInfo {Position = new Vector3(2940.26, 4659.32, 47.67), Rotation = new Vector3(0.05, 0.00, 44.99), Hash = VehicleHash.Tractor2},
            new SpawnVehicleInfo {Position = new Vector3(2936.57, 4655.17, 47.67), Rotation = new Vector3(-0.03, 0.00, 43.50), Hash = VehicleHash.Tractor2},
            new SpawnVehicleInfo {Position = new Vector3(2923.51, 4642.69, 47.67), Rotation = new Vector3(0.17, 0.00, 44.38), Hash = VehicleHash.Tractor2},
            new SpawnVehicleInfo {Position = new Vector3(2920.22, 4638.85, 47.67), Rotation = new Vector3(0.14, 0.00, 42.07), Hash = VehicleHash.Tractor2},
            new SpawnVehicleInfo {Position = new Vector3(2916.53, 4634.71, 47.67), Rotation = new Vector3(-0.06, 0.00, 42.83), Hash = VehicleHash.Tractor2},
            new SpawnVehicleInfo {Position = new Vector3(2913.16, 4630.74, 47.65), Rotation = new Vector3(0.09, -1.03, 44.62), Hash = VehicleHash.Tractor2},
            new SpawnVehicleInfo {Position = new Vector3(2917.83, 4645.83, 47.68), Rotation = new Vector3(0.17, 0.56, -35.34), Hash = VehicleHash.RakeTrailer},
            new SpawnVehicleInfo {Position = new Vector3(2920.09, 4650.68, 47.71), Rotation = new Vector3(0.17, 0.56, -35.34), Hash = VehicleHash.RakeTrailer},
            new SpawnVehicleInfo {Position = new Vector3(2925.35, 4654.58, 47.67), Rotation = new Vector3(0.17, 0.56, -35.34), Hash = VehicleHash.RakeTrailer},
            new SpawnVehicleInfo {Position = new Vector3(2929.52, 4658.18, 47.68), Rotation = new Vector3(0.17, 0.56, -35.34), Hash = VehicleHash.RakeTrailer},
            new SpawnVehicleInfo {Position = new Vector3(2933.19, 4662.01, 47.67), Rotation = new Vector3(0.17, 0.56, -35.34), Hash = VehicleHash.RakeTrailer},
            new SpawnVehicleInfo {Position = new Vector3(2911.48, 4643.75, 47.78), Rotation = new Vector3(0.17, 0.56, -35.34), Hash = VehicleHash.RakeTrailer}
        };

        /// <summary>
        /// Данные загруженных трейлеров
        /// </summary>
        internal static List<SpawnVehicleInfo> LoadedTrailers = new List<SpawnVehicleInfo> {
            new SpawnVehicleInfo {Position = new Vector3(2948.81, 4643.51, 47.67), Rotation = new Vector3(0.01, 0.03, 46.08), Hash = VehicleHash.BaleTrailer},
            new SpawnVehicleInfo {Position = new Vector3(2945.92, 4639.94, 47.67), Rotation = new Vector3(0.28, 0.01, 45.41), Hash = VehicleHash.BaleTrailer}
        };

        /// <summary>
        /// Маршрут трактора по полю
        /// </summary>
        internal static List<Vector3> TractorRoute = new List<Vector3> {
            new Vector3(2944.23, 4681.19, 49.24),
            new Vector3(2931.00, 4683.83, 49.51),
            new Vector3(2920.42, 4675.90, 48.46),
            new Vector3(2936.68, 4686.02, 49.76),
            new Vector3(2926.16, 4678.05, 48.80),
            new Vector3(2914.02, 4677.72, 48.39),
            new Vector3(2905.46, 4660.73, 48.21),
            new Vector3(2907.84, 4677.74, 48.25),
            new Vector3(2881.12, 4666.32, 47.47),
            new Vector3(2872.08, 4653.80, 47.49),
            new Vector3(2858.73, 4655.23, 47.37),
            new Vector3(2878.55, 4652.38, 47.57),
            new Vector3(2865.03, 4656.93, 47.30),
            new Vector3(2854.54, 4649.81, 47.53),
            new Vector3(2839.97, 4654.75, 46.68),
            new Vector3(2830.14, 4644.90, 45.63),
            new Vector3(2840.34, 4653.88, 46.75),
            new Vector3(2831.32, 4641.04, 45.83),
            new Vector3(2817.12, 4646.54, 44.48),
            new Vector3(2820.16, 4609.40, 45.42),
            new Vector3(2846.31, 4581.06, 46.47),
            new Vector3(2858.70, 4584.27, 46.47),
            new Vector3(2844.95, 4612.71, 47.18),
            new Vector3(2856.72, 4614.73, 47.49),
            new Vector3(2871.81, 4582.93, 46.46),
            new Vector3(2859.89, 4602.12, 47.04),
            new Vector3(2869.07, 4618.67, 47.68),
            new Vector3(2877.12, 4586.54, 46.51),
            new Vector3(2886.63, 4598.66, 47.02),
            new Vector3(2879.65, 4625.19, 47.85),
            new Vector3(2886.47, 4622.24, 47.72)
        };
    }
}