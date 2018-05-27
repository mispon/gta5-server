using System.Collections.Generic;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Work.Loader {
    /// <summary>
    /// Данные работы грузчиков
    /// </summary>
    internal class LoaderDataGetter {
        /// <summary>
        /// Возвращает позиции получения груза
        /// </summary>
        internal static List<Vector3> TakePositions =  new List<Vector3> {
            new Vector3(-427.51, -1701.34, 18.07),
            new Vector3(-412.95, -1678.10, 18.03),
            new Vector3(-419.22, -1675.83, 18.03),
            new Vector3(-443.62, -1666.83, 18.03),
            new Vector3(-475.71, -1672.37, 17.78),
            new Vector3(-443.04, -1683.23, 18.03),
            new Vector3(-429.51, -1723.20, 18.04),
            new Vector3(-431.10, -1719.38, 18.03),
            new Vector3(-481.69, -1712.55, 17.70),
            new Vector3(-485.52, -1750.83, 17.39),
            new Vector3(-513.32, -1700.05, 18.33),
            new Vector3(-532.13, -1695.92, 18.23),
            new Vector3(-553.67, -1706.78, 17.89)

        };

        /// <summary>
        /// Возвращает позиции сдачи груза
        /// </summary>
        internal static List<Vector3> PutPositions = new List<Vector3> {
            new Vector3(-468.55, -1719.43, 18.69),
            new Vector3(-440.72, -1694.85, 19.07),
            new Vector3(-453.79, -1736.13, 16.76),
            new Vector3(-511.91, -1737.73, 19.26),
            new Vector3(-538.07, -1718.89, 19.23)
        };
    }
}