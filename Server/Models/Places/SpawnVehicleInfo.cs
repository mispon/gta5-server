using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models.Places {
    /// <summary>
    /// Модель спавна рабочего транспорта
    /// </summary>
    internal class SpawnVehicleInfo {
        public VehicleHash Hash { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
    }
}