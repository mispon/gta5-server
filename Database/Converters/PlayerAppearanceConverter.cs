using gta_mp_database.Models.Player;
using Appearance = gta_mp_data.Entity.PlayerAppearance;

namespace gta_mp_database.Converters {
    /// <summary>
    /// Конвертер параметров внешнего вида игроков
    /// </summary>
    public class PlayerAppearanceConverter {
        /// <summary>
        /// Конвертация сущности в модель
        /// </summary>
        public static PlayerAppearance ConvertToModel(Appearance entity) {
            return new PlayerAppearance {
                Skin = entity.Skin,
                FatherFace = entity.FatherFace,
                MotherFace = entity.MotherFace,
                SkinMix = entity.SkinMix,
                ShapeMix = entity.ShapeMix,
                HairStyle = entity.HairStyle,
                HairColor = entity.HairColor,
                EyesColor = entity.HairColor
            };
        }

        /// <summary>
        /// Конвертация модели в сущность
        /// </summary>
        public static Appearance ConverToEntity(long accountId, PlayerAppearance model) {
            return new Appearance {
                AccountId = accountId,
                Skin = model.Skin,
                FatherFace = model.FatherFace,
                MotherFace = model.MotherFace,
                SkinMix = model.SkinMix,
                ShapeMix = model.ShapeMix,
                HairStyle = model.HairStyle,
                HairColor = model.HairColor,
                EyesColor = model.HairColor
            };
        }
    }
}