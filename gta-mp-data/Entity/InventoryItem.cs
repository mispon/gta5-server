using gta_mp_data.Enums;
using LinqToDB.Mapping;

namespace gta_mp_data.Entity {
    /// <summary>
    /// Инвентарь игрока
    /// </summary>
    [Table(Name = "Inventory")]
    public class InventoryItem {
        /// <summary>
        /// Уникальный идентификатор предмета
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
        /// Название вещи
        /// </summary>
        [Column(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Тип
        /// </summary>
        [Column(Name = "Type")]
        public InventoryType Type { get; set; }

        /// <summary>
        /// Количество
        /// </summary>
        [Column(Name = "Count")]
        public int Count { get; set; }

        /// <summary>
        /// Количество в хранилище дома
        /// </summary>
        [Column(Name = "CountInHouse")]
        public int CountInHouse { get; set; }

        /// <summary>
        /// Хэш для оружия и тип для патронов
        /// </summary>
        [Column(Name = "Model")]
        public int Model { get; set; }

        /// <summary>
        /// Уменьшает количество предметов
        /// </summary>
        public void DecreaseCount(int coef = 1) {
            Count = Count - coef;
        }
    }
}