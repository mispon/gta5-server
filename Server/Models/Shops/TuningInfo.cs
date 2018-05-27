using System.Collections.Generic;

namespace gta_mp_server.Models.Shops {
    /// <summary>
    /// Модель элемента тюнинга
    /// </summary>
    internal class TuningInfo {
        public string Name { get; set; }
        public int Slot { get; set; }
        public int Price { get; set; }
        public List<int> Values { get; set; } = new List<int>();
    }
}