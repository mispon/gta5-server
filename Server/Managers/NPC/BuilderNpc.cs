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
    /// Нпс грузчиков высокого уровня
    /// </summary>
    internal class BuilderNpc : Script, INpc {
        private readonly IPointCreator _pointCreator;

        public BuilderNpc() {}
        public BuilderNpc(IPointCreator pointCreator) {
            _pointCreator = pointCreator;
        }

        /// <summary>
        /// Создать нпс
        /// </summary>
        public void Initialize() {
            var pedMarkerPos = new Vector3(-509.62, -954.46, 22.79);
            var point = _pointCreator.CreatePed(
                PedHash.Dockwork01SMM, "Прораб Стивен", MainPosition.Building, new Vector3(0, 0, -149.1), pedMarkerPos, Colors.VividCyan
            );
            point.ColShape.onEntityEnterColShape += OnEntityEnterColShape;
            point.ColShape.onEntityExitColShape += OnEntityExitColShape;
        }

        /// <summary>
        /// Обработчик входа в маркер
        /// </summary>
        private void OnEntityEnterColShape(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.SHOW_BUILDER_MENU);
        }

        /// <summary>
        /// Обработчик выхода из маркера
        /// </summary>
        private void OnEntityExitColShape(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_BUILDER_MENU);
        }
    }
}