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
    /// Нпс погрузчиков
    /// </summary>
    internal class ForkliftNpc : Script, INpc {
        private readonly IPointCreator _pointCreator;

        public ForkliftNpc() { }
        public ForkliftNpc(IPointCreator pointCreator) {
            _pointCreator = pointCreator;
        }

        /// <summary>
        /// Создать нпс
        /// </summary>
        public void Initialize() {
            var ped = _pointCreator.CreatePed(
                PedHash.Gardener01SMM, "Прораб Боб", MainPosition.TextileMill, 
                new Vector3(0, 0, -90), new Vector3(770.95, -1318.42, 25.33), Colors.Yellow
            );
            ped.ColShape.onEntityEnterColShape += OnEntityEnterColShape;
            ped.ColShape.onEntityExitColShape += OnEntityExitColShape;
        }

        /// <summary>
        /// Обработчик входа в маркер
        /// </summary>
        private void OnEntityEnterColShape(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.SHOW_FORKLIFT_MENU);
        }

        /// <summary>
        /// Обработчик выхода из маркера
        /// </summary>
        private void OnEntityExitColShape(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_FORKLIFT_MENU);
        }
    }
}