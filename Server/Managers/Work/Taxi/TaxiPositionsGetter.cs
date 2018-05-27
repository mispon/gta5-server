using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Work.Taxi {
    internal class TaxiPositionsGetter {
        /// <summary>
        /// Возвращает позиции машин
        /// Item1 - позиция
        /// Item2 - поворот
        /// </summary>
        internal static List<Tuple<Vector3, Vector3>> GetCarPositions() {
            return new List<Tuple<Vector3, Vector3>> {
                new Tuple<Vector3, Vector3>(new Vector3(215.2678, -804.2937, 30.38998), new Vector3(0, 0, -110.7)),
                new Tuple<Vector3, Vector3>(new Vector3(217.1346, -799.1877, 30.36135), new Vector3(0, 0, -110.7)),
                new Tuple<Vector3, Vector3>(new Vector3(222.4734, -784.1276, 30.37395), new Vector3(0, 0, -110.7)),
                new Tuple<Vector3, Vector3>(new Vector3(221.1164, -789.3, 30.33957), new Vector3(0, 0, -110.7)),
                new Tuple<Vector3, Vector3>(new Vector3(226.1074, -773.9306, 30.38568), new Vector3(0, 0, -110.7)),
                new Tuple<Vector3, Vector3>(new Vector3(236.3277, -795.0295, 30.11725), new Vector3(0, 0, -110.7)),
                new Tuple<Vector3, Vector3>(new Vector3(234.6621, -800.1157, 30.0878), new Vector3(0, 0, -110.7)),
                new Tuple<Vector3, Vector3>(new Vector3(232.5324, -805.2254, 30.06647), new Vector3(0, 0, -110.7)),
                new Tuple<Vector3, Vector3>(new Vector3(226.1487, -794.2987, 30.26764), new Vector3(0, 0, 70.5)),
                new Tuple<Vector3, Vector3>(new Vector3(228.6427, -786.5721, 30.29974), new Vector3(0, 0, 70.5)),
                //new Tuple<Vector3, Vector3>(new Vector3(234.5948, -771.4394, 30.36076), new Vector3(0, 0, 70.5)),
                //new Tuple<Vector3, Vector3>(new Vector3(232.2209, -776.3861, 30.33179), new Vector3(0, 0, 70.5)),
                //new Tuple<Vector3, Vector3>(new Vector3(222.1684, -804.2155, 30.28617), new Vector3(0, 0, 70.5)),
                //new Tuple<Vector3, Vector3>(new Vector3(207.305, -798.5256, 30.5891), new Vector3(0, 0, 70.5)),
                //new Tuple<Vector3, Vector3>(new Vector3(210.3145, -791.169, 30.52705), new Vector3(0, 0, 70.5)),
                //new Tuple<Vector3, Vector3>(new Vector3(212.7675, -783.5684, 30.49448), new Vector3(0, 0, 70.5)),
                //new Tuple<Vector3, Vector3>(new Vector3(219.5112, -766.1215, 30.43521), new Vector3(0, 0, 70.5)),
                //new Tuple<Vector3, Vector3>(new Vector3(244.4914, -792.4506, 30.05695), new Vector3(0, 0, 70.5)),
                //new Tuple<Vector3, Vector3>(new Vector3(240.3331, -805.389, 29.92834), new Vector3(0, 0, 70.5)),
                //new Tuple<Vector3, Vector3>(new Vector3(241.2792, -782.4235, 30.20767), new Vector3(0, 0, 70.5)),
                //new Tuple<Vector3, Vector3>(new Vector3(250.3268, -777.3243, 30.24127), new Vector3(0, 0, 70.5))
            };
        }
    }
}