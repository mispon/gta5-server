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
    /// Испектор автошколы
    /// </summary>
    internal class DrivingSchoolNpc : Script, INpc {
        private readonly IPointCreator _pointCreator;

        public DrivingSchoolNpc() { }
        public DrivingSchoolNpc(IPointCreator pointCreator) {
            _pointCreator = pointCreator;
        }

        /// <summary>
        /// Создать нпс
        /// </summary>
        public void Initialize() {
            var point = _pointCreator.CreatePed(
                PedHash.Andreas, "Инспектор Андреас", new Vector3(1156.85, -3198.15, -39.01), 
                new Vector3(0.00, 0.00, -87.29), new Vector3(1157.65, -3198.13, -39.81), Colors.VividCyan
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
            API.triggerClientEvent(player, ServerEvent.SHOW_ANDREAS_MENU);
        }

        /// <summary>
        /// Обработчик выхода из маркера
        /// </summary>
        private void OnEntityExitColShape(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_ANDREAS_MENU);
        }
    }
}