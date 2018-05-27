using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Marker = gta_mp_server.Enums.Marker;

namespace gta_mp_server.Managers.Fun {
    /// <summary>
    /// Логика аттракциона прыжков с парашютом
    /// </summary>
    internal class Parachute : Place {
        private const string NAME = "Инструктор";

        private readonly IPointCreator _pointCreator;
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IClanManager _clanManager;

        public Parachute() {}
        public Parachute(IPointCreator pointCreator, IPlayerInfoManager playerInfoManager, IClanManager clanManager) {
            _pointCreator = pointCreator;
            _playerInfoManager = playerInfoManager;
            _clanManager = clanManager;
        }

        /// <summary>
        /// Инизиализировать место прыжков с парашютом
        /// </summary>
        public override void Initialize() {
            var position = new Vector3(-75.68, -826.21, 326.18);
            _pointCreator.CreateBlip(position, 377, 63, scale: 1.3f, name: "Прыжок с парашютом");
            CreateEnters();
            var ped = _pointCreator.CreatePed(
                PedHash.ExArmy01, NAME, position, new Vector3(0.00, 0.00, -15.22),
                new Vector3(-75.47, -825.44, 325.18), Colors.VividCyan
            );
            ped.ColShape.onEntityEnterColShape += (shape, entity) =>
                PlayerHelper.ProcessAction(entity, player => API.triggerClientEvent(player, ServerEvent.SHOW_PARACHUTE_MENU)
            );
            ped.ColShape.onEntityExitColShape += (shape, entity) =>
                PlayerHelper.ProcessAction(entity, player => API.triggerClientEvent(player, ServerEvent.HIDE_PARACHUTE_MENU)
            );
            ClientEventHandler.Add(ClientEvent.BUY_PARACHUTE, BuyParachute);
        }

        /// <summary>
        /// Покупка парашюта
        /// </summary>
        private void BuyParachute(Client player, object[] args) {
            var price = (int) args[0];
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (playerInfo.Balance < price) {
                API.sendNotificationToPlayer(player, "~r~Недостаточно денег", true);
                return;
            }
            playerInfo.Balance -= price;
            _playerInfoManager.RefreshUI(player, playerInfo);
            API.givePlayerWeapon(player, WeaponHash.Parachute, 1, true);
            _clanManager.ReplenishClanBalance(1, price);
            API.sendChatMessageToPlayer(player, $"~b~{NAME}: Нажми ЛЕВУЮ кнопку мыши в полёте, чтобы раскрыть парашют.");
            API.sendChatMessageToPlayer(player, $"~b~{NAME}: Надеюсь тебя не придется соскребать с асфальта!");
            API.triggerClientEvent(player, ServerEvent.HIDE_PARACHUTE_MENU);
        }

        /// <summary>
        /// Инициализировать вход и выход
        /// </summary>
        private void CreateEnters() {
            var enterPosition = new Vector3(-68.86, -801.40, 44.23);
            var roof = new Vector3(-64.36, -820.48, 321.79);
            var enter = _pointCreator.CreateMarker(Marker.UpsideDownCone, enterPosition, Colors.Yellow, 1f);
            enter.ColShape.onEntityEnterColShape += (shape, entity) => PlayerHelper.ProcessAction(entity, player => API.setEntityPosition(player, roof));
            var exitPosition = new Vector3(-67.35, -821.77, 321.29);
            var street = new Vector3(-61.62, -792.95, 44.23);
            var exit = _pointCreator.CreateMarker(Marker.UpsideDownCone, exitPosition, Colors.Yellow, 1f);
            exit.ColShape.onEntityEnterColShape += (shape, entity) => PlayerHelper.ProcessAction(entity, player => API.setEntityPosition(player, street));
        }
    }
}