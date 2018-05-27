using System;
using System.Collections.Generic;
using System.Linq;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Helpers {
    /// <summary>
    /// Конвертация строк в вектор и наоборот
    /// </summary>
    public class PositionConverter {
        private const char DELIMITER = ';';
        private const char POS_DELIMITER = '|';

        /// <summary>
        /// Возвращает вектор позиции
        /// </summary>
        public static Vector3 ToVector3(string position) {
            var coord = position.Split(DELIMITER).Select(Convert.ToDouble).ToArray();
            return new Vector3(coord[0], coord[1], coord[2]);
        }

        /// <summary>
        /// Возвращает вектора позиций
        /// </summary>
        public static List<Vector3> ToListVector3(string position) {
            var positions = position.Split(POS_DELIMITER);
            return positions.Select(ToVector3).ToList();
        }

        /// <summary>
        /// Возвращает строковое представление
        /// </summary>
        public static string VectorToString(Vector3 position) {
            return $"{position.X}{DELIMITER}{position.Y}{DELIMITER}{position.Z}";
        }
    }
}