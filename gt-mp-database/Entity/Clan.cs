using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB.Mapping;

namespace gta_mp_database.Entity {
    /// <summary>
    /// Сущность клана
    /// </summary>
    [Table(Name = "Clans")]
    public class Clan {
        /// <summary>
        /// Идентификатор клана
        /// </summary>
        [PrimaryKey, Column(Name = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        [Column(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Общий бюджет членов клана
        /// </summary>
        [Column(Name = "Balance")]
        public int Balance { get; set; }

        /// <summary>
        /// Подконтрольные районы
        /// </summary>
        [Column(Name = "Districts")]
        public string Districts { get; set; }

        /// <summary>
        /// Авторитет клана
        /// </summary>
        [Column(Name = "Authority")]
        public int Authority { get; set; }

        /// <summary>
        /// Возвращает список контрольных районов
        /// </summary>
        public List<int> GetDistricts() {
            return string.IsNullOrEmpty(Districts) 
                ? new List<int>(0)
                : Districts.Split(',').Select(e => Convert.ToInt32(e)).ToList();
        }

        /// <summary>
        /// Записывает района клана
        /// </summary>
        public void SetDistricts(ICollection<int> districts) {
            Districts = districts.Any() ? string.Join(",", districts) : string.Empty;
        }
    }
}