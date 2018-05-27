using gta_mp_server.Enums.Vehicles;

namespace gta_mp_server.Managers.Tuning {
    /// <summary>
    /// Вспомогательные данные данные тюнинга
    /// </summary>
    internal class TuningData {
        /// <summary>
        /// Возвращает стоимость элемента тюнинга
        /// </summary>
        internal static int GetPrice(VehicleMod type) {
            switch (type) {
                case VehicleMod.Spoilers:
                    return 800;
                case VehicleMod.FrontBumper:
                case VehicleMod.RearBumper:
                case VehicleMod.SideSkirt:
                case VehicleMod.Exhaust:
                case VehicleMod.Frame:
                case VehicleMod.Grille:
                    return 600;
                case VehicleMod.Hood:
                case VehicleMod.Fender:
                case VehicleMod.Roof:
                    return 700;
                case VehicleMod.Engine:
                    return 2000;
                case VehicleMod.Suspension:
                case VehicleMod.Transmission:
                    return 1300;
                case VehicleMod.Horns:
                    return 500;
                case VehicleMod.Armor:
                    return 1500;
                case VehicleMod.Brakes:
                case VehicleMod.Xenon:
                    return 1000;
                case VehicleMod.FrontWheels:
                case VehicleMod.BackWheels:
                    return 1500;
                default:
                    return 0;
            }
        }
    }
}