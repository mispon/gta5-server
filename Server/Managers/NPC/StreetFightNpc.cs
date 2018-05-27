using gta_mp_server.Constant;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.Fun;
using gta_mp_server.Managers.NPC.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.NPC {
    internal class StreetFightNpc : Script, INpc {
        private readonly IPointCreator _pointCreator;

        public StreetFightNpc() {}
        public StreetFightNpc(IPointCreator pointCreator) {
            _pointCreator = pointCreator;
        }

        /// <summary>
        /// Создать нпс
        /// </summary>
        public void Initialize() {
            _pointCreator.CreateBlip(MainPosition.StreetFights, 311, 49, name: "Уличные драки");
            var ped = _pointCreator.CreatePed(
                PedHash.StrPunk02GMY, "Тайлер", MainPosition.StreetFights,
                new Vector3(0.00, 0.00, 84.50), new Vector3(-22.78, -1228.62, 28.43), Colors.VividCyan
            );
            ped.ColShape.onEntityEnterColShape += (shape, entity) => 
                PlayerHelper.ProcessAction(entity, player => API.triggerClientEvent(player, ServerEvent.SHOW_FIGHT_MENU, StreetFights.Members.Count));
            ped.ColShape.onEntityExitColShape += (shape, entity) =>
                PlayerHelper.ProcessAction(entity, player => API.triggerClientEvent(player, ServerEvent.HIDE_FIGHT_MENU));
        }
    }
}