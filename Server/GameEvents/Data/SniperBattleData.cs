using System.Collections.Generic;
using gta_mp_database.Models.Player;
using gta_mp_server.Enums;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.GameEvents.Data {
    /// <summary>
    /// Данные эвента снайперского поединка
    /// </summary>
    internal class SniperBattleData {
        /// <summary>
        /// Позиция зоны эвента
        /// </summary>
        internal static Vector3 EventZonePosition = new Vector3(3546.81, 3723.54, 37.35);

        /// <summary>
        /// Позиция убитых игроков
        /// </summary>
        internal static Vector3 DeadPosition = new Vector3(3509.94, 3614.84, 42.09);

        /// <summary>
        /// Стартовые позиции красной команды
        /// </summary>
        internal static List<Vector3> RedTeamPositions = new List<Vector3> {
            new Vector3(3483.13, 3790.23, 30.30),
            new Vector3(3495.57, 3792.09, 30.12),
            new Vector3(3510.28, 3788.83, 30.01),
            new Vector3(3524.89, 3792.22, 30.06),
            new Vector3(3539.82, 3787.17, 29.99),
            new Vector3(3554.15, 3791.73, 30.09),
            new Vector3(3566.37, 3787.47, 29.99),
            new Vector3(3580.69, 3789.57, 30.00),
            new Vector3(3594.98, 3783.95, 29.96),
            new Vector3(3572.84, 3773.22, 29.92)
        };

        /// <summary>
        /// Стартовые позиции синей команды
        /// </summary>
        internal static List<Vector3> BlueTeamPositions = new List<Vector3> {
            new Vector3(3485.29, 3674.47, 33.89),
            new Vector3(3495.95, 3669.00, 33.89),
            new Vector3(3509.62, 3665.86, 33.89),
            new Vector3(3523.35, 3662.99, 33.89),
            new Vector3(3536.75, 3660.26, 33.89),
            new Vector3(3550.14, 3665.36, 33.89),
            new Vector3(3561.28, 3661.58, 33.89),
            new Vector3(3578.75, 3666.65, 33.92),
            new Vector3(3563.72, 3673.76, 33.89),
            new Vector3(3528.35, 3674.19, 33.89)
        };

        /// <summary>
        /// Возвращает одежду игрока для эвента
        /// </summary>
        internal static List<ClothesModel> GetEventClothes(EventTeam team, bool isMale) {
            var top = new ClothesModel {Slot = 11, Undershirt = isMale ? 57 : 3, Torso = 0, IsClothes = true};
            if (team == EventTeam.Red) {
                top.Variation = isMale ? 1 : 0;
                top.Texture = isMale ? 4 : 5;
            }
            else {
                top.Variation = isMale ? 47 : 0;
                top.Texture = isMale ? 0 : 6;
            }
            return new List<ClothesModel> {top};
        }
    }
}