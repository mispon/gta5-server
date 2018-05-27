using System.Collections.Generic;
using System.Linq;
using gta_mp_data.Enums;
using gta_mp_database.Models.Work;
using gta_mp_server.Global;
using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
// ReSharper disable PossibleNullReferenceException

namespace gta_mp_server.Managers.Player {
    /// <summary>
    /// Логика работы с информацией о работах игрока
    /// </summary>
    internal class WorkInfoManager : Script, IWorkInfoManager {
        private const int MAX_WORK_LEVEL = 5;

        /// <summary>
        /// Добавляет игроку работу, если до этого ее еще не было
        /// </summary>
        public void CreateInfoIfNeed(Client player, WorkType type) {
            ServerState.Players.TryGetValue(player, out var playerInfo);
            if (!playerInfo.Works.TryGetValue(type, out WorkInfo info)) {
                info = new WorkInfo {
                    Type = type,
                    Level = 1,
                    Active = false,
                    Experience = 0,
                    Salary = 0
                };
                SetWorkInfo(player, info);
            }
        }

        /// <summary>
        /// Возвращает информацию о работе игрока
        /// </summary>
        public WorkInfo GetWorkInfo(Client player, WorkType type) {
            ServerState.Players.TryGetValue(player, out var playerInfo);
            return playerInfo.Works[type];
        }

        /// <summary>
        /// Записывает информацию о работе игрока
        /// </summary>
        public void SetWorkInfo(Client player, WorkInfo info) {
            ServerState.Players.TryGetValue(player, out var playerInfo);
            if (playerInfo.Works.ContainsKey(info.Type)) {
                playerInfo.Works[info.Type] = info;
            }
            else {
                playerInfo.Works.Add(info.Type, info);
            }
            ServerState.Players.TryUpdate(player, playerInfo, playerInfo);
        }

        /// <summary>
        /// Начислить зарплату
        /// </summary>
        public void SetSalary(Client player, WorkType type, int salary) {
            var workInfo = GetWorkInfo(player, type);
            if (ServerState.Players[player].IsPremium()) {
                salary += (int) (salary * 0.5);
            }
            workInfo.Salary += salary;
            SetWorkInfo(player, workInfo);
            var message = salary >= 0 ? $"~g~Зачислена зарплата: {salary}$" : $"~r~Получен штраф: {salary}$";
            API.sendNotificationToPlayer(player, message);
        }

        /// <summary>
        /// Устанавливает уровень работы
        /// </summary>
        public void SetExperience(Client player, WorkType type, int exp) {
            var workInfo = GetWorkInfo(player, type);
            if (workInfo.Level == MAX_WORK_LEVEL) {
                return;
            }
            if (ServerState.Players[player].IsPremium()) {
                exp += (int) (exp * 0.2);
            }
            var expToLevelUp = _experienceByLevel[workInfo.Level];
            var newExp = workInfo.Experience + exp;
            if (newExp >= expToLevelUp) {
                workInfo.Level += 1;
                workInfo.Experience = newExp - expToLevelUp;
                API.sendNotificationToPlayer(player, $"~b~Вы повысили уровень работы до {workInfo.Level}-го уровня");
            }
            else {
                workInfo.Experience += exp;
            }
            SetWorkInfo(player, workInfo);
        }

        /// <summary>
        /// Выставляет активность работы
        /// </summary>
        public void SetActivity(Client player, WorkType type, bool active) {
            ServerState.Players[player].Works[type].Active = active;
        }

        /// <summary>
        /// Возвращает название активной работы или пустую строку
        /// </summary>
        public WorkInfo GetActiveWork(Client player) {
            var playerWorks = ServerState.Players[player].Works.Select(e => e.Value).ToList();
            return playerWorks.FirstOrDefault(e => e.Active);
        }

        /// <summary>
        /// Проверяет, достаточный ли уровень работы игрока
        /// </summary>
        public bool WorkLevelEnough(Client player, WorkType type, int neededLevel) {
            if (!ServerState.Players[player].Works.TryGetValue(type, out var info)) {
                return false;
            }
            return info.Level >= neededLevel;
        }

        /// <summary>
        /// Уровень и кол-во опыта до следующего уровня
        /// </summary>
        // todo: вынести в конфиг
        private readonly Dictionary<int, int> _experienceByLevel = new Dictionary<int, int> {
            { 1, 250 },
            { 2, 550 },
            { 3, 750 },
            { 4, 1200 },
            { 5, 0 }
        };
    }
}