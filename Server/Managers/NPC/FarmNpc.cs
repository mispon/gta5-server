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
    /// Нпс Роберт, фермеры начального уровня
    /// </summary>
    internal class FarmNpc : Script, INpc {
        private readonly IPointCreator _pointCreator;

        public FarmNpc() {}
        public FarmNpc(IPointCreator pointCreator) {
            _pointCreator = pointCreator;
        }

        /// <summary>
        /// Создать нпс
        /// </summary>
        public void Initialize() {
            var npc = _pointCreator.CreatePed(
                PedHash.Farmer01AMM, "Фермер Роберт", MainPosition.Farm, 
                new Vector3(0.00, 0.00, 42.24), new Vector3(2931.27, 4625.05, 47.82), Colors.VividCyan
            );
            npc.ColShape.onEntityEnterColShape += OnEntityEnterColShape;
            npc.ColShape.onEntityExitColShape += OnEntityExitColShape;
        }

        /// <summary>
        /// Обработчик входа в маркер
        /// </summary>
        private void OnEntityEnterColShape(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.SHOW_FARM_MENU);
        }

        /// <summary>
        /// Обработчик выхода из маркера
        /// </summary>
        private void OnEntityExitColShape(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_FARM_MENU);
        }
    }
}