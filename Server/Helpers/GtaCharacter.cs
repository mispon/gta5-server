using System.Collections.Generic;
using gta_mp_database.Models.Player;
using gta_mp_server.Constant;
using gta_mp_server.Helpers.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Helpers {
    internal class GtaCharacter : Script, IGtaCharacter {
        private const string GTAO_HAS_CHARACTER_DATA = "GTAO_HAS_CHARACTER_DATA";
        private const string GTAO_SHAPE_FIRST_ID = "GTAO_SHAPE_FIRST_ID";
        private const string GTAO_SHAPE_SECOND_ID = "GTAO_SHAPE_SECOND_ID";
        private const string GTAO_SKIN_FIRST_ID = "GTAO_SKIN_FIRST_ID";
        private const string GTAO_SKIN_SECOND_ID = "GTAO_SKIN_SECOND_ID";
        private const string GTAO_SHAPE_MIX = "GTAO_SHAPE_MIX";
        private const string GTAO_SKIN_MIX = "GTAO_SKIN_MIX";
        private const string GTAO_HAIR_COLOR = "GTAO_HAIR_COLOR";
        private const string GTAO_EYE_COLOR = "GTAO_EYE_COLOR";

        private const int HAIR_SLOT = 2;

        /// <summary>
        /// Применить внешность игрока
        /// </summary>
        public void SetAppearance(Client player, PlayerAppearance appearance) {
            API.setPlayerSkin(player, (PedHash) appearance.Skin);
            API.setPlayerClothes(player, HAIR_SLOT, appearance.HairStyle, 0);
            API.setEntitySyncedData(player.handle, GTAO_HAS_CHARACTER_DATA, true);
            API.setEntitySyncedData(player.handle, GTAO_SHAPE_FIRST_ID, appearance.MotherFace);
            API.setEntitySyncedData(player.handle, GTAO_SHAPE_SECOND_ID, appearance.FatherFace);
            API.setEntitySyncedData(player.handle, GTAO_SKIN_FIRST_ID, appearance.MotherFace);
            API.setEntitySyncedData(player.handle, GTAO_SKIN_SECOND_ID, appearance.FatherFace);
            API.setEntitySyncedData(player.handle, GTAO_SHAPE_MIX, appearance.ShapeMix);
            API.setEntitySyncedData(player.handle, GTAO_SKIN_MIX, appearance.SkinMix);
            API.setEntitySyncedData(player.handle, GTAO_HAIR_COLOR, appearance.HairColor);
            API.setEntitySyncedData(player.handle, GTAO_EYE_COLOR, appearance.EyesColor);
            API.triggerClientEventForAll(ServerEvent.UPDATE_APPEARANCE, player);
        }

        /// <summary>
        /// Установить одежду
        /// </summary>
        public void SetClothes(Client player, ClothesModel clothes) {
            if (clothes.Torso.HasValue) {
                API.setPlayerClothes(player, 3, clothes.Torso.Value, 0);
            }
            if (clothes.Undershirt.HasValue) {
                API.setPlayerClothes(player, 8, clothes.Undershirt.Value, 0);
            }
            if (clothes.IsClothes) {
                API.setPlayerClothes(player, clothes.Slot, clothes.Variation, clothes.Texture);
            }
            else {
                API.setPlayerAccessory(player, clothes.Slot, clothes.Variation, clothes.Texture);
            }
        }

        /// <summary>
        /// Установить одежду
        /// </summary>
        public void SetClothes(Client player, ICollection<ClothesModel> clothes) {
            foreach (var good in clothes) {
                SetClothes(player, good);
            }
        }
    }
}