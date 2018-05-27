using Newtonsoft.Json;

namespace gta_mp_server.Helpers {
    /// <summary>
    /// Вспомогательный класс для полного копирования объектов
    /// </summary>
    internal class CopyHelper {
        /// <summary>
        /// Выполняет глубокое копирование
        /// </summary>
        internal static T DeepCopy<T>(T value) where T : new() {
            var serializedObj = JsonConvert.SerializeObject(value);
            return JsonConvert.DeserializeObject<T>(serializedObj);
        }
    }
}