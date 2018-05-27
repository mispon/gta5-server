using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Models.Shops;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using Newtonsoft.Json;

namespace gta_mp_server.Managers.Places.ClothesShop {
    /// <summary>
    /// Магазин одежды
    /// </summary>
    internal class ClothesShop : Place {
        private readonly IDoormanager _doormanager;
        private readonly IPointCreator _pointCreator;
        private readonly IPlayerInfoManager _playerInfoManager;

        public ClothesShop() {}
        public ClothesShop(IDoormanager doormanager, IPointCreator pointCreator, IPlayerInfoManager playerInfoManager) {
            _doormanager = doormanager;
            _pointCreator = pointCreator;
            _playerInfoManager = playerInfoManager;
        }

        /// <summary>
        /// Инизиализировать место
        /// </summary>
        public override void Initialize() {
            foreach (var shop in PositionsGetter.GetShops()) {
                RegisterDoors(shop);
                _pointCreator.CreateBlip(shop.BlipPosition, shop.Blip, 
                    shop.Type == ClothesShopType.SubUrban ? 25 : 63, name: shop.Type.GetDescription()
                );
                CreateSeller(shop);
            }
        }

        /// <summary>
        /// Зарегистрировать двери магазина
        /// </summary>
        private void RegisterDoors(ClothesShopModel shop) {
            var leftDoorId = _doormanager.Register(shop.DoorId, shop.LeftDoorPosition);
            _doormanager.SetDoorState(leftDoorId, false, 0);
            if (shop.Type == ClothesShopType.Ponsonbys) {
                var rightDoorId = _doormanager.Register(shop.DoorId, shop.RightDoorPosition);
                _doormanager.SetDoorState(rightDoorId, false, 0);
            }
        }

        /// <summary>
        /// Создать продавца
        /// </summary>
        private void CreateSeller(ClothesShopModel shop) {
            var seller = _pointCreator.CreatePed(shop.Seller, "Продавец", shop.SellerPosition, shop.SellerRotation, shop.MarkerPosition, Colors.VividCyan);
            seller.ColShape.onEntityEnterColShape += (shape, entity) => OnPlayerComeToSeller(entity, shop);
            seller.ColShape.onEntityExitColShape += OnPlayerAwayFromSeller;
        }

        /// <summary>
        /// Игрок подошел к продавцу
        /// </summary>
        private void OnPlayerComeToSeller(NetHandle handle, ClothesShopModel shop) {
            var player = API.getPlayerFromHandle(handle);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            var clothes = ClothesGetter.GetShopClothes(shop.Type, _playerInfoManager.IsMale(player));
            API.triggerClientEvent(player, ServerEvent.SHOW_CLOTHES_MENU, 
                (int) shop.Type, JsonConvert.SerializeObject(shop.DressingRoom), JsonConvert.SerializeObject(clothes), shop.District
            );
        }

        /// <summary>
        /// Игрок отошел от продавца
        /// </summary>
        private void OnPlayerAwayFromSeller(ColShape shape, NetHandle handle) {
            var player = API.getPlayerFromHandle(handle);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_CLOTHES_MENU);
        }
    }
}