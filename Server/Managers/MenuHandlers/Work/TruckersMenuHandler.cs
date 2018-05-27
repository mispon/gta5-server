using System.Collections.Generic;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Interfaces;
using gta_mp_server.Managers.Work.Trucker;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.Elements;
using Newtonsoft.Json;

namespace gta_mp_server.Managers.MenuHandlers.Work {
    /// <summary>
    /// Обработчик меню дальнобойщиков
    /// </summary>
    internal class TruckersMenuHandler : BaseWorkMenu {
        private const int MIN_LEVEL = 7;

        private readonly IWorkEquipmentManager _workEquipmentManager;

        /// <summary>
        /// Инициализировать обработчик меню работы дальнобойщика
        /// </summary>
        public override void Initialize() {
            ClientEventHandler.Add(ClientEvent.CHOOSE_TRUCKER_CONTRACT, ChooseContract);
            ClientEventHandler.Add(ClientEvent.TRUCKER_SALARY, TruckerSalary);
        }

        public TruckersMenuHandler() {}
        public TruckersMenuHandler(IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager, IWorkEquipmentManager workEquipmentManager)
            : base(playerInfoManager, workInfoManager) {
            _workEquipmentManager = workEquipmentManager;
        }

        /// <summary>
        /// Выбор контракта на доставку груза
        /// </summary>
        private void ChooseContract(Client player, object[] args) {
            WorkInfoManager.CreateInfoIfNeed(player, WorkType.Trucker);
            if (!CanWork(player, MIN_LEVEL) || HasContract(player) || HasActiveWork(player, new List<WorkType> {WorkType.Trucker})) {
                return;
            }
            var contract = JsonConvert.DeserializeObject<DeliveryContract>(args[0].ToString());
            player.setSyncedData(WorkData.IS_TRUCKER, true);
            player.setData(WorkData.DELIVERY_CONTRACT, contract);
            player.setData(TruckersManager.TRUCKER_CONTRACT_TYPE, contract.Type);
            WorkInfoManager.SetActivity(player, WorkType.Trucker, true);
            _workEquipmentManager.SetEquipment(player);
            API.triggerClientEvent(player, ServerEvent.SHOW_SUBTITLE, "Чтобы выполнить контракт, найдите ~y~грузовик и прицеп");
            API.triggerClientEvent(player, ServerEvent.HIDE_TRUCKERS_MENU);
        }

        /// <summary>
        /// Проверяет, что у игрока нет контракта
        /// </summary>
        private bool HasContract(Client player) {
            if (player.hasData(WorkData.DELIVERY_CONTRACT)) {
                API.sendNotificationToPlayer(player, "~r~У вас уже есть контракт на доставку груза", true);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Получение зарплаты дальнобойщика
        /// </summary>
        private void TruckerSalary(Client player, object[] args) {
            var activeWork = WorkInfoManager.GetActiveWork(player);
            if (!WorkIsCorrect(player, activeWork, () => activeWork.Type == WorkType.Trucker)) {
                return;
            }
            WorkInfoManager.SetActivity(player, WorkType.Trucker, false);
            player.resetSyncedData(WorkData.IS_TRUCKER);
            player.resetData(WorkData.DELIVERY_CONTRACT);
            PayOut(player, activeWork);
            PlayerInfoManager.SetPlayerClothes(player);
            API.triggerClientEvent(player, ServerEvent.HIDE_TRUCKERS_MENU);
        }
    }
}