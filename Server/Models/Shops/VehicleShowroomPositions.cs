using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models.Shops {
    /// <summary>
    /// Позиции камеры и транспорта в режиме просмотра
    /// </summary>
    internal class VehicleShowroomPositions {
        public Vector3 PreviewPosition { get; set; }
        public Vector3 PreviewRotation { get; set; }
        public Vector3 CameraPosition { get; set; }
        public Vector3 CameraRotation { get; set; }
    }
}