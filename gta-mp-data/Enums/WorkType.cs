using System.ComponentModel;

namespace gta_mp_data.Enums {
    /// <summary>
    /// Типы работ
    /// </summary>
    public enum WorkType {
        [Description("Грузчик")]
        Loader = 1,

        [Description("Водитель погрузчика")]
        Forklift,

        [Description("Водитель автобуса")]
        BusDriver,

        [Description("Водитель такси")]
        TaxiDriver,

        [Description("Полицейский")]
        Police,

        [Description("Водитель фургона с едой")]
        FoodTrunk,

        [Description("Доставщик еды")]
        FoodDeliveryMan,

        [Description("Дальнобойщик")]
        Trucker,

        [Description("Эвакуаторщик")]
        Wrecker,

        [Description("Летчик")]
        Pilot,

        [Description("Строитель")]
        Builder,

        [Description("Рыбак")]
        Fisherman,

        [Description("Фермер")]
        Farmer,

        [Description("Тракторист")]
        TractorDriver
    }
}