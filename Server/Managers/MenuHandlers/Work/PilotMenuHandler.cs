using System.Collections.Generic;
using System.Linq;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Global;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Interfaces;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.Elements;
using Newtonsoft.Json;

namespace gta_mp_server.Managers.MenuHandlers.Work {
    /// <summary>
    /// Обработчик меню пилота
    /// </summary>
    internal class PilotMenuHandler : BaseWorkMenu {
        private const int MIN_LEVEL = 8;

        /// <summary>
        /// Группы контрактов, которые открываются с прогрессом
        /// </summary>
        private static readonly DeliveryContractType[] _farmContracts = {DeliveryContractType.Fertilizer, DeliveryContractType.Crop};
        private static readonly DeliveryContractType[] _militaryContracts = {DeliveryContractType.MilitaryEquipment, DeliveryContractType.MilitaryReports};
        internal static readonly DeliveryContractType[] ClanFarmContracts = {DeliveryContractType.MoneyToFarm, DeliveryContractType.Marijuana};
        internal static readonly DeliveryContractType[] ClanMilitaryContracts = {DeliveryContractType.MoneyToMilitary, DeliveryContractType.SmugglingWeapon};

        private readonly IWorkEquipmentManager _workEquipmentManager;

        public PilotMenuHandler() { }
        public PilotMenuHandler(IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager, IWorkEquipmentManager workEquipmentManager)
            : base(playerInfoManager, workInfoManager) {
            _workEquipmentManager = workEquipmentManager;
        }

        /// <summary>
        /// Инициализировать обработчик меню работы
        /// </summary>
        public override void Initialize() {
            ClientEventHandler.Add(ClientEvent.CHOOSE_PILOT_CONTRACT, ChooseContract);
            ClientEventHandler.Add(ClientEvent.PILOT_SALARY, PilotSalary);
        }

        /// <summary>
        /// Выбор контракта на доставку груза
        /// </summary>
        private void ChooseContract(Client player, object[] args) {
            WorkInfoManager.CreateInfoIfNeed(player, WorkType.Pilot);
            if (!CanWork(player, MIN_LEVEL) || HasContract(player) || HasActiveWork(player, new List<WorkType> {WorkType.Pilot})) {
                return;
            }
            var contract = JsonConvert.DeserializeObject<DeliveryContract>(args[0].ToString());
            if (!AllowChooseContract(player, contract)) {
                return;
            }
            player.setSyncedData(WorkData.IS_PILOT, true);
            player.setData(WorkData.DELIVERY_CONTRACT, contract);
            WorkInfoManager.SetActivity(player, WorkType.Pilot, true);
            _workEquipmentManager.SetEquipment(player);
            API.triggerClientEvent(player, ServerEvent.SHOW_SUBTITLE, "Найдите подходящий самолет в ~y~ангаре ~w~или ~y~на улице");
            API.triggerClientEvent(player, ServerEvent.HIDE_PILOT_MENU);
        }

        /// <summary>
        /// Получение зарплаты летчика
        /// </summary>
        private void PilotSalary(Client player, object[] args) {
            var activeWork = WorkInfoManager.GetActiveWork(player);
            if (!WorkIsCorrect(player, activeWork, () => activeWork.Type == WorkType.Pilot)) {
                return;
            }
            WorkInfoManager.SetActivity(player, WorkType.Pilot, false);
            player.resetSyncedData(WorkData.IS_PILOT);
            player.resetData(WorkData.DELIVERY_CONTRACT);
            PayOut(player, activeWork);
            PlayerInfoManager.SetPlayerClothes(player);
            API.triggerClientEvent(player, ServerEvent.HIDE_PILOT_MENU);
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
        /// Проверяет, что игрок выбрал доступный контракт
        /// </summary>
        private bool AllowChooseContract(Client player, DeliveryContract contract) {
            var workLevel = WorkInfoManager.GetWorkInfo(player, WorkType.Pilot).Level;
            if (_farmContracts.Contains(contract.Type) && !LevelEnough(player, workLevel, 2) ||
                _militaryContracts.Contains(contract.Type) && !LevelEnough(player, workLevel, 3) ||
                ClanFarmContracts.Contains(contract.Type) && !ClanRankEnough(player, workLevel, 3, ClanRank.Low) ||
                ClanMilitaryContracts.Contains(contract.Type) && !ClanRankEnough(player, workLevel, 4, ClanRank.Middle)) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Проверяет, что игроку доступен контракт банды
        /// </summary>
        private bool ClanRankEnough(Client player, int workLevel, int minLevel, ClanRank minRank) {
            if (!LevelEnough(player, workLevel, minLevel)) {
                return false;
            }
            var clanInfo = PlayerInfoManager.GetInfo(player).Clan;
            if (clanInfo == null || clanInfo.Rank < minRank) {
                API.sendNotificationToPlayer(player, $"~r~Необходимый ранг в банде \"{minRank.GetDescription()}\" и выше", true);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Проверяет, что у игрока достаточный уровень работы
        /// </summary>
        private bool LevelEnough(Client player, int workLevel, int minLevel) {
            if (workLevel < minLevel) {
                API.sendNotificationToPlayer(player, $"~r~Для выполнения контракта необходим {minLevel} уровень работы", true);
                return false;
            }
            return true;
        }
    }
}