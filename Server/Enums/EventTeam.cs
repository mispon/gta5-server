using System.ComponentModel;

namespace gta_mp_server.Enums {
    /// <summary>
    /// Команды на эвенте
    /// </summary>
    internal enum EventTeam {
        None = 0,

        [Description("красная")]
        Red = 1,

        [Description("синяя")]
        Blue = 2
    }
}