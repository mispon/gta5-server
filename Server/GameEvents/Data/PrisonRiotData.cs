using System;
using System.Collections.Generic;
using gta_mp_database.Models.Player;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.GameEvents.Data {
    /// <summary>
    /// Данные для эвента "Поножевщина"
    /// </summary>
    internal class PrisonRiotData {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Оружие заключенных
        /// </summary>
        private static readonly List<WeaponHash> _prisonerWepon = new List<WeaponHash> {
            WeaponHash.Knife, WeaponHash.Bottle, WeaponHash.KnuckleDuster
        };

        /// <summary>
        /// Оружие охранников
        /// </summary>
        private static readonly List<WeaponHash> _officerWepon = new List<WeaponHash> {
            WeaponHash.Nightstick, WeaponHash.Flashlight, WeaponHash.PoolCue
        };

        private static readonly List<PedHash> _prisonerHashes = new List<PedHash> {
            PedHash.PrisMuscl01SMY, PedHash.Prisoner01, PedHash.Prisoner01SMY
        };

        /// <summary>
        /// Позиция зоны эвента
        /// </summary>
        internal static Vector3 EventZonePosition = new Vector3(1678.63, 2524.66, 45.56);

        /// <summary>
        /// Позиция убитых игроков
        /// </summary>
        internal static Vector3 DeadPosition = new Vector3(1577.71, 2512.68, 45.56);

        /// <summary>
        /// Стартовые позиции заключенных
        /// </summary>
        internal static List<Vector3> PrisonersPositions = new List<Vector3> {
            new Vector3(1636.68, 2495.15, 45.56),
            new Vector3(1631.56, 2501.00, 45.56),
            new Vector3(1632.47, 2507.80, 45.56),
            new Vector3(1629.58, 2514.20, 45.56),
            new Vector3(1630.20, 2526.78, 45.56),
            new Vector3(1633.39, 2533.41, 45.56),
            new Vector3(1634.64, 2539.87, 45.56),
            new Vector3(1639.73, 2541.91, 45.56),
            new Vector3(1648.21, 2548.37, 45.56),
            new Vector3(1657.04, 2551.61, 45.56),
            new Vector3(1657.72, 2543.49, 45.56),
            new Vector3(1656.18, 2534.82, 45.56),
            new Vector3(1652.88, 2525.66, 45.56),
            new Vector3(1649.22, 2520.58, 45.56),
            new Vector3(1639.88, 2510.74, 45.56)
        };

        /// <summary>
        /// Стартовые позиции охранников
        /// </summary>
        internal static List<Vector3> OfficersPositions = new List<Vector3> {
            new Vector3(1706.36, 2488.86, 45.56),
            new Vector3(1712.69, 2492.87, 45.56),
            new Vector3(1718.99, 2500.52, 45.56),
            new Vector3(1714.68, 2501.37, 45.56),
            new Vector3(1717.11, 2507.30, 45.56),
            new Vector3(1711.68, 2512.64, 45.56),
            new Vector3(1702.62, 2518.07, 45.56),
            new Vector3(1708.80, 2520.59, 45.56),
            new Vector3(1719.22, 2524.64, 45.56),
            new Vector3(1718.22, 2531.32, 45.56),
            new Vector3(1712.01, 2529.65, 45.56),
            new Vector3(1705.62, 2527.71, 45.56),
            new Vector3(1702.65, 2533.05, 45.56),
            new Vector3(1703.22, 2537.34, 45.56),
            new Vector3(1709.94, 2540.97, 45.56)
        };

        /// <summary>
        /// Возвращает оружие
        /// </summary>
        internal static WeaponHash GetWeapon(bool isPrisoner) {
            return isPrisoner
                ? _prisonerWepon[_random.Next(_prisonerWepon.Count)]
                : _officerWepon[_random.Next(_officerWepon.Count)];
        }

        /// <summary>
        /// Возвращает внешний вид
        /// </summary>
        internal static PedHash GetSkin(bool isPrisoner) {
            return isPrisoner ? _prisonerHashes[_random.Next(_prisonerHashes.Count)] : PedHash.Prisguard01SMM;
        }
    }
}