using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models.Clan {
    /// <summary>
    /// Параметры добычи миссии
    /// </summary>
    internal class BootyObjectParams {
        public int Model { get; set; }
        public Vector3 PositionOffset { get; set; }
        public Vector3 RotationOffset { get; set; }
    }
}