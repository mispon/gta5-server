namespace gta_mp_database.Models.Player {
    /// <summary>
    /// Модель внешнего вида игрока
    /// </summary>
    public class PlayerAppearance {
        /// <summary>
        /// Внешний вид - муж. / жен.
        /// </summary>
        public int Skin { get; set; }

        /// <summary>
        /// Лицо отца
        /// </summary>
        public int FatherFace { get; set; }

        /// <summary>
        /// Лицо матери
        /// </summary>
        public int MotherFace { get; set; }

        /// <summary>
        /// Отношение преобладания родительского цвета кожи
        /// </summary>
        public float SkinMix { get; set; }

        /// <summary>
        /// Отношение преобладания родительской внешности
        /// </summary>
        public float ShapeMix { get; set; }

        /// <summary>
        /// Прическа
        /// </summary>
        public int HairStyle { get; set; }

        /// <summary>
        /// Цвет волос
        /// </summary>
        public int HairColor { get; set; }

        /// <summary>
        /// Цвет глаз
        /// </summary>
        public int EyesColor { get; set; }
    }
}