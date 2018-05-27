using System;

namespace gta_mp_server.Helpers {
    /// <summary>
    /// Проверка валидности значений
    /// </summary>
    internal class Validator {
        internal const long INVALID_ID = -1;
        private static readonly DateTime _invalDatetime = DateTime.MinValue;

        public static bool IsValid(int value) {;
            return value != INVALID_ID;
        }

        public static bool IsValid(long value) {
            return value != INVALID_ID;
        }

        public static bool IsValid(DateTime value) {
            return value != _invalDatetime;
        }
    }
}