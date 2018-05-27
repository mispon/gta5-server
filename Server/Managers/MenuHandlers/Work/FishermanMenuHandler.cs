using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Fisherman;
using gta_mp_server.Managers.Work.Interfaces;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.MenuHandlers.Work {
    /// <summary>
    /// Обработчик меню рыбаков
    /// </summary>
    internal class FishermanMenuHandler : BaseWorkMenu {
        private const int MIN_LEVEL = 4;
        private const int BAITS_COUNT = 50;

        private readonly IWorkEquipmentManager _workEquipmentManager;

        public FishermanMenuHandler() {}
        public FishermanMenuHandler(IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager,
            IWorkEquipmentManager workEquipmentManager) : base(playerInfoManager, workInfoManager) {
            _workEquipmentManager = workEquipmentManager;
        }

        /// <summary>
        /// Инициализировать меню работы рыбаком
        /// </summary>
        public override void Initialize() {
            ClientEventHandler.Add(ClientEvent.WORK_AS_FISHERMAN, StartWork);
            ClientEventHandler.Add(ClientEvent.BUY_FISH_BAITS, BuyFishBaits);
            ClientEventHandler.Add(ClientEvent.FISHERMAN_SALARY, GetSalary);
        }

        /// <summary>
        /// Обработчик начала работы
        /// </summary>
        private void StartWork(Client player, object[] args) {
            var onBoat = (bool) args[0];
            WorkInfoManager.CreateInfoIfNeed(player, WorkType.Fisherman);
            if (!CanWork(player, MIN_LEVEL, false) || HasActiveWork(player)) {
                return;
            }
            var work = WorkInfoManager.GetWorkInfo(player, WorkType.Fisherman);
            if (onBoat && work.Level < 3) {
                API.sendNotificationToPlayer(player, "~r~Необходимый уровень работы 3 и выше", true);
                return;
            }
            WorkInfoManager.SetActivity(player, WorkType.Fisherman, true);
            player.setSyncedData(onBoat ? WorkData.IS_FISHERMAN_ON_BOAT : WorkData.IS_FISHERMAN, true);
            if (!player.hasData(FishermanManager.FISH_BAITS_COUNT)) {
                player.setData(FishermanManager.FISH_BAITS_COUNT, 0);
            }
            player.setData(FishermanManager.POINT_BAITS_COUNT, 0);
            FishermanManager.ShowNextPoint(player);
            _workEquipmentManager.SetEquipment(player);
            API.sendNotificationToPlayer(player, "Вы начали работу ~b~рыбаком");
            API.triggerClientEvent(player, ServerEvent.HIDE_FISHERMAN_MENU);
        }

        /// <summary>
        /// Обработчик покупки наживки
        /// </summary>
        private void BuyFishBaits(Client player, object[] args) {
            var price = (int) args[0];
            var playerInfo = PlayerInfoManager.GetInfo(player);
            if (!PlayerHelper.EnoughMoney(player, playerInfo, price)) {
                return;
            }
            playerInfo.Balance -= price;
            if (player.hasData(FishermanManager.FISH_BAITS_COUNT)) {
                var currentCount = (int) player.getData(FishermanManager.FISH_BAITS_COUNT);
                player.setData(FishermanManager.FISH_BAITS_COUNT, currentCount + BAITS_COUNT);
            }
            else {
                player.setData(FishermanManager.FISH_BAITS_COUNT, BAITS_COUNT);
            }
            PlayerInfoManager.RefreshUI(player, playerInfo);
            API.sendNotificationToPlayer(player, "~b~Приобретена наживка");
        }

        /// <summary>
        /// Получение зарплаты
        /// </summary>
        private void GetSalary(Client player, object[] args) {
            var activeWork = WorkInfoManager.GetActiveWork(player);
            bool TypeChecker() => activeWork.Type == WorkType.Fisherman;
            if (!WorkIsCorrect(player, activeWork, TypeChecker)) {
                return;
            }
            WorkInfoManager.SetActivity(player, activeWork.Type, false);
            player.resetData(FishermanManager.FISHERMAN_POINT_KEY);
            player.resetSyncedData(WorkData.IS_FISHERMAN);
            player.resetSyncedData(WorkData.IS_FISHERMAN_ON_BOAT);
            PlayerInfoManager.SetPlayerClothes(player);
            PayOut(player, activeWork);
            API.triggerClientEvent(player, ServerEvent.HIDE_FISHERMAN_POINT);
            API.triggerClientEvent(player, ServerEvent.HIDE_FISHERMAN_MENU);
        }
    }
}