using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_data.Entity;
using gta_mp_database.Models.Player;

namespace gta_mp_database.Converters {
    /// <summary>
    /// Конвертер информации об одеждах игроков
    /// </summary>
    internal class PlayerClothesConverter {
        /// <summary>
        /// Конвертация сущностей в модели
        /// </summary>
        public static List<ClothesModel> ConvertTModels(IEnumerable<PlayerClothes> entities) {
            var result = new List<ClothesModel>();
            foreach (var entity in entities) {
                var model = new ClothesModel {
                    Variation = entity.Variation,
                    Slot = entity.Slot,
                    Torso = entity.Torso,
                    Undershirt = entity.Undershirt,
                    Texture = entity.Texture,
                    Textures = entity.Textures.Split(',').Select(e => Convert.ToInt32(e)).ToList(),
                    OnPlayer = entity.OnPlayer,
                    IsClothes = entity.IsClothes
                };
                result.Add(model);
            }
            return result;
        }

        /// <summary>
        /// Конвертация моделей в сущности
        /// </summary>
        public static List<PlayerClothes> ConvertToEntities(long accountId, IEnumerable<ClothesModel> models) {
            var result = new List<PlayerClothes>();
            foreach (var model in models) {
                var entity = new PlayerClothes {
                    AccountId = accountId,
                    Variation = model.Variation,
                    Slot = model.Slot,
                    Torso = model.Torso,
                    Undershirt = model.Undershirt,
                    Texture = model.Texture,
                    Textures = string.Join(",", model.Textures),
                    OnPlayer = model.OnPlayer,
                    IsClothes = model.IsClothes
                };
                result.Add(entity);
            }
            return result;
        }
    }
}