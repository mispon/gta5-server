using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;

namespace gta_mp_server.Models.Utils {
    /// <summary>
    /// Результат создания точки взаимодействия
    /// </summary>
    public class PointResult {
        public ColShape ColShape { get; set; }
        public Marker Marker { get; set; }
        public TextLabel Label { get; set; }
        public Ped Npc { get; set; }
    }
}