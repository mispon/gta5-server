using System.Collections.Generic;
using System.IO;
using System.Linq;
using gta_mp_server.Enums;
using gta_mp_server.Helpers;
using gta_mp_server.Models.Shops;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;

namespace gta_mp_server.Managers.Places.VehicleShowroom {
    /// <summary>
    /// Хранит информацию автосалонов
    /// </summary>
    internal class ShowroomsGetter {
        private static List<ShowroomVehicle> _cheapVehicles;
        private static List<ShowroomVehicle> _expensiveVehicles;

        private static List<ShowroomVehicle> CheapVehicles =>
            _cheapVehicles ??
            (_cheapVehicles = JsonConvert.DeserializeObject<List<ShowroomVehicle>>(File.ReadAllText("data/cheap-vehicles.json")));

        private static List<ShowroomVehicle> ExpensiveVehicles =>
           _expensiveVehicles ??
           (_expensiveVehicles = JsonConvert.DeserializeObject<List<ShowroomVehicle>>(File.ReadAllText("data/expensive-vehicles.json")));


        /// <summary>
        /// Возвращает список автосалонов
        /// </summary>
        internal static IEnumerable<VehicleShowroomModel> GetShowrooms() {
            return new List<VehicleShowroomModel> {
                new VehicleShowroomModel {
                    Name = "Автосалон (эконом)",
                    Blip = 225,
                    Position = new Vector3(-61.76, -1093.21, 26.48),
                    Seller = PedHash.SiemonYetarian,
                    SellerPosition = new Vector3(-31.57, -1113.1, 26.42),
                    SellerRotation = new Vector3(0, 0, 23.26),
                    SellerMarkerPosition = new Vector3(-31.86, -1112.1, 25.50),
                    ShowroomPositions = new VehicleShowroomPositions {
                        PreviewPosition = new Vector3(-43.86, -1095.68, 25.89),
                        PreviewRotation = new Vector3(0.52, -10.84, 108.48),
                        CameraPosition = new Vector3(-45.40, -1101.25, 28.42),
                        CameraRotation = new Vector3(-20.00, 0.00, -23.78),
                    },
                    Type = ShowroomType.Cheap,
                    District = 1
                },
                new VehicleShowroomModel {
                    Name = "Автосалон (премиум)",
                    Blip = 523,
                    Position = new Vector3(-803.12, -223.88, 37.23),
                    PositionAfterEnter = new Vector3(-140.89, -590.22, 167.00),
                    ExitPosition = new Vector3(-138.86, -588.34, 167.00),
                    PositionAfterExit = new Vector3(-805.02, -225.15, 37.22),
                    Seller = PedHash.Business04AFY,
                    SellerPosition = new Vector3(-139.71, -603.88, 167.60),
                    SellerRotation = new Vector3(0.00, 0.00, 38.36),
                    SellerMarkerPosition = new Vector3(-140.31, -603.26, 166.70),
                    ShowroomPositions = new VehicleShowroomPositions {
                        PreviewPosition = new Vector3(-149.86, -595.32, 166.34),
                        PreviewRotation = new Vector3(0.70, 0.02, 164.17),
                        CameraPosition = new Vector3(-146.80, -600.40, 169.00),
                        CameraRotation = new Vector3(-20.00, 0.00, 22.67),
                    },
                    Type = ShowroomType.Expensive,
                    District = 4
                }
            };
        }

        /// <summary>
        /// Возвращает продаваемый в салоне транспорт
        /// </summary>
        internal static IEnumerable<ShowroomVehicle> GetVehicles(ShowroomType type) {
            return type == ShowroomType.Cheap ? CheapVehicles : ExpensiveVehicles;
        }

        /// <summary>
        /// Возвращает магазинную цену тс
        /// </summary>
        internal static int GetSellPrice(int hash) {
            var vehicle = CheapVehicles.FirstOrDefault(e => e.Hash == hash) ?? ExpensiveVehicles.FirstOrDefault(e => e.Hash == hash);
            return vehicle?.Price / 2 ?? (int) Validator.INVALID_ID;
        }
    }
}