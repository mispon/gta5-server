using LinqToDB.Mapping;

namespace gta_mp_data.Entity {
    /// <summary>
    /// Сущность параметров внешности игрока
    /// </summary>
    [Table(Name = "PlayersAppearance")]
    public class PlayerAppearance {
        /// <summary>
        /// Идентификатор аккаунта
        /// </summary>
        [PrimaryKey]
        [Column(Name = "AccountId")]
        public long AccountId { get; set; }

        /// <summary>
        /// Внешний вид - муж. / жен.
        /// </summary>
        [Column(Name = "Skin")]
        public int Skin { get; set; }

        /// <summary>
        /// Лицо отца
        /// </summary>
        [Column(Name = "FatherFace")]
        public int FatherFace { get; set; }

        /// <summary>
        /// Лицо матери
        /// </summary>
        [Column(Name = "MotherFace")]
        public int MotherFace { get; set; }

        /// <summary>
        /// Отношение преобладания родительского цвета кожи
        /// </summary>
        [Column(Name = "SkinMix")]
        public float SkinMix { get; set; }

        /// <summary>
        /// Отношение преобладания родительской внешности
        /// </summary>
        [Column(Name = "ShapeMix")]
        public float ShapeMix { get; set; }

        /// <summary>
        /// Прическа
        /// </summary>
        [Column(Name = "HairStyle")]
        public int HairStyle { get; set; }

        /// <summary>
        /// Цвет волос
        /// </summary>
        [Column(Name = "HairColor")]
        public int HairColor { get; set; }

        /// <summary>
        /// Цвет глаз
        /// </summary>
        [Column(Name = "EyesColor")]
        public int EyesColor { get; set; }
    }
}