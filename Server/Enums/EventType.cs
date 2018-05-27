using System.ComponentModel;

namespace gta_mp_server.Enums {
    /// <summary>
    /// Тип эвента
    /// </summary>
    internal enum EventType {
        [Description("Коммандный")]
        Commands = 1,

        [Description("Сам за себя")]
        Himself = 2
    }
}