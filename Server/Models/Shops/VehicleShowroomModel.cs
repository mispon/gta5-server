using gta_mp_server.Enums;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models.Shops {
    /// <summary>
    /// Модель автосалона
    /// </summary>
    internal class VehicleShowroomModel {
        public string Name { get; set; }
        public int Blip { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 PositionAfterEnter { get; set; }
        public Vector3 RotationAfterEnter { get; set; }
        public Vector3 ExitPosition { get; set; }
        public Vector3 PositionAfterExit { get; set; }
        public Vector3 RotationAfterExit { get; set; }
        public PedHash Seller { get; set; }
        public Vector3 SellerPosition { get; set; }
        public Vector3 SellerRotation { get; set; }
        public Vector3 SellerMarkerPosition { get; set; }
        public VehicleShowroomPositions ShowroomPositions { get; set; }
        public ShowroomType Type { get; set; }
        public int District { get; set; }
    }
}