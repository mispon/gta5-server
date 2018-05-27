using System;
using gta_mp_data.Entity;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.Player {
    /// <summary>
    /// Логика выдачи наград и бонусов игроку
    /// </summary>
    internal class GiftsManager : Script, IGiftsManager {
        private const int MAX_DAYS_IN_ROW = 7;
        private const int DAYS_REWARD = 14;
        private const int FIRST_GIFT_INTERVAL = 300; // 5 мин
        private const int SECOND_GIFT_INTERVAL = 900; // 15 мин
        private const int LAST_GIFT_INTERVAL = 3600; // 1 час

        private readonly IPlayerInfoManager _playerInfoManager;

        public GiftsManager() {}
        public GiftsManager(IPlayerInfoManager playerInfoManager) {
            _playerInfoManager = playerInfoManager;
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.GIVE_ONLINE_GIFT, OnGiveOnlineGift);
        }

        /// <summary>
        /// Награда за каждодневный вход в игру
        /// </summary>
        public void ProcessDaysGift(Client player, Account account) {
            if (account.LastLogin.Date == DateTime.Today) {
                return;
            }
            var totalHours = (DateTime.Now - account.LastLogin).TotalHours;
            if (totalHours >= 24 && totalHours < 48) {
                account.DaysInRow = account.DaysInRow < MAX_DAYS_IN_ROW ? account.DaysInRow + 1 : MAX_DAYS_IN_ROW;
            }
            else {
                account.DaysInRow = 0;
                return;
            }
            var playerInfo = _playerInfoManager.GetInfo(player);
            var reward = account.DaysInRow * playerInfo.Level * DAYS_REWARD;
            _playerInfoManager.SetBalance(player, reward, true);
            API.sendNotificationToPlayer(player, "~b~Выдана награда за каждодневный вход в игру");
        }

        /// <summary>
        /// Запустить таймер выдачи награды за нахождение в игре
        /// </summary>
        public void StartGiftsTimer(Client player) {
            player.setData(PlayerData.GIFT_INTERVAL, FIRST_GIFT_INTERVAL);
            API.triggerClientEvent(player, ServerEvent.SET_GIFTS_TIMER, FIRST_GIFT_INTERVAL, ClientEvent.GIVE_ONLINE_GIFT);
        }

        /// <summary>
        /// Выдает награду за нахождение в игре
        /// </summary>
        private void OnGiveOnlineGift(Client player, object[] args) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            var lastInterval = (int) player.getData(PlayerData.GIFT_INTERVAL);
            player.resetData(PlayerData.GIFT_INTERVAL);
            var reward = playerInfo.Level * GetCoef(lastInterval);
            _playerInfoManager.SetBalance(player, reward, true);
            API.sendNotificationToPlayer(player, "~b~Выдана награда за нахождение в игре");
            var interval = GetGiveTime(lastInterval);
            player.setData(PlayerData.GIFT_INTERVAL, interval);
            API.triggerClientEvent(player, ServerEvent.SET_GIFTS_TIMER, interval, ClientEvent.GIVE_ONLINE_GIFT);
        }

        /// <summary>
        /// Возвращает коэффициент награды
        /// </summary>
        private static int GetCoef(int interval) {
            switch (interval) {
                case FIRST_GIFT_INTERVAL:
                    return 4;
                case SECOND_GIFT_INTERVAL:
                    return 20;
                case LAST_GIFT_INTERVAL:
                    return 100;
                default:
                    throw new ArgumentException("Неизвестный интервал!");
            }
        }

        /// <summary>
        /// Возвращает следующий интервал
        /// </summary>
        private static int GetGiveTime(int lastInterval) {
            if (lastInterval == FIRST_GIFT_INTERVAL) {
                return SECOND_GIFT_INTERVAL;
            }
            return LAST_GIFT_INTERVAL;
        }
    }
}