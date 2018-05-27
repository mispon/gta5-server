using System;
using System.Linq;
using gta_mp_database.Models.Player;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Helpers {
    /// <summary>
    /// Общие вспомогательные методы над игроками
    /// </summary>
    internal class PlayerHelper : Script {
        internal const int MAX_HEALTH = 100;
        internal const int LOADER_FLAGS =
            (int) (AnimationFlags.StopOnLastFrame | AnimationFlags.OnlyAnimateUpperBody | AnimationFlags.AllowPlayerControl);

        /// <summary>
        /// Возвращает ближайшего игрока в радиусе
        /// </summary>
        public static Client GetNearestPlayer(Client player, float range = 1f) {
            var playersInRadius = API.shared.getPlayersInRadiusOfPlayer(range, player).Where(e => e != null && e != player);
            Client result = null;
            var nearestDistance = float.MaxValue;
            foreach (var nearPlayer in playersInRadius) {
                var distance = Vector3.Distance(nearPlayer.position, player.position);
                if (distance >= nearestDistance) {
                    continue;
                }
                nearestDistance = distance;
                result = nearPlayer;
            }
            return result;
        }

        /// <summary>
        /// Проверяет, что игрок корректный
        /// </summary>
        internal static bool PlayerCorrect(Client player, bool allowVehicle = false) {
            var result = player != null;
            if (!allowVehicle) {
                result &= !player?.isInVehicle ?? false;
            }
            return result;
        }

        /// <summary>
        /// Получить привязанные данные
        /// </summary>
        internal static T GetData<T>(Client player, string key, T defaultValue) {
            var data = player.getData(key);
            return data != null ? (T) data : defaultValue;
        }

        /// <summary>
        /// Выполнить действие с проверкой игрока
        /// </summary>
        internal static void ProcessAction(NetHandle entity, Action<Client> action, bool allowVehicle = false) {
            var player = API.shared.getPlayerFromHandle(entity);
            if (!PlayerCorrect(player, allowVehicle)) {
                return;
            }
            action(player);
        }

        /// <summary>
        /// Восстановить позицию игрока
        /// </summary>
        internal static void RestorePosition(Client player) {
            var position = (Vector3) player.getData(PlayerData.LAST_POSITION);
            var dimension = (int) player.getData(PlayerData.LAST_DIMENSION);
            API.shared.setEntityPosition(player, position);
            API.shared.setEntityDimension(player, dimension);
        }

        /// <summary>
        /// Отобразить анимацию еды игрока
        /// </summary>
        internal static void PlayEatAnimation(Client player) {
            const int flag = (int) (AnimationFlags.AllowPlayerControl | AnimationFlags.OnlyAnimateUpperBody);
            API.shared.playPlayerAnimation(player, flag, "amb@code_human_wander_eating_donut@male@idle_a", "idle_c");
            //API.shared.playPlayerAnimation(player, flag, "mp_player_inteat@burger", "mp_player_int_eat_exit_burger");
        }

        /// <summary>
        /// Проверяет, что у игрока достаточно денег
        /// </summary>
        internal static bool EnoughMoney(Client player, PlayerInfo playerInfo, int price) {
            if (playerInfo.Balance < price) {
                API.shared.sendNotificationToPlayer(player, "~r~Недостаточно денег", true);
                return false;
            }
            return true;
        }
    }
}