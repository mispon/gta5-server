using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.MenuHandlers {
    /// <summary>
    /// Обработчик меню медсестры
    /// </summary>
    internal class NurseMenuHandler : Script, IMenu {
        private const string CHAT = "[Госпиталь]";
        private const float PRICE_FOR_HP = 0.1f;
        private const int FULL_HP_TYPE = 0;
        private const int HEALT_COEF = 10;
        private const int FULL_HP = 100;

        private readonly IPlayerInfoManager _playerInfoManager;

        public NurseMenuHandler() { }
        public NurseMenuHandler(IPlayerInfoManager playerInfoManager) {
            _playerInfoManager = playerInfoManager;
        }

        /// <summary>
        /// Инициализировать обработчик
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.RESTORE_HEALTH, RestoreHealth);
        }

        /// <summary>
        /// Восстановление здоворья игрока
        /// </summary>
        private void RestoreHealth(Client player, object[] args) {
            var healType = (int) args[0];
            var healAmount = healType == FULL_HP_TYPE 
                ? FULL_HP - player.health
                : HEALT_COEF * healType;
            var recoveryAmount = player.health + healAmount > FULL_HP
                ? FULL_HP - player.health
                : healAmount;
            var price = GetPrice(healAmount);
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (price > playerInfo.Balance) {
                API.sendNotificationToPlayer(player, $"~r~{CHAT} Недостаточно денег!");
                return;
            }
            playerInfo.Balance -= price;
            playerInfo.Health += healAmount;
            _playerInfoManager.RefreshUI(player, playerInfo);
            API.setPlayerHealth(player, playerInfo.Health);
            API.sendNotificationToPlayer(player, $"~b~{CHAT} Восстановлено {recoveryAmount} ед. здоровья");
            API.triggerClientEvent(player, ServerEvent.HIDE_NURSE_MENU);
        }

        /// <summary>
        /// Возвращает стоимость лечения
        /// </summary>
        private static int GetPrice(int healAmount) {
            return (int) (PRICE_FOR_HP * healAmount);
        }
    }
}