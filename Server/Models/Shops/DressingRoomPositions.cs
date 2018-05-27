using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models.Shops {
    /// <summary>
    /// Позиции примерочной комнаты
    /// </summary>
    public class DressingRoomPositions {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 CameraHatsPosition { get; set; }
        public Vector3 CameraTopsPosition { get; set; }
        public Vector3 CameraLegsPosition { get; set; }
        public Vector3 CameraFeetsPosition { get; set; }
        public Vector3 CameraRotation { get; set; }
    }
}