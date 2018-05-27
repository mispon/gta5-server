using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_data.Entity;
using gta_mp_data.Enums;
using gta_mp_database.Entity;
using gta_mp_database.Models.Work;
using VehicleInfo = gta_mp_database.Entity.Vehicle;

namespace gta_mp_database.Models.Player {
    /// <summary>
    /// Данные игрока
    /// </summary>
    public class PlayerInfo {
        public const int MAX_VALUE = 100;

        /// <summary>
        /// Идентификатор
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Уровень
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Опыт
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        /// Баланс игрока
        /// </summary>
        public int Balance {
            get { return Inventory.First(e => e.Type == InventoryType.Money).Count; }
            set { Inventory.First(e => e.Type == InventoryType.Money).Count = value; }
        }

        /// <summary>
        /// Сытость
        /// </summary>
        public float Satiety { get; set; }

        /// <summary>
        /// Здоровье
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Позиция игрока на момент сохранения
        /// </summary>
        public string LastPosition { get; set; }

        /// <summary>
        /// Работы игрока
        /// </summary>
        public Dictionary<WorkType, WorkInfo> Works { get; set; } = new Dictionary<WorkType, WorkInfo>();

        /// <summary>
        /// Водительская лицензия
        /// </summary>
        public DriverInfo Driver { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public Skin Skin { get; set; }

        /// <summary>
        /// Внешний вид
        /// </summary>
        public PlayerAppearance Appearance { get; set; }

        /// <summary>
        /// Степень преступности игрока
        /// </summary>
        public Wanted Wanted { get; set; }

        /// <summary>
        /// Саб-ник
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Цвет тэга
        /// </summary>
        public string TagColor { get; set; }

        /// <summary>
        /// Измерение игрока
        /// </summary>
        public int Dimension { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public int PhoneNumber { get; set; }

        /// <summary>
        /// Баланс телефона
        /// </summary>
        public float PhoneBalance { get; set; }

        /// <summary>
        /// Время последнего перемещения к дому
        /// </summary>
        public DateTime LastTeleportToHouse { get; set; }

        /// <summary>
        /// Список контактов
        /// </summary>
        public List<PhoneContact> PhoneContacts = new List<PhoneContact>();

        /// <summary>
        /// Настройки игрока
        /// </summary>
        public Settings Settings { get; set; }

        /// <summary>
        /// Одежда игрока
        /// </summary>
        public List<ClothesModel> Clothes { get; set; } = new List<ClothesModel>();

        /// <summary>
        /// Транспорт игрока
        /// </summary>
        public Dictionary<long, VehicleInfo> Vehicles { get; set; } = new Dictionary<long, VehicleInfo>();

        /// <summary>
        /// Инвентарь
        /// </summary>
        public List<InventoryItem> Inventory { get; set; } = new List<InventoryItem>();

        /// <summary>
        /// Информация о клане
        /// </summary>
        public PlayerClanInfo Clan { get; set; }

        /// <summary>
        /// Дата завершения премиума
        /// </summary>
        public DateTime PremiumEnd { get; set; }

        /// <summary>
        /// Возвращает статус премиума
        /// </summary>
        public bool IsPremium() {
            return DateTime.Now < PremiumEnd;
        }
    }
}