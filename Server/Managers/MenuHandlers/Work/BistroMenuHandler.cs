using gta_mp_data.Enums;
using gta_mp_database.Models.Player;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Bistro;
using gta_mp_server.Managers.Work.Bistro.Interfaces;
using gta_mp_server.Managers.Work.Interfaces;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.MenuHandlers.Work {
    /// <summary>
    /// Обработчик менюшек закусочной
    /// </summary>
    internal class BistroMenuHandler : BaseWorkMenu {
        private const int MIN_LEVEL = 5;

        private readonly IWorkEquipmentManager _workEquipmentManager;
        private readonly IBistroManager _bistroManager;
        private readonly IClanManager _clanManager;

        public BistroMenuHandler() {}
        public BistroMenuHandler(IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager, IWorkEquipmentManager workEquipmentManager,
            IBistroManager bistroManager, IClanManager clanManager) : base(playerInfoManager, workInfoManager) {
            _workEquipmentManager = workEquipmentManager;
            _bistroManager = bistroManager;
            _clanManager = clanManager;
        }

        /// <summary>
        /// Инициализировать меню работы
        /// </summary>
        public override void Initialize() {
            ClientEventHandler.Add(ClientEvent.FOOD_TRUNK_DRIVER, FoodTrunkDriver);
            ClientEventHandler.Add(ClientEvent.FOOD_DELIVERYMAN, FoodDeliveryman);
            ClientEventHandler.Add(ClientEvent.BISTRO_SALARY, BistroSalary);
            ClientEventHandler.Add(ClientEvent.BUY_BISTRO_FOOD, BuyBistroFood);
        }

        /// <summary>
        /// Водитель фургона с едой
        /// </summary>
        private void FoodTrunkDriver(Client player, object[] args) {
            WorkInfoManager.CreateInfoIfNeed(player, WorkType.FoodTrunk);
            var truckRentCost = (int) args[0];
            if (!CanWork(player, MIN_LEVEL) || HasActiveWork(player) || !PayTrunkRent(player, truckRentCost)) {
                return;
            }
            WorkInfoManager.SetActivity(player, WorkType.FoodTrunk, true);
            player.setData(WorkData.IS_FOOD_TRUCK_DRIVER, true);
            _workEquipmentManager.SetEquipment(player);
            API.sendNotificationToPlayer(player, "Вы начали работу ~b~водителем фургона с едой");
            API.triggerClientEvent(player, ServerEvent.HIDE_BISTRO_MENU);
        }

        /// <summary>
        /// Оплата аренды фургона
        /// </summary>
        private bool PayTrunkRent(Client player, int rentCost) {
            var playerInfo = PlayerInfoManager.GetInfo(player);
            if (playerInfo.Balance < rentCost) {
                API.sendNotificationToPlayer(player, "~r~Недостаточно денег для аренды фургона", true);
                return false;
            }
            playerInfo.Balance -= rentCost;
            PlayerInfoManager.RefreshUI(player, playerInfo);
            return true;
        }

        /// <summary>
        /// Доставщик еды
        /// </summary>
        private void FoodDeliveryman(Client player, object[] args) {
            WorkInfoManager.CreateInfoIfNeed(player, WorkType.FoodDeliveryMan);
            if (!CanWork(player, MIN_LEVEL) || HasActiveWork(player)) {
                return;
            }
            WorkInfoManager.SetActivity(player, WorkType.FoodDeliveryMan, true);
            player.setData(WorkData.IS_FOOD_DELIVERYMAN, true);
            _workEquipmentManager.SetEquipment(player);
            API.sendNotificationToPlayer(player, "Вы начали работу ~b~доставщиком еды");
            API.triggerClientEvent(player, ServerEvent.SHOW_SUBTITLE, "Садитесь на ~y~скутер ~w~и двигайтесь по точкам заказов");
            API.triggerClientEvent(player, ServerEvent.HIDE_BISTRO_MENU);
        }

        /// <summary>
        /// Получение зарплаты
        /// </summary>
        private void BistroSalary(Client player, object[] args) {
            var activeWork = WorkInfoManager.GetActiveWork(player);
            bool TypeCheker() => activeWork.Type == WorkType.FoodTrunk || activeWork.Type == WorkType.FoodDeliveryMan;
            if (!WorkIsCorrect(player, activeWork, TypeCheker)) {
                return;
            }
            WorkInfoManager.SetActivity(player, activeWork.Type, false);
            player.resetData(WorkData.IS_FOOD_TRUCK_DRIVER);
            player.resetData(WorkData.IS_FOOD_DELIVERYMAN);
            player.resetSyncedData(BistroManager.POINT_NUMBER_KEY);
            API.setPlayerClothes(player, 5, 0, 0); // снимаем сумку
            PlayerInfoManager.SetPlayerClothes(player);
            PayOut(player, activeWork);
            API.triggerClientEvent(player, ServerEvent.STOP_TIMER);
            API.triggerClientEvent(player, ServerEvent.HIDE_BISTRO_MENU);
        }

        /// <summary>
        /// Покупка еды закусочной
        /// </summary>
        private void BuyBistroFood(Client player, object[] args) {
            var price = (int) args[0];
            var playerInfo = PlayerInfoManager.GetInfo(player);
            if (playerInfo.Balance < price) {
                API.sendNotificationToPlayer(player, "~r~Недостаточно денег", true);
                return;
            }
            var satiety = (int) args[1];
            var newSatiety = playerInfo.Satiety + satiety;
            playerInfo.Satiety = newSatiety <= PlayerInfo.MAX_VALUE ? newSatiety : PlayerInfo.MAX_VALUE;
            playerInfo.Balance -= price;
            PlayerInfoManager.RefreshUI(player, playerInfo);
            var truckDriverName = args[2].ToString();
            if (!string.IsNullOrEmpty(truckDriverName)) {
                _bistroManager.SetTruckDriverRevard(truckDriverName, price, playerInfo.Name);
            }
            API.sendNotificationToPlayer(player, $"Сытость восстановлена на ~b~{satiety} ед.");
            API.triggerClientEvent(player, ServerEvent.HIDE_BISTRO_FOOD_MENU);
            var street = (int) args[0];
            if (street != Validator.INVALID_ID) {
                _clanManager.ReplenishClanBalance(street, price);
            }
        }
    }
}