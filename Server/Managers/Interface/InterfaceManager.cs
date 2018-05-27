using gta_mp_data.Enums;
using gta_mp_database.Models.Player;
using gta_mp_database.Models.Work;
using gta_mp_server.Clan;
using gta_mp_server.Constant;
using gta_mp_server.GameEvents;
using gta_mp_server.Global;
using gta_mp_server.Managers.Interface.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Police.Data;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using Newtonsoft.Json;

namespace gta_mp_server.Managers.Interface {
    /// <summary>
    /// Обработчик событий интерфейса
    /// </summary>
    internal class InterfaceManager : Script, IInterfaceManager {
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IWorkInfoManager _workInfoManager;

        public InterfaceManager() {}
        public InterfaceManager(IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager) {
            _playerInfoManager = playerInfoManager;
            _workInfoManager = workInfoManager;
        }

        /// <summary>
        /// Инициализировать интерфейсные обработчики
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.GET_PLAYER_INFO, GetPlayerInfo);
            ClientEventHandler.Add(ClientEvent.SAVE_SETTINGS, SaveSettings);
        }

        /// <summary>
        /// Отобращает информацию игрока
        /// </summary>
        private void GetPlayerInfo(Client player, object[] args) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            var activeWork = _workInfoManager.GetActiveWork(player);
            var info = CreateShortPlayerInfo(playerInfo, activeWork);
            var events = EventsScheduler.ActiveEvent?.GetInfo(player);
            API.triggerClientEvent(player, ServerEvent.SHOW_PLAYER_INFO, JsonConvert.SerializeObject(info), JsonConvert.SerializeObject(events));
        }

        /// <summary>
        /// Сохранить настройки игрока
        /// </summary>
        private void SaveSettings(Client player, object[] args) {
            var value = (bool) args[0];
            var playerInfo = _playerInfoManager.GetInfo(player);
            playerInfo.Settings.SvgSpeedometer = value;
            _playerInfoManager.RefreshUI(player, playerInfo);
        }

        /// <summary>
        /// Возвращает основную информацию об игроке
        /// </summary>
        private object CreateShortPlayerInfo(PlayerInfo playerInfo, WorkInfo activeWork) {
            var hasWork = activeWork != null;
            var workLevelInfo = string.Empty;
            if (hasWork) {
                workLevelInfo = activeWork.Type == WorkType.Police ? PoliceDataGetter.RankNames[activeWork.Level] : activeWork.Level.ToString();
            }
            var info = new {
                playerInfo.Name,
                TagName = playerInfo.Clan != null ? ClanManager.GetClanName(playerInfo.Clan.ClanId) : null,
                playerInfo.Balance,
                playerInfo.Level,
                playerInfo.Experience,
                Driver = playerInfo.Driver.CanDriveB,
                Work = hasWork ? activeWork.Type.GetDescription() : string.Empty,
                WorkLevel = workLevelInfo,
                WorkExp = hasWork ? activeWork.Experience : 0,
                Salary = hasWork ? activeWork.Salary : 0,
                Wanted = playerInfo.Wanted.WantedLevel,
                playerInfo.PhoneNumber,
                playerInfo.PhoneBalance,
                playerInfo.Settings,
                ClanRank = playerInfo.Clan?.Rank.GetDescription(),
                ClanRep = playerInfo.Clan?.Reputation
            };
            return info;
        }
    }
}