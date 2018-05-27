using System.Collections.Generic;
using gta_mp_data.Entity;
using DriverInfo = gta_mp_data.Entity.DriverInfo;
using PlayerInfo = gta_mp_database.Models.Player.PlayerInfo;
using Settings = gta_mp_data.Entity.Settings;

namespace gta_mp_database.Converters {
    /// <summary>
    /// Конвертер моделей игрока
    /// </summary>
    public class PlayerInfoConverter {
        /// <summary>
        /// Сконвертировать данные игрока в модель
        /// </summary>
        public static PlayerInfo ConvertToModel(gta_mp_data.Entity.PlayerInfo playerInfo, PlayerAdditionalInfo additionalInfo,
            DriverInfo driverInfo, IEnumerable<PlayerWork> worksInfo, Wanted wantedInfo, Settings settings, IEnumerable<PlayerClothes> clothes) {
            var result = new PlayerInfo {
                AccountId = playerInfo.AccountId,
                Name = playerInfo.Name,
                Level = playerInfo.Level,
                Experience = playerInfo.Experience,
                Satiety = playerInfo.Satiety,
                Health = playerInfo.Health,
                LastPosition = playerInfo.LastPosition,
                Dimension = playerInfo.Dimension,
                Driver = DriverInfoConverter.ConvertToModel(driverInfo),
                Works = WorksInfoConverter.ConvertToModels(worksInfo),
                Wanted = wantedInfo,
                PhoneNumber = additionalInfo.PhoneNumber,
                PhoneBalance = additionalInfo.PhoneBalance,
                TagName = additionalInfo.TagName,
                TagColor = additionalInfo.TagColor,
                LastTeleportToHouse = additionalInfo.LastTeleportToHouse,
                Settings = settings,
                Clothes = PlayerClothesConverter.ConvertTModels(clothes)
            };
            return result;
        }

        /// <summary>
        /// Сконвертировать основную информацию игроков в сущности
        /// </summary>
        public static gta_mp_data.Entity.PlayerInfo ConvertToEntity(PlayerInfo model) {
            return new gta_mp_data.Entity.PlayerInfo {
                AccountId = model.AccountId,
                Level = model.Level,
                Experience = model.Experience,
                Dimension = model.Dimension,
                Name = model.Name,
                Satiety = model.Satiety,
                Health = model.Health,
                LastPosition = model.LastPosition
            };
        }

        /// <summary>
        /// Сконвертировать дополнительные данные игрока в сущность
        /// </summary>
        public static PlayerAdditionalInfo ConvertToAdditionalEntity(PlayerInfo model) {
           return new PlayerAdditionalInfo {
                AccountId = model.AccountId,
                TagName = model.TagName,
                TagColor = model.TagColor,
                PhoneNumber = model.PhoneNumber,
                PhoneBalance = model.PhoneBalance,
                LastTeleportToHouse = model.LastTeleportToHouse
            };
        }
    }
}