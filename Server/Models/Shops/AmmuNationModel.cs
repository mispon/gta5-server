using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models.Shops {
    /// <summary>
    /// Модель данных оружейного магазина
    /// </summary>
    internal class AmmuNationModel {
        public string Name { get; set; }
        public Vector3 SellerPosition { get; set; }
        public Vector3 SellerRotation { get; set; }
        public Vector3 Marker { get; set; }
        public Vector3 LeftDoorPosition { get; set; }
        public Vector3 RightDoorPosition { get; set; }
        public int District { get; set; }
    }
}