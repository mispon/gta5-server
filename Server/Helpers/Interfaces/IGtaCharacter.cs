using System.Collections.Generic;
using gta_mp_database.Models.Player;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Helpers.Interfaces {
    public interface IGtaCharacter {
        /// <summary>
        /// Применить внешность игрока
        /// </summary>
        void SetAppearance(Client player, PlayerAppearance appearance);

        /// <summary>
        /// Установить одежду
        /// </summary>
        void SetClothes(Client player, ClothesModel clothes);

        /// <summary>
        /// Установить одежду
        /// </summary>
        void SetClothes(Client player, ICollection<ClothesModel> clothes);
    }
}