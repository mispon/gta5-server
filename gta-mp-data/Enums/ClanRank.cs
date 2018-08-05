using System.ComponentModel;

namespace gta_mp_data.Enums {
    /// <summary>
    /// Ранг игрока в клане
    /// </summary>
    public enum ClanRank {
        [Description("Громила")]
        Lowest = 1,

        [Description("Наемник")]
        Low,

        [Description("Гангстер")]
        Middle,

        [Description("Авторитет")]
        High,

        [Description("Советник")]
        Highest
    }
}