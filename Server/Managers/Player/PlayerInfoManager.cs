using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using gta_mp_data.Enums;
using gta_mp_database.Models.Player;
using gta_mp_database.Providers.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
// ReSharper disable All

namespace gta_mp_server.Managers.Player {
    /// <summary>
    /// Логика работы с общими данными игрока
    /// </summary>
    internal class PlayerInfoManager : Script, IPlayerInfoManager {
        internal const string ID_KEY = "PlayerId";
        private const int MAX_PLAYER_LEVEL = 15;
        private const int REFERAL_REWARD = 10000;

        private readonly IAccountsProvider _accountsProvider;
        private readonly IPlayersProvider _playersProvider;
        private readonly IVehiclesProvider _vehiclesProvider;
        private readonly IGtaCharacter _gtaCharacter;
        private readonly IOpportunitiesNotifier _opportunitiesNotifier;

        public PlayerInfoManager() {}
        public PlayerInfoManager(IAccountsProvider accountsProvider, IPlayersProvider playersProvider, IVehiclesProvider vehiclesProvider,
            IGtaCharacter gtaCharacter, IOpportunitiesNotifier opportunitiesNotifier) {
            _accountsProvider = accountsProvider;
            _playersProvider = playersProvider;
            _vehiclesProvider = vehiclesProvider;
            _gtaCharacter = gtaCharacter;
            _opportunitiesNotifier = opportunitiesNotifier;
        }

        /// <summary>
        /// Добавляет игрока в список игроков онлайн
        /// </summary>
        public void Add(Client player, PlayerInfo playerInfo) {
            player.setData(ID_KEY, playerInfo.AccountId);
            ServerState.Players.TryAdd(player, playerInfo);
        }

        /// <summary>
        /// Удаляет игрока из списка
        /// </summary>
        public void Remove(Client player) {
            ServerState.Players.TryRemove(player, out var _);
        }

        /// <summary>
        /// Возвращает игрока и его данные по идентификатору
        /// </summary>
        public PlayerWithData GetWithData(long accountId, bool needFromDb = true) {
            var player = API.getAllPlayers().FirstOrDefault(e => e.hasData(ID_KEY) && accountId == (long) e.getData(ID_KEY));
            if (PlayerHelper.PlayerCorrect(player, true)) {
                return new PlayerWithData {Player = player, PlayerInfo = GetInfo(player)};
            }
            PlayerInfo playerInfo = null;
            if (needFromDb) {
                playerInfo = _playersProvider.GetInfo(accountId);
                playerInfo.Vehicles = _vehiclesProvider.GetVehicles(accountId).ToDictionary(e => e.Id);
            }
            return new PlayerWithData {Player = null, PlayerInfo = playerInfo};
        }

        /// <summary>
        /// Возвращает данные игрока
        /// </summary>
        public PlayerInfo GetInfo(Client player) {
            return ServerState.Players.TryGetValue(player, out var playerInfo) ? playerInfo : null;
        }

        /// <summary>
        /// Возвращает данные игрока по его идентификатору
        /// </summary>
        public PlayerInfo Get(long accontId) {
            return _playersProvider.GetInfo(accontId);
        }

        /// <summary>
        /// Возвращает всех игроков, соответствующих условию
        /// </summary>
        public Dictionary<Client, PlayerInfo> GetWhere(Func<PlayerInfo, bool> predicate) {
            return ServerState.Players
                .Where(e => e.Key != null && API.isPlayerConnected(e.Key) && predicate(e.Value))
                .ToDictionary(e => e.Key, e => e.Value);
        }

        /// <summary>
        /// Записать данные игрока
        /// </summary>
        public void Set(PlayerInfo playerInfo) {
            _playersProvider.SetInfo(playerInfo);
        }

        /// <summary>
        /// Обновляет данные игрока
        /// </summary>
        public void RefreshUI(Client player, PlayerInfo info) {
            API.triggerClientEvent(player, ServerEvent.UPDATE_INFO,
                (int) info.Satiety,
                info.Balance,
                GetExpPercent(info.Level, info.Experience)
            );
        }

        /// <summary>
        /// Записывает данные игрока
        /// </summary>
        public void SetExperience(Client player, int exp) {
            var playerInfo = ServerState.Players[player];
            if (playerInfo.Level == MAX_PLAYER_LEVEL || exp == 0) {
                return;
            }
            if (playerInfo.IsPremium()) {
                exp += (int) (exp * 0.5);
            }
            var expToLevelUp = _experienceByLevel[playerInfo.Level];
            var newExp = playerInfo.Experience + exp;
            API.sendNotificationToPlayer(player, $"~g~Получено {exp} ед. опыта");
            if (newExp >= expToLevelUp) {
                playerInfo.Level += 1;
                playerInfo.Experience = newExp - expToLevelUp;
                API.sendNotificationToPlayer(player, $"~b~Вы достигли {playerInfo.Level}-го уровня!");
                _opportunitiesNotifier.Notify(player, playerInfo.Level);
                if (playerInfo.Level == 3) {
                    GiveReferalReward(player, playerInfo);
                }
            }
            else {
                playerInfo.Experience += exp;
            }
            RefreshUI(player, playerInfo);
        }

        /// <summary>
        /// Обновляет баланс
        /// </summary>
        public void SetBalance(Client player, int balance, bool considerPremium = false) {
            ServerState.Players.TryGetValue(player, out var playerInfo);
            if (considerPremium && playerInfo.IsPremium()) {
                balance += (int) (balance * 0.5);
            }
            playerInfo.Balance += balance;
            ServerState.Players.TryUpdate(player, playerInfo, playerInfo);
            API.sendNotificationToPlayer(player, $"~g~Получено {balance}$");
            API.triggerClientEvent(
                player, ServerEvent.UPDATE_INFO,
                (int) playerInfo.Satiety,
                playerInfo.Balance,
                GetExpPercent(playerInfo.Level, playerInfo.Experience)
            );
        }

        /// <summary>
        /// Очистить уровень розыска игрока
        /// </summary>
        public void ClearWanted(Client player) {
            var info = GetInfo(player);
            info.Wanted.Beatings = 0;
            info.Wanted.Kills = 0;
            RefreshUI(player, info);
        }

        /// <summary>
        /// Записывает измерение игрока
        /// </summary>
        public void SetDimension(Client player, int dimension) {
            var playerInfo = GetInfo(player);
            playerInfo.Dimension = dimension;
            RefreshUI(player, playerInfo);
            API.setEntityDimension(player, dimension);
            _playersProvider.SetDimension(playerInfo.AccountId, dimension);
        }

        /// <summary>
        /// Проверяет пол игрока
        /// </summary>
        public bool IsMale(Client player) {
            return GetInfo(player).Skin == Skin.Male;
        }

        /// <summary>
        /// Одеть одежду игрока
        /// </summary>
        public void SetPlayerClothes(Client player, bool withAppearanse = false) {
            if (!API.isPlayerConnected(player)) {
                return;
            }
            if (withAppearanse) {
                var playerInfo = GetInfo(player);
                API.setPlayerSkin(player, (PedHash) playerInfo.Skin);
                _gtaCharacter.SetAppearance(player, playerInfo.Appearance);
            }
            var clothes = GetInfo(player).Clothes.Where(e => e.OnPlayer).ToList();
            _gtaCharacter.SetClothes(player, clothes);
        }

        /// <summary>
        /// Возвращает игрока по номеру телефона
        /// </summary>
        public Client GetByNumber(int number) {
            return ServerState.Players.FirstOrDefault(e => e.Value.PhoneNumber == number).Key;
        }

        /// <summary>
        /// Записывает номер телефона
        /// </summary>
        public void SetPhoneNumber(Client player) {
            var playerInfo = GetInfo(player);
            playerInfo.PhoneNumber = _playersProvider.GetPhoneNumber();
        }

        /// <summary>
        /// Возвращает процентное соотношение опыта
        /// </summary>
        private int GetExpPercent(int level, int exp) {
            if (exp < 0) {
                return 0;
            }
            var expToUp = _experienceByLevel[level];
            var percent = (double) exp / expToUp;
            return (int) (percent * 100);
        }

        /// <summary>
        /// Выдает награду игроку за приведенного реферала
        /// </summary>
        private void GiveReferalReward(Client player, PlayerInfo playerInfo) {
            var referal = _accountsProvider.GetFriendReferal(playerInfo.AccountId);
            if (string.IsNullOrEmpty(referal)) {
                return;
            }
            var referalPlayer = API.getAllPlayers().FirstOrDefault(e => e.name == referal);
            if (referalPlayer != null) {
                SetBalance(referalPlayer, REFERAL_REWARD, true);
                API.sendNotificationToPlayer(referalPlayer, "~b~Ваш друг достиг третьего уровня!");
            }
            else {
                _playersProvider.SetReferalReward(referal, REFERAL_REWARD);
            }
        }

        /// <summary>
        /// Уровень и кол-во опыта до следующего уровня
        /// </summary>
        private readonly Dictionary<int, int> _experienceByLevel = new Dictionary<int, int> {
            { 1, 100 },
            { 2, 150 },
            { 3, 225 },
            { 4, 350 },
            { 5, 550 },
            { 6, 830 },
            { 7, 1250 },
            { 8, 1900 },
            { 9, 2850 },
            { 10, 4300 },
            { 11, 6000 },
            { 12, 8400 },
            { 13, 11800 },
            { 14, 16550 },
            { 15, 23200 }
            //{ 16, 14700 },
            //{ 17, 18375 },
            //{ 18, 22968 },
            //{ 19, 28710 },
            //{ 20, 0 }
        };
    }
}