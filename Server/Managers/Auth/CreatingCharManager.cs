using System.Collections.Generic;
using System.Linq;
using gta_mp_data.Enums;
using gta_mp_database.Models.Player;
using gta_mp_database.Providers.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.Auth.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;

namespace gta_mp_server.Managers.Auth {
    /// <summary>
    /// Логика создания персонажа
    /// </summary>
    internal class CreatingCharManager : Script, ICreatingCharManager {
        private readonly IPlayersProvider _playersProvider;
        private readonly IPlayersAppearanceProvider _playersAppearanceProvider;
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IGtaCharacter _gtaCharacter;
        private readonly IGiftsManager _giftsManager;
        
        public CreatingCharManager() {}
        public CreatingCharManager(IPlayersProvider playersProvider, IPlayersAppearanceProvider playersAppearanceProvider,
            IPlayerInfoManager playerInfoManager, IGtaCharacter gtaCharacter, IGiftsManager giftsManager) {
            _playersProvider = playersProvider;
            _playersAppearanceProvider = playersAppearanceProvider;
            _playerInfoManager = playerInfoManager;
            _gtaCharacter = gtaCharacter;
            _giftsManager = giftsManager;

            ClientEventHandler.Add(ClientEvent.SAVE_CHARACTER, SaveCharacter);
        }

        /// <summary>
        /// Показать окно создания персонажа
        /// </summary>
        public void ShowCreator(Client player) {
            API.triggerClientEvent(player, ServerEvent.SHOW_CHAR_CREATE);
            API.setEntityRotation(player, new Vector3(0.00, 0.00, 180.00));
            API.setEntityPosition(player, new Vector3(402.89, -996.78, -99.00));
            player.freeze(true);
        }

        /// <summary>
        /// Сохранить кастомизацию персонажа
        /// </summary>
        private void SaveCharacter(Client player, object[] args) {
            var name = args[1].ToString();
            var surname = args[2].ToString();
            var playerInfo = _playerInfoManager.GetInfo(player);
            var playerName = $"{GetPretty(name)} {GetPretty(surname)}";
            var nameSuccess = _playersProvider.SetName(playerInfo.AccountId, playerName);
            if (!nameSuccess) {
                API.triggerClientEvent(player, ServerEvent.NAME_ALREADY_EXIST);
                return;
            }
            var character = JsonConvert.DeserializeObject<PlayerAppearance>(args[0].ToString());
            _gtaCharacter.SetAppearance(player, character);
            SetPlayerInfo(player, playerInfo, playerName, character.Skin);
            _playersAppearanceProvider.Save(playerInfo.AccountId, character);
            SetPlayerInStartLocation(player);
            API.sendNativeToPlayer(player, Hash.DISPLAY_RADAR, true);
        }

        /// <summary>
        /// Устанавливает данные игрока
        /// </summary>
        private void SetPlayerInfo(Client player, PlayerInfo playerInfo, string playerName, int skin) {
            API.setPlayerName(player, playerName);
            playerInfo.Name = playerName;
            playerInfo.Skin = (Skin) skin;
            var clothes = CreateDefaultClothes(playerInfo.Skin);
            playerInfo.Clothes.AddRange(clothes);
            _gtaCharacter.SetClothes(player, clothes);
            _playerInfoManager.RefreshUI(player, playerInfo);
        }

        /// <summary>
        /// Создает начальную одежду
        /// </summary>
        private static ICollection<ClothesModel> CreateDefaultClothes(Skin skin) {
            var isMale = skin == Skin.Male;
            return new List<ClothesModel> {
                new ClothesModel {
                    Slot = 11, Variation = 0, Torso = 0, Undershirt = isMale ? 57 : 2, Texture = 0,
                    Textures = new List<int>{0}, IsClothes = true, OnPlayer = true
                },
                new ClothesModel {Slot = 4, Variation = 0, Texture = 0, Textures = new List<int>{0}, IsClothes = true, OnPlayer = true},
                new ClothesModel {Slot = 6, Variation = isMale ? 1 : 3, Texture = 0, Textures = new List<int>{0}, IsClothes = true, OnPlayer = true},
                new ClothesModel {Slot = 0, Variation = isMale ? 11 : 120, Texture = 0, Textures = new List<int>{0}, IsClothes = false, OnPlayer = true}
            };
        }

        /// <summary>
        /// Поместить игрока в начальную локацию
        /// </summary>
        private void SetPlayerInStartLocation(Client player) {
            API.triggerClientEvent(player, ServerEvent.HIDE_CHAR_CREATE);
            API.triggerClientEvent(player, ServerEvent.SHOW_INTERFACE);
            API.setEntityDimension(player, 0);
            API.setEntityPosition(player, MainPosition.StartSpawn);
            player.resetSyncedData(LoginManager.DISABLE_HOTKEYS);
            player.freeze(false);
            _giftsManager.StartGiftsTimer(player);
        }

        /// <summary>
        /// Форматирует имя к стандартному виду
        /// </summary>
        private static string GetPretty(string value) {
            var prettyView = value.ToLower();
            return prettyView.First().ToString().ToUpper() + prettyView.Substring(1);
        }
    }
}