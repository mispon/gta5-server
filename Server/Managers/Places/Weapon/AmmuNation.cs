using System.Linq;
using gta_mp_data.Entity;
using gta_mp_data.Enums;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Models.Shops;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using Newtonsoft.Json;
using PlayerInfo = gta_mp_database.Models.Player.PlayerInfo;

namespace gta_mp_server.Managers.Places.Weapon {
    /// <summary>
    /// 
    /// </summary>
    internal class AmmuNation : Place {
        private readonly IPointCreator _pointCreator;
        private readonly IDoormanager _doormanager;
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IInventoryManager _inventoryManager;
        private readonly IClanManager _clanManager;
        private readonly IInventoryHelper _inventoryHelper;

        public AmmuNation() {}
        public AmmuNation(IPointCreator pointCreator, IDoormanager doormanager, IPlayerInfoManager playerInfoManager,
            IInventoryManager inventoryManager, IClanManager clanManager, IInventoryHelper inventoryHelper) {
            _pointCreator = pointCreator;
            _doormanager = doormanager;
            _playerInfoManager = playerInfoManager;
            _inventoryManager = inventoryManager;
            _clanManager = clanManager;
            _inventoryHelper = inventoryHelper;
        }

        /// <summary>
        /// Инизиализировать место
        /// </summary>
        public override void Initialize() {
            ClientEventHandler.Add(ClientEvent.BUY_WEAPON, BuyWeapon);
            ClientEventHandler.Add(ClientEvent.BUY_AMMO, BuyAmmo);
            foreach (var shop in AmmuNationData.Shops) {
                _pointCreator.CreateBlip(shop.LeftDoorPosition, 110, 45, name: shop.Name);
                var leftDoodId = _doormanager.Register(97297972, shop.LeftDoorPosition);
                var rightDoorId = _doormanager.Register(-8873588, shop.RightDoorPosition);
                _doormanager.SetDoorState(leftDoodId, false, 1);
                _doormanager.SetDoorState(rightDoorId, false, 1);
                var seller = _pointCreator.CreatePed(
                    PedHash.Ammucity01SMY, "Продавец", shop.SellerPosition,
                    shop.SellerRotation, shop.Marker, Colors.VividCyan
                );
                seller.ColShape.onEntityEnterColShape += (shape, entity) => PlayerComeToSeller(entity, shop.District);
                seller.ColShape.onEntityExitColShape += PlayerAwayFromSeller;
            }
        }

        /// <summary>
        /// Покупка оружия
        /// </summary>
        private void BuyWeapon(Client player, object[] args) {
            var goodItem = JsonConvert.DeserializeObject<WeaponGood>(args[0].ToString());
            var playerInfo = _playerInfoManager.GetInfo(player);
            var district = (int) args[1];
            if (!HasMoney(player, playerInfo, goodItem.Price) || district != Validator.INVALID_ID && !HasLicense(player, playerInfo)) {
                return;
            }
            var item = CreateWeaponItem(playerInfo, goodItem);
            if (!_inventoryHelper.CanCarry(playerInfo.Inventory, item, item.Count)) {
                API.sendColoredNotificationToPlayer(player, "Превышена грузоподъемность персонажа", 0, 6);
                return;
            }
            playerInfo.Inventory.Add(item);
            playerInfo.Balance -= goodItem.Price;
            _playerInfoManager.RefreshUI(player, playerInfo);
            if (!player.hasData(WorkData.IS_POLICEMAN)) {
                _inventoryManager.EquipWeapon(player);
            }
            API.sendNotificationToPlayer(player, $"Приобретено ~b~\"{goodItem.Name}\"");
            if (district != Validator.INVALID_ID) {
                _clanManager.ReplenishClanBalance(district, goodItem.Price);
            }
        }

        /// <summary>
        /// Создает итем оружия
        /// </summary>
        private static InventoryItem CreateWeaponItem(PlayerInfo playerInfo, WeaponGood goodItem) {
            return new InventoryItem {
                OwnerId = playerInfo.AccountId,
                Name = goodItem.Name,
                Type = InventoryType.Weapon,
                Count = 1,
                CountInHouse = 0,
                Model = goodItem.Model
            };
        }

        /// <summary>
        /// Покупка патронов
        /// </summary>
        private void BuyAmmo(Client player, object[] args) {
            var ammoItem = JsonConvert.DeserializeObject<WeaponGood>(args[0].ToString());
            var count = (int) args[1];
            var district = (int) args[2];
            var playerInfo = _playerInfoManager.GetInfo(player);
            var price = ammoItem.Price * count;
            if (!HasMoney(player, playerInfo, price) || district != Validator.INVALID_ID && !HasLicense(player, playerInfo)) {
                return;
            }
            var item = CreateAmmoItem(playerInfo, ammoItem, count);
            if (!_inventoryHelper.CanCarry(playerInfo.Inventory, item, item.Count)) {
                API.sendColoredNotificationToPlayer(player, "Превышена грузоподъемность персонажа", 0, 6);
                return;
            }
            var ammo = playerInfo.Inventory.FirstOrDefault(e => e.Type == InventoryType.Ammo && e.Model == ammoItem.Model);
            if (ammo == null)
                playerInfo.Inventory.Add(item);
            else
                ammo.Count += count;
            playerInfo.Balance -= price;
            if (!player.hasData(WorkData.IS_POLICEMAN)) {
                _inventoryManager.EquipWeapon(player);
            }
            API.sendNotificationToPlayer(player, $"Приобретено ~b~\"{ammoItem.Name}\"~w~, ~b~{count} ~w~шт.");
            _playerInfoManager.RefreshUI(player, playerInfo);
            if (district != Validator.INVALID_ID) {
                _clanManager.ReplenishClanBalance(district, ammoItem.Price);
            }
        }

        /// <summary>
        /// Создает итем патронов
        /// </summary>
        private static InventoryItem CreateAmmoItem(PlayerInfo playerInfo, WeaponGood ammo, int count) {
            return new InventoryItem {
                OwnerId = playerInfo.AccountId,
                Name = ammo.Name,
                Type = InventoryType.Ammo,
                Count = count,
                CountInHouse = 0,
                Model = ammo.Model
            };
        }

        /// <summary>
        /// Проверяет наличие лицензии на покупку оружия
        /// </summary>
        private bool HasLicense(Client player, PlayerInfo playerInfo) {
            var license = playerInfo.Inventory.FirstOrDefault(e => e.Type == InventoryType.WeaponLicense);
            if (license == null) {
                API.sendNotificationToPlayer(player, "~r~У вас нет лицензии на покупку оружия", true);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Проверяет, что у игрока достаточно денег
        /// </summary>
        private bool HasMoney(Client player, PlayerInfo playerInfo, int price) {
            if (playerInfo.Balance < price) {
                API.sendNotificationToPlayer(player, "~r~Недостаточно средств", true);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Игрок подошел к продавцу
        /// </summary>
        private void PlayerComeToSeller(NetHandle entity, int district) {
            PlayerHelper.ProcessAction(entity, player => {
                API.triggerClientEvent(
                    player, ServerEvent.SHOW_AMMU_NATION_MENU,
                    JsonConvert.SerializeObject(AmmuNationData.Weapons),
                    JsonConvert.SerializeObject(AmmuNationData.Ammo),
                    district
                );
            });
        }

        /// <summary>
        /// Игрок отошел от продавца
        /// </summary>
        private void PlayerAwayFromSeller(ColShape shape, NetHandle entity) {
            PlayerHelper.ProcessAction(entity, player => {
                API.triggerClientEvent(player, ServerEvent.HIDE_AMMU_NATION_MENU);
            });
        }
    }
}