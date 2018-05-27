using System;
using System.Collections.Generic;
using gta_mp_database.Enums;
using gta_mp_server.Models;
using gta_mp_server.Models.Shops;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.House {
    /// <summary>
    /// Класс хранит внутренние позиции домов и гаражей (потому что они одинаковы для всех)
    /// </summary>
    internal class HousesPositionsGetter {
        /// <summary>
        /// Позиции гардероба по типу дома
        /// </summary>
        internal static Dictionary<HouseType, DressingRoomPositions> DressingRoom = new Dictionary<HouseType, DressingRoomPositions> {
            [HouseType.Eco] = new DressingRoomPositions {
                Position = new Vector3(1970.17, 3815.01, 33.43),
                Rotation = new Vector3(0.00, 0.00, 33.39),
                CameraHatsPosition = new Vector3(1969.54, 3815.62, 34.0),
                CameraTopsPosition = new Vector3(1969.54, 3815.92, 33.73),
                CameraLegsPosition = new Vector3(1969.54, 3815.92, 33.00),
                CameraFeetsPosition = new Vector3(1969.54, 3815.72, 32.70),
                CameraRotation = new Vector3(0.00, 0.00, -150.00)
            },
            [HouseType.Eco2] = new DressingRoomPositions {
                Position = new Vector3(152.46, -1000.10, -99.00),
                Rotation = new Vector3(0.00, 0.00, -179.26),
                CameraHatsPosition = new Vector3(152.38, -1001.22, -98.30),
                CameraTopsPosition = new Vector3(152.38, -1001.42, -98.70),
                CameraLegsPosition = new Vector3(152.38, -1001.42, -99.30),
                CameraFeetsPosition = new Vector3(152.38, -1001.32, -99.60),
                CameraRotation = new Vector3(0.00, 0.00, 1.14)
            },
            [HouseType.Eco3] = new DressingRoomPositions {
                Position = new Vector3(260.90, -1004.03, -99.01),
                Rotation = new Vector3(0.00, 0.00, -0.10),
                CameraHatsPosition = new Vector3(260.85, -1003.30, -98.30),
                CameraTopsPosition = new Vector3(260.85, -1003.00, -98.70),
                CameraLegsPosition = new Vector3(260.85, -1003.00, -99.45),
                CameraFeetsPosition = new Vector3(260.85, -1003.20, -99.80),
                CameraRotation = new Vector3(0.00, 0.00, 177.55)
            },
            [HouseType.Standart] = new DressingRoomPositions {
                Position = new Vector3(349.97, -993.50, -99.20),
                Rotation = new Vector3(0.00, 0.00, 178.46),
                CameraHatsPosition = new Vector3(349.97, -994.24, -98.50),
                CameraTopsPosition = new Vector3(349.97, -994.64, -98.90),
                CameraLegsPosition = new Vector3(349.97, -994.64, -99.60),
                CameraFeetsPosition = new Vector3(349.97, -994.34, -99.99),
                CameraRotation = new Vector3(0.00, 0.00, -0.18)
            },
            [HouseType.Standart2] = new DressingRoomPositions(),
            [HouseType.Premium] = new DressingRoomPositions {
                Position = new Vector3(334.57, 428.66, 145.57),
                Rotation = new Vector3(0.00, 0.00, 110.79),
                CameraHatsPosition = new Vector3(333.95, 428.35, 146.35),
                CameraTopsPosition = new Vector3(333.33, 428.00, 145.87),
                CameraLegsPosition = new Vector3(333.63, 428.10, 145.17),
                CameraFeetsPosition = new Vector3(333.93, 428.25, 144.77),
                CameraRotation = new Vector3(0.00, 0.00, -62.21)
            },
            [HouseType.Premium2] = new DressingRoomPositions {
                Position = new Vector3(-38.31, -589.87, 78.83),
                Rotation = new Vector3(0.00, 0.00, -21.90),
                CameraHatsPosition = new Vector3(-38.11, -589.08, 79.55),
                CameraTopsPosition = new Vector3(-38.01, -588.58, 78.93),
                CameraLegsPosition = new Vector3(-38.05, -588.70, 78.43),
                CameraFeetsPosition = new Vector3(-38.08, -588.95, 78.03),
                CameraRotation = new Vector3(0.00, 0.00, 167.19)
            },
            [HouseType.Premium3] = new DressingRoomPositions {
                Position = new Vector3(-855.43, 680.36, 149.05),
                Rotation = new Vector3(0.00, 0.00, 177.42),
                CameraHatsPosition = new Vector3(-855.40, 679.75, 149.75),
                CameraTopsPosition = new Vector3(-855.25, 678.95, 149.15),
                CameraLegsPosition = new Vector3(-855.30, 679.25, 148.65),
                CameraFeetsPosition = new Vector3(-855.30, 679.55, 148.25),
                CameraRotation = new Vector3(0.00, 0.00, 5.39)
            },
            [HouseType.Elite] = new DressingRoomPositions {
                Position = new Vector3(-1468.25, -537.82, 50.73),
                Rotation = new Vector3(0.00, 0.00, -57.79),
                CameraHatsPosition = new Vector3(-1467.70, -537.40, 51.43),
                CameraTopsPosition = new Vector3(-1467.06, -536.95, 50.90),
                CameraLegsPosition = new Vector3(-1467.40, -537.14, 50.33),
                CameraFeetsPosition = new Vector3(-1467.55, -537.25, 49.93),
                CameraRotation = new Vector3(0.00, 0.00, 123.56)
            },
            [HouseType.Elite2] = new DressingRoomPositions {
                Position = new Vector3(-797.73, 327.38, 220.44),
                Rotation = new Vector3(0.00, 0.00, 1.04),
                CameraHatsPosition = new Vector3(-797.72, 328.00, 221.14),
                CameraTopsPosition = new Vector3(-797.86, 328.70, 220.54),
                CameraLegsPosition = new Vector3(-797.86, 328.40, 220.05),
                CameraFeetsPosition = new Vector3(-797.86, 328.20, 219.64),
                CameraRotation = new Vector3(0.00, 0.00, -176.73)
            }
        };

        /// <summary>
        /// Возвращает внутренние позиции дома по типу
        /// </summary>
        internal static Dictionary<HouseType, HousesInnerPositions> InnerPositions = new Dictionary<HouseType, HousesInnerPositions> {
            [HouseType.Eco] = new HousesInnerPositions {
                Hallway = new Vector3(1973.67, 3818.35, 33.44),
                Storage = new Vector3(1978.28, 3819.81, 33.45),
                Wardrobe = new Vector3(1969.22, 3814.66, 33.43),
                Exit = new Vector3(1973.18, 3816.15, 33.43)
            },
            [HouseType.Eco2] = new HousesInnerPositions {
                Hallway = new Vector3(152.16, -1006.39, -99.0),
                Storage = new Vector3(151.24, -1003.18, -99.0),
                Wardrobe = new Vector3(151.78, -1001.45, -99.0),
                Exit = new Vector3(151.37, -1007.96, -99.0)
            },
            [HouseType.Eco3] = new HousesInnerPositions {
                Hallway = new Vector3(265.49, -1002.84, -99.01),
                Storage = new Vector3(265.90, -999.38, -99.01),
                Wardrobe = new Vector3(259.79, -1003.78, -99.01),
                Exit = new Vector3(266.0, -1007.24, -101.01)
            },
            [HouseType.Standart] = new HousesInnerPositions {
                Hallway = new Vector3(346.72, -1010.35, -99.2),
                Storage = new Vector3(351.9, -998.86, -99.2),
                Wardrobe = new Vector3(350.67, -993.77, -99.2),
                Exit = new Vector3(346.46, -1012.75, -99.2)
            },
            [HouseType.Standart2] = new HousesInnerPositions(),
            [HouseType.Premium] = new HousesInnerPositions {
                Hallway = new Vector3(339.57, 435.91, 149.39),
                Storage = new Vector3(336.75, 437.38, 141.77),
                Wardrobe = new Vector3(334.39, 428.5, 145.57),
                Exit = new Vector3(342.13, 437.9, 149.38)
            },
            [HouseType.Premium2] = new HousesInnerPositions {
                Hallway = new Vector3(-21.97, -598.5, 80.1),
                Storage = new Vector3(-11.73, -597.92, 79.43),
                Wardrobe = new Vector3(-38.23, -589.44, 78.83),
                Exit = new Vector3(-24.2, -597.57, 80.0)
            },
            [HouseType.Premium3] = new HousesInnerPositions {
                Hallway = new Vector3(-859.57, 689.1, 152.86),
                Storage = new Vector3(-857.74, 698.30, 145.25),
                Wardrobe = new Vector3(-855.31, 680.1, 149.1),
                Exit = new Vector3(-859.93, 691.41, 152.86)
            },
            [HouseType.Elite] = new HousesInnerPositions {
                Hallway = new Vector3(-1459.62, -521.72, 56.93),
                Storage = new Vector3(-1457.35, -531.11, 56.94),
                Wardrobe = new Vector3(-1467.32, -537.01, 50.73),
                Exit = new Vector3(-1457.61, -520.40, 56.93)
            },
            [HouseType.Elite2] = new HousesInnerPositions {
                Hallway = new Vector3(-783.26, 320.57, 217.64),
                Storage = new Vector3(-796.1, 327.19, 217.1),
                Wardrobe = new Vector3(-797.77, 328.22, 220.44),
                Exit = new Vector3(-781.89, 317.99, 217.64)
            }
        };

        /// <summary>
        /// Возвращает внутренние позиции гаража по типу
        /// </summary>
        public static GarageInnerPositions GetGarageInnerPositions(HouseType type) {
            switch (type) {
                case HouseType.Eco:
                case HouseType.Eco2:
                case HouseType.Eco3:
                case HouseType.Standart:
                case HouseType.Standart2:
                    return new GarageInnerPositions {
                        AfterEnter = new Vector3(173.7, -1006.10, -99.0),
                        EnterRotation = new Vector3(-0.11, 0.01, -2.52),
                        Positions = new List<Tuple<Vector3, Vector3>> {
                            new Tuple<Vector3, Vector3>(new Vector3(171.6, -1002.0, -99.6), new Vector3(0.1, 1.48, -0.30)),
                            new Tuple<Vector3, Vector3>(new Vector3(175.0, -1002.0, -99.6), new Vector3(0.1, 1.48, -0.30))
                        },
                        GarageExits = new List<Vector3> {
                            new Vector3(173.0, -1007.00, -99.00),
                            new Vector3(174.02, -1007.43, -99.00),
                            new Vector3(180.1, -1000.1, -99.0)
                        }
                    };
                case HouseType.Premium:
                case HouseType.Premium2:
                case HouseType.Premium3:
                    return new GarageInnerPositions {
                        AfterEnter = new Vector3(198.39, -1005.37, -99.7),
                        EnterRotation = new Vector3(-0.35, 0.01, 2.12),
                        Positions = new List<Tuple<Vector3, Vector3>> {
                            new Tuple<Vector3, Vector3>(new Vector3(192.86, -997.1, -99.74), new Vector3(-0.35, 0.01, 2.12)),
                            new Tuple<Vector3, Vector3>(new Vector3(196.1, -996.93, -99.47), new Vector3(-0.35, 0.01, 2.12)),
                            new Tuple<Vector3, Vector3>(new Vector3(199.41, -996.95, -99.48), new Vector3(-0.35, 0.01, 2.12)),
                            new Tuple<Vector3, Vector3>(new Vector3(203.2, -997.14, -99.57), new Vector3(-0.35, 0.01, 2.12))
                        },
                        GarageExits = new List<Vector3> {
                            new Vector3(194.35, -1005.22, -99.7),
                            new Vector3(202.11, -1005.38, -99.69),
                            new Vector3(212.16, -999.13, -99.0)
                        }
                    };
                case HouseType.Elite:
                case HouseType.Elite2:
                    return new GarageInnerPositions {
                        AfterEnter = new Vector3(228.18, -1003.16, -99.60),
                        EnterRotation = new Vector3(-0.1, 0.54, 0.17),
                        Positions = new List<Tuple<Vector3, Vector3>> {
                            new Tuple<Vector3, Vector3>(new Vector3(233.39, -999.55, -99.6), new Vector3(0.04, -0.001, -74.5)),
                            new Tuple<Vector3, Vector3>(new Vector3(233.34, -995.20, -99.6), new Vector3(0.04, -0.001, -74.5)),
                            new Tuple<Vector3, Vector3>(new Vector3(233.33, -991.2, -99.6), new Vector3(0.04, -0.001, -74.5)),
                            new Tuple<Vector3, Vector3>(new Vector3(233.13, -987.43, -99.6), new Vector3(0.04, -0.001, -74.5)),
                            new Tuple<Vector3, Vector3>(new Vector3(232.39, -983.14, -99.6), new Vector3(0.04, -0.001, -74.5)),
                            new Tuple<Vector3, Vector3>(new Vector3(223.45, -998.61, -99.6), new Vector3(-0.01, 0.03, 78.6)),
                            new Tuple<Vector3, Vector3>(new Vector3(223.1, -994.7, -99.6), new Vector3(-0.01, 0.03, 78.6)),
                            new Tuple<Vector3, Vector3>(new Vector3(223.4, -990.25, -99.6), new Vector3(-0.01, 0.03, 78.6)),
                            new Tuple<Vector3, Vector3>(new Vector3(223.35, -986.51, -99.6), new Vector3(-0.01, 0.03, 78.6)),
                            new Tuple<Vector3, Vector3>(new Vector3(223.91, -982.33, -99.6), new Vector3(-0.01, 0.03, 78.6))
                        },
                        GarageExits = new List<Vector3> {
                            new Vector3(224.1167, -1004.545, -99.43636),
                            new Vector3(231.8385, -1004.522, -99.43661)
                        }
                    };
                default:
                    throw new ArgumentException("Неизвестный тип дома!");
            }
        }
    }
}