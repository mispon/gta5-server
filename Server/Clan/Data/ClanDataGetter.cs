using System.Collections.Generic;
using gta_mp_database.Models.Player;
using gta_mp_server.Models.Clan;
using gta_mp_server.Models.Shops;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Clan.Data {
    /// <summary>
    /// Содержит различные данные кланов
    /// </summary>
    internal class ClanDataGetter {
        /// <summary>
        /// Данные кланов
        /// </summary>
        internal static List<ClanInfo> ClansInfo = new List<ClanInfo> {
            new ClanInfo {
                ClanId = 1,
                ClanName = "Семья Лос-Сантоса",
                BlipColor = 38,
                Enter = new Vector3(-698.63, 47.02, 44.03),
                AfterEnter = new Vector3(-1576.73, -567.36, 108.52),
                Exit = new Vector3(-1582.60, -558.73, 108.52),
                AfterExit = new Vector3(-697.29, 43.87, 43.32),
                LeaderName = "Майкл",
                LeaderPosition = new Vector3(-1554.13, -573.89, 108.52),
                LeaderRotation = new Vector3(0.00, 0.00, 126.50),
                LeaderMarker = new Vector3(-1554.95, -574.54, 107.62),
                LeaderHash = PedHash.Michael,
                AdminPosition = new Vector3(-1570.99, -574.87, 108.52),
                AdminRotation = new Vector3(0.00, 0.00, 31.09),
                AdminMarker = new Vector3(-1572.12, -573.49, 107.66),
                AdminHash = PedHash.Business01AFY,
                GunsmithPosition = new Vector3(-1573.33, -588.83, 108.53),
                GunsmithRotation = new Vector3(0.00, 0.00, -52.07),
                GunsmithMarker = new Vector3(-1571.30, -587.10, 107.63),
                GunsmithHash = PedHash.Paper,
                MechPosition = new Vector3(-1580.48, -579.97, 108.52),
                MechRotation = new Vector3(0.00, 0.00, -62.48),
                MechMarker = new Vector3(-1579.80, -579.36, 107.62),
                MechHash = PedHash.SolomonCutscene,
                DressingMarker = new Vector3(-1564.83, -572.09, 107.52),
                DressingRoomPositions = new DressingRoomPositions {
                    Position = new Vector3(-1566.04, -570.98, 108.52),
                    Rotation = new Vector3(0.00, 0.00, -55.46),
                    CameraRotation = new Vector3(0.00, 0.00, 127.71),
                    CameraHatsPosition = new Vector3(-1565.50, -570.60, 109.20),
                    CameraTopsPosition = new Vector3(-1565.30, -570.40, 108.80),
                    CameraLegsPosition = new Vector3(-1565.30, -570.40, 108.10),
                    CameraFeetsPosition = new Vector3(-1565.40, -570.35, 107.70)
                },
                Courtyard = new ClanCourtyardInfo {
                    MissionEndPoint = new Vector3(-683.33, 45.82, 42.17),
                    VansGarage = new Vector3(-667.66, 83.60, 50.88),
                    VansSpawnPosition = new Vector3(-671.30, 81.44, 49.81),
                    VansSpawnRotation = new Vector3(-9.74, 2.42, -154.84)
                }
            },
            new ClanInfo {
                ClanId = 2,
                ClanName = "Картель дяди Ти",
                BlipColor = 1,
                Enter = new Vector3(1165.57, -1347.24, 35.96),
                AfterEnter = new Vector3(-75.30, -823.51, 243.39),
                Exit = new Vector3(-78.86, -832.99, 243.39),
                AfterExit = new Vector3(1167.54, -1347.04, 34.91),
                LeaderName = "Тревор",
                LeaderPosition = new Vector3(-82.60, -801.02, 243.39),
                LeaderRotation = new Vector3(0.00, 0.00, -108.97),
                LeaderMarker = new Vector3(-81.85, -801.20, 242.41),
                LeaderHash = PedHash.Trevor,
                AdminPosition = new Vector3(-72.13, -814.37, 243.39),
                AdminRotation = new Vector3(0.00, 0.00, 157.95),
                AdminMarker = new Vector3(-72.98, -816.10, 242.43),
                AdminHash = PedHash.Ashley,
                GunsmithPosition = new Vector3(-59.44, -808.67, 243.39),
                GunsmithRotation = new Vector3(0.00, 0.00, 71.16),
                GunsmithMarker = new Vector3(-61.86, -807.89, 242.41),
                GunsmithHash = PedHash.AviSchwartzman,
                MechPosition = new Vector3(-62.70, -819.54, 243.39),
                MechRotation = new Vector3(0.00, 0.00, 67.02),
                MechMarker = new Vector3(-63.45, -819.36, 242.41),
                MechHash = PedHash.Cletus,
                DressingMarker = new Vector3(-78.05, -811.14, 242.39),
                DressingRoomPositions = new DressingRoomPositions {
                    Position = new Vector3(-78.31, -812.78, 243.39),
                    Rotation = new Vector3(0.00, 0.00, 61.87),
                    CameraRotation = new Vector3(0.00, 0.00, -105.69),
                    CameraHatsPosition = new Vector3(-79.16, -812.55, 244.09),
                    CameraTopsPosition = new Vector3(-79.46, -812.50, 243.79),
                    CameraLegsPosition = new Vector3(-79.46, -812.50, 243.09),
                    CameraFeetsPosition = new Vector3(-79.26, -812.60, 242.69)
                },
                Courtyard = new ClanCourtyardInfo {
                    MissionEndPoint = new Vector3(1145.23, -1331.87, 33.66),
                    VansGarage = new Vector3(1158.75, -1313.07, 33.74),
                    VansSpawnPosition = new Vector3(1163.79, -1316.68, 33.14),
                    VansSpawnRotation = new Vector3(-0.15, 0.00, 175.85)
                }
            },
            new ClanInfo {
                ClanId = 3,
                ClanName = "Короли долины",
                BlipColor = 25,
                Enter = new Vector3(-1040.51, -1475.31, 5.58),
                AfterEnter = new Vector3(-140.18, -624.45, 168.82),
                Exit = new Vector3(-141.12, -614.12, 168.82),
                AfterExit = new Vector3(-1041.88, -1472.91, 5.06),
                LeaderName = "Франклин",
                LeaderPosition = new Vector3(-123.94, -641.21, 168.82),
                LeaderRotation = new Vector3(0.00, 0.00, 97.95),
                LeaderMarker = new Vector3(-124.74, -641.28, 167.92),
                LeaderHash = PedHash.Franklin,
                AdminPosition = new Vector3(-139.08, -633.95, 168.82),
                AdminRotation = new Vector3(0.00, 0.00, 7.89),
                AdminMarker = new Vector3(-139.32, -632.00, 167.86),
                AdminHash = PedHash.Families01GFY,
                GunsmithPosition = new Vector3(-147.92, -644.60, 168.82),
                GunsmithRotation = new Vector3(0.00, 0.00, -84.87),
                GunsmithMarker = new Vector3(-145.43, -644.23, 167.92),
                GunsmithHash = PedHash.LamarDavis,
                MechPosition = new Vector3(-149.76, -633.52, 168.82),
                MechRotation = new Vector3(0.00, 0.00, -86.61),
                MechMarker = new Vector3(-149.00, -633.40, 167.92),
                MechHash = PedHash.Claypain,
                DressingMarker = new Vector3(-132.20, -634.79, 167.82),
                DressingRoomPositions = new DressingRoomPositions {
                    Position = new Vector3(-132.74, -632.89, 168.82),
                    Rotation = new Vector3(0.00, 0.00, -91.73),
                    CameraRotation = new Vector3(0.00, 0.00, 95.58),
                    CameraHatsPosition = new Vector3(-132.10, -632.76, 169.50),
                    CameraTopsPosition = new Vector3(-131.80, -632.66, 169.10),
                    CameraLegsPosition = new Vector3(-131.80, -632.66, 168.40),
                    CameraFeetsPosition = new Vector3(-132.00, -632.76, 168.00)
                },
                Courtyard = new ClanCourtyardInfo {
                    MissionEndPoint = new Vector3(-1003.25, -1461.82, 3.98),
                    VansGarage = new Vector3(-1016.94, -1472.21, 4.00),
                    VansSpawnPosition = new Vector3(-1012.18, -1468.16, 3.40),
                    VansSpawnRotation = new Vector3(0.00, 0.14, 36.17)
                }
            }
        };

        /// <summary>
        /// Позиции для кланового превью машин
        /// </summary>
        internal static Dictionary<long, VehicleShowroomPositions> ClanVehicleShowroom = new Dictionary<long, VehicleShowroomPositions> {
            [1] = new VehicleShowroomPositions {
                PreviewPosition = new Vector3(-1565.51, -563.98, 85.97),
                PreviewRotation = new Vector3(-0.06, 0.01, -0.76),
                CameraPosition = new Vector3(-1569.42, -559.54, 88.50),
                CameraRotation = new Vector3(-20.00, 0.00, -140.47)
            },
            [2] = new VehicleShowroomPositions {
                PreviewPosition = new Vector3(-73.99, -824.10, 221.47),
                PreviewRotation = new Vector3(-0.02, -0.06, -150.65),
                CameraPosition = new Vector3(-68.00, -826.00, 224.00),
                CameraRotation = new Vector3(-20.00, 0.00, 70.71)
            },
            [3] = new VehicleShowroomPositions {
                PreviewPosition = new Vector3(-120.36, -564.99, 135.47),
                PreviewRotation = new Vector3(-0.14, 0.00, 166.58),
                CameraPosition = new Vector3(-117.45, -569.82, 138.00),
                CameraRotation = new Vector3(-20.00, 0.00, 29.34)
            }
        };

        /// <summary>
        /// Клановый транспорт
        /// </summary>
        internal static Dictionary<long, List<ShowroomVehicle>> ClanVehicles = new Dictionary<long, List<ShowroomVehicle>> {
            [1] = new List<ShowroomVehicle> {
                new ShowroomVehicle {Hash = 634118882, MaxFuel = 70, Price = 20000},
                new ShowroomVehicle {Hash = 941800958, MaxFuel = 80, Price = 50000},
                new ShowroomVehicle {Hash = -1660945322, MaxFuel = 100, Price = 100000}
            },
            [2] = new List<ShowroomVehicle> {
                new ShowroomVehicle {Hash = -2064372143, MaxFuel = 70, Price = 20000},
                new ShowroomVehicle {Hash = -1479664699, MaxFuel = 80, Price = 50000},
                new ShowroomVehicle {Hash = 272929391, MaxFuel = 100, Price = 100000}
            },
            [3] = new List<ShowroomVehicle> {
                new ShowroomVehicle {Hash = -808457413, MaxFuel = 70, Price = 20000},
                new ShowroomVehicle {Hash = 2049897956, MaxFuel = 80, Price = 50000},
                new ShowroomVehicle {Hash = -295689028, MaxFuel = 100, Price = 100000}
            }
        };

        /// <summary>
        /// Возвращает клановую одежду
        /// </summary>
        internal static List<ClothesModel> GetClanClothes(long clanId, bool isMale) {
            return isMale ? _maleClanClothes[clanId] : _femaleClanClothes[clanId];
        }

        /// <summary>
        /// Каталог оружия
        /// </summary>
        internal static List<WeaponGood> Weapons = new List<WeaponGood> {
            new WeaponGood {Name = "Кинжал", Model = -1834847097, Price = 600},
            new WeaponGood {Name = "Мачете", Model = -581044007, Price = 800},
            new WeaponGood {Name = "Боевой топор", Model = -853065399, Price = 800},
            new WeaponGood {Name = "Тяжелый пистолет", Model = -771403250, Price = 2000},
            new WeaponGood {Name = "Револьвер", Model = -1045183535, Price = 2500},
            new WeaponGood {Name = "Gusenberg", Model = 1627465347, Price = 4000},
            new WeaponGood {Name = "Микро СМГ", Model = 324215364, Price = 4500},
            new WeaponGood {Name = "Штурмовая винтовка", Model = -1074790547, Price = 6000},
            new WeaponGood {Name = "Автоматическая винтовка", Model = -2084633992, Price = 6500},
            new WeaponGood {Name = "Улучшенная винтовка", Model = -1357824103, Price = 7500},
            new WeaponGood {Name = "Особый карабин", Model = -1063057011, Price = 7000},
            new WeaponGood {Name = "Винтовка-«буллпап»", Model = 2132975508, Price = 6000},
            new WeaponGood {Name = "Укороченная винтовка", Model = 1649403952, Price = 5000},
            new WeaponGood {Name = "Снайперская винтовка", Model = 100416529, Price = 10000},
            new WeaponGood {Name = "Тяжёлая снайперская винтовка", Model = 205991906, Price = 12000},
            new WeaponGood {Name = "Марксманская винтовка", Model = -952879014, Price = 11000},
        };

        /// <summary>
        /// Каталог патронов
        /// </summary>
        internal static List<WeaponGood> Ammo = new List<WeaponGood> {
            new WeaponGood {Name = "Патроны пистолета", Model = (int) WeaponAmmoType.Handguns, Price = 2},
            new WeaponGood {Name = "Патроны пистолета-автомата", Model = (int) WeaponAmmoType.MachineGuns, Price = 6},
            new WeaponGood {Name = "Патроны дробовика", Model = (int) WeaponAmmoType.Shotguns, Price = 5},
            new WeaponGood {Name = "Патроны автомата", Model = (int) WeaponAmmoType.AssaultRifles, Price = 9},
            new WeaponGood {Name = "Патроны для снайперской винтовки", Model = (int) WeaponAmmoType.SniperRifles, Price = 15}
        };

        /// <summary>
        /// Мужская клановая одежда
        /// </summary>
        private static readonly Dictionary<long, List<ClothesModel>> _maleClanClothes = new Dictionary<long, List<ClothesModel>> {
            [1] = new List<ClothesModel> {
                new ClothesModel {Variation = 26, Slot = 0, Price = 0, IsClothes = false, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 29, Slot = 0, Price = 0, IsClothes = false, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 61, Slot = 0, Price = 0, IsClothes = false, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 28, Slot = 11, Price = 0, Torso = 4, Undershirt = 39, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 43, Slot = 11, Price = 0, Torso = 11, Undershirt = 15, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 115, Slot = 11, Price = 0, Torso = 4, Undershirt = 72, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 20, Slot = 4, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 24, Slot = 4, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 37, Slot = 4, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 21, Slot = 6, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 18, Slot = 6, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}}
            },
            [2] = new List<ClothesModel> {
                new ClothesModel {Variation = 20, Slot = 0, Price = 0, IsClothes = false, Texture = 3, Textures = new List<int>{3}},
                new ClothesModel {Variation = 30, Slot = 0, Price = 0, IsClothes = false, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 55, Slot = 0, Price = 0, IsClothes = false, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 39, Slot = 11, Price = 0, Torso = 0, Undershirt = 57, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 79, Slot = 11, Price = 0, Torso = 14, Undershirt = 57, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 167, Slot = 11, Price = 0, Torso = 6, Undershirt = 40, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 5, Slot = 4, Price = 0, IsClothes = true, Texture = 5, Textures = new List<int>{5}},
                new ClothesModel {Variation = 19, Slot = 4, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 38, Slot = 4, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 27, Slot = 6, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 71, Slot = 6, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}}
            },
            [3] = new List<ClothesModel> {
                new ClothesModel {Variation = 14, Slot = 0, Price = 0, IsClothes = false, Texture = 4, Textures = new List<int>{4}},
                new ClothesModel {Variation = 45, Slot = 0, Price = 0, IsClothes = false, Texture = 4, Textures = new List<int>{4}},
                new ClothesModel {Variation = 54, Slot = 0, Price = 0, IsClothes = false, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 14, Slot = 11, Price = 0, Torso = 12, Undershirt = 57, IsClothes = true, Texture = 6, Textures = new List<int>{6}},
                new ClothesModel {Variation = 87, Slot = 11, Price = 0, Torso = 1, Undershirt = 57, IsClothes = true, Texture = 3, Textures = new List<int>{3}},
                new ClothesModel {Variation = 128, Slot = 11, Price = 0, Torso = 0, Undershirt = 57, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 3, Slot = 4, Price = 0, IsClothes = true, Texture = 6, Textures = new List<int>{6}},
                new ClothesModel {Variation = 42, Slot = 4, Price = 0, IsClothes = true, Texture = 6, Textures = new List<int>{6}},
                new ClothesModel {Variation = 7, Slot = 6, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 9, Slot = 6, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}}
            }
        };

        /// <summary>
        /// Женская клановая одежда
        /// </summary>
        private static readonly Dictionary<long, List<ClothesModel>> _femaleClanClothes = new Dictionary<long, List<ClothesModel>> {
            [1] = new List<ClothesModel> {
                new ClothesModel {Variation = 13, Slot = 0, Price = 0, IsClothes = false, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 14, Slot = 0, Price = 0, IsClothes = false, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 26, Slot = 0, Price = 0, IsClothes = false, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 7, Slot = 11, Price = 0, Torso = 5, Undershirt = 38, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 66, Slot = 11, Price = 0, Torso = 6, Undershirt = 44, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 107, Slot = 11, Price = 0, Torso = 6, Undershirt = 47, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 23, Slot = 4, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 36, Slot = 4, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 37, Slot = 4, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 7, Slot = 6, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 8, Slot = 6, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}}
            },
            [2] = new List<ClothesModel> {
                new ClothesModel {Variation = 55, Slot = 0, Price = 0, IsClothes = false, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 21, Slot = 0, Price = 0, IsClothes = false, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 16, Slot = 11, Price = 0, Torso = 15, Undershirt = 2, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 45, Slot = 11, Price = 0, Torso = 3, Undershirt = 2, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 72, Slot = 11, Price = 0, Torso = 3, Undershirt = 2, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 18, Slot = 4, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 31, Slot = 4, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 38, Slot = 4, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 2, Slot = 6, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 5, Slot = 6, Price = 0, IsClothes = true, Texture = 0, Textures = new List<int>{0}}
            },
            [3] = new List<ClothesModel> {
                new ClothesModel {Variation = 53, Slot = 0, Price = 0, IsClothes = false, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 63, Slot = 0, Price = 0, IsClothes = false, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 38, Slot = 11, Price = 0, Torso = 2, Undershirt = 2, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 125, Slot = 11, Price = 0, Torso = 14, Undershirt = 2, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 140, Slot = 11, Price = 0, Torso = 5, Undershirt = 2, IsClothes = true, Texture = 0, Textures = new List<int>{0}},
                new ClothesModel {Variation = 18, Slot = 4, Price = 0, IsClothes = true, Texture = 1, Textures = new List<int>{1}},
                new ClothesModel {Variation = 4, Slot = 4, Price = 0, IsClothes = true, Texture = 4, Textures = new List<int>{4}},
                new ClothesModel {Variation = 87, Slot = 4, Price = 0, IsClothes = true, Texture = 6, Textures = new List<int>{6}},
                new ClothesModel {Variation = 4, Slot = 6, Price = 0, IsClothes = true, Texture = 1, Textures = new List<int>{1}},
                new ClothesModel {Variation = 33, Slot = 6, Price = 0, IsClothes = true, Texture = 6, Textures = new List<int>{6}}
            }
        };
    }
}