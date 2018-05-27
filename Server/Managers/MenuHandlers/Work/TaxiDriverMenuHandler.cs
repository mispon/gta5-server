using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Interfaces;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.MenuHandlers.Work {
    /// <summary>
    /// Обработчик меню работы таксистом
    /// </summary>
    internal class TaxiDriverMenuHandler : BaseWorkMenu {
        private const int MIN_LEVEL = 3;

        private readonly IWorkEquipmentManager _workEquipmentManager;

        public TaxiDriverMenuHandler() {}
        public TaxiDriverMenuHandler(IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager, IWorkEquipmentManager workEquipmentManager)
            : base(playerInfoManager, workInfoManager) {
            _workEquipmentManager = workEquipmentManager;
        }

        /// <summary>
        /// Инициализировать меню работы
        /// </summary>
        public override void Initialize() {
            ClientEventHandler.Add(ClientEvent.WORK_IN_TAXI, WorkInTaxi);
            ClientEventHandler.Add(ClientEvent.TAXI_DRIVER_SALARY, GetSalary);
        }

        /// <summary>
        /// Начать работу в такси
        /// </summary>
        private void WorkInTaxi(Client player, object[] objects) {
            WorkInfoManager.CreateInfoIfNeed(player, WorkType.TaxiDriver);
            if (HasActiveWork(player) || !CanWork(player, MIN_LEVEL)) {
                return;
            }
            WorkInfoManager.SetActivity(player, WorkType.TaxiDriver, true);
            player.setData(WorkData.IS_TAXI_DIVER, true);
            _workEquipmentManager.SetEquipment(player);
            API.triggerClientEvent(player, ServerEvent.SHOW_SUBTITLE, "Чтобы начать работу, ~y~сядьте в машину");
            API.triggerClientEvent(player, ServerEvent.HIDE_SIEMON_MENU);
        }

        /// <summary>
        /// Завершить работу и получить зарплату
        /// </summary>
        private void GetSalary(Client player, object[] objects) {
            var activeWork = WorkInfoManager.GetActiveWork(player);
            if (!WorkIsCorrect(player, activeWork, () => activeWork.Type == WorkType.TaxiDriver)) {
                return;
            }
            WorkInfoManager.SetActivity(player, WorkType.TaxiDriver, false);
            player.setData(WorkData.IS_TAXI_DIVER, null);
            PlayerInfoManager.SetPlayerClothes(player);
            PayOut(player, activeWork);
            API.triggerClientEvent(player, ServerEvent.HIDE_SIEMON_MENU);
        }
    }
}