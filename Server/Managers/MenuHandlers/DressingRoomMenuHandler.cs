using System.Collections.Generic;
using System.Linq;
using gta_mp_database.Models.Player;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using Newtonsoft.Json;

namespace gta_mp_server.Managers.MenuHandlers {
    /// <summary>
    /// Обработчик меню примерочной
    /// </summary>
    internal class DressingRoomMenuHandler : Script, IMenu {
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IGtaCharacter _gtaCharacter;
        private readonly IClanManager _clanManager;

        public DressingRoomMenuHandler() {}

        public DressingRoomMenuHandler(IPlayerInfoManager playerInfoManager, IGtaCharacter gtaCharacter, IClanManager clanManager) {
            _playerInfoManager = playerInfoManager;
            _gtaCharacter = gtaCharacter;
            _clanManager = clanManager;
        }

        /// <summary>
        /// Инициализировать обработчик меню
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.DRESS_OR_BUY_GOOD, DressOrBuyGood);
            ClientEventHandler.Add(ClientEvent.DRESS_PLAYER_CLOTHES, DressPlayerClothes);
        }

        /// <summary>
        /// Одеть или купить вещь
        /// </summary>
        private void DressOrBuyGood(Client player, object[] args) {
            var good = JsonConvert.DeserializeObject<ClothesModel>(args[0].ToString());
            var playerInfo = _playerInfoManager.GetInfo(player);
            var playerGood = playerInfo.Clothes.FirstOrDefault(e => e.Slot == good.Slot && e.Variation == good.Variation);
            if (playerGood == null) {
                if (!BuyGood(player, playerInfo, good)) {
                    return;
                }
                playerGood = CreateGood(good);
                playerInfo.Clothes.Add(playerGood);
            }
            else if (!playerGood.Textures.Contains(good.Texture)) {
                if (!BuyGood(player, playerInfo, good)) {
                    return;
                }
                playerGood.Textures.Add(good.Texture);
            }
            ChangeGood(playerInfo, playerGood, good.Texture);
            _playerInfoManager.RefreshUI(player, playerInfo);
            _gtaCharacter.SetClothes(player, playerGood);
            var district = (int) args[1];
            if (district != Validator.INVALID_ID) {
                _clanManager.ReplenishClanBalance(district, good.Price);
            }
        }

        /// <summary>
        /// Создать новую одежду
        /// </summary>
        private static ClothesModel CreateGood(ClothesModel good) {
            return new ClothesModel {
                Variation = good.Variation,
                Slot = good.Slot,
                Torso = good.Torso,
                Undershirt = good.Undershirt,
                Textures = new List<int> {good.Texture},
                IsClothes = good.IsClothes
            };
        }

        /// <summary>
        /// Купить одежду
        /// </summary>
        public bool BuyGood(Client player, PlayerInfo playerInfo, ClothesModel good) {
            if (playerInfo.Balance < good.Price) {
                API.sendNotificationToPlayer(player, "~r~Недостаточно средств", true);
                return false;
            }
            playerInfo.Balance -= good.Price;
            return true;
        }

        /// <summary>
        /// Сменить одежду
        /// </summary>
        private static void ChangeGood(PlayerInfo playerInfo, ClothesModel playerGood, int texture) {
            var oldGoods = playerInfo.Clothes.Where(e => e.Slot == playerGood.Slot).ToList();
            foreach (var oldGood in oldGoods) {
                oldGood.OnPlayer = false;
            }
            playerGood.Texture = texture;
            playerGood.OnPlayer = true;
        }

        /// <summary>
        /// Одеть текущую одежду игрока
        /// </summary>
        private void DressPlayerClothes(Client player, object[] args) {
            _playerInfoManager.SetPlayerClothes(player);
        }
    }
}