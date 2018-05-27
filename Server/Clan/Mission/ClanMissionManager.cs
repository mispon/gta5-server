using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Enums.Clan;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.IoC;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using Ninject;
using Ninject.Syntax;
using Object = GrandTheftMultiplayer.Server.Elements.Object;

namespace gta_mp_server.Clan.Mission {
    /// <summary>
    /// Логика миссий банды
    /// </summary>
    internal class ClanMissionManager : Script, IClanMissionManager {
        internal const int MISSION_DURATION = 2;
        private const int MEMBERS_TO_START = 2;

        private readonly IClanCourtyard _clanCourtyard;

        public ClanMissionManager() : this(ServerKernel.Kernel) {
            API.onPlayerEnterVehicle += OnPlayerEnterVehicle;
        }

        public ClanMissionManager(IResolutionRoot kernel) {
            _clanCourtyard = kernel.Get<IClanCourtyard>();
        }

        /// <summary>
        /// Активные миссии кланов
        /// </summary>
        internal static Dictionary<long, ClanMission> Missions = new Dictionary<long, ClanMission>();

        /// <summary>
        /// Обработчик входа в транспорт
        /// </summary>
        private void OnPlayerEnterVehicle(Client player, NetHandle vehicle, int seat) {
            if (!player.hasData(ClanMission.BOOTY_OBJECT)) return;
            DetachBooty(player);
        }

        /// <summary>
        /// Удаляет груз у игрока и возвращает на склад миссии
        /// </summary>
        internal static void DetachBooty(Client player) {
            var bootyObject = (Object)player.getData(ClanMission.BOOTY_OBJECT);
            var place = (MissionPlace)bootyObject.getData(ClanMission.PLACE_KEY);
            Missions.Values.FirstOrDefault(e => e.Place == place)?.RestoreBootyCount();
            API.shared.deleteEntity(bootyObject);
            API.shared.stopPlayerAnimation(player);
            player.resetData(ClanMission.BOOTY_OBJECT);
        }

        /// <summary>
        /// Игрок присоединяется к запуску миссии
        /// </summary>
        public void VoteToStart(long clanId) {
            ClanMission mission;
            if (Missions.ContainsKey(clanId)) {
                mission = Missions[clanId];
                mission.Members++;
            }
            else {
                mission = CreateMission();
                Missions.Add(clanId, mission);
            }
            if (mission.Members == MEMBERS_TO_START) {
                StartMission(mission, clanId);
            }
        }

        /// <summary>
        /// Игрок отказывается от запуска миссии
        /// </summary>
        public bool CancelVote(long clanId) {
            if (!Missions.ContainsKey(clanId)) {
                return false;
            }
            Missions[clanId].Members--;
            if (Missions[clanId].Members == 0) {
                Missions.Remove(clanId);
            }
            return true;
        }

        /// <summary>
        /// Возвращает количество проголосовавших участников
        /// </summary>
        internal static int GetMissionVotes(long clanId) {
            return Missions.ContainsKey(clanId) ? Missions[clanId].Members : 0;
        }

        /// <summary>
        /// Проверяет, есть ли запущенные миссии в данный момент
        /// </summary>
        internal static bool HasActiveMission() {
            return Missions.Values.Any(e => e.Active);
        }

        /// <summary>
        /// Создает миссию
        /// </summary>
        private static ClanMission CreateMission() {
            MissionPlace place;
            var places = Enum.GetValues(typeof(MissionPlace));
            do {
                place = (MissionPlace)places.GetValue(ActionHelper.Random.Next(places.Length));
            } while (place == MissionPlace.Unknown || Missions.Values.Any(e => e.Place == place));
            var mission = new ClanMission {Place = place};
            return mission;
        }

        /// <summary>
        /// Запускает миссию клана
        /// </summary>
        private void StartMission(ClanMission mission, long clanId) {
            if (!HasActiveMission()) {
                _clanCourtyard.ShowMarkers();
            }
            mission.Start();
            ClanManager.SetAuthority(clanId, -MissionMenuHandler.NEEDED_AUTHORITY);
            API.sendChatMessageToAll($"~b~\"{ClanManager.GetClanName(clanId)}\" ~w~запустили миссию ~b~\"{mission.Place.GetDescription()}\"");
            ActionHelper.SetAction(MISSION_DURATION * 3600000, () => FinishMission(clanId));
        }

        /// <summary>
        /// Завершает миссию клана
        /// </summary>
        private async void FinishMission(long clanId) {
            var mission = Missions[clanId];
            if (mission.Active) {
                mission.Finish();
            }
            API.sendChatMessageToAll($"Миссия ~b~\"{mission.Place.GetDescription()}\" ~w~завершилась");
            Missions.Remove(clanId);
            if (!HasActiveMission()) {
                _clanCourtyard.HideMarkers();
            }
            await RemoveMissionVans();
        }

        /// <summary>
        /// Удаляет заспавненные фургоны
        /// </summary>
        private async Task<bool> RemoveMissionVans() {
            var vehicles = API.getAllVehicles();
            foreach (var vehicle in vehicles) {
                if (!API.hasEntityData(vehicle, ClanCourtyard.MISSION_VANS_KEY) || API.getVehicleDriver(vehicle) != null) continue;
                API.deleteEntity(vehicle);
            }
            return await Task.FromResult(true);
        }
    }
}