using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Events.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Police;
using gta_mp_server.Managers.Work.Police.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Events {
    /// <summary>
    /// Обработка получения повреждений игроком
    /// </summary>
    internal class PlayerDamagedManager : Script, IPlayerDamagedManager {
        private const int POLICEMAN_PENALTY = -50;

        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IWorkInfoManager _workInfoManager;
        private readonly IPoliceAlertManager _policeAlertManager;

        public PlayerDamagedManager() {}
        public PlayerDamagedManager(IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager, IPoliceAlertManager policeAlertManager) {
            _playerInfoManager = playerInfoManager;
            _workInfoManager = workInfoManager;
            _policeAlertManager = policeAlertManager;
        }

        /// <summary>
        /// Обработчик получения повреждений
        /// </summary>
        public void OnPlayerDamaged(Client player, Client enemy, int cause, int id) {
            if (player.health <= 0 || IsEnemyIncorrect(enemy)) {
                return;
            }
            if (enemy.hasData(WorkData.IS_POLICEMAN)) {
                var playerInfo = _playerInfoManager.GetInfo(player);
                if (playerInfo.Wanted.WantedLevel == 0) {
                    API.sendNotificationToPlayer(enemy, "~b~Постарайтесь не причинять вред игроку");
                    _workInfoManager.SetSalary(enemy, WorkType.Police, POLICEMAN_PENALTY);
                }
                return;
            }
            _policeAlertManager.SendAlert(enemy.position, PoliceAlertManager.HASSLE_ALERT);
            var info = _playerInfoManager.GetInfo(enemy);
            var oldWanted = info.Wanted.WantedLevel;
            info.Wanted.Beatings += 1;
            _playerInfoManager.RefreshUI(enemy, info);
            if (oldWanted != info.Wanted.WantedLevel) API.sendNotificationToPlayer(enemy, $"~m~Уровень розыска увеличен до: {info.Wanted.WantedLevel}");
        }

        /// <summary>
        /// Проверяет, не является ли враг некорректным для обработки
        /// </summary>
        private static bool IsEnemyIncorrect(Client enemy) {
            return enemy == null || enemy.hasData(PlayerData.ON_EVENT) || enemy.hasData(PlayerData.FIGHTER);
        }
    }
}