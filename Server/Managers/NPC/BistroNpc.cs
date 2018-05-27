using gta_mp_server.Constant;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.NPC.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.NPC {
    /// <summary>
    /// Нпс ресторана быстрого питания
    /// </summary>
    internal class BistroNpc : Script, INpc {
        private const string NAME = "Родригез";

        private readonly IPointCreator _pointCreator;

        public BistroNpc() {}
        public BistroNpc(IPointCreator pointCreator) {
            _pointCreator = pointCreator;
        }

        /// <summary>
        /// Создать нпс
        /// </summary>
        public void Initialize() {
            var ped = _pointCreator.CreatePed(
                PedHash.BurgerDrug, NAME, MainPosition.Bistro, new Vector3(0.00, 0.00, 52.77),
                new Vector3(154.27, -1431.29, 28.36), Colors.VividCyan
            );
            ped.ColShape.onEntityEnterColShape += PlayerComeToNpc;
            ped.ColShape.onEntityExitColShape += PlayerAwayFromNpc;
        }

        /// <summary>
        /// Игрок подошел к нпс
        /// </summary>
        private void PlayerComeToNpc(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.SHOW_BISTRO_MENU);
        }

        /// <summary>
        /// Игрок отошел от Npc
        /// </summary>
        private void PlayerAwayFromNpc(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_BISTRO_MENU);
        }
    }
}