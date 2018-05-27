using System.Collections.Generic;
using System.Linq;
using gta_mp_data.Entity;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Interfaces;
using gta_mp_server.Managers.Work.Police;
using gta_mp_server.Managers.Work.Police.Data;
using gta_mp_server.Managers.Work.Police.Interfaces;
using GrandTheftMultiplayer.Server.Elements;
using PlayerInfo = gta_mp_database.Models.Player.PlayerInfo;

namespace gta_mp_server.Managers.MenuHandlers.Work {
    /// <summary>
    /// Обработчик меню работы в полиции
    /// </summary>
    internal class PoliceMenuHandler : BaseWorkMenu {
        private const int MIN_LEVEL = 9;
        private const int MIN_LEVEL_TO_LICENCE = 6;

        private readonly IWorkEquipmentManager _workEquipmentManager;
        private readonly IInventoryManager _inventoryManager;
        private readonly IPoliceAlertManager _policeAlertManager;

        public PoliceMenuHandler() {}
        public PoliceMenuHandler(IWorkEquipmentManager workEquipmentManager, IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager,
            IInventoryManager inventoryManager, IPoliceAlertManager policeAlertManager): base(playerInfoManager, workInfoManager) {
            _workEquipmentManager = workEquipmentManager;
            _inventoryManager = inventoryManager;
            _policeAlertManager = policeAlertManager;
        }

        /// <summary>
        /// Инициализировать меню работы
        /// </summary>
        public override void Initialize() {
            ClientEventHandler.Add(ClientEvent.WORK_IN_POLICE, WorkInPolice);
            ClientEventHandler.Add(ClientEvent.POLICE_SALARY, GetSalary);
            ClientEventHandler.Add(ClientEvent.PAY_PENALTY, PayPenalty);
            ClientEventHandler.Add(ClientEvent.BUY_WEAPON_LICENSE, BuyWeaponLicense);
        }

        /// <summary>
        /// Начать работу в полиции
        /// </summary>
        private void WorkInPolice(Client player, object[] args) {
            WorkInfoManager.CreateInfoIfNeed(player, WorkType.Police);
            if (HasActiveWork(player) || !CanWork(player, MIN_LEVEL)) {
                return;
            }
            var playerInfo = PlayerInfoManager.GetInfo(player);
            if (playerInfo.Clan != null) {
                API.sendNotificationToPlayer(player, "~r~Вы являетесь членом банды", true);
                return;
            }
            WorkInfoManager.SetActivity(player, WorkType.Police, true);
            player.setData(WorkData.IS_POLICEMAN, true);
            player.setSyncedData("HasPoliceActions", true);
            _workEquipmentManager.SetEquipment(player);
            _policeAlertManager.ShowAllAlerts(player);
            SetPolicemanName(player, playerInfo);
            player.setData(PoliceManager.STATE_KEY, (int) PolicemanState.NoPrisoner);
            API.triggerClientEvent(player, ServerEvent.HIDE_SARAH_MENU);
            API.sendNotificationToPlayer(player, "Вы начали работу ~b~в полиции");
        }

        /// <summary>
        /// Устанавливает имя полицейского
        /// </summary>
        private void SetPolicemanName(Client player, PlayerInfo playerInfo) {
            var workLevel = WorkInfoManager.GetActiveWork(player).Level;
            var rankName = PoliceDataGetter.RankNames[workLevel];
            API.setPlayerNametag(player, $"[{rankName}] {playerInfo.Name}");
            if (string.IsNullOrEmpty(playerInfo.TagColor)) {
                API.resetPlayerNametagColor(player);
                API.setPlayerNametagColor(player, 99, 154, 242);
            }
        }

        /// <summary>
        /// Получить зарплату
        /// </summary>
        private void GetSalary(Client player, object[] args) {
            var activeWork = WorkInfoManager.GetActiveWork(player);
            if (!WorkIsCorrect(player, activeWork, () => activeWork.Type == WorkType.Police)) {
                return;
            }
            WorkInfoManager.SetActivity(player, WorkType.Police, false);
            player.resetData(WorkData.IS_POLICEMAN);
            player.resetSyncedData("HasPoliceActions");
            PayOut(player, activeWork);
            PlayerInfoManager.SetPlayerClothes(player);
            PlayerManager.SetPlayerName(player, PlayerInfoManager.GetInfo(player));
            _inventoryManager.EquipWeapon(player);
            _policeAlertManager.HideAllAlerts(player);
            API.triggerClientEvent(player, ServerEvent.HIDE_SARAH_MENU);
        }

        /// <summary>
        /// Обработчик оплаты штрафа для снятия розыска
        /// </summary>
        private void PayPenalty(Client player, object[] objects) {
            var playerInfo = PlayerInfoManager.GetInfo(player);
            if (playerInfo.Wanted.WantedLevel == 0) {
                API.sendNotificationToPlayer(player, "~r~Вы не в розыске", true);
                return;
            }
            var penalty = PoliceManager.CalculatePenalty(playerInfo);
            if (playerInfo.Balance < penalty) {
                API.sendNotificationToPlayer(player, "~b~Недостаточно средств", true);
                return;
            }
            playerInfo.Balance -= penalty;
            PlayerInfoManager.RefreshUI(player, playerInfo);
            PlayerInfoManager.ClearWanted(player);
            API.sendNotificationToPlayer(player, "~b~Вы сняты с розыска");
        }

        /// <summary>
        /// Обработчик покупки лицензии на оружие
        /// </summary>
        private void BuyWeaponLicense(Client player, object[] args) {
            var price = (int) args[0];
            var playerInfo = PlayerInfoManager.GetInfo(player);
            if (!CanBuyLicense(player, playerInfo, price)) {
                return;
            }
            playerInfo.Balance -= price;
            var license = new InventoryItem {
                OwnerId = playerInfo.AccountId,
                Name = InventoryType.WeaponLicense.GetDescription(),
                Type = InventoryType.WeaponLicense,
                Count = 1,
                CountInHouse = 0,
                Model = (int) Validator.INVALID_ID
            };
            playerInfo.Inventory.Add(license);
            PlayerInfoManager.RefreshUI(player, playerInfo);
            API.sendNotificationToPlayer(player, "Приобретена ~b~лицензия ~w~на оружие");
        }

        /// <summary>
        /// Проверяет корректность покупки лицензии
        /// </summary>
        private bool CanBuyLicense(Client player, PlayerInfo playerInfo, int price) {
            var result = true;
            var messages = new List<string>();
            if (playerInfo.Level < MIN_LEVEL_TO_LICENCE) {
                messages.Add($"Необходимо достигнуть {5}-го уровня");
                result = false;
            }
            if (playerInfo.Wanted.WantedLevel > 0) {
                messages.Add("Вы находитесь в розыске");
                result = false;
            }
            if (playerInfo.Balance < price) {
                messages.Add("Недостаточно средств");
                result = false;
            }
            if (playerInfo.Inventory.Any(e => e.Type == InventoryType.WeaponLicense)) {
                messages.Add("У вас уже есть лицензия");
                result = false;
            }
            foreach (var message in messages) {
                API.sendNotificationToPlayer(player, $"~r~{message}", true);
            }
            return result;
        }
    }
}