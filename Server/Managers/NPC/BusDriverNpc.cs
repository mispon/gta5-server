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
    /// Нпс в автопарке
    /// </summary>
    internal class BusDriverNpc : Script, INpc {
        private readonly IPointCreator _pointCreator;

        public BusDriverNpc() { }
        public BusDriverNpc(IPointCreator pointCreator) {
            _pointCreator = pointCreator;
        }

        /// <summary>
        /// Создать нпс
        /// </summary>
        public void Initialize() {
            _pointCreator.CreateBlip(MainPosition.BusDriver, 513, 47, name: "Автобусный парк");
            var point = _pointCreator.CreatePed(
                PedHash.ONeil, "Онэил", MainPosition.BusDriver, new Vector3(0, 0, -100), new Vector3(46.5, -843.66, 29.97), Colors.VividCyan
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
            API.triggerClientEvent(player, ServerEvent.SHOW_ONEIL_MENU);
        }

        /// <summary>
        /// Обработчик выхода из маркера
        /// </summary>
        private void OnEntityExitColShape(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_ONEIL_MENU);
        }
    }
}