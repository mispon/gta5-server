using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_data.Enums;
using gta_mp_database.Providers.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.Auth.Interfaces;
using gta_mp_server.Managers.Interface.Interfaces;
using gta_mp_server.Managers.Player;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Managers.Work.Police.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using PlayerInfo = gta_mp_database.Models.Player.PlayerInfo;

namespace gta_mp_server.Managers.Auth {
    /// <summary>
    /// Логика авторизации
    /// </summary>
    internal class LoginManager : Script, IInterfaceManager {
        internal const string DISABLE_HOTKEYS = "DisableHotkeys";

        private readonly List<string> _admins = new List<string> {"Pinbaxter", "Caesium666", "RandowGM"};

        private readonly IAccountsProvider _accountsProvider;
        private readonly IPlayersProvider _playersProvider;
        private readonly IPlayersAppearanceProvider _playersAppearanceProvider;
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IVehicleManager _vehicleManager;
        private readonly IVehicleInfoManager _vehicleInfoManager;
        private readonly IJailManager _jailManager;
        private readonly ICreatingCharManager _creatingCharManager;
        private readonly IGtaCharacter _gtaCharacter;
        private readonly IInventoryManager _inventoryManager;
        private readonly IGiftsManager _giftsManager;

        public LoginManager() {}
        public LoginManager(
            IAccountsProvider accountsProvider,
            IPlayersProvider playersProvider,
            IPlayersAppearanceProvider playersAppearanceProvider,
            IPlayerInfoManager playerInfoManager,
            IVehicleManager vehicleManager,
            IVehicleInfoManager vehicleInfoManager,
            IJailManager jailManager,
            ICreatingCharManager creatingCharManager,
            IGtaCharacter gtaCharacter,
            IInventoryManager inventoryManager,
            IGiftsManager giftsManager) {
            _accountsProvider = accountsProvider;
            _playersProvider = playersProvider;
            _playerInfoManager = playerInfoManager;
            _vehicleManager = vehicleManager;
            _vehicleInfoManager = vehicleInfoManager;
            _playersAppearanceProvider = playersAppearanceProvider;
            _jailManager = jailManager;
            _creatingCharManager = creatingCharManager;
            _gtaCharacter = gtaCharacter;
            _inventoryManager = inventoryManager;
            _giftsManager = giftsManager;
        }

        /// <summary>
        /// Инициализировать интерфейсные обработчики
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.PLAYER_LOGIN, PlayerLogin);
        }

        /// <summary>
        /// Обработчик логина игрока
        /// </summary>
        private void PlayerLogin(Client player, object[] args) {
            //var accesses = _fakeLogins[player.socialClubName];
            //if (!_admins.Contains(player.socialClubName)) {
            //    player.kick("На сервере ведутся технические работы. Приносим извенения!");
            //    return;
            //}
            var email = args[0].ToString();
            var password = args[1].ToString();
            var account = _accountsProvider.Get(email, password);
            if (account == null) {
                API.triggerClientEvent(player, ServerEvent.BAD_LOGIN);
                return;
            }
            var playerInfo = _playersProvider.GetInfo(account.Id);
            _playerInfoManager.Add(player, playerInfo);
            API.setPlayerHealth(player, playerInfo.Health);
            API.triggerClientEvent(player, ServerEvent.HIDE_AUTH);
            if (!string.IsNullOrEmpty(playerInfo.Name)) {
                ProcessLogin(player, playerInfo);
                LoadPlayerVehicles(player, playerInfo);
                _inventoryManager.EquipWeapon(player);
                _giftsManager.ProcessDaysGift(player, account);
            }
            else {
                API.setEntityDimension(player, (int) -playerInfo.AccountId);
                _creatingCharManager.ShowCreator(player);
            }
            account.LastLogin = DateTime.Now;
            _accountsProvider.Update(account);
            _giftsManager.StartGiftsTimer(player);
        }

        /// <summary>
        /// Обработывает успешную авторизацию игрока
        /// </summary>
        private void ProcessLogin(Client player, PlayerInfo playerInfo) {
            var appearance = _playersAppearanceProvider.Get(playerInfo.AccountId);
            playerInfo.Skin = (Skin) appearance.Skin;
            playerInfo.Appearance = appearance;
            API.triggerClientEvent(player, ServerEvent.SHOW_INTERFACE);
            _gtaCharacter.SetAppearance(player, appearance);
            _gtaCharacter.SetClothes(player, playerInfo.Clothes.Where(e => e.OnPlayer).ToList());
            SetPosition(player, playerInfo);
            PlayerManager.SetPlayerName(player, playerInfo);
            player.resetSyncedData(DISABLE_HOTKEYS);
            if (_admins.Contains(player.socialClubName)) {
                player.setSyncedData("IsAdmin", true);
            }
            if (playerInfo.PhoneNumber != 0) {
                player.setSyncedData("HasPhone", true);
            }
            player.freeze(false);
            _playerInfoManager.RefreshUI(player, playerInfo);
            API.sendNativeToPlayer(player, Hash.DISPLAY_RADAR, true);
            API.sendChatMessageToAll($"~g~[Информация]: {playerInfo.Name} зашел на сервер. Онлайн: {ServerState.Players.Count}");
        }

        /// <summary>
        /// Поместить игрока на позицию до выхода
        /// </summary>
        private void SetPosition(Client player, PlayerInfo info) {
            var lastPosition = !string.IsNullOrEmpty(info.LastPosition) ? PositionConverter.ToVector3(info.LastPosition) : null;
            if (info.Wanted.JailTime > 0) {
                _jailManager.SetInJail(player, lastPosition);
            }
            else {
                API.setEntityPosition(player, lastPosition ?? MainPosition.StartSpawn);
            }
            API.setEntityDimension(player, info.Dimension);
        }

        /// <summary>
        /// Загрузить транспорт игрока
        /// </summary>
        private void LoadPlayerVehicles(Client player, PlayerInfo playerInfo) {
            var vehicles = _vehicleInfoManager.GetPlayerVehicles(playerInfo.AccountId);
            foreach (var vehicle in vehicles) {
                if (vehicle.IsSpawned) {
                    var instance = _vehicleManager.GetInstanceById(vehicle.Id);
                    if (instance != null) {
                        vehicle.Instance = instance;
                    }
                    else {
                        vehicle.IsSpawned = false;
                    }
                }
                _vehicleInfoManager.SetInfo(player, vehicle);
            }
        }

        /// <summary>
        /// Для быстрого логина на тестовом
        /// </summary>
        private static readonly Dictionary<string, string[]> _fakeLogins = new Dictionary<string, string[]> {
            ["Pinbaxter"] = new[] { "dev@dev.com", "abc123" },
            ["Caesium666"] = new[] {"caesium2000@gmail.com", "147369"},
            ["RandowGM"] = new[] { "randowgm@gmail.com", "123456"}
        };
    }
}