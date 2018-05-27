using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models.Work {
    /// <summary>
    /// Точка сдачи / получения груза
    /// </summary>
    internal class LoaderPoint {
        public ColShape ColShape { get; set; }
        public Vector3 Position { get; set; }
        public int Number { get; set; }
    }
}