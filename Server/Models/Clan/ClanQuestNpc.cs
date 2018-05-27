using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models.Clan {
    /// <summary>
    /// Клановый квестовый нпс
    /// </summary>
    internal class ClanQuestNpc {
        public int Index { get; set; }
        public PedHash Hash { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 ShapePosition { get; set; }
    }
}