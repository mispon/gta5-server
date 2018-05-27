using System.Collections.Generic;
using gta_mp_server.Models.Places;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Places.AirPorts {
    /// <summary>
    /// Данные аэропортов
    /// </summary>
    internal class AirPortData {
        /// <summary>
        /// Позиция после входа
        /// </summary>
        public static Vector3 AfterEnter = new Vector3(-1563.37, -3204.75, 13.94);

        /// <summary>
        /// Выходы из основного аэропорта 
        /// </summary>
        public static List<Vector3> Exits = new List<Vector3> {
            new Vector3(-1583.99, -3181.08, 14.65),
            new Vector3(-1522.60, -3216.56, 14.65)
        };

        /// <summary>
        /// Позиция после выхода с основного аэропорта
        /// </summary>
        public static Vector3 AfterExit = new Vector3(-1039.13, -2739.75, 20.17);

        /// <summary>
        /// Нпс аэропортов
        /// </summary>
        public static List<DeliveryNpc> Npcs = new List<DeliveryNpc> {
            new DeliveryNpc {
                Name = "Рабочий аэропорта",
                Position = new Vector3(-1536.49, -3192.63, 13.94),
                Rotation = new Vector3(0.00, 0.00, 24.72),
                MarkerPosition = new Vector3(-1536.81, -3191.78, 13.04),
                Hash = PedHash.AirworkerSMY,
                Contracts = new List<DeliveryContract> {
                    AirPort.Contracts[0], AirPort.Contracts[2], AirPort.Contracts[4], AirPort.Contracts[6], AirPort.Contracts[8]
                }
            },
            new DeliveryNpc {
                Name = "Авиамеханик",
                Position = new Vector3(1760.02, 3296.54, 41.14),
                Rotation = new Vector3(0.00, 0.00, 159.75),
                MarkerPosition = new Vector3(1759.71, 3295.95, 40.20),
                Hash = PedHash.Armymech01SMY,
                Contracts = new List<DeliveryContract> {AirPort.Contracts[1]}
            },
            new DeliveryNpc {
                Name = "Фермер",
                Position = new Vector3(2119.32, 4788.77, 41.15),
                Rotation = new Vector3(0.00, 0.00, 40.39),
                MarkerPosition = new Vector3(2118.96, 4789.45, 40.20),
                Hash = PedHash.Farmer01AMM,
                Contracts = new List<DeliveryContract> {AirPort.Contracts[3], AirPort.Contracts[7]}
            },
            new DeliveryNpc {
                Name = "Военный летчик",
                Position = new Vector3(-1911.19, 2981.88, 32.81),
                Rotation = new Vector3(0.00, 0.00, 153.25),
                MarkerPosition = new Vector3(-1911.58, 2981.04, 31.90),
                Hash = PedHash.Blackops01SMY,
                Contracts = new List<DeliveryContract> {AirPort.Contracts[5], AirPort.Contracts[9]}
            }
        };

        /// <summary>
        /// Самолеты
        /// </summary>
        public static List<SpawnVehicleInfo> Planes = new List<SpawnVehicleInfo> {
            // город
            new SpawnVehicleInfo {Hash = VehicleHash.Mammatus, Position = new Vector3(-1548.73, -3105.91, 14.29), Rotation = new Vector3(10.72, 0.00, -29.91)},
            new SpawnVehicleInfo {Hash = VehicleHash.Mammatus, Position = new Vector3(-1566.59, -3096.09, 14.29), Rotation = new Vector3(10.72, 0.00, -29.63)},
            new SpawnVehicleInfo {Hash = VehicleHash.Mammatus, Position = new Vector3(-1583.76, -3085.26, 14.29), Rotation = new Vector3(10.70, 0.00, -28.93)},
            new SpawnVehicleInfo {Hash = VehicleHash.Duster, Position = new Vector3(-1423.74, -3177.76, 14.29), Rotation = new Vector3(10.66, 0.00, -28.78)},
            new SpawnVehicleInfo {Hash = VehicleHash.Duster, Position = new Vector3(-1405.38, -3188.37, 14.29), Rotation = new Vector3(10.70, 0.00, -29.12)},
            new SpawnVehicleInfo {Hash = VehicleHash.Duster, Position = new Vector3(-1382.78, -3201.57, 14.29), Rotation = new Vector3(10.69, 0.00, -28.23)},
            new SpawnVehicleInfo {Hash = VehicleHash.Seabreeze, Position = new Vector3(-1632.91, -3146.41, 14.64), Rotation = new Vector3(0.07, 0.00, -30.38)},
            new SpawnVehicleInfo {Hash = VehicleHash.Seabreeze, Position = new Vector3(-1664.64, -3127.41, 14.63), Rotation = new Vector3(0.37, 0.00, -27.60)},
            new SpawnVehicleInfo {Hash = VehicleHash.Seabreeze, Position = new Vector3(-1648.43, -3136.38, 14.63), Rotation = new Vector3(0.40, 0.00, -29.78)},
            new SpawnVehicleInfo {Hash = VehicleHash.Vestra, Position = new Vector3(-1254.13, -3386.87, 14.54), Rotation = new Vector3(-0.06, -0.06, -29.94)},
            new SpawnVehicleInfo {Hash = VehicleHash.Vestra, Position = new Vector3(-1262.67, -3402.16, 14.12), Rotation = new Vector3(1.30, 0.00, -30.56)},
            new SpawnVehicleInfo {Hash = VehicleHash.Nimbus, Position = new Vector3(-1285.58, -3368.75, 14.54), Rotation = new Vector3(-0.20, -0.06, -28.27)},
            new SpawnVehicleInfo {Hash = VehicleHash.Nimbus, Position = new Vector3(-1272.00, -3381.63, 14.58), Rotation = new Vector3(0.32, 0.00, -29.64)},
            // авиамастерская
            new SpawnVehicleInfo {Hash = VehicleHash.Duster, Position = new Vector3(1730.01, 3314.55, 41.57), Rotation = new Vector3(10.72, 0.00, -164.28)},
            new SpawnVehicleInfo {Hash = VehicleHash.Duster, Position = new Vector3(1733.09, 3303.67, 41.57), Rotation = new Vector3(10.64, 0.00, -164.76)},
            new SpawnVehicleInfo {Hash = VehicleHash.Mammatus, Position = new Vector3(1696.72, 3280.30, 41.48), Rotation = new Vector3(10.59, -0.39, 36.31)},
            new SpawnVehicleInfo {Hash = VehicleHash.Seabreeze, Position = new Vector3(1752.28, 3235.92, 42.36), Rotation = new Vector3(12.52, 0.33, -168.52)},
            // ферма
            new SpawnVehicleInfo {Hash = VehicleHash.Duster, Position = new Vector3(2132.89, 4785.37, 41.32), Rotation = new Vector3(10.71, 0.00, 27.68)},
            new SpawnVehicleInfo {Hash = VehicleHash.Mammatus, Position = new Vector3(2141.57, 4819.99, 41.59), Rotation = new Vector3(10.24, 0.03, 120.32)},
            new SpawnVehicleInfo {Hash = VehicleHash.Seabreeze, Position = new Vector3(2066.87, 4752.04, 41.81), Rotation = new Vector3(9.90, 0.45, 25.46)},
            // военная база
            new SpawnVehicleInfo {Hash = VehicleHash.Vestra, Position = new Vector3(-1840.91, 2961.75, 33.41), Rotation = new Vector3(-0.12, -0.06, 61.58)},
            new SpawnVehicleInfo {Hash = VehicleHash.Vestra, Position = new Vector3(-1832.13, 2976.85, 33.41), Rotation = new Vector3(-0.23, -0.06, 59.56)},
            new SpawnVehicleInfo {Hash = VehicleHash.Nimbus, Position = new Vector3(-1823.08, 2993.34, 33.41), Rotation = new Vector3(-0.30, -0.06, 62.45)}
        };
    }
}