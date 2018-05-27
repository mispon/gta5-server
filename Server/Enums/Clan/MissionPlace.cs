using System.ComponentModel;

namespace gta_mp_server.Enums.Clan {
    /// <summary>
    /// Места, где происходят миссии
    /// </summary>
    internal enum MissionPlace {
        Unknown = 0,

        [Description("Плантация марихуаны")]
        Weed,

        [Description("Склад оружия")]
        Weapon,

        [Description("Центральный банк")]
        Bank,

        [Description("Производство кокаина")]
        Coke
    }
}