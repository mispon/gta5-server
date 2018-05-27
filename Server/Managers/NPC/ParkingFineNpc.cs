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
    /// <summary>
    /// Нпс штрафстоянки
    /// </summary>
    internal class ParkingFineNpc: Script, INpc {
        private readonly IPointCreator _pointCreator;
        private readonly IVehicleInfoManager _vehicleInfoManager;

        public ParkingFineNpc() {}
        public ParkingFineNpc(IPointCreator pointCreator, IVehicleInfoManager vehicleInfoManager) {
            _pointCreator = pointCreator;
            _vehicleInfoManager = vehicleInfoManager;
        }

        /// <summary>
        /// Создать нпс
        /// </summary>
        public void Initialize() {
            var ped = _pointCreator.CreatePed(
                PedHash.Ammucity01SMY, "Эвакуаторщик Джереми", MainPosition.ParkingFine,
                new Vector3(0.00, 0.00, -126.44), new Vector3(408.57, -1624.97, 28.39), Colors.VividCyan
            );
            ped.ColShape.onEntityEnterColShape += (shape, entity) => PlayerHelper.ProcessAction(entity, PlayerComeToNpc);
            ped.ColShape.onEntityExitColShape += (shape, entity) => PlayerHelper.ProcessAction(entity, PlayerAwayFromNpc);
        }

        /// <summary>
        /// Игрок подошел к нпц
        /// </summary>
        private void PlayerComeToNpc(Client player) {
            var vehicles = _vehicleInfoManager.GetPlayerVehicles(player).Where(e => e.HouseId == Validator.INVALID_ID && e.OnParkingFine);
            API.triggerClientEvent(player, ServerEvent.SHOW_PARKING_FINE_MENU, JsonConvert.SerializeObject(vehicles));
        }

        /// <summary>
        /// Игрок отошел от нпц
        /// </summary>
        private void PlayerAwayFromNpc(Client player) {
            API.triggerClientEvent(player, ServerEvent.HIDE_PARKING_FINE_MENU);
        }
    }
}