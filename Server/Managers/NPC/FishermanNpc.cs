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
    /// Нпс работы рыбаков
    /// </summary>
    internal class FishermanNpc : Script, INpc {
        private readonly IPointCreator _pointCreator;

        public FishermanNpc() {}
        public FishermanNpc(IPointCreator pointCreator) {
            _pointCreator = pointCreator;
        }

        /// <summary>
        /// Создать нпс
        /// </summary>
        public void Initialize() {
            var point = _pointCreator.CreatePed(
                PedHash.PrologueHostage01, "Рыбак Сьюзен", MainPosition.FishingVillage, 
                new Vector3(0.00, 0.00, -51.61), new Vector3(1300.68, 4321.40, 37.35), Colors.VividCyan
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
            API.triggerClientEvent(player, ServerEvent.SHOW_FISHERMAN_MENU);
        }

        /// <summary>
        /// Обработчик выхода из маркера
        /// </summary>
        private void OnEntityExitColShape(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_FISHERMAN_MENU);
        }
    }
}