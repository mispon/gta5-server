using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Work.Bistro {
    /// <summary>
    /// Хранит позиции закусочной
    /// </summary>
    internal class BistroPositionsGetter {
        /// <summary>
        /// Точки доставки
        /// </summary>
        internal static readonly List<PointInfo> DeliveryPoints = new List<PointInfo> {
            new PointInfo {
                Point = new Vector3(-1871.65, -340.24, 48.41),
                PedPosition = new Vector3(-1871.46, -339.22, 49.42),
                PedRotation = new Vector3(0.00, 0.00, 167.81),
                Hash = PedHash.BarryCutscene,
                IsMale = true
            },
            new PointInfo {
                Point = new Vector3(-1275.98, 303.53, 63.99),
                PedPosition = new Vector3(-1275.41, 303.74, 64.99),
                PedRotation = new Vector3(0.00, 0.00, 114.83),
                Hash = PedHash.Abigail,
                IsMale = false
            },
            new PointInfo {
                Point = new Vector3(-338.45, 267.47, 84.74),
                PedPosition = new Vector3(-339.18, 267.36, 85.69),
                PedRotation = new Vector3(0.00, 0.00, -82.28),
                Hash = PedHash.Prisguard01SMM,
                IsMale = true
            },
            new PointInfo {
                Point = new Vector3(230.39, 338.47, 104.53),
                PedPosition = new Vector3(230.13, 337.90, 105.54),
                PedRotation = new Vector3(0.00, 0.00, -24.91),
                Hash = PedHash.AviSchwartzman,
                IsMale = true
            },
            new PointInfo {
                Point = new Vector3(922.79, 41.70, 79.91),
                PedPosition = new Vector3(923.62, 41.22, 80.90),
                PedRotation = new Vector3(0.00, 0.00, 56.11),
                Hash = PedHash.Dale,
                IsMale = true
            },
            new PointInfo {
                Point = new Vector3(1047.43, -496.36, 63.08),
                PedPosition = new Vector3(1046.32, -496.12, 64.08),
                PedRotation = new Vector3(0.00, 0.00, -102.13),
                Hash = PedHash.Eastsa02AFM,
                IsMale = false
            },
            new PointInfo {
                Point = new Vector3(222.51, -380.60, 43.42),
                PedPosition = new Vector3(222.36, -381.44, 44.43),
                PedRotation = new Vector3(0.00, 0.00, -11.24),
                Hash = PedHash.Business03AMY,
                IsMale = true
            },
            new PointInfo {
                Point = new Vector3(-248.38, -318.92, 28.99),
                PedPosition = new Vector3(-247.83, -318.84, 29.99),
                PedRotation = new Vector3(0.00, 0.00, 102.60),
                Hash = PedHash.Eastsa03AFY,
                IsMale = false
            },
            new PointInfo {
                Point = new Vector3(-785.86, -392.25, 36.10),
                PedPosition = new Vector3(-785.43, -391.37, 37.10),
                PedRotation = new Vector3(0.00, 0.00, 155.26),
                Hash = PedHash.Soucent04AMY,
                IsMale = true
            },
            new PointInfo {
                Point = new Vector3(-1285.33, -650.89, 25.59),
                PedPosition = new Vector3(-1284.93, -651.55, 26.59),
                PedRotation = new Vector3(0.00, 0.00, 30.44),
                Hash = PedHash.Stretch,
                IsMale = true
            },
            new PointInfo {
                Point = new Vector3(-1334.89, -935.72, 10.35),
                PedPosition = new Vector3(-1335.73, -936.56, 11.35),
                PedRotation = new Vector3(0.00, 0.00, -42.75),
                Hash = PedHash.Genfat01AMM,
                IsMale = true
            },
            new PointInfo {
                Point = new Vector3(-1207.78, -1553.94, 3.37),
                PedPosition = new Vector3(-1207.70, -1554.84, 4.37),
                PedRotation = new Vector3(0.00, 0.00, 7.59),
                Hash = PedHash.Musclbeac02AMY,
                IsMale = true
            },
            new PointInfo {
                Point = new Vector3(-766.66, -1059.48, 12.48),
                PedPosition = new Vector3(-767.18, -1058.59, 13.48),
                PedRotation = new Vector3(0.00, 0.00, -157.24),
                Hash = PedHash.KorBoss01GMM,
                IsMale = true
            },
            new PointInfo {
                Point = new Vector3(-254.00, -979.87, 30.22),
                PedPosition = new Vector3(-253.19, -979.58, 31.22),
                PedRotation = new Vector3(0.00, 0.00, 99.64),
                Hash = PedHash.Hipster01AMY,
                IsMale = true
            },
            new PointInfo {
                Point = new Vector3(188.43, -952.57, 29.09),
                PedPosition = new Vector3(187.59, -953.41, 30.09),
                PedRotation = new Vector3(0.00, 0.00, -38.60),
                Hash = PedHash.Tourist01AFY,
                IsMale = false
            },
            new PointInfo {
                Point = new Vector3(873.58, -1014.50, 30.12),
                PedPosition = new Vector3(873.62, -1015.17, 31.13),
                PedRotation = new Vector3(0.00, 0.00, 3.50),
                Hash = PedHash.MrsPhillips,
                IsMale = false
            },
            new PointInfo {
                Point = new Vector3(1208.26, -1303.13, 34.23),
                PedPosition = new Vector3(1208.04, -1304.32, 35.23),
                PedRotation = new Vector3(0.00, 0.00, -9.41),
                Hash = PedHash.Construct02SMY,
                IsMale = true
            },
            new PointInfo {
                Point = new Vector3(1297.56, -1737.70, 52.88),
                PedPosition = new Vector3(1297.02, -1738.45, 53.88),
                PedRotation = new Vector3(0.00, 0.00, -36.23),
                Hash = PedHash.ExArmy01,
                IsMale = true
            },
            new PointInfo {
                Point = new Vector3(956.44, -2105.48, 29.62),
                PedPosition = new Vector3(957.31, -2105.41, 30.63),
                PedRotation = new Vector3(0.00, 0.00, 94.06),
                Hash = PedHash.Josef,
                IsMale = true
            },
            new PointInfo {
                Point = new Vector3(777.55, -2382.93, 21.19),
                PedPosition = new Vector3(778.45, -2382.92, 22.21),
                PedRotation = new Vector3(0.00, 0.00, 87.61),
                Hash = PedHash.Construct01SMY,
                IsMale = true
            },
            new PointInfo {
                Point = new Vector3(416.44, -2059.19, 21.10),
                PedPosition = new Vector3(417.02, -2058.52, 22.13),
                PedRotation = new Vector3(0.00, 0.00, 138.46),
                Hash = PedHash.Dealer01SMY,
                IsMale = true
            },
            new PointInfo {
                Point = new Vector3(-82.16, -1646.75, 28.31),
                PedPosition = new Vector3(-83.05, -1646.03, 29.31),
                PedRotation = new Vector3(0.00, 0.00, -126.91),
                Hash = PedHash.Downtown01AFM,
                IsMale = false
            },
            new PointInfo {
                Point = new Vector3(-201.29, -1380.24, 30.26),
                PedPosition = new Vector3(-201.73, -1379.68, 31.26),
                PedRotation = new Vector3(0.00, 0.00, -146.13),
                Hash = PedHash.Car3Guy2,
                IsMale = true
            },
            new PointInfo {
                Point = new Vector3(22.42, -366.92, 38.31),
                PedPosition = new Vector3(21.76, -366.68, 39.31),
                PedRotation = new Vector3(0.00, 0.00, -104.53),
                Hash = PedHash.AirworkerSMY,
                IsMale = true
            }
        };

        /// <summary>
        /// Возвращает позиции фургонов бистро, где
        /// Item1 - позиция,
        /// Item2 - поворот
        /// </summary>
        internal static IEnumerable<BistroVehicle> GetVehiclesInfo() {
            return new List<BistroVehicle> {
                new BistroVehicle {Position = new Vector3(148.86, -1443.76, 28.42), Rotation = new Vector3(-0.24, -8.53, -40.53), IsTruck = true},
                new BistroVehicle {Position = new Vector3(151.88, -1446.30, 28.42), Rotation = new Vector3(-0.24, -8.87, -43.12), IsTruck = true},
                new BistroVehicle {Position = new Vector3(154.41, -1448.58, 28.42), Rotation = new Vector3(-0.24, -9.04, -40.21), IsTruck = true},
                new BistroVehicle {Position = new Vector3(157.04, -1450.67, 28.42), Rotation = new Vector3(-0.24, -9.20, -40.93), IsTruck = true},
                new BistroVehicle {Position = new Vector3(159.73, -1453.10, 28.42), Rotation = new Vector3(-0.24, -8.07, -42.45), IsTruck = true},
                new BistroVehicle {Position = new Vector3(162.47, -1455.14, 28.42), Rotation = new Vector3(-0.24, -9.33, -41.41), IsTruck = true},
                new BistroVehicle {Position = new Vector3(165.07, -1457.37, 28.42), Rotation = new Vector3(-0.24, -9.81, -42.13), IsTruck = true},
                new BistroVehicle {Position = new Vector3(137.56, -1455.5, 28.62), Rotation = new Vector3(1.41, -13.73, 143.43), IsTruck = false},
                new BistroVehicle {Position = new Vector3(138.75, -1456.42, 28.62), Rotation = new Vector3(1.41, -13.73, 143.43), IsTruck = false},
                new BistroVehicle {Position = new Vector3(139.86, -1457.62, 28.62), Rotation = new Vector3(1.41, -13.73, 143.43), IsTruck = false},
                new BistroVehicle {Position = new Vector3(141.14, -1458.59, 28.62), Rotation = new Vector3(1.41, -13.73, 143.43), IsTruck = false},
                new BistroVehicle {Position = new Vector3(142.61, -1459.74, 28.62), Rotation = new Vector3(1.41, -13.73, 143.43), IsTruck = false},
                new BistroVehicle {Position = new Vector3(143.98, -1461.02, 28.62), Rotation = new Vector3(1.41, -13.73, 143.43), IsTruck = false},
                new BistroVehicle {Position = new Vector3(145.73, -1462.34, 28.62), Rotation = new Vector3(1.41, -13.73, 143.43), IsTruck = false},
                //new BistroVehicle {Position = new Vector3(147.41, -1464.06, 28.62), Rotation = new Vector3(1.41, -13.73, 143.43), IsTruck = false},
                //new BistroVehicle {Position = new Vector3(149.05, -1465.35, 28.62), Rotation = new Vector3(1.41, -13.73, 143.43), IsTruck = false},
                //new BistroVehicle {Position = new Vector3(150.52, -1466.74, 28.62), Rotation = new Vector3(1.41, -13.73, 143.43), IsTruck = false},
                //new BistroVehicle {Position = new Vector3(152.32, -1468.32, 28.62), Rotation = new Vector3(1.41, -13.73, 143.43), IsTruck = false},
                //new BistroVehicle {Position = new Vector3(154.19, -1469.75, 28.62), Rotation = new Vector3(1.41, -13.73, 143.43), IsTruck = false},
                //new BistroVehicle {Position = new Vector3(155.60, -1470.98, 28.62), Rotation = new Vector3(1.41, -13.73, 143.43), IsTruck = false},
                //new BistroVehicle {Position = new Vector3(157.10, -1472.42, 28.62), Rotation = new Vector3(1.41, -13.73, 143.43), IsTruck = false}
            };
        }

        /// <summary>
        /// Информация о транспорте закусочной
        /// </summary>
        internal class BistroVehicle {
            public Vector3 Position { get; set; }
            public Vector3 Rotation { get; set; }
            public bool IsTruck { get; set; }
        }

        /// <summary>
        /// Информация точки доставки
        /// </summary>
        internal class PointInfo {
            public Vector3 Point { get; set; }
            public Vector3 PedPosition { get; set; }
            public Vector3 PedRotation { get; set; }
            public PedHash Hash { get; set; }
            public bool IsMale { get; set; }
        }
    }
}