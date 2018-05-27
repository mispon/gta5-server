using System;
using System.Collections.Generic;
using System.IO;
using gta_mp_database.Models.Player;
using gta_mp_server.Enums;
using Newtonsoft.Json;

namespace gta_mp_server.Managers.Places.ClothesShop {
    /// <summary>
    /// Вспомогательный класс для хранения информации об одежде
    /// </summary>
    internal class ClothesGetter {
        /// <summary>
        /// Sub Urban
        /// </summary>
        private static List<ClothesModel> MaleUrbanClothes => 
            _maleUrbanClothes ?? (_maleUrbanClothes = JsonConvert.DeserializeObject<List<ClothesModel>>(File.ReadAllText("data/male-urban.json")));

        private static List<ClothesModel> FemaleUrbanClothes =>
            _femaleUrbanClothes ?? (_femaleUrbanClothes = JsonConvert.DeserializeObject<List<ClothesModel>>(File.ReadAllText("data/female-urban.json")));

        private static List<ClothesModel> _maleUrbanClothes;
        private static List<ClothesModel> _femaleUrbanClothes;

        /// <summary>
        /// Ponsonbys
        /// </summary>
        private static List<ClothesModel> MalePonsonbysClothes =>
            _malePonsonbysClothes ?? (_malePonsonbysClothes = JsonConvert.DeserializeObject<List<ClothesModel>>(File.ReadAllText("data/male-ponsonbys.json")));

        private static List<ClothesModel> FemalePonsonbysClothes =>
            _femalePonsonbysClothes ?? (_femalePonsonbysClothes = JsonConvert.DeserializeObject<List<ClothesModel>>(File.ReadAllText("data/female-ponsonbys.json")));

        private static List<ClothesModel> _malePonsonbysClothes;
        private static List<ClothesModel> _femalePonsonbysClothes;

        /// <summary>
        /// Возвращает одежду для магазина Urban
        /// </summary>
        public static IEnumerable<ClothesModel> GetShopClothes(ClothesShopType type, bool isMale) {
            switch (type) {
                case ClothesShopType.SubUrban:
                    return isMale ? MaleUrbanClothes : FemaleUrbanClothes;
                case ClothesShopType.Ponsonbys:
                    return isMale ? MalePonsonbysClothes : FemalePonsonbysClothes;
                default:
                    throw new ArgumentException("Неизвестный тип магазина");
            }
        }
    }
}