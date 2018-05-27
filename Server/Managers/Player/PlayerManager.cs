using System;
using System.Linq;
using gta_mp_database.Models.Player;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Voice;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Player {
    /// <summary>
    /// Логика общих действий игроков
    /// </summary>
    internal class PlayerManager : Script, IPlayerManager {
        internal const float ONE_METER = 0.5f;
        private const string POSITION_KEY = "player_position";
        private const float SATIETY_CONSUMPTION = 0.003f;
        private const float PASSIVE_CONSUMPTION = 0.07f;

        private readonly IPlayerInfoManager _playerInfoManager;

        public PlayerManager() {}
        public PlayerManager(IPlayerInfoManager playerInfoManager) {
            _playerInfoManager = playerInfoManager;
        }

        /// <summary>
        /// Обновить состояния игроков
        /// </summary>
        public void UpdatePlayers() {
            var players = API.getAllPlayers();
            foreach (var player in players) {
                var playerInfo = _playerInfoManager.GetInfo(player);
                if (playerInfo == null) {
                    continue;
                }
                UpdateSatiety(player, playerInfo);
            }
        }

        /// <summary>
        /// Устанавливает ник игрока
        /// </summary>
        public static void SetPlayerName(Client player, PlayerInfo playerInfo) {
            API.shared.setPlayerName(player, playerInfo.Name);
            API.shared.setPlayerNametag(player, $"{playerInfo.TagName}{playerInfo.Name}");
            var tagColor = string.IsNullOrEmpty(playerInfo.TagColor) 
                ? new byte[] { 255, 255, 255 }
                : playerInfo.TagColor.Split(';').Select(e => Convert.ToByte(e)).ToArray();
            API.shared.resetPlayerNametagColor(player);
            API.shared.setPlayerNametagColor(player, tagColor[0], tagColor[1], tagColor[2]);
            API.shared.setPlayerNametagVisible(player, true);
            var voiceClient = VoiceManager.VoiceServer.GetClients().FirstOrDefault(e => e.Player == player);
            voiceClient?.SetNickname(playerInfo.Name);
        }

        /// <summary>
        /// Обновить сытость
        /// </summary>
        private void UpdateSatiety(Client player, PlayerInfo playerInfo) {
            var position = API.getEntityPosition(player);
            var lastPosition = PlayerHelper.GetData(player, POSITION_KEY, new Vector3());
            var distance = Vector3.Distance(position, lastPosition);
            player.setData(POSITION_KEY, position);
            SetSatiety(player, playerInfo, distance);
            _playerInfoManager.RefreshUI(player, playerInfo);
        }

        /// <summary>
        /// Установить корректную сытость
        /// </summary>
        private void SetSatiety(Client player, PlayerInfo playerInfo, float distance) {
            if (playerInfo.Satiety <= 0) {
                var health = API.getPlayerHealth(player) - 1;
                API.setPlayerHealth(player, health);
                return;
            }
            playerInfo.Satiety -= GetConsumption(playerInfo.Satiety, PASSIVE_CONSUMPTION);
            if (!player.isInVehicle && DistanceCorect(distance)) {
                var consumption = (distance / ONE_METER) * SATIETY_CONSUMPTION;
                playerInfo.Satiety -= GetConsumption(playerInfo.Satiety, consumption);
            }
        }

        /// <summary>
        /// Возвращает допустимое значение расхода сытости
        /// </summary>
        private static float GetConsumption(float satiety, float consumption) {
            return satiety < consumption ? satiety : consumption;
        }

        /// <summary>
        /// Проверка, что, пройденная за время между апдейтами, дистанция реальная
        /// т.е. не было перемещений
        /// </summary>
        private static bool DistanceCorect(float distance) {
            return distance < 75;
        }
    }
}