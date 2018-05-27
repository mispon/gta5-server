using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.Vehicles;
using gta_mp_server.Quests.ClanQuests;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Quests.QuestsData {
    /// <summary>
    /// Данные задания угона машины
    /// </summary>
    internal class VehicleTheftData {
        /// <summary>
        /// Модели угоняемых машин
        /// </summary>
        private static readonly List<VehicleHash> _hijackedVehicles = new List<VehicleHash> {
            VehicleHash.Superd, VehicleHash.Tyrant, VehicleHash.T20, VehicleHash.Zentorno,
            VehicleHash.Tezeract, VehicleHash.Entity2, VehicleHash.Taipan, VehicleHash.Autarch
        };

        /// <summary>
        /// Возвращает случайную модель транспорта
        /// </summary>
        internal static VehicleHash GetVehicleHash() {
            return _hijackedVehicles[ActionHelper.Random.Next(_hijackedVehicles.Count)];
        }

        /// <summary>
        /// Позиции угоняемых машин
        /// </summary>
        private static readonly List<Tuple<Vector3, Vector3>> _hijackedVehiclesPositions = new List<Tuple<Vector3, Vector3>> {
            new Tuple<Vector3, Vector3>(new Vector3(1018.50, -773.24, 56.31), new Vector3(-2.40, -0.17, 129.67)),
            new Tuple<Vector3, Vector3>(new Vector3(912.17, -484.75, 57.43), new Vector3(-0.15, 0.15, 24.19)),
            new Tuple<Vector3, Vector3>(new Vector3(319.23, 495.15, 151.07), new Vector3(-6.51, 1.35, -74.72)),
            new Tuple<Vector3, Vector3>(new Vector3(-1857.92, 326.87, 87.07), new Vector3(-0.91, 0.06, 15.99)),
            new Tuple<Vector3, Vector3>(new Vector3(-1948.42, 200.43, 84.46), new Vector3(5.40, -0.91, 114.91)),
            new Tuple<Vector3, Vector3>(new Vector3(-1613.42, -376.67, 41.77), new Vector3(-0.50, 0.24, -140.65)),
            new Tuple<Vector3, Vector3>(new Vector3(-1021.73, -892.61, 3.94), new Vector3(-2.28, 2.91, -148.01)),
            new Tuple<Vector3, Vector3>(new Vector3(-323.95, -1110.99, 21.36), new Vector3(-1.23, 0.03, 161.29)),
            new Tuple<Vector3, Vector3>(new Vector3(-106.42, -607.64, 34.47), new Vector3(-0.39, -1.11, 161.11)),
            new Tuple<Vector3, Vector3>(new Vector3(-180.87, -178.35, 42.02), new Vector3(-0.06, -0.01, 161.29)),
            new Tuple<Vector3, Vector3>(new Vector3(-531.58, -34.10, 42.91), new Vector3(-0.17, -0.03, 175.44)),
            new Tuple<Vector3, Vector3>(new Vector3(241.31, -372.04, 42.69), new Vector3(0.52, 1.36, -109.80)),
            new Tuple<Vector3, Vector3>(new Vector3(459.35, -1095.04, 27.60), new Vector3(-0.12, -0.01, 91.55)),
            new Tuple<Vector3, Vector3>(new Vector3(98.82, -1824.27, 24.63), new Vector3(-0.67, -3.60, 84.45)),
            new Tuple<Vector3, Vector3>(new Vector3(850.06, -2180.42, 28.73), new Vector3(-0.63, -2.60, -1.20)),
            new Tuple<Vector3, Vector3>(new Vector3(-67.84, 894.78, 233.94), new Vector3(0.08, 0.05, -66.51)),
            new Tuple<Vector3, Vector3>(new Vector3(-615.21, 678.17, 148.12), new Vector3(1.34, -2.46, 169.98)),
            new Tuple<Vector3, Vector3>(new Vector3(-975.59, 524.84, 79.87), new Vector3(-0.19, 0.00, -32.92)),
            new Tuple<Vector3, Vector3>(new Vector3(-876.01, -25.00, 40.16), new Vector3(3.93, 0.28, -15.18)),
            new Tuple<Vector3, Vector3>(new Vector3(-841.19, -768.22, 19.71), new Vector3(-0.22, 5.12, -91.04))
        };

        /// <summary>
        /// Возвращает позицию угона
        /// </summary>
        internal static Tuple<Vector3, Vector3> GetHijackedPosition() {
            Tuple<Vector3, Vector3> result;
            var allVehicles = VehicleManager.GetAllVehicles(VehicleTheft.THIEFT_VEHICLE);
            var tries = 0;
            do {
                result = tries < _hijackedVehiclesPositions.Count 
                    ? _hijackedVehiclesPositions[ActionHelper.Random.Next(_hijackedVehiclesPositions.Count)] 
                    : _hijackedVehiclesPositions.First();
                tries++;
            } while (allVehicles.Any(e => Vector3.Distance(e.position, result.Item1) < 2f));
            return result;
        }

        /// <summary>
        /// Позиции сдачи транспорта
        /// </summary>
        internal static Dictionary<long, Vector3> EndPointPositions = new Dictionary<long, Vector3> {
            [1] = new Vector3(-726.81, 93.33, 53.98),
            [2] = new Vector3(1130.13, -1302.30, 33.74),
            [3] = new Vector3(-1009.73, -1465.62, 3.40)
        };
    }
}