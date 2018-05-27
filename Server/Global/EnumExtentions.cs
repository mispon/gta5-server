using System;
using System.ComponentModel;

namespace gta_mp_server.Global {
    /// <summary>
    /// Разширения Enum
    /// </summary>
    public static class EnumExtentions {
        /// <summary>
        /// Возвращает описание поля
        /// </summary>
        public static string GetDescription(this Enum value) {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var descriptionAttribute = (DescriptionAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
            return descriptionAttribute != null ? descriptionAttribute.Description : value.ToString();
        }
    }
}