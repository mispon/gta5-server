using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models.Work {
    /// <summary>
    /// Рабочая точка строителя
    /// </summary>
    internal class WorkPoint {
        public Vector3 Position { get; set; }
        public Vector3 WorkerRotation { get; set; }
    }
}