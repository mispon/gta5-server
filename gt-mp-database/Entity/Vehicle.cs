using System.Collections.Generic;
using gta_mp_data.Entity;
using LinqToDB.Mapping;
using Newtonsoft.Json;

namespace gta_mp_database.Entity {
    /// <summary>
    /// Сущность транспортного средства игрока
    /// </summary>
    [Table(Name = "Vehicles")]
    public class Vehicle {
        /// <summary>
        /// Идентификатор тс
        /// </summary>
        [PrimaryKey, Identity]
        [Column(Name = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Идентификатор владельца
        /// </summary>
        [Column(Name = "OwnerId")]
        public long OwnerId { get; set; }

        /// <summary>
        /// Идентификатор дома, в гараже которого стоит тс
        /// </summary>
        [Column(Name = "HouseId")]
        public long HouseId { get; set; }

        /// <summary>
        /// Хэш модели
        /// </summary>
        [Column(Name = "Hash")]
        public int Hash { get; set; }

        /// <summary>
        /// Количество топлива
        /// </summary>
        [Column(Name = "Fuel")]
        public float Fuel { get; set; }

        /// <summary>
        /// Максимальная вместимость бака
        /// </summary>
        [Column(Name = "MaxFuel")]
        public int MaxFuel { get; set; }

        /// <summary>
        /// Заспавнено ли уже тс
        /// </summary>
        [Column(Name = "IsSpawned")]
        public bool IsSpawned { get; set; }

        /// <summary>
        /// Находится ли тс на штрафстоянке
        /// </summary>
        [Column(Name = "OnParkingFine")]
        public bool OnParkingFine { get; set; }

        /// <summary>
        /// Багажник
        /// </summary>
        [Column(Name = "Trunk")]
        public string Trunk { get; set; }

        /// <summary>
        /// Тюнинг транспорта
        /// </summary>
        public VehicleTuning Tuning { get; set; }

        /// <summary>
        /// Ссылка на физически заспавненный транспорт
        /// </summary>
        [JsonIgnore]
        public GrandTheftMultiplayer.Server.Elements.Vehicle Instance { get; set; }

        /// <summary>
        /// Записать данные багажника
        /// </summary>
        public void SetTrunk(IEnumerable<InventoryItem> trunk) {
            Trunk = JsonConvert.SerializeObject(trunk);
        }

        /// <summary>
        /// Получить данные багажника
        /// </summary>
        public List<InventoryItem> GetTrunk() {
            return string.IsNullOrEmpty(Trunk)
                ? new List<InventoryItem>(0) 
                : JsonConvert.DeserializeObject<List<InventoryItem>>(Trunk);
        }
    }
}