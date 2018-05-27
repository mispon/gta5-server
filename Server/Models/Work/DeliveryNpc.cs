using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models.Work {
    /// <summary>
    /// Модель нпс, выдающих контракты на перевозку
    /// </summary>
    internal class DeliveryNpc {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 MarkerPosition { get; set; }
        public PedHash Hash { get; set; }
        public string Name { get; set; }
        public List<DeliveryContract> Contracts { get; set; }
    }
}