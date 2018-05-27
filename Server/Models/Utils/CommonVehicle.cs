using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models.Utils {
    /// <summary>
    /// Информация о коммунальных транспортных средствах
    /// </summary>
    public class CommonVehicle {
        /// <summary>
        /// Хэш автомобиля
        /// </summary>
        public VehicleHash Hash { get; set; }

        /// <summary>
        /// Ключ автомобиля
        /// </summary>
        public string VehicleType { get; set; }

        /// <summary>
        /// Позиция при респавне
        /// </summary>
        public Vector3 SpawnPosition { get; set; }

        /// <summary>
        /// Поворот при респавне
        /// </summary>
        public Vector3 SpawnRotation { get; set; }

        /// <summary>
        /// Основной цвет
        /// </summary>
        public int MainColor { get; set; }

        /// <summary>
        /// Вторичный цвет
        /// </summary>
        public int SecondColor { get; set; }

        /// <summary>
        /// Топливо
        /// </summary>
        public float Fuel { get; set; }

        /// <summary>
        /// Максимальная вместимость бака
        /// </summary>
        public float MaxFuel { get; set; }
    }
}