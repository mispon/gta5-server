using gta_mp_server.Enums.Vehicles;

namespace gta_mp_server.Models.Shops {
    /// <summary>
    /// Модель продаваемой машины
    /// </summary>
    internal class ShowroomVehicle {
        public int Id { get; set; }
        public int Hash { get; set; }
        public int Price { get; set; }
        public int MaxFuel { get; set; }
        public VehicleType Type { get; set; }
    }
}