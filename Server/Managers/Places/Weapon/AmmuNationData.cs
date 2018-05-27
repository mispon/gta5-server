using System.Collections.Generic;
using gta_mp_server.Models.Shops;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Places.Weapon {
    /// <summary>
    /// Данные магазинов оружия
    /// </summary>
    internal class AmmuNationData {
        /// <summary>
        /// Данные магазинов
        /// </summary>
        internal static List<AmmuNationModel> Shops = new List<AmmuNationModel> {
            new AmmuNationModel {
                Name = "Ammu Nation Vespucci",
                SellerPosition = new Vector3(841.12, -1035.27, 28.19),
                SellerRotation = new Vector3(0.00, 0.00, -8.23),
                Marker = new Vector3(841.90, -1033.60, 27.19),
                LeftDoorPosition = new Vector3(845.3694, -1024.539, 28.34478),
                RightDoorPosition = new Vector3(842.7685, -1024.539, 28.34478),
                District = 7
            },
            new AmmuNationModel {
                Name = "Ammu Nation Lindsay",
                SellerPosition = new Vector3(-661.04, -933.57, 21.83),
                SellerRotation = new Vector3(0.00, 0.00, 157.88),
                Marker = new Vector3(-661.73, -934.97, 20.83),
                LeftDoorPosition = new Vector3(-665.2424, -944.3256, 21.97915),
                RightDoorPosition = new Vector3(-662.6415, -944.3256, 21.97915),
                District = 2
            },
            new AmmuNationModel {
                Name = "Ammu Nation Popular",
                SellerPosition = new Vector3(809.06, -2159.08, 29.62),
                SellerRotation = new Vector3(0.00, 0.00, -7.93),
                Marker = new Vector3(809.72, -2157.52, 28.62),
                LeftDoorPosition = new Vector3(813.1779, -2148.27, 29.76892),
                RightDoorPosition = new Vector3(810.5769, -2148.27, 29.76892),
                District = 7
            },
            new AmmuNationModel {
                Name = "Ammu Nation Popular",
                SellerPosition = new Vector3(23.52, -1105.89, 29.80),
                SellerRotation = new Vector3(0.00, 0.00, 148.36),
                Marker = new Vector3(22.63, -1107.03, 28.80),
                LeftDoorPosition = new Vector3(16.12787, -1114.606, 29.94694),
                RightDoorPosition = new Vector3(18.572, -1115.495, 29.94694),
                District = 1
            },
            new AmmuNationModel {
                Name = "Ammu Nation Vinewood",
                SellerPosition = new Vector3(253.47, -51.46, 69.94),
                SellerRotation = new Vector3(0.00, 0.00, 66.18),
                Marker = new Vector3(252.28, -50.58, 68.94),
                LeftDoorPosition = new Vector3(244.7275, -44.07911, 70.09098),
                RightDoorPosition = new Vector3(243.8379, -46.52324, 70.09098),
                District = 5
            }
        };

        /// <summary>
        /// Каталог оружия
        /// </summary>
        internal static List<WeaponGood> Weapons = new List<WeaponGood> {
            new WeaponGood {Name = "Нож", Model = -1716189206, Price = 500},
            new WeaponGood {Name = "Молоток", Model = 1317494643, Price = 400},
            new WeaponGood {Name = "Монтировка", Model = -2067956739, Price = 400},
            new WeaponGood {Name = "Бита", Model = -1786099057, Price = 360},
            new WeaponGood {Name = "Розочка", Model = -102323637, Price = 200},
            new WeaponGood {Name = "Топор", Model = -102973651, Price = 500},
            new WeaponGood {Name = "Кастет", Model = -656458692, Price = 350},
            new WeaponGood {Name = "Раскладной нож", Model = -538741184, Price = 600},
            new WeaponGood {Name = "Кий", Model = -1810795771, Price = 250},
            new WeaponGood {Name = "Гаечный ключ", Model = 419712736, Price = 250},
            new WeaponGood {Name = "Пистолет", Model = 453432689, Price = 1500},
            new WeaponGood {Name = "Боевой пистолет", Model = 1593441988, Price = 2100},
            new WeaponGood {Name = "Пистолет(50 калибр)", Model = -1716589765, Price = 2400},
            new WeaponGood {Name = "SNS пистолет", Model = -1076751822, Price = 1900},
            new WeaponGood {Name = "Винтажный пистолет", Model = 137902532, Price = 3300},
            new WeaponGood {Name = "Пистолет Marksma", Model = -598887786, Price = 2900},
            new WeaponGood {Name = "Мини СМГ", Model = -1121678507, Price = 5700},
            new WeaponGood {Name = "Пистолет-автомат", Model = -619010992, Price = 5500},
            new WeaponGood {Name = "Помповый дробовик", Model = 487013001, Price = 4000},
            new WeaponGood {Name = "Обрез", Model = 2017895192, Price = 4300},
            new WeaponGood {Name = "Мушкет", Model = -1466123874, Price = 3200},
            new WeaponGood {Name = "Двузарядный дробовик", Model = -275439685, Price = 3500}
        };

        /// <summary>
        /// Каталог брони
        /// </summary>
        internal static List<WeaponGood> Ammo = new List<WeaponGood> {
            new WeaponGood {Name = "Патроны пистолета", Model = (int) WeaponAmmoType.Handguns, Price = 3},
            new WeaponGood {Name = "Патроны пистолета-автомата", Model = (int) WeaponAmmoType.MachineGuns, Price = 6},
            new WeaponGood {Name = "Патроны дробовика", Model = (int) WeaponAmmoType.Shotguns, Price = 5}
        };
    }
}