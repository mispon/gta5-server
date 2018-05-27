using System;
using System.Collections.Generic;
using gta_mp_data.Enums;
using gta_mp_database.Models.Work;
using gta_mp_server.Global;
using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.MenuHandlers.Interfaces {
    /// <summary>
    /// Базовый класс меню работ
    /// </summary>
    internal abstract class BaseWorkMenu : Script, IMenu {
        protected readonly IPlayerInfoManager PlayerInfoManager;
        protected readonly IWorkInfoManager WorkInfoManager;

        protected BaseWorkMenu() {}
        protected BaseWorkMenu(IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager) {
            PlayerInfoManager = playerInfoManager;
            WorkInfoManager = workInfoManager;
        }

        /// <summary>
        /// Инициализировать обработчик меню работы
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Выдать зарплату
        /// </summary>
        protected void PayOut(Client player, WorkInfo activeWork) {
            var workInfo = WorkInfoManager.GetWorkInfo(player, activeWork.Type);
            PlayerInfoManager.SetBalance(player, workInfo.Salary);
            workInfo.Salary = 0;
            WorkInfoManager.SetWorkInfo(player, workInfo);
        }

        /// <summary>
        /// Проверить, что у игрока нет активной работы
        /// </summary>
        protected bool HasActiveWork(Client player, List<WorkType> allowedTypes = null) {
            var activeWork = WorkInfoManager.GetActiveWork(player);
            if (activeWork == null || allowedTypes != null && allowedTypes.Contains(activeWork.Type)) {
                return false;
            }
            API.sendNotificationToPlayer(player, $"~r~Вы уже работаете. Активная: {activeWork.Type.GetDescription()}", true);
            return true;
        }

        /// <summary>
        /// Проверить, что игрок завершает смену с корректной работой
        /// </summary>
        protected bool WorkIsCorrect(Client player, WorkInfo activeWork, Func<bool> typeChecker) {
            if (activeWork == null) {
                API.sendNotificationToPlayer(player, "~r~Вы нигде не работаете");
                return false;
            }
            if (!typeChecker()) {
                API.sendNotificationToPlayer(player, $"~r~Вы работаете в др. месте. Активная работа: {activeWork.Type.GetDescription()}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Может ли игрок работать на выбранной работе
        /// </summary>
        protected virtual bool CanWork(Client player, int minLevel, bool needDrive = true) {
            var playerInfo = PlayerInfoManager.GetInfo(player);
            var result = true;
            if (needDrive && !playerInfo.Driver.CanDriveB) {
                API.sendNotificationToPlayer(player, "~r~Необходимо получить водительскую лицензию", true);
                result = false;
            }
            if (playerInfo.Level < minLevel) {
                API.sendNotificationToPlayer(player, $"~r~Необходим {minLevel}-й уровень и выше", true);
                result = false;
            }
            return result;
        }
    }
}