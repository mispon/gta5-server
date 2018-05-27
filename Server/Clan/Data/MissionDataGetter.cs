using System;
using System.Collections.Generic;
using gta_mp_server.Enums.Clan;
using gta_mp_server.Models.Clan;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Clan.Data {
    /// <summary>
    /// Данные миссий
    /// </summary>
    internal class MissionDataGetter {
        /// <summary>
        /// Возвращает позицию миссии в зависимости от места
        /// </summary>
        internal static Vector3 GetMissionPosition(MissionPlace place) {
            switch (place) {
                case MissionPlace.Weed:
                    return new Vector3(2203.95, 5568.47, 52.9);
                case MissionPlace.Weapon:
                    return new Vector3(-1786.99, 3091.38, 31.8);
                case MissionPlace.Bank:
                    return new Vector3(-144.37, -577.16, 31.4);
                case MissionPlace.Coke:
                    return new Vector3(2305.99, 4885.37, 40.8);
                default:
                    throw new ArgumentOutOfRangeException(nameof(place));
            }
        }

        /// <summary>
        /// Параметры груза
        /// </summary>
        internal static Dictionary<MissionPlace, BootyObjectParams> BootyParams = new Dictionary<MissionPlace, BootyObjectParams> {
            [MissionPlace.Weed] = new BootyObjectParams {Model = -54433116, PositionOffset = new Vector3(0.12, 0.05, -0.25), RotationOffset = new Vector3(20, -70, 180)},
            [MissionPlace.Weapon] = new BootyObjectParams {Model = -371004270, PositionOffset = new Vector3(0.2, 0, -0.25), RotationOffset = new Vector3(20, -110, -60)},
            [MissionPlace.Bank] = new BootyObjectParams {Model = 289396019, PositionOffset = new Vector3(0.18, -0.05, -0.21), RotationOffset = new Vector3(20, -110, -60)},
            [MissionPlace.Coke] = new BootyObjectParams {Model = 765087784, PositionOffset = new Vector3(0.15, 0.15, -0.25), RotationOffset = new Vector3(20, -110, -60)}
        };

        /// <summary>
        /// Позиция ящика в багажнике фургона в зависимости от количества
        /// </summary>
        internal static Dictionary<MissionPlace, Dictionary<int, Vector3>> VansOffsets = new Dictionary<MissionPlace, Dictionary<int, Vector3>> {
            [MissionPlace.Weed] = new Dictionary<int, Vector3> {
                [5] = new Vector3(0, -0.3, -0.25),
                [10] = new Vector3(0.35, -0.3, -0.25),
                [15] = new Vector3(-0.35, -0.3, -0.25),
                [20] = new Vector3(0.7, -0.3, -0.25),
                [25] = new Vector3(-0.7, -0.3, -0.25),
                [30] = new Vector3(0, -0.55, -0.25),
                [35] = new Vector3(0.35, -0.55, -0.25),
                [40] = new Vector3(-0.35, -0.55, -0.25),
                [45] = new Vector3(0.7, -0.55, -0.25),
                [50] = new Vector3(-0.7, -0.55, -0.25),
                [55] = new Vector3(0, -0.8, -0.25),
                [60] = new Vector3(0.35, -0.8, -0.25),
                [65] = new Vector3(-0.35, -0.8, -0.25),
                [70] = new Vector3(0.7, -0.8, -0.25),
                [75] = new Vector3(-0.7, -0.8, -0.25),
                [80] = new Vector3(0, -0.3, -0.1),
                [85] = new Vector3(0.35, -0.3, -0.1),
                [90] = new Vector3(-0.35, -0.3, -0.1),
                [95] = new Vector3(0.7, -0.3, -0.1),
                [100] = new Vector3(-0.7, -0.3, -0.1)
            },
            [MissionPlace.Weapon] = new Dictionary<int, Vector3> {
                [5] = new Vector3(0, -0.5, -0.3),
                [10] = new Vector3(0.7, -0.5, -0.3),
                [15] = new Vector3(-0.7, -0.5, -0.3),
                [20] = new Vector3(0, -1, -0.3),
                [30] = new Vector3(0.68, -1, -0.3),
                [40] = new Vector3(-0.68, -1, -0.3),
                [50] = new Vector3(0, -1.5, -0.3),
                [60] = new Vector3(-0.4, -0.5, 0.28),
                [70] = new Vector3(0.4, -0.5, 0.28),
                [80] = new Vector3(-0.4, -1, 0.28),
                [90] = new Vector3(0.4, -1, 0.28),
                [100] = new Vector3(0, -1.5, 0.28)
            },
            [MissionPlace.Bank] = new Dictionary<int, Vector3> {
                [5] = new Vector3(0, -0.3, -0.3),
                [10] = new Vector3(0.4, -0.3, -0.3),
                [15] = new Vector3(-0.4, -0.3, -0.3),
                [20] = new Vector3(0, -0.7, -0.3),
                [25] = new Vector3(0.4, -0.7, -0.3),
                [30] = new Vector3(-0.4, -0.7, -0.3),
                [35] = new Vector3(0.75, -0.7, -0.3),
                [40] = new Vector3(-0.75, -0.7, -0.3),
                [45] = new Vector3(0, -1.1, -0.3),
                [50] = new Vector3(-0.4, -1.1, -0.3),
                [55] = new Vector3(0.4, -1.1, -0.3),
                [60] = new Vector3(0.75, -1.1, -0.3),
                [70] = new Vector3(-0.75, -1.1, -0.3),
                [80] = new Vector3(0, -1.5, -0.3),
                [90] = new Vector3(-0.4, -1.5, -0.3),
                [100] = new Vector3(0.4, -1.5, -0.3)
            },
            [MissionPlace.Coke] = new Dictionary<int, Vector3> {
                [5] = new Vector3(0, -0.5, -0.15),
                [10] = new Vector3(0.6, -0.5, -0.15),
                [15] = new Vector3(-0.6, -0.5, -0.15),
                [20] = new Vector3(0, -1, -0.15),
                [25] = new Vector3(0.6, -1, -0.15),
                [30] = new Vector3(-0.6, -1, -0.15),
                [35] = new Vector3(0.27, -1.5, -0.15),
                [40] = new Vector3(-0.27, -1.5, -0.15),
                [45] = new Vector3(0, -0.5, 0.12),
                [50] = new Vector3(0.6, -0.5, 0.12),
                [55] = new Vector3(-0.6, -0.5, 0.12),
                [60] = new Vector3(0, -1, 0.12),
                [70] = new Vector3(0.6, -1, 0.12),
                [80] = new Vector3(-0.6, -1, 0.12),
                [90] = new Vector3(0.27, -1.5, 0.12),
                [100] = new Vector3(-0.27, -1.5, 0.12)
            }
        };
    }
}