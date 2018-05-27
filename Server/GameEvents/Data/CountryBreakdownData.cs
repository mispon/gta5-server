using System;
using System.Collections.Generic;
using gta_mp_database.Models.Player;
using gta_mp_server.Enums;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.GameEvents.Data {
    /// <summary>
    /// Данные эвента "Разборка в деревне"
    /// </summary>
    internal class CountryBreakdownData {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Оружие эвента
        /// </summary>
        private static readonly List<WeaponHash> _weaponHashes = new List<WeaponHash> {
            WeaponHash.PumpShotgun, WeaponHash.Musket, WeaponHash.DoubleBarrelShotgun, WeaponHash.SawnOffShotgun
        };

        /// <summary>
        /// Позиция зоны эвента
        /// </summary>
        internal static Vector3 EventZonePosition = new Vector3(-183.12, 6272.98, 31.49);

        /// <summary>
        /// Позиция убитых игроков
        /// </summary>
        internal static Vector3 DeadPosition = new Vector3(33.95, 6099.86, 134.94);

        /// <summary>
        /// Возвращает случайное оружие из списка
        /// </summary>
        internal static WeaponHash GetWeapon() {
            return _weaponHashes[_random.Next(_weaponHashes.Count)];
        }

        /// <summary>
        /// Стартовые позиции красной команды
        /// </summary>
        internal static List<Vector3> RedTeamPositions = new List<Vector3> {
            new Vector3(-344.88, 6159.36, 31.49),
            new Vector3(-347.25, 6158.68, 31.49),
            new Vector3(-350.67, 6156.85, 31.49),
            new Vector3(-353.30, 6153.51, 31.49),
            new Vector3(-352.22, 6150.60, 31.48),
            new Vector3(-350.21, 6150.43, 31.49),
            new Vector3(-347.47, 6151.73, 31.49),
            new Vector3(-344.09, 6152.80, 31.49),
            new Vector3(-340.69, 6154.28, 31.49),
            new Vector3(-337.06, 6153.24, 31.49),
            new Vector3(-337.76, 6150.67, 31.49),
            new Vector3(-339.79, 6148.19, 31.49),
            new Vector3(-341.35, 6145.58, 31.49),
            new Vector3(-345.32, 6143.70, 31.48),
            new Vector3(-342.22, 6139.79, 31.47),
            new Vector3(-339.39, 6140.49, 31.48),
            new Vector3(-336.73, 6142.50, 31.48),
            new Vector3(-332.94, 6146.23, 31.49),
            new Vector3(-329.86, 6145.13, 31.49),
            new Vector3(-327.26, 6142.40, 31.49),
            new Vector3(-327.97, 6140.17, 31.49),
            new Vector3(-330.02, 6137.70, 31.49),
            new Vector3(-332.50, 6134.65, 31.48),
            new Vector3(-331.74, 6131.36, 31.49),
            new Vector3(-327.61, 6131.68, 31.49)
        };

        /// <summary>
        /// Стартовые позиции синей команды
        /// </summary>
        internal static List<Vector3> BlueTeamPositions = new List<Vector3> {
            new Vector3(-20.04, 6321.70, 31.23),
            new Vector3(-18.01, 6323.89, 31.23),
            new Vector3(-14.90, 6325.83, 31.23),
            new Vector3(-12.93, 6324.01, 31.24),
            new Vector3(-13.72, 6320.16, 31.23),
            new Vector3(-14.21, 6316.83, 31.23),
            new Vector3(-14.03, 6313.23, 31.23),
            new Vector3(-12.74, 6309.92, 31.23),
            new Vector3(-10.11, 6310.33, 31.23),
            new Vector3(-9.69, 6313.36, 31.23),
            new Vector3(-9.48, 6316.91, 31.24),
            new Vector3(-6.71, 6318.04, 31.24),
            new Vector3(-4.99, 6315.81, 31.23),
            new Vector3(-3.24, 6311.42, 31.23),
            new Vector3(0.36, 6311.80, 31.23),
            new Vector3(2.15, 6315.02, 31.23),
            new Vector3(0.20, 6319.85, 31.24),
            new Vector3(-3.91, 6325.82, 31.33),
            new Vector3(-3.85, 6330.63, 31.24),
            new Vector3(-7.47, 6333.80, 31.23),
            new Vector3(-10.42, 6332.29, 31.23),
            new Vector3(-10.57, 6327.67, 31.23),
            new Vector3(-13.76, 6326.88, 31.23),
            new Vector3(4.18, 6336.09, 31.23),
            new Vector3(10.08, 6333.62, 31.24)
        };

        /// <summary>
        /// Возвращает одежду игрока для эвента
        /// </summary>
        internal static List<ClothesModel> GetEventClothes(EventTeam team, bool isMale) {
            var head = new ClothesModel {Slot = 0, Variation = isMale ? 20 : 22, Texture = isMale ? 5 : 3, IsClothes = false};
            var top = new ClothesModel {
                Slot = 11, Undershirt = isMale ? 57 : 3, IsClothes = true
            };
            if (team == EventTeam.Red) {
                top.Variation = isMale ? 14 : 169;
                top.Texture = isMale ? 9 : 2;
                top.Torso = isMale ? 0 : 15;
            }
            else {
                top.Variation = isMale ? 14 : 195;
                top.Texture = isMale ? 0 : 8;
                top.Torso = isMale ? 0 : 4;
            }
            var legs = new ClothesModel {Slot = 4, Variation = isMale ? 43 : 4, IsClothes = true};
            var feets = new ClothesModel {Slot = 6, Variation = isMale ? 12 : 4, IsClothes = true};
            return new List<ClothesModel> {head, top, legs, feets};
        }
    }
}