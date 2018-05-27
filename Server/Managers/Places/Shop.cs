using System.Collections.Generic;
using gta_mp_server.Constant;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Models.Places;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Places {
    /// <summary>
    /// Логика магазина
    /// </summary>
    internal class Shop : Place {
        private readonly IPointCreator _pointCreator;

        public Shop() {}
        public Shop(IPointCreator pointCreator) {
            _pointCreator = pointCreator;
        }

        /// <summary>
        /// Инизиализировать магазин
        /// </summary>
        public override void Initialize() {
            var shops = GetShops();
            foreach (var shop in shops) {
                var position = shop.Position;
                _pointCreator.CreateBlip(position, 52, 69, name: "Магазин 24/7");
                InitializeInside(shop);
            }
        }

        /// <summary>
        /// Инициализировать магазин внутри
        /// </summary>
        private void InitializeInside(ShopModel shop) {
            var pedPositions = PositionConverter.ToListVector3(shop.NpcPositions);
            var ped = _pointCreator.CreatePed(PedHash.Sweatshop01SFY, "Продавец", pedPositions[0], pedPositions[1], pedPositions[2], Colors.VividCyan);
            ped.ColShape.onEntityEnterColShape += (shape, entity) => PlayerComeToSeller(entity, shop.District);
            ped.ColShape.onEntityExitColShape += PlayerComeAwayFromSeller;
        }

        /// <summary>
        /// Игрок подошел к продавцу
        /// </summary>
        private void PlayerComeToSeller(NetHandle entity, int district) {
            var player = API.getPlayerFromHandle(entity);
            if (PlayerHelper.PlayerCorrect(player)) {
                API.triggerClientEvent(player, ServerEvent.SHOW_SHOP_MENU, district);
            }
        }

        /// <summary>
        /// Игрок отошел от продавца
        /// </summary>
        private void PlayerComeAwayFromSeller(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (PlayerHelper.PlayerCorrect(player)) {
                API.triggerClientEvent(player, ServerEvent.HIDE_SHOP_MENU);
            }
        }

        /// <summary>
        /// Метод-заглушка получения магазинов
        /// </summary>
        private static IEnumerable<ShopModel> GetShops() {
            return new List<ShopModel> {
                new ShopModel {
                    Position = new Vector3(1730.88, 6410.72, 35.0),
                    NpcPositions = "1727,81;6415,16;35,04|0,00;0,00;-118,98|1729,04;6414,51;34,14",
                    District = 9
                },
                new ShopModel {
                    Position = new Vector3(1965.45, 3740.15, 32.33),
                    NpcPositions = "1960,13;3739,93;32,34|0,00;0,00;-63,50|1961,43;3740,72;31,44",
                    District = 9
                },
                new ShopModel {
                    Position = new Vector3(2682.5, 3282.23, 55.24),
                    NpcPositions = "2678,13;3279,29;55,24|0,00;0,00;-28,27|2678,74;3280,60;54,34",
                    District = 9
                },
                new ShopModel {
                    Position = new Vector3(544.11, 2673.12, 42.16),
                    NpcPositions = "549,00;2671,66;42,16|0,00;0,00;100,31|547,64;2671,34;41,26",
                    District = 9
                },
                new ShopModel {
                    Position = new Vector3(-3239.26, 1004.41, 12.46),
                    NpcPositions = "-3242,14;999,93;12,83|0,00;0,00;-11,01|-3241,99;1001,40;11,93",
                    District = 8
                },
                new ShopModel {
                    Position = new Vector3(-3038.1, 589.76, 7.82),
                    NpcPositions = "-3038,89;584,59;7,91|0,00;0,00;9,31|-3039,24;585,91;7,01",
                    District = 8
                },
                new ShopModel {
                    Position = new Vector3(2559.80, 385.35, 108.62),
                    NpcPositions = "2557,45;380,74;108,62|0,00;0,00;2,17|2557,51;382,20;107,72",
                    District = 6
                },
                new ShopModel {
                    Position = new Vector3(376.51, 322.89, 103.57),
                    NpcPositions = "372,49;326,22;103,57|0,00;0,00;-109,11|373,83;325,83;102,67",
                    District = 6
                },
                new ShopModel {
                    Position = new Vector3(29.23, -1349.95, 29.33),
                    NpcPositions = "24,40;-1347,55;29,50|0,00;0,00;-87,47|25,89;-1347,50;28,60",
                    District = 10
                }
            };
        }
    }
}