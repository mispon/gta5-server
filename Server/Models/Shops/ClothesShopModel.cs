using gta_mp_server.Enums;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models.Shops {
    /// <summary>
    /// Основная информация магазина одежды
    /// </summary>
    internal class ClothesShopModel {
        public PedHash Seller { get; set; }
        public Vector3 SellerPosition { get; set; }
        public Vector3 SellerRotation { get; set; }
        public Vector3 MarkerPosition { get; set; }
        public int DoorId { get; set; }
        public Vector3 LeftDoorPosition { get; set; }
        public Vector3 RightDoorPosition { get; set; }
        public int Blip { get; set; }
        public Vector3 BlipPosition { get; set; }
        public ClothesShopType Type { get; set; }
        public DressingRoomPositions DressingRoom { get; set; }
        public int District { get; set; }
    }
}