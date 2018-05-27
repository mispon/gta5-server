using gta_mp_data.Entity;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.Vehicles.Interfaces {
    internal interface IVehicleTrunkManager {
        /// <summary>
        /// Положить предмет в багажник
        /// </summary>
        bool PutInTrunk(Client player, Vehicle vehicle, InventoryItem item, int count);

        /// <summary>
        /// Забрать предмет из багажника
        /// </summary>
        bool TakeFromTrunk(Client player, Vehicle vehicle, InventoryItem item, int count);
    }
}