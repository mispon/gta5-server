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
    /// Работа в такси
    /// </summary>
    internal class TaxiDriverNpc : Script, INpc {
        private readonly IPointCreator _pointCreator;

        public TaxiDriverNpc() {}
        public TaxiDriverNpc(IPointCreator pointCreator) {
            _pointCreator = pointCreator;
        }

        /// <summary>
        /// Создать нпс
        /// </summary>
        public void Initialize() {
            _pointCreator.CreateBlip(MainPosition.TaxiDriver, 56, 46, name: "Таксопарк");
            var point = _pointCreator.CreatePed(
                PedHash.ArmLieut01GMM, "Луи", MainPosition.TaxiDriver, new Vector3(0, 0, 80),
                new Vector3(211.36, -808.47, 29.93), Colors.VividCyan
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
            API.triggerClientEvent(player, ServerEvent.SHOW_SIEMON_MENU);
        }

        /// <summary>
        /// Обработчик выхода из маркера
        /// </summary>
        private void OnEntityExitColShape(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_SIEMON_MENU);
        }
    }
}