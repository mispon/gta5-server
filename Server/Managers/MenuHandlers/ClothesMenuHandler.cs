using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Models.Shops;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;

namespace gta_mp_server.Managers.MenuHandlers {
    /// <summary>
    /// Обработчик меню магазина одежды
    /// </summary>
    internal class ClothesMenuHandler : Script, IMenu {
        internal const string IN_DRESSING_ROOM = "InDressingRoom";

        private readonly IPlayerInfoManager _playerInfoManager;

        public ClothesMenuHandler() { }
        public ClothesMenuHandler(IPlayerInfoManager playerInfoManager) {
            _playerInfoManager = playerInfoManager;
        }

        /// <summary>
        /// Инициализировать обработчик меню
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.GO_TO_DRESSING_ROOM, GoToDressingRoom);
            ClientEventHandler.Add(ClientEvent.EXIT_FROM_DRESSING_ROOM, ExitFromDressingRoom);
        }

        /// <summary>
        /// Перенести игрока в примерочную
        /// </summary>
        private void GoToDressingRoom(Client player, object[] args) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            var dressingRoom = JsonConvert.DeserializeObject<DressingRoomPositions>(args[0].ToString());
            player.setData(PlayerData.LAST_POSITION, player.position);
            player.setData(PlayerData.LAST_DIMENSION, player.dimension);
            API.setEntityRotation(player, dressingRoom.Rotation);
            API.setEntityPosition(player, dressingRoom.Position);
            API.setEntityDimension(player, (int) -playerInfo.AccountId);
            player.freeze(true);
            player.setSyncedData(IN_DRESSING_ROOM, true);   
            API.triggerClientEvent(player, ServerEvent.SHOW_CLOTHES_LIST, args[0]);
            API.triggerClientEvent(player, ServerEvent.SHOW_HINT, "Если случайно закрылось меню, нажмите О, чтобы снова открыть", 120);
        }
        
        /// <summary>
        /// Выйти из примерчной
        /// </summary>
        private void ExitFromDressingRoom(Client player, object[] args) {
            var lastPosition = (Vector3) player.getData(PlayerData.LAST_POSITION);
            var lastDimension = (int) player.getData(PlayerData.LAST_DIMENSION);
            API.setEntityDimension(player, lastDimension);
            API.setEntityPosition(player, lastPosition);
            player.resetSyncedData(IN_DRESSING_ROOM);
            player.freeze(false);
            API.triggerClientEvent(player, ServerEvent.HIDE_HINT);
        }
    }
}