using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Work.Wrecker {
    /// <summary>
    /// Позиции штрафстоянки
    /// </summary>
    internal class WreckerPositionsGetter {
        /// <summary>
        /// Позиция сдачи эвакуируемого транспорта
        /// </summary>
        internal static Vector3 DropZonePosition = new Vector3(401.74, -1631.46, 28.29);

        /// <summary>
        /// Позиции спавна транспорта
        /// </summary>
        internal static List<Tuple<Vector3, Vector3>> SpawnPlaces = new List<Tuple<Vector3, Vector3>> {
            new Tuple<Vector3, Vector3>(new Vector3(412.86, -1634.19, 28.79), new Vector3(-0.11, 0.00, -131.20)),
            new Tuple<Vector3, Vector3>(new Vector3(410.84, -1636.70, 28.79), new Vector3(-0.09, 0.00, -130.21)),
            new Tuple<Vector3, Vector3>(new Vector3(408.86, -1639.02, 28.79), new Vector3(-0.10, 0.00, -130.33)),
            new Tuple<Vector3, Vector3>(new Vector3(406.87, -1641.35, 28.79), new Vector3(-0.10, 0.00, -131.79))
        };

        /// <summary>
        /// Позиции эвакуаторов
        /// Item1 - позиция
        /// Item2 - поворот
        /// </summary>
        internal static List<Tuple<Vector3, Vector3>> TowTrucksPositions = new List<Tuple<Vector3, Vector3>> {
            new Tuple<Vector3, Vector3>(new Vector3(396.15, -1644.44, 28.79), new Vector3(-0.10, 0.00, 139.94)),
            new Tuple<Vector3, Vector3>(new Vector3(398.60, -1646.35, 28.79), new Vector3(-0.10, 0.00, 140.02)),
            new Tuple<Vector3, Vector3>(new Vector3(401.07, -1648.38, 28.79), new Vector3(-0.11, 0.02, 139.94)),
            new Tuple<Vector3, Vector3>(new Vector3(403.42, -1650.44, 28.79), new Vector3(-0.09, 0.02, 139.41)),
            new Tuple<Vector3, Vector3>(new Vector3(405.71, -1652.52, 28.79), new Vector3(-0.10, -0.02, 139.24)),
            new Tuple<Vector3, Vector3>(new Vector3(408.18, -1654.44, 28.79), new Vector3(-0.10, 0.00, 138.52)),
            new Tuple<Vector3, Vector3>(new Vector3(410.43, -1656.81, 28.79), new Vector3(-0.11, 0.00, 139.12))
        };
    }
}