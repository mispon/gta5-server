using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB.Mapping;
using Newtonsoft.Json;

namespace gta_mp_database.Entity {
    /// <summary>
    /// Тюнинг тс
    /// </summary>
    [Table(Name = "VehiclesTuning")]
    public class VehicleTuning {
        /// <summary>
        /// Идентификатор транспорта
        /// </summary>
        [PrimaryKey]
        [Column(Name = "VehicleId")]
        public long VehicleId { get; set; }

        /// <summary>
        /// Основной цвет
        /// </summary>
        [Column(Name = "PrimaryColor")]
        public int PrimaryColor { get; set; }

        /// <summary>
        /// Второстепенный цвет
        /// </summary>
        [Column(Name = "SecondColor")]
        public int SecondColor { get; set; }

        /// <summary>
        /// Разгон двигателя
        /// </summary>
        [Column(Name = "EnginePower")]
        public int EnginePower { get; set; }

        /// <summary>
        /// Неоновая подсветка
        /// </summary>
        [Column(Name = "Neon")]
        public string Neon { get; set; }

        /// <summary>
        /// Тонировка
        /// </summary>
        [Column(Name = "WindowsTint")]
        public int WindowsTint { get; set; }

        /// <summary>
        /// Детали транспорта и их значения
        /// </summary>
        [Column(Name = "Mods")]
        public string Mods { get; set; }

        #region getters / setters

        /// <summary>
        /// Возвращает rgb-цвет неона
        /// </summary>
        public byte[] GetNeonColor() {
            return string.IsNullOrEmpty(Neon) 
                ? new byte[0]
                : Neon.Split(';').Select(e => Convert.ToByte(e)).ToArray();
        }

        /// <summary>
        /// Записывает rgb-цвет неона
        /// </summary>
        public void SetNeonColor(byte[] color) {
            Neon = string.Join(";", color);
        }

        /// <summary>
        /// Возвращает значения тюнинга
        /// </summary>
        public Dictionary<int, int> GetMods() {
            return string.IsNullOrEmpty(Mods)
                ? new Dictionary<int, int>(0) 
                : JsonConvert.DeserializeObject<Dictionary<int, int>>(Mods);
        }

        /// <summary>
        /// Записывает значения тюнинга
        /// </summary>
        public void SetMods(Dictionary<int, int> mods) {
            Mods = JsonConvert.SerializeObject(mods);
        }

        #endregion
    }
}