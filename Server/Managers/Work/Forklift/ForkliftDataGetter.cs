using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Work.Forklift {
    /// <summary>
    /// Данные для погрузчиков
    /// </summary>
    internal class ForkliftDataGetter {
        /// <summary>
        /// Возвращает позиции погрузчиков
        /// Item1 - позиция
        /// Item2 - поворот
        /// </summary>
        internal static List<Tuple<Vector3, Vector3>> ForkliftsPositions = new List<Tuple<Vector3, Vector3>> {
            new Tuple<Vector3, Vector3>(new Vector3(728.026, -1355.0, 26.14766), new Vector3(0, 0, 178.5)),
            new Tuple<Vector3, Vector3>(new Vector3(725.0572, -1354.889, 26.09336), new Vector3(0, 0, 178.5)),
            new Tuple<Vector3, Vector3>(new Vector3(722.2488, -1354.865, 26.05451), new Vector3(0, 0, 178.5)),
            new Tuple<Vector3, Vector3>(new Vector3(719.3862, -1354.823, 26.00656), new Vector3(0, 0, 178.5)),
            new Tuple<Vector3, Vector3>(new Vector3(716.1828, -1354.541, 25.92439), new Vector3(0, 0, 178.5)),
            new Tuple<Vector3, Vector3>(new Vector3(700.5656, -1364.381, 25.7072), new Vector3(0, 0, 94.3)),
            new Tuple<Vector3, Vector3>(new Vector3(701.0263, -1367.548, 25.8911), new Vector3(0, 0, 94.3)),
            new Tuple<Vector3, Vector3>(new Vector3(701.4625, -1370.673, 26.05436), new Vector3(0, 0, 94.3)),
            new Tuple<Vector3, Vector3>(new Vector3(701.8596, -1373.57, 26.13715), new Vector3(0, 0, 94.3)),
            new Tuple<Vector3, Vector3>(new Vector3(702.2407, -1377.014, 26.20074), new Vector3(0, 0, 94.3)),
            new Tuple<Vector3, Vector3>(new Vector3(703.2822, -1380.267, 26.25851), new Vector3(0, 0, 94.3)),
            new Tuple<Vector3, Vector3>(new Vector3(707.0726, -1392.389, 26.30506), new Vector3(0, 0, 102.3)),
            new Tuple<Vector3, Vector3>(new Vector3(708.37, -1395.746, 26.33346), new Vector3(0, 0, 102.3)),
            new Tuple<Vector3, Vector3>(new Vector3(708.8408, -1398.615, 26.34484), new Vector3(0, 0, 102.3)),
            new Tuple<Vector3, Vector3>(new Vector3(709.6347, -1401.259, 26.35175), new Vector3(0, 0, 102.3))
        };

        /// <summary>
        /// Возвращает позиции получения груза
        /// </summary>
        internal static List<Vector3> TakePositions = new List<Vector3> {
            new Vector3(749.1867, -1260.317, 25.32527),
            new Vector3(698.5429, -1290.139, 24.94398),
            new Vector3(722.1227, -1331.396, 25.27162),
            new Vector3(699.2418, -1342.134, 24.56786),
            new Vector3(705.8492, -1385.061, 25.28213),
            new Vector3(733.0801, -1387.237, 25.50141),
            new Vector3(773.9621, -1367.887, 25.56832)
        };

        /// <summary>
        /// Возвращает позиции сдачи груза
        /// </summary>
        internal static List<Vector3> PutPositions = new List<Vector3> {         
            new Vector3(731.4193, -1300.901, 26.28956),
            new Vector3(732.7197, -1284.443, 26.28728),
            new Vector3(759.2291, -1262.096, 26.33726),
            new Vector3(767.5604, -1328.357, 26.22943),
            new Vector3(750.1934, -1357.494, 26.43786),
            new Vector3(750.8112, -1334.946, 26.23604)
        };
    }
}