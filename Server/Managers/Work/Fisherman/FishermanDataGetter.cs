using System.Collections.Generic;
using gta_mp_server.Models.Places;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Work.Fisherman {
    /// <summary>
    /// Данные работы рыбаков
    /// </summary>
    internal class FishermanDataGetter {
        /// <summary>
        /// Лодки
        /// </summary>
        internal static List<SpawnVehicleInfo> Boats = new List<SpawnVehicleInfo> {
            new SpawnVehicleInfo {Position = new Vector3(1319.75, 4281.72, 29.70), Rotation = new Vector3(5.45, -0.60, 82.00)},
            new SpawnVehicleInfo {Position = new Vector3(1318.80, 4274.36, 29.70), Rotation = new Vector3(4.36, -0.71, 81.62)},
            new SpawnVehicleInfo {Position = new Vector3(1317.16, 4266.86, 29.70), Rotation = new Vector3(4.04, 0.13, 81.42)},
            new SpawnVehicleInfo {Position = new Vector3(1316.24, 4259.72, 29.70), Rotation = new Vector3(1.33, 1.14, 81.69)},
            new SpawnVehicleInfo {Position = new Vector3(1314.23, 4252.19, 29.60), Rotation = new Vector3(4.68, -0.64, 76.88)},
            new SpawnVehicleInfo {Position = new Vector3(1313.80, 4243.72, 29.65), Rotation = new Vector3(7.45, 3.84, 74.58)},
            new SpawnVehicleInfo {Position = new Vector3(1312.09, 4236.95, 28.95), Rotation = new Vector3(5.40, 0.07, 80.18)}
        };

        /// <summary>
        /// Рабочие точки на пристани
        /// </summary>
        internal static List<WorkPoint> PierPoints = new List<WorkPoint> {
            new WorkPoint {Position = new Vector3(1361.02, 4280.09, 30.30), WorkerRotation = new Vector3(0.00, 0.00, 174.04)},
            new WorkPoint {Position = new Vector3(1337.77, 4269.37, 30.50), WorkerRotation = new Vector3(0.00, 0.00, -146.24)},
            new WorkPoint {Position = new Vector3(1331.03, 4270.29, 30.50), WorkerRotation = new Vector3(0.00, 0.00, 175.82)},
            new WorkPoint {Position = new Vector3(1306.97, 4269.88, 32.91), WorkerRotation = new Vector3(0.00, 0.00, 95.74)},
            new WorkPoint {Position = new Vector3(1305.66, 4262.12, 32.91), WorkerRotation = new Vector3(0.00, 0.00, 77.65)},
            new WorkPoint {Position = new Vector3(1303.12, 4248.84, 32.91), WorkerRotation = new Vector3(0.00, 0.00, 89.19)},
            new WorkPoint {Position = new Vector3(1300.23, 4234.90, 32.91), WorkerRotation = new Vector3(0.00, 0.00, 93.39)},
            new WorkPoint {Position = new Vector3(1298.32, 4224.67, 32.91), WorkerRotation = new Vector3(0.00, 0.00, 80.54)},
            new WorkPoint {Position = new Vector3(1302.26, 4219.42, 32.91), WorkerRotation = new Vector3(0.00, 0.00, -107.26)},
            new WorkPoint {Position = new Vector3(1299.03, 4215.51, 32.91), WorkerRotation = new Vector3(0.00, 0.00, 168.18)},
            new WorkPoint {Position = new Vector3(1310.50, 4229.55, 32.92), WorkerRotation = new Vector3(0.00, 0.00, -162.31)},
            new WorkPoint {Position = new Vector3(1323.30, 4229.76, 32.92), WorkerRotation = new Vector3(0.00, 0.00, -15.01)},
            new WorkPoint {Position = new Vector3(1335.45, 4224.96, 32.92), WorkerRotation = new Vector3(0.00, 0.00, 176.13)},
            new WorkPoint {Position = new Vector3(1340.65, 4224.89, 32.92), WorkerRotation = new Vector3(0.00, 0.00, -94.97)},
            new WorkPoint {Position = new Vector3(1297.60, 4286.09, 30.25), WorkerRotation = new Vector3(0.00, 0.00, 128.18)},
            new WorkPoint {Position = new Vector3(1279.28, 4293.09, 30.34), WorkerRotation = new Vector3(0.00, 0.00, 145.74)},
            new WorkPoint {Position = new Vector3(1256.06, 4322.85, 30.57), WorkerRotation = new Vector3(0.00, 0.00, 136.39)}
        };

        /// <summary>
        /// Рабочие точки на пристани
        /// </summary>
        internal static List<WorkPoint> BoatPoints = new List<WorkPoint> {
            new WorkPoint {Position = new Vector3(1172.65, 4271.19, 30.06)},
            new WorkPoint {Position = new Vector3(1155.45, 4010.01, 29.43)},
            new WorkPoint {Position = new Vector3(1252.12, 3823.67, 29.86)},
            new WorkPoint {Position = new Vector3(1470.39, 3863.52, 30.44)},
            new WorkPoint {Position = new Vector3(1383.72, 4043.10, 29.77)},
            new WorkPoint {Position = new Vector3(1381.50, 4203.60, 29.66)},
            new WorkPoint {Position = new Vector3(1504.40, 4147.15, 29.93)},
            new WorkPoint {Position = new Vector3(1592.17, 3976.69, 29.01)},
            new WorkPoint {Position = new Vector3(1788.93, 4049.95, 29.12)},
            new WorkPoint {Position = new Vector3(1776.13, 4204.37, 30.04)},
            new WorkPoint {Position = new Vector3(1672.10, 4394.49, 29.30)},
            new WorkPoint {Position = new Vector3(1503.65, 4243.67, 29.42)},
            new WorkPoint {Position = new Vector3(986.26, 4235.16, 29.84)},
            new WorkPoint {Position = new Vector3(793.82, 4072.70, 28.57)},
            new WorkPoint {Position = new Vector3(915.28, 3893.71, 29.68)},
            new WorkPoint {Position = new Vector3(964.32, 3719.18, 29.40)},
            new WorkPoint {Position = new Vector3(1079.37, 3842.77, 29.66)},
            new WorkPoint {Position = new Vector3(1076.93, 4025.47, 29.30)}
        };
    }
}