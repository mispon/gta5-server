using System.Collections.Generic;
using gta_mp_server.Constant;
using gta_mp_server.Managers.Places;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Work.Trucker {
    /// <summary>
    /// Хранит позиции связанные с дальнобойщиками
    /// </summary>
    internal class TruckersDataGetter {
        /// <summary>
        /// Нпс дальнобойщиков
        /// </summary>
        internal static List<DeliveryNpc> TruckerNpcs = new List<DeliveryNpc> {
            new DeliveryNpc {
                Position = MainPosition.Port,
                Rotation = new Vector3(0.00, 0.00, 0.47),
                MarkerPosition = new Vector3(1225.66, -3231.75, 5.10),
                Hash = PedHash.GentransportSMM,
                Name = "Сэм",
                Contracts = new List<DeliveryContract> {
                    Port.TruckerContracts[0],
                    Port.TruckerContracts[1],
                    Port.TruckerContracts[2],
                }
            },
            new DeliveryNpc {
                Position = new Vector3(-568.29, 5251.08, 70.49),
                Rotation = new Vector3(0.00, 0.00, 78.54),
                MarkerPosition = new Vector3(-569.1, 5251.16, 69.49),
                Hash = PedHash.Hillbilly01AMM,
                Name = "Билли",
                Contracts = new List<DeliveryContract> {
                    Port.TruckerContracts[3],
                    Port.TruckerContracts[4]
                }
            },
            new DeliveryNpc {
                Position = new Vector3(2672.99, 1385.31, 24.55),
                Rotation = new Vector3(0.00, 0.00, -90.15),
                MarkerPosition = new Vector3(2673.73, 1385.29, 23.60),
                Hash = PedHash.Armoured01SMM,
                Name = "Ричард",
                Contracts = new List<DeliveryContract> {
                    Port.TruckerContracts[5],
                    Port.TruckerContracts[6]
                }
            },
            new DeliveryNpc {
                Position = new Vector3(2859.92, 4699.64, 47.52),
                Rotation = new Vector3(0.00, 0.00, -172.80),
                MarkerPosition = new Vector3(2860.03, 4698.85, 46.49),
                Hash = PedHash.Farmer01AMM,
                Name = "Клетус",
                Contracts = new List<DeliveryContract> {
                    Port.TruckerContracts[7],
                    Port.TruckerContracts[8]
                }
            }
        };
        
        /// <summary>
        /// Позиции грузовых машин
        /// </summary>
        internal static List<TruckInfo> TruckerPositions = new List<TruckInfo> {
            new TruckInfo {Position = new Vector3(1066.59, -3182.30, 5.37), Rotation = new Vector3(-0.11, 0.00, -0.26), IsTrailer = false},
            new TruckInfo {Position = new Vector3(1062.39, -3182.36, 5.99), Rotation = new Vector3(-0.32, 0.00, 0.64), IsTrailer = false},
            new TruckInfo {Position = new Vector3(1058.31, -3182.65, 5.99), Rotation = new Vector3(-0.41, -0.07, -0.39), IsTrailer = false},
            new TruckInfo {Position = new Vector3(1054.27, -3182.57, 5.99), Rotation = new Vector3(-0.36, -0.01, -0.49), IsTrailer = false},
            new TruckInfo {Position = new Vector3(1046.18, -3182.43, 5.99), Rotation = new Vector3(-0.36, 0.02, -1.00), IsTrailer = false},
            new TruckInfo {Position = new Vector3(1042.10, -3182.17, 5.99), Rotation = new Vector3(-0.28, 0.00, -1.07), IsTrailer = false},
            new TruckInfo {Position = new Vector3(1029.92, -3182.58, 5.99), Rotation = new Vector3(-0.25, 0.02, 0.06), IsTrailer = false, Hash = VehicleHash.Phantom},
            new TruckInfo {Position = new Vector3(1025.89, -3182.59, 5.99), Rotation = new Vector3(-0.40, 0.01, 0.07), IsTrailer = false, Hash = VehicleHash.Phantom},
            new TruckInfo {Position = new Vector3(1021.80, -3182.57, 5.99), Rotation = new Vector3(-0.34, 0.00, -0.33), IsTrailer = false, Hash = VehicleHash.Phantom},
            new TruckInfo {Position = new Vector3(1017.80, -3182.24, 5.99), Rotation = new Vector3(-0.39, 0.07, 0.67), IsTrailer = false, Hash = VehicleHash.Phantom},
            new TruckInfo {Position = new Vector3(1005.55, -3182.27, 5.99), Rotation = new Vector3(-0.36, 0.00, -0.83), IsTrailer = false, Hash = VehicleHash.Phantom3},
            new TruckInfo {Position = new Vector3(1001.43, -3182.69, 5.99), Rotation = new Vector3(-0.28, 0.00, 0.08), IsTrailer = false, Hash = VehicleHash.Phantom3},
            new TruckInfo {Position = new Vector3(993.24, -3182.59, 5.99), Rotation = new Vector3(-0.39, 0.06, -0.45), IsTrailer = false, Hash = VehicleHash.Phantom3},
            new TruckInfo {Position = new Vector3(2725.08, 1355.63, 23.86), Rotation = new Vector3(0.72, 0.09, -179.11), IsTrailer = false},
            new TruckInfo {Position = new Vector3(2732.49, 1368.78, 23.86), Rotation = new Vector3(0.65, 0.00, 1.03), IsTrailer = false},
            new TruckInfo {Position = new Vector3(2739.72, 1392.15, 23.87), Rotation = new Vector3(1.36, 0.10, -0.96), IsTrailer = false},
            new TruckInfo {Position = new Vector3(2710.41, 1391.24, 23.87), Rotation = new Vector3(1.32, 0.01, 0.51), IsTrailer = false, Hash = VehicleHash.Phantom},
            new TruckInfo {Position = new Vector3(2732.48, 1346.36, 23.86), Rotation = new Vector3(0.70, 0.02, 0.28), IsTrailer = false, Hash = VehicleHash.Phantom},
            new TruckInfo {Position = new Vector3(2866.65, 4709.22, 47.88), Rotation = new Vector3(1.37, -4.19, 18.19), IsTrailer = false, Hash = VehicleHash.Hauler},
            new TruckInfo {Position = new Vector3(2872.20, 4710.49, 48.96), Rotation = new Vector3(0.11, -4.09, 19.89), IsTrailer = false, Hash = VehicleHash.Hauler},
            new TruckInfo {Position = new Vector3(2877.34, 4712.35, 48.49), Rotation = new Vector3(-0.17, -1.51, 23.69), IsTrailer = false, Hash = VehicleHash.Hauler},
            new TruckInfo {Position = new Vector3(-602.74, 5294.53, 69.78), Rotation = new Vector3(1.08, 2.23, 93.45), IsTrailer = false, Hash = VehicleHash.Hauler},
            new TruckInfo {Position = new Vector3(-603.09, 5300.64, 69.74), Rotation = new Vector3(2.58, -0.53, 96.09), IsTrailer = false, Hash = VehicleHash.Hauler},
            new TruckInfo {Position = new Vector3(-603.07, 5306.88, 69.86), Rotation = new Vector3(2.14, -0.37, 96.64), IsTrailer = false, Hash = VehicleHash.Hauler},
            new TruckInfo {Position = new Vector3(1062.45, -3209.02, 5.36), Rotation = new Vector3(-0.28, -0.02, -179.98), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(1058.17, -3208.25, 5.40), Rotation = new Vector3(-0.28, 0.06, 179.15), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(1054.22, -3208.24, 5.39), Rotation = new Vector3(-0.27, 0.00, 179.92), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(1050.21, -3208.76, 5.38), Rotation = new Vector3(-0.72, 0.17, 178.33), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(1038.12, -3208.67, 5.38), Rotation = new Vector3(-0.93, 0.03, 179.69), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(1033.88, -3208.30, 5.38), Rotation = new Vector3(-0.83, 0.02, -179.00), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(1021.77, -3209.30, 5.37), Rotation = new Vector3(-1.07, 0.02, 179.63), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(1017.62, -3208.89, 5.38), Rotation = new Vector3(-1.03, 0.00, -179.83), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(1013.54, -3209.06, 5.38), Rotation = new Vector3(-0.73, -0.32, 178.36), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(1005.48, -3208.19, 5.40), Rotation = new Vector3(-0.10, 0.01, 178.40), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(1001.34, -3207.94, 5.40), Rotation = new Vector3(-0.06, 0.00, 179.16), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(997.33, -3208.98, 5.40), Rotation = new Vector3(-0.12, 0.01, -178.90), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(993.30, -3209.14, 5.40), Rotation = new Vector3(-0.09, 0.00, 179.68), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(2688.64, 1333.15, 23.85), Rotation = new Vector3(0.73, 0.03, 0.45), IsTrailer = true, Hash = VehicleHash.Tanker},
            new TruckInfo {Position = new Vector3(2699.53, 1333.47, 23.86), Rotation = new Vector3(0.69, 0.03, 0.45), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(2677.77, 1347.53, 23.85), Rotation = new Vector3(0.75, -0.07, -90.12), IsTrailer = true, Hash = VehicleHash.Tanker},
            new TruckInfo {Position = new Vector3(2754.37, 1333.07, 23.86), Rotation = new Vector3(0.71, 0.00, 0.45), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(2717.85, 1333.69, 23.86), Rotation = new Vector3(0.61, 0.01, 0.45), IsTrailer = true, Hash = VehicleHash.Tanker},
            new TruckInfo {Position = new Vector3(2828.78, 4700.56, 45.91), Rotation = new Vector3(1.52, 1.56, -166.89), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(2821.31, 4700.28, 45.75), Rotation = new Vector3(1.32, 1.05, -166.14), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(2814.49, 4699.85, 45.65), Rotation = new Vector3(0.62, 0.88, -169.21), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(2806.21, 4698.65, 45.57), Rotation = new Vector3(-0.32, 0.36, -168.97), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(2797.65, 4698.10, 45.40), Rotation = new Vector3(-0.20, 1.43, -169.64), IsTrailer = true, Hash = VehicleHash.Trailers},
            new TruckInfo {Position = new Vector3(-512.41, 5258.81, 80.08), Rotation = new Vector3(0.02, 0.00, 157.69), IsTrailer = true, Hash = VehicleHash.TrailerLogs},
            new TruckInfo {Position = new Vector3(-582.88, 5253.02, 70.70), Rotation = new Vector3(-0.35, -0.04, 153.31), IsTrailer = true, Hash = VehicleHash.TrailerLogs},
            new TruckInfo {Position = new Vector3(-573.33, 5237.68, 70.72), Rotation = new Vector3(-0.29, -0.01, 142.03), IsTrailer = true, Hash = VehicleHash.TrailerLogs},
            new TruckInfo {Position = new Vector3(-573.14, 5375.67, 70.47), Rotation = new Vector3(-0.92, -0.02, -73.85), IsTrailer = true, Hash = VehicleHash.TrailerLogs},
            new TruckInfo {Position = new Vector3(-545.90, 5377.74, 70.78), Rotation = new Vector3(-1.30, -0.19, 72.79), IsTrailer = true, Hash = VehicleHash.TrailerLogs}
        };

        internal class TruckInfo {
            public Vector3 Position { get; set; }
            public Vector3 Rotation { get; set; }
            public bool IsTrailer { get; set; }
            public VehicleHash Hash { get; set; } = VehicleHash.Packer;
        }
    }
}