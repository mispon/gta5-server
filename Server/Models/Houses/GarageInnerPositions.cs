using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models {
    /// <summary>
    /// Позиции внутри гаража
    /// </summary>
    public class GarageInnerPositions {
        /// <summary>
        /// Позиция после входа в гараж
        /// </summary>
        public Vector3 AfterEnter { get; set; }

        /// <summary>
        /// Поворот после входа в гараж
        /// </summary>
        public Vector3 EnterRotation { get; set; }

        /// <summary>
        /// Парковочные места
        /// Item1 - позиция
        /// Item2 - поворот
        /// </summary>
        public List<Tuple<Vector3, Vector3>> Positions { get; set; }

        /// <summary>
        /// Выход из гаража
        /// </summary>
        public List<Vector3> GarageExits { get; set; }
    }
}