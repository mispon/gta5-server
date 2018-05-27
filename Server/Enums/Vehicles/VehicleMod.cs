using System.ComponentModel;

namespace gta_mp_server.Enums.Vehicles {
    /// <summary>
    /// Детали корпуса транспорта
    /// </summary>
    internal enum VehicleMod {
        [Description("Спойлер")]
        Spoilers = 0,

        [Description("Передний бампер")]
        FrontBumper = 1,

        [Description("Задний бампер")]
        RearBumper = 2,

        [Description("Боковые обвесы")]
        SideSkirt = 3,

        [Description("Выхлопная труба")]
        Exhaust = 4,

        [Description("Каркас")]
        Frame = 5,

        [Description("Радиаторная решетка")]
        Grille = 6,

        [Description("Капот")]
        Hood = 7,

        [Description("Багажник")]
        Fender = 8,

        [Description("Крыша")]
        Roof = 10,

        [Description("Двигатель")]
        Engine = 11,

        [Description("Тормоза")]
        Brakes = 12,

        [Description("Трансмиссия")]
        Transmission = 13,

        [Description("Подвеска")]
        Suspension = 15,

        [Description("Клаксон")]
        Horns = 14,

        [Description("Укрепление корпуса")]
        Armor = 16,

        [Description("Ксенон")]
        Xenon = 22,

        // авто
        [Description("Диски")]
        FrontWheels = 23,

        // мото
        [Description("Диски")]
        BackWheels = 24,

        [Description("Внутренний каркас")]
        Dashboard = 29,

        [Description("Измерительные приборы")]
        DialDesign = 30,

        [Description("Обивка дверей")]
        DoorCover = 31,

        [Description("Обивка сидений")]
        SeatCover = 32,

        [Description("Руль")]
        SteeringWheel = 33,

        [Description("Наклейки на кузов")]
        Stickers = 48
    }
}