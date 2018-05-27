using System.Linq;
using gta_mp_server.Constant;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.NPC.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;

namespace gta_mp_server.Managers.NPC {
    internal class ParkingNpc : Script, INpc {
        private readonly IPointCreator _pointCreator;
        private readonly IVehicleInfoManager _vehicleInfoManager;

        public ParkingNpc() {}
        public ParkingNpc(IPointCreator pointCreator, IVehicleInfoManager vehicleInfoManager) {
            _pointCreator = pointCreator;
            _vehicleInfoManager = vehicleInfoManager;
        }

        /// <summary>
        /// Создать нпс парковки
        /// </summary>
        public void Initialize() {
            _pointCreator.CreateBlip(MainPosition.Parking, 524, 41, name: "Главная парковка");
            var ped = _pointCreator.CreatePed(
                PedHash.TrafficWarden, "Парковщик Себастьян",MainPosition.Parking,
                new Vector3(0.00, 0.00, 84.02), new Vector3(-346.20, -822.24, 30.64), Colors.VividCyan
            );
            ped.ColShape.onEntityEnterColShape += (shape, entity) => PlayerHelper.ProcessAction(entity, PlayerComeToNpc);
            ped.ColShape.onEntityExitColShape += (shape, entity) => PlayerHelper.ProcessAction(entity, PlayerAwayFromNpc);
        }

        /// <summary>
        /// Игрок подошел к нпц
        /// </summary>
        private void PlayerComeToNpc(Client player) {
            var vehicles = _vehicleInfoManager.GetPlayerVehicles(player).Where(e => e.HouseId == Validator.INVALID_ID && !e.OnParkingFine);
            API.triggerClientEvent(player, ServerEvent.SHOW_PARKING_MENU, JsonConvert.SerializeObject(vehicles));
        }

        /// <summary>
        /// Игрок отошел от нпц
        /// </summary>
        private void PlayerAwayFromNpc(Client player) {
            API.triggerClientEvent(player, ServerEvent.HIDE_PARKING_MENU);
        }
    }
}