using System;
using System.Linq;
using gta_mp_database.Models.Player;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.House.Interfaces;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;

namespace gta_mp_server.Managers.Player {
    /// <summary>
    /// Обработчик меню действий игрока
    /// </summary>
    internal class PlayerActionsMenuHandler : Script, IMenu {
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IHouseManager _houseManager;

        public PlayerActionsMenuHandler() {}
        public PlayerActionsMenuHandler(IPlayerInfoManager playerInfoManager, IHouseManager houseManager) {
            _playerInfoManager = playerInfoManager;
            _houseManager = houseManager;
        }

        /// <summary>
        /// Инициализировать обработчик меню
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.TRIGGER_PLAYER_ACTION_MENU, TriggerActionMenu);
            ClientEventHandler.Add(ClientEvent.PLAY_PLAYER_ANIM, PlayPlayerAnimation);
            ClientEventHandler.Add(ClientEvent.STOP_PLAYER_ANIM, (player, args) => API.stopPlayerAnimation(player));
            ClientEventHandler.Add(ClientEvent.SEND_MONEY_TO_PLAYER, SendMoneyToPlayer);
            ClientEventHandler.Add(ClientEvent.TELEPORT_TO_HOUSE, TeleportToHouse);
        }

        private void TriggerActionMenu(Client player, object[] args) {
            var isOpen = (bool) args[0];
            if (isOpen) {
                var accountId = (long) player.getData(PlayerInfoManager.ID_KEY);
                var housesPositions = _houseManager.GetPlayerHouses(accountId).Select(e => PositionConverter.ToVector3(e.Position));
                API.triggerClientEvent(player, ServerEvent.SHOW_PLAYER_ACTION_MENU, JsonConvert.SerializeObject(housesPositions));
            }
            else {
                API.triggerClientEvent(player, ServerEvent.HIDE_PLAYER_ACTION_MENU);
            }
        }

        /// <summary>
        /// Запускает анимацию игрока
        /// </summary>
        private void PlayPlayerAnimation(Client player, object[] args) {
            var scenarioName = args[0].ToString();
            if (scenarioName == "WORLD_HUMAN_AA_SMOKE") {
                var newHealth = API.getPlayerHealth(player) - 3;
                API.setPlayerHealth(player, newHealth);
            }
            API.playPlayerScenario(player, scenarioName);
        }
        
        /// <summary>
        /// Передает деньги другому игроку
        /// </summary>
        private void SendMoneyToPlayer(Client player, object[] args) {
            var targetName = args[0].ToString().Trim();
            var amount = (int) args[1];
            var targetPlayer = API.getPlayerFromName(targetName);
            if (targetPlayer == null) {
                API.sendNotificationToPlayer(player, "~r~Игрок не в сети или не существует", true);
                return;
            }
            if (targetPlayer == player) {
                API.sendNotificationToPlayer(player, "~r~Нельзя передать деньги самому себе", true);
                return;
            }
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (!PlayerHelper.EnoughMoney(player, playerInfo, amount)) {
                return;
            }
            playerInfo.Balance -= amount;
            _playerInfoManager.RefreshUI(player, playerInfo);
            API.sendNotificationToPlayer(player, $"~b~Вы передали {targetPlayer.name}'у {amount}$");
            var targetInfo = _playerInfoManager.GetInfo(targetPlayer);
            targetInfo.Balance += amount;
            _playerInfoManager.RefreshUI(targetPlayer, targetInfo);
            API.sendNotificationToPlayer(targetPlayer, $"~b~{player.name} передал вам {amount}$");
        }

        /// <summary>
        /// Обработчик перемещения к дому
        /// </summary>
        private void TeleportToHouse(Client player, object[] args) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (IsTeleportCooldown(playerInfo)) {
                API.sendNotificationToPlayer(player, "~r~Перемещение к дому перезаряжается", true);
                return;
            }
            playerInfo.LastTeleportToHouse = DateTime.Now;
            var position = (Vector3) args[0];
            API.setEntityPosition(player, position);
        }

        /// <summary>
        /// Проверяет, находится ли телепорт на перезарядке
        /// </summary>
        private static bool IsTeleportCooldown(PlayerInfo playerInfo) {
            var hoursPassed = (DateTime.Now - playerInfo.LastTeleportToHouse).TotalHours;
            return playerInfo.IsPremium() ? hoursPassed < 1 : hoursPassed < 3;
        }
    }
}