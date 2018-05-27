using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Builder;
using gta_mp_server.Managers.Work.Interfaces;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.MenuHandlers.Work {
    /// <summary>
    /// Обработчик меню работы на стройке
    /// </summary>
    internal class BuilderMenuHandler : BaseWorkMenu {
        private const int MIN_LEVEL = 3;

        private readonly IWorkEquipmentManager _workEquipmentManager;

        public BuilderMenuHandler() {}
        public BuilderMenuHandler(IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager,
            IWorkEquipmentManager workEquipmentManager) : base(playerInfoManager, workInfoManager) {
            _workEquipmentManager = workEquipmentManager;
        }

        /// <summary>
        /// Инициализировать меню работы строителем
        /// </summary>
        public override void Initialize() {
            ClientEventHandler.Add(ClientEvent.WORK_AS_BUILDER, StartWork);
            ClientEventHandler.Add(ClientEvent.BUILDER_SALARY, GetSalary);
        }

        /// <summary>
        /// Обработчик начала работы
        /// </summary>
        private void StartWork(Client player, object[] args) {
            WorkInfoManager.CreateInfoIfNeed(player, WorkType.Builder);
            if (!CanWork(player, MIN_LEVEL, false) || HasActiveWork(player)) {
                return;
            }
            WorkInfoManager.SetActivity(player, WorkType.Builder, true);
            player.setSyncedData(WorkData.IS_BUILDER, true);
            BuilderManager.ShowNextPoint(player);
            _workEquipmentManager.SetEquipment(player);
            API.sendNotificationToPlayer(player, "Вы начали работу ~b~строителем");
            API.triggerClientEvent(player, ServerEvent.HIDE_BUILDER_MENU);
        }

        /// <summary>
        /// Получение зарплаты
        /// </summary>
        private void GetSalary(Client player, object[] args) {
            var activeWork = WorkInfoManager.GetActiveWork(player);
            bool TypeChecker() => activeWork.Type == WorkType.Builder;
            if (!WorkIsCorrect(player, activeWork, TypeChecker)) {
                return;
            }
            WorkInfoManager.SetActivity(player, activeWork.Type, false);
            player.resetData(BuilderManager.BUILDER_POINT_KEY);
            player.resetSyncedData(WorkData.IS_BUILDER);
            PlayerInfoManager.SetPlayerClothes(player);
            PayOut(player, activeWork);
            API.triggerClientEvent(player, ServerEvent.HIDE_BUILDER_POINT);
            API.triggerClientEvent(player, ServerEvent.HIDE_BUILDER_MENU);
        }
    }
}