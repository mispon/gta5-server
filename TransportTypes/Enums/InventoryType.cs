using System.ComponentModel;

namespace gta_mp_data.Enums {
    /// <summary>
    /// Типы вещей в инвентаре
    /// </summary>
    public enum InventoryType {
        [Description("Аптечка")]
        Medicine = 1,

        [Description("Сухой паёк")]
        Food,

        [Description("Канистра бензина")]
        Canister,

        [Description("Деньги")]
        Money,

        [Description("Вод. лицензия B-кат.")]
        DriverLicenceB,

        [Description("Вод. лицензия C-кат.")]
        DriverLicenceC,

        [Description("Лицензия на оружие")]
        WeaponLicense,

        [Description("Оружие")]
        Weapon,

        [Description("Патроны")]
        Ammo,

        [Description("Временная смена внешности")]
        TempoSkin,

        [Description("Бутылка воды")]
        WaterBottle,

        [Description("Марихуана")]
        Marijuana
    }
}