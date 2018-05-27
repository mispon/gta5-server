using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_server.Constant;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.Work.Police.Data;
using gta_mp_server.Managers.Work.Police.Interfaces;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;

namespace gta_mp_server.Managers.Work.Police {
    /// <summary>
    /// Логика вызовов полиции
    /// </summary>
    internal class PoliceAlertManager : Script, IPoliceAlertManager {
        internal const string PATROL_ALERT = "Патрулирование";
        internal const string HASSLE_ALERT = "Драка";
        internal const string MURDER_ALERT = "Убийство";
        private const int MAX_FAKE_ALERTS = 5;

        private static readonly List<PoliceAlert> _policeAlerts = new List<PoliceAlert>();

        /// <summary>
        /// Запускает таймер, генерящий события патрулирования
        /// </summary>
        public void RunAlertsGenerator() {
            ActionHelper.StartTimer(120000, () => {
                var expiredAlerts = _policeAlerts.Where(e => (DateTime.Now - e.Date).TotalMinutes >= 30).ToList();
                foreach (var alert in expiredAlerts) {
                    FinishAlert(alert.Id);
                }
                if (_policeAlerts.Count < MAX_FAKE_ALERTS) {
                    var position = PoliceDataGetter.GetAlertPosition();
                    CreateAlert(position, PATROL_ALERT);
                }
            });
        }

        /// <summary>
        /// Показывает игроку все вызовы на карте
        /// </summary>
        public void ShowAllAlerts(Client player) {
            API.triggerClientEvent(player, ServerEvent.SHOW_ALL_ALERTS, JsonConvert.SerializeObject(_policeAlerts));
        }

        /// <summary>
        /// Скрывает все вызовы с карты
        /// </summary>
        public void HideAllAlerts(Client player) {
            API.triggerClientEvent(player, ServerEvent.HIDE_ALL_ALERTS);
        }

        /// <summary>
        /// Отправить вызов
        /// </summary>
        public void SendAlert(Vector3 position, string name) {
            var existAlert = _policeAlerts.FirstOrDefault(e => Vector3.Distance(e.Position, position) <= 50f);
            if (existAlert == null) {
                CreateAlert(position, name);
            }
            else {
                UpdateAlert(existAlert, position, name);
            }
        }

        /// <summary>
        /// Создать новый вызов
        /// </summary>
        private void CreateAlert(Vector3 position, string name) {
            var alert = new PoliceAlert(GetAlertId(), position, name);
            alert.CreateZone();
            _policeAlerts.Add(alert);
            SendAlertEvent(alert);
        }

        /// <summary>
        /// Обновить существующий вызов
        /// </summary>
        private void UpdateAlert(PoliceAlert existAlert, Vector3 position, string name) {
            existAlert.Position = position;
            if (name == MURDER_ALERT) {
                existAlert.Name = name;
            }
            API.deleteColShape(existAlert.Zone);
            existAlert.CreateZone();
            SendAlertEvent(existAlert, true);
        }

        /// <summary>
        /// Завершить вызов
        /// </summary>
        public void FinishAlert(int alertId) {
            var alert = _policeAlerts.FirstOrDefault(e => e.Id == alertId);
            if (alert == null) {
                return;
            }
            API.deleteColShape(alert.Zone);
            _policeAlerts.Remove(alert);
            SendFinishAlertEvent(alertId);
        }

        /// <summary>
        /// Уведомляет всех полицейских о новом вызове
        /// </summary>
        private void SendAlertEvent(PoliceAlert alert, bool isUpdate = false) {
            var message = $"~r~[ВЫЗОВ] ~b~Тип: \"{alert.Name}\"";
            foreach (var policeman in GetPolicemens()) {
                if (!isUpdate || alert.Name == MURDER_ALERT) {
                    API.sendNotificationToPlayer(policeman, message, true);
                }
                API.triggerClientEvent(policeman, isUpdate ? ServerEvent.UPDATE_ALERT : ServerEvent.CREATE_ALERT, JsonConvert.SerializeObject(alert));
            }
        }

        /// <summary>
        /// Удаляет завершенный вызов у копов
        /// </summary>
        private void SendFinishAlertEvent(int alertId) {
            foreach (var policeman in GetPolicemens()) {
                API.triggerClientEvent(policeman, ServerEvent.FINISH_ALERT, alertId);
            }
        }

        /// <summary>
        /// Возвращает идентификатор для нового вызова
        /// </summary>
        private static int GetAlertId() {
            return _policeAlerts.OrderByDescending(e => e.Id).FirstOrDefault()?.Id + 1 ?? 0;
        }

        /// <summary>
        /// Возвращает всех полицейских в игре
        /// </summary>
        private IEnumerable<Client> GetPolicemens() {
            return API.getAllPlayers().Where(e => e.hasData(WorkData.IS_POLICEMAN));
        }
    }
}