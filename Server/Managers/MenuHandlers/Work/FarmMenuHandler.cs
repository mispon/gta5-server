using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Farmer;
using gta_mp_server.Managers.Work.Interfaces;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.MenuHandlers.Work {
    /// <summary>
    /// Обработчик меню фермы
    /// </summary>
    internal class FarmMenuHandler : BaseWorkMenu {
        private const int TRACTOR_MIN_LEVEL = 2;

        private readonly IWorkEquipmentManager _workEquipmentManager;

        public FarmMenuHandler() {}
        public FarmMenuHandler(IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager,
            IWorkEquipmentManager workEquipmentManager) : base(playerInfoManager, workInfoManager) {
            _workEquipmentManager = workEquipmentManager;
        }

        /// <summary>
        /// Инициализировать обработчик меню работы
        /// </summary>
        public override void Initialize() {
            ClientEventHandler.Add(ClientEvent.WORK_AS_FARMER, StartFarmerWork);
            ClientEventHandler.Add(ClientEvent.WORK_AS_TRACTOR_DRIVER, StartTractorWork);
            ClientEventHandler.Add(ClientEvent.FARMER_SALARY, GetSalary);
        }

        /// <summary>
        /// Начать работу фермером
        /// </summary>
        private void StartFarmerWork(Client player, object[] args) {
            WorkInfoManager.CreateInfoIfNeed(player, WorkType.Farmer);
            if (HasActiveWork(player)) {
                return;
            }
            WorkInfoManager.SetActivity(player, WorkType.Farmer, true);
            player.setData(WorkData.IS_FARMER, true);
            FarmerManager.ShowNextPoint(player);
            _workEquipmentManager.SetEquipment(player);
            API.sendNotificationToPlayer(player, "Вы начали работу ~b~фермером");
            API.triggerClientEvent(player, ServerEvent.HIDE_FARM_MENU);
        }

        /// <summary>
        /// Начать работу трактористом
        /// </summary>
        private void StartTractorWork(Client player, object[] args) {
            WorkInfoManager.CreateInfoIfNeed(player, WorkType.TractorDriver);
            if (!CanWork(player, TRACTOR_MIN_LEVEL) || HasActiveWork(player)) {
                return;
            }
            WorkInfoManager.SetActivity(player, WorkType.TractorDriver, true);
            player.setData(WorkData.IS_TRACTOR_DRIVER, true);
            _workEquipmentManager.SetEquipment(player);
            API.sendNotificationToPlayer(player, "Вы начали работу ~b~трактористом");
            API.triggerClientEvent(player, ServerEvent.SHOW_SUBTITLE, "Найдите ~y~трактор ~w~поблизости");
            API.triggerClientEvent(player, ServerEvent.HIDE_FARM_MENU);
        }

        /// <summary>
        /// Получить зарплату
        /// </summary>
        private void GetSalary(Client player, object[] args) {
            var activeWork = WorkInfoManager.GetActiveWork(player);
            bool TypeChecker() => activeWork.Type == WorkType.Farmer || activeWork.Type == WorkType.TractorDriver;
            if (!WorkIsCorrect(player, activeWork, TypeChecker)) {
                return;
            }
            FarmerManager.PutHarvest(player);
            WorkInfoManager.SetActivity(player, activeWork.Type, false);
            player.resetData(WorkData.IS_FARMER);
            player.resetData(WorkData.IS_TRACTOR_DRIVER);
            player.resetData(FarmerManager.FARMER_POINT_KEY);
            player.resetData(TractorDriverManager.TRACTOR_POINT_KEY);
            PlayerInfoManager.SetPlayerClothes(player);
            PayOut(player, activeWork);
            API.triggerClientEvent(player, ServerEvent.HIDE_FARMER_POINT);
            API.triggerClientEvent(player, ServerEvent.HIDE_FARMER_END_POINT);
            API.triggerClientEvent(player, ServerEvent.HIDE_FARM_MENU);
        }
    }
}