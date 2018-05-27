using System.Collections.Generic;
using gta_mp_server.Enums;
using gta_mp_server.Models.Shops;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Places.ClothesShop {
    /// <summary>
    /// Вспомогательный класс для хранения позиций магазинов
    /// </summary>
    internal class PositionsGetter {
        /// <summary>
        /// Возвращает информацию о магазинах
        /// </summary>
        public static List<ClothesShopModel> GetShops() {
            return new List<ClothesShopModel> {
                new ClothesShopModel {
                    Type = ClothesShopType.SubUrban,
                    Seller = PedHash.ShopLowSFY,
                    SellerPosition = new Vector3(127.00, -224.32, 54.56),
                    SellerRotation = new Vector3(0.00, 0.00, 60.67),
                    MarkerPosition = new Vector3(125.47, -223.73, 53.56),
                    Blip = 73,
                    BlipPosition = new Vector3(127.8201, -211.8274, 55.22751),
                    DoorId = 1780022985,
                    LeftDoorPosition = new Vector3(127.8201, -211.8274, 55.22751),
                    DressingRoom = new DressingRoomPositions {
                        Position = new Vector3(119.30, -220.05, 54.56),
                        Rotation = new Vector3(0.00, 0.00, -109.17),
                        CameraHatsPosition = new Vector3(120.00, -220.20, 55.20),
                        CameraTopsPosition = new Vector3(120.40, -220.20, 54.90),
                        CameraLegsPosition = new Vector3(120.40, -220.20, 54.15),
                        CameraFeetsPosition = new Vector3(120.10, -220.20, 53.80),
                        CameraRotation = new Vector3(0.00, 0.00, 74.72)
                    },
                    District = 5
                },
                new ClothesShopModel {
                    Type = ClothesShopType.SubUrban,
                    Seller = PedHash.AlDiNapoli,
                    SellerPosition = new Vector3(-1194.12, -767.09, 17.32),
                    SellerRotation = new Vector3(0.00, 0.00, -149.37),
                    MarkerPosition = new Vector3(-1193.12, -768.31, 16.32),
                    Blip = 73,
                    BlipPosition = new Vector3(-1201.435, -776.8566, 17.99184),
                    DoorId = 1780022985,
                    LeftDoorPosition = new Vector3(-1201.435, -776.8566, 17.99184),
                    DressingRoom = new DressingRoomPositions {
                        Position = new Vector3(-1189.70, -774.64, 17.33),
                        Rotation = new Vector3(0.00, 0.00, 37.89),
                        CameraHatsPosition = new Vector3(-1190.20, -773.90, 17.93),
                        CameraTopsPosition = new Vector3(-1190.40, -773.60, 17.53),
                        CameraLegsPosition = new Vector3(-1190.40, -773.60, 17.03),
                        CameraFeetsPosition = new Vector3(-1190.30, -773.70, 16.63),
                        CameraRotation = new Vector3(0.00, 0.00, -147.00)
                    },
                    District = 3
                },
                new ClothesShopModel {
                    Type = ClothesShopType.SubUrban,
                    Seller = PedHash.AlDiNapoli,
                    SellerPosition = new Vector3(612.95, 2762.45, 42.09),
                    SellerRotation = new Vector3(0.00, 0.00, -86.94),
                    MarkerPosition = new Vector3(614.47, 2762.59, 41.09),
                    Blip = 73,
                    BlipPosition = new Vector3(617.2458, 2751.022, 42.75777),
                    DoorId = 1780022985,
                    LeftDoorPosition = new Vector3(617.2458, 2751.022, 42.75777),
                    DressingRoom = new DressingRoomPositions {
                        Position = new Vector3(621.69, 2762.14, 42.09),
                        Rotation = new Vector3(0.00, 0.00, 94.87),
                        CameraHatsPosition = new Vector3(620.75, 2761.96, 42.80),
                        CameraTopsPosition = new Vector3(620.45, 2761.96, 42.45),
                        CameraLegsPosition = new Vector3(620.45, 2761.96, 41.70),
                        CameraFeetsPosition = new Vector3(620.75, 2761.96, 41.35),
                        CameraRotation = new Vector3(0.00, 0.00, -88.77)
                    },
                    District = 9
                },
                new ClothesShopModel {
                    Type = ClothesShopType.SubUrban,
                    Seller = PedHash.ShopLowSFY,
                    SellerPosition = new Vector3(-3169.30, 1043.44, 20.86),
                    SellerRotation = new Vector3(0.00, 0.00, 65.70),
                    MarkerPosition = new Vector3(-3170.76, 1043.94, 19.86),
                    Blip = 73,
                    BlipPosition = new Vector3(-3167.75, 1055.536, 21.53288),
                    DoorId = 1780022985,
                    LeftDoorPosition = new Vector3(-3167.75, 1055.536, 21.53288),
                    DressingRoom = new DressingRoomPositions {
                        Position = new Vector3(-3176.89, 1047.73, 20.86),
                        Rotation = new Vector3(0.00, 0.00, -112.33),
                        CameraHatsPosition = new Vector3(-3176.00, 1047.55, 21.60),
                        CameraTopsPosition = new Vector3(-3175.80, 1047.39, 21.10),
                        CameraLegsPosition = new Vector3(-3175.80, 1047.39, 20.40),
                        CameraFeetsPosition = new Vector3(-3176.00, 1047.55, 20.10),
                        CameraRotation = new Vector3(0.00, 0.00, 69.01)
                    },
                    District = 8
                },
                new ClothesShopModel {
                    Type = ClothesShopType.Ponsonbys,
                    Seller = PedHash.ShopHighSFM,
                    SellerPosition = new Vector3(-709.12, -151.36, 37.42),
                    SellerRotation = new Vector3(0.00, 0.00, 118.87),
                    MarkerPosition = new Vector3(-710.60, -152.31, 36.42),
                    Blip = 73,
                    BlipPosition = new Vector3(-716.6755, -155.42, 37.67493),
                    DoorId = -1922281023,
                    LeftDoorPosition = new Vector3(-716.6755, -155.42, 37.67493),
                    RightDoorPosition = new Vector3(-715.6154, -157.2561, 37.67493),
                    DressingRoom = new DressingRoomPositions {
                        Position = new Vector3(-703.59, -150.74, 37.42),
                        Rotation = new Vector3(0.00, 0.00, 163.95),
                        CameraHatsPosition = new Vector3(-703.71, -151.57, 38.12),
                        CameraTopsPosition = new Vector3(-703.81, -151.97, 37.82),
                        CameraLegsPosition = new Vector3(-703.81, -151.97, 37.05),
                        CameraFeetsPosition = new Vector3(-703.71, -151.67, 36.65),
                        CameraRotation = new Vector3(0.00, 0.00, -20.99)
                    },
                    District = 4
                },
                new ClothesShopModel {
                    Type = ClothesShopType.Ponsonbys,
                    Seller = PedHash.ShopHighSFM,
                    SellerPosition = new Vector3(-1448.78, -237.85, 49.81),
                    SellerRotation = new Vector3(0.00, 0.00, 43.74),
                    MarkerPosition = new Vector3(-1450.03, -236.76, 48.81),
                    Blip = 73,
                    BlipPosition = new Vector3(-1454.782, -231.7927, 50.05649),
                    DoorId = -1922281023,
                    LeftDoorPosition = new Vector3(-1454.782, -231.7927, 50.05649),
                    RightDoorPosition = new Vector3(-1456.201, -233.3682, 50.05648),
                    DressingRoom = new DressingRoomPositions {
                        Position = new Vector3(-1446.29, -242.71, 49.82),
                        Rotation = new Vector3(0.00, 0.00, 98.36),
                        CameraHatsPosition = new Vector3(-1447.11, -242.89, 50.52),
                        CameraTopsPosition = new Vector3(-1447.51, -242.89, 50.22),
                        CameraLegsPosition = new Vector3(-1447.51, -242.89, 49.42),
                        CameraFeetsPosition = new Vector3(-1447.21, -242.89, 49.02),
                        CameraRotation = new Vector3(0.00, 0.00, -86.22)
                    },
                    District = 3
                },
                new ClothesShopModel {
                    Type = ClothesShopType.Ponsonbys,
                    Seller = PedHash.ShopHighSFM,
                    SellerPosition = new Vector3(-165.13, -303.30, 39.73),
                    SellerRotation = new Vector3(0.00, 0.00, -111.57),
                    MarkerPosition = new Vector3(-163.46, -303.82, 38.73),
                    Blip = 73,
                    BlipPosition = new Vector3(-157.1293, -306.4341, 39.99308),
                    DoorId = -1922281023,
                    LeftDoorPosition = new Vector3(-157.1293, -306.4341, 39.99308),
                    RightDoorPosition = new Vector3(-156.439, -304.4294, 39.99308),
                    DressingRoom = new DressingRoomPositions {
                        Position = new Vector3(-169.20, -299.73, 39.73),
                        Rotation = new Vector3(0.00, 0.00, -66.13),
                        CameraHatsPosition = new Vector3(-168.51, -299.43, 40.43),
                        CameraTopsPosition = new Vector3(-168.21, -299.23, 40.09),
                        CameraLegsPosition = new Vector3(-168.21, -299.23, 39.28),
                        CameraFeetsPosition = new Vector3(-168.41, -299.37, 38.97),
                        CameraRotation = new Vector3(0.00, 0.00, 106.97)
                    },
                    District = 5
                }
            };
        }
    }
}