using System.Collections.Generic;
using gta_mp_server.Helpers;
using gta_mp_server.Models.Places;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Work.Police.Data {
    /// <summary>
    /// Вспомогательные данные полицейских
    /// </summary>
    internal class PoliceDataGetter {
        /// <summary>
        /// Возвращает рандомную позицию для патрулирования
        /// </summary>
        internal static Vector3 GetAlertPosition() {
            return _alertPointPositions[ActionHelper.Random.Next(_alertPointPositions.Count)];
        }

        /// <summary>
        /// Звания полиции
        /// </summary>
        internal static Dictionary<int, string> RankNames = new Dictionary<int, string> {
            [1] = "Рядовой",
            [2] = "Сержант",
            [3] = "Лейтенант",
            [4] = "Капитан",
            [5] = "Майор"
        };

        /// <summary>
        /// Снаряжение копа
        /// </summary>
        internal static Dictionary<int, List<WeaponHash>> Ammo = new Dictionary<int, List<WeaponHash>> {
            [1] = new List<WeaponHash> { WeaponHash.Nightstick },
            [2] = new List<WeaponHash> { WeaponHash.Nightstick, WeaponHash.StunGun },
            [3] = new List<WeaponHash> { WeaponHash.Nightstick, WeaponHash.StunGun, WeaponHash.CombatPistol },
            [4] = new List<WeaponHash> { WeaponHash.Nightstick, WeaponHash.StunGun, WeaponHash.CombatPistol, WeaponHash.CombatPDW },
            [5] = new List<WeaponHash> { WeaponHash.Nightstick, WeaponHash.StunGun, WeaponHash.CombatPistol, WeaponHash.CombatPDW, WeaponHash.AdvancedRifle }
        };

        /// <summary>
        /// Возвращает данные машин полиции
        /// </summary>
        internal static IEnumerable<SpawnVehicleInfo> GetVehiclePositions() {
            return new List<SpawnVehicleInfo> {
                new SpawnVehicleInfo {Hash = VehicleHash.Police, Position = new Vector3(408.1, -979.9, 28.7), Rotation = new Vector3(0.227, -0.03, -129.1)},
                new SpawnVehicleInfo {Hash = VehicleHash.Police, Position = new Vector3(407.87, -984.32, 28.7), Rotation = new Vector3(0.227, -0.03, -129.1)},
                new SpawnVehicleInfo {Hash = VehicleHash.Police, Position = new Vector3(407.76, -988.64, 28.7), Rotation = new Vector3(0.227, -0.03, -129.1)},
                new SpawnVehicleInfo {Hash = VehicleHash.Police, Position = new Vector3(407.87, -993.15, 28.7), Rotation = new Vector3(0.227, -0.03, -129.1)},
                new SpawnVehicleInfo {Hash = VehicleHash.Police, Position = new Vector3(407.72, -997.77, 28.7), Rotation = new Vector3(0.227, -0.03, -129.1)},
                new SpawnVehicleInfo {Hash = VehicleHash.Policeb, Position = new Vector3(406.9, -1002.75, 29.3), Rotation = new Vector3(0, 0, -53)},
                new SpawnVehicleInfo {Hash = VehicleHash.Policeb, Position = new Vector3(407.76, -1005.3, 28.75), Rotation = new Vector3(0, 0, -53)},
                new SpawnVehicleInfo {Hash = VehicleHash.Policeb, Position = new Vector3(407.68, -1008.5, 28.74), Rotation = new Vector3(0, 0, -53)},
                new SpawnVehicleInfo {Hash = VehicleHash.Police2, Position = new Vector3(452.5, -997, 25.37), Rotation = new Vector3(0.85, 0.014, -1.4)},
                new SpawnVehicleInfo {Hash = VehicleHash.Police2, Position = new Vector3(447.41, -997, 25.37), Rotation = new Vector3(0.85, 0.014, -1.4)},
                new SpawnVehicleInfo {Hash = VehicleHash.Police3, Position = new Vector3(463.31, -1014.9, 27.7), Rotation = new Vector3(0.16, -0.2, -89.34)},
                new SpawnVehicleInfo {Hash = VehicleHash.Police3, Position = new Vector3(463.36, -1019.87, 27.7), Rotation = new Vector3(0.16, -0.2, -89.34)},
            };
        }

        /// <summary>
        /// Позиции точек патрулирования
        /// </summary>
        private static readonly List<Vector3> _alertPointPositions = new List<Vector3> {
            new Vector3(1372.48, -1535.74, 54.71),
            new Vector3(1686.21, -1852.85, 106.97),
            new Vector3(1261.12, -2565.87, 41.29),
            new Vector3(209.25, -3119.97, 4.37),
            new Vector3(57.54, -2555.68, 4.58),
            new Vector3(1191.22, -3218.43, 4.38),
            new Vector3(-1037.37, -2667.30, 12.41),
            new Vector3(-423.37, -1711.14, 17.90),
            new Vector3(23.98, -1723.17, 27.88),
            new Vector3(486.00, -1975.48, 23.22),
            new Vector3(847.61, -2346.59, 28.91),
            new Vector3(1067.86, -2449.56, 27.61),
            new Vector3(1015.53, -2022.64, 29.55),
            new Vector3(870.72, -1662.93, 28.94),
            new Vector3(1201.27, -1499.07, 33.27),
            new Vector3(714.97, -1374.29, 24.80),
            new Vector3(865.89, -914.87, 24.59),
            new Vector3(1153.38, -478.27, 64.75),
            new Vector3(972.91, -123.39, 72.88),
            new Vector3(904.56, 29.33, 78.28),
            new Vector3(703.39, 242.65, 91.47),
            new Vector3(436.40, 258.90, 101.78),
            new Vector3(348.26, 48.61, 91.24),
            new Vector3(529.20, -179.06, 52.98),
            new Vector3(289.91, -337.67, 43.50),
            new Vector3(222.33, -35.28, 68.29),
            new Vector3(29.91, 82.44, 73.60),
            new Vector3(-81.53, 182.35, 86.22),
            new Vector3(-62.02, -219.06, 44.02),
            new Vector3(121.93, -416.91, 39.64),
            new Vector3(-39.18, -777.95, 42.80),
            new Vector3(385.71, -753.83, 27.87),
            new Vector3(309.14, -999.63, 27.52),
            new Vector3(439.25, -1158.44, 27.57),
            new Vector3(129.35, -1071.41, 27.47),
            new Vector3(147.70, -1281.41, 27.30),
            new Vector3(404.20, -1435.96, 27.71),
            new Vector3(504.89, -1499.93, 27.57),
            new Vector3(245.90, -1514.79, 27.40),
            new Vector3(484.14, -1776.92, 26.81),
            new Vector3(326.03, -2032.16, 19.21),
            new Vector3(186.37, -1725.26, 27.58),
            new Vector3(-227.82, -1385.64, 29.54),
            new Vector3(282.65, -1260.89, 27.52),
            new Vector3(-114.38, -1049.02, 25.56),
            new Vector3(-189.27, -822.71, 29.29),
            new Vector3(-546.60, -900.39, 22.27),
            new Vector3(-574.59, -1151.11, 20.46),
            new Vector3(-799.73, -1305.59, 3.28),
            new Vector3(-921.74, -942.62, 0.45),
            new Vector3(-833.46, -810.51, 17.77),
            new Vector3(-959.50, -337.00, 35.99),
            new Vector3(-1503.70, -539.16, 31.10),
            new Vector3(-1331.70, -392.13, 34.87),
            new Vector3(-1448.69, -363.44, 41.84),
            new Vector3(-1796.43, -397.13, 43.40),
            new Vector3(-1608.13, -923.92, 7.12),
            new Vector3(-1313.72, -1256.36, 2.85),
            new Vector3(-1191.33, -1494.29, 2.66),
            new Vector3(-1053.31, -1398.36, 3.71),
            new Vector3(-380.34, -120.00, 36.97),
            new Vector3(-947.19, 95.84, 50.24),
            new Vector3(-1549.42, 37.16, 56.12),
            new Vector3(-1238.54, 483.72, 91.28),
            new Vector3(-770.47, 279.51, 84.00),
            new Vector3(-646.10, 979.92, 237.87),
            new Vector3(151.35, 684.32, 206.18),
            new Vector3(-353.94, 475.59, 111.23),
            new Vector3(-1172.49, -735.60, 18.62)
        };
    }
}