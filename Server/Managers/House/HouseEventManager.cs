using System.Linq;
using gta_mp_database.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.House.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Shared;
using Newtonsoft.Json;

namespace gta_mp_server.Managers.House {
    /// <summary>
    /// Обработчик действий дома
    /// </summary>
    internal class HouseEventManager : Script, IHouseEventManager {
        private readonly IPlayerInfoManager _playerInfoManager;

        public HouseEventManager() {}
        public HouseEventManager(IPlayerInfoManager playerInfoManager) {
            _playerInfoManager = playerInfoManager;
        }

        /// <summary>
        /// Игрок подошел ко входу
        /// </summary>
        public void OnPlayerWentToEnter(NetHandle entity, long houseId) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            var house = ServerState.Houses[houseId];
            var playerId = _playerInfoManager.GetInfo(player).AccountId;
            var houseState = HouseHelper.GetHouseState(house.OwnerId, playerId);
            API.triggerClientEvent(player, ServerEvent.SHOW_HOUSE_MENU, (int) houseState, JsonConvert.SerializeObject(house));
        }

        /// <summary>
        /// Игрок отошел от входа / выхода
        /// </summary>
        public void OnPlayerAway(NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player, true)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_HOUSE_MENU);
        }

        /// <summary>
        /// Обработчик гардероба
        /// </summary>
        public void OnPlayerEnterWardrobe(NetHandle entity, HouseType type) {
            var player = API.getPlayerFromHandle(entity);
            var dressingRoom = HousesPositionsGetter.DressingRoom[type];
            var clothes = _playerInfoManager.GetInfo(player).Clothes;
            API.triggerClientEvent(
                player, ServerEvent.SHOW_CLOTHES_MENU, 0, JsonConvert.SerializeObject(dressingRoom),
                JsonConvert.SerializeObject(clothes), (int) Validator.INVALID_ID
            );
        }

        /// <summary>
        /// Обработчик хранилища
        /// </summary>
        public void OnPlayerEnterStorage(NetHandle entity, HouseType type) {
            var player = API.getPlayerFromHandle(entity);
            var playerInfo = _playerInfoManager.GetInfo(player);
            var inventory = playerInfo.Inventory.Where(e => e.Count > 0 || e.CountInHouse > 0);
            API.triggerClientEvent(player, ServerEvent.SHOW_HOUSE_STORAGE_MENU, JsonConvert.SerializeObject(inventory));
        }

        /// <summary>
        /// Обработчик выхода из дома
        /// </summary>
        public void OnPlayerExit(NetHandle entity, long houseId) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            var house = ServerState.Houses[houseId];
            API.triggerClientEvent(player, ServerEvent.SHOW_HOUSE_MENU, (int) HouseState.Exit, JsonConvert.SerializeObject(house));
        }

        /// <summary>
        /// Обработчик входа в гараж
        /// </summary>
        public void OnPlayerWentToGarageEnter(NetHandle entity, long houseId) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player, true)) {
                return;
            }
            var playerId = _playerInfoManager.GetInfo(player).AccountId;
            var house = ServerState.Houses[houseId];
            if (playerId != house.OwnerId) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.SHOW_HOUSE_MENU, (int) HouseState.GarageEnter, JsonConvert.SerializeObject(house));
        }

        /// <summary>
        /// Обработчик выхода из гаража
        /// </summary>
        public void OnPlayerExitGarage(NetHandle entity, long houseId) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player, true)) {
                return;
            }
            var house = ServerState.Houses[houseId];
            API.triggerClientEvent(player, ServerEvent.SHOW_HOUSE_MENU, (int) HouseState.GarageExit, JsonConvert.SerializeObject(house));
        }
    }
}