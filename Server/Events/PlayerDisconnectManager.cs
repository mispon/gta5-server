using System.Collections.Generic;
using gta_mp_database.Models.Player;
using gta_mp_database.Providers.Interfaces;
using gta_mp_server.Clan.Mission;
using gta_mp_server.Constant;
using gta_mp_server.Events.Interfaces;
using gta_mp_server.Helpers;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Player;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Races.Interfaces;
using gta_mp_server.Managers.Work.Police.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using Ninject;

namespace gta_mp_server.Events {
    /// <summary>
    /// Обработка выхода игрока с сервера
    /// </summary>
    internal class PlayerDisconnectManager : Script, IPlayerDisconnectManager {
        private readonly IAccountsProvider _accountsProvider;
        private readonly IPlayersProvider _playersProvider;
        private readonly IVehiclesProvider _vehiclesProvider;
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IPoliceManager _policeManager;
        private readonly IJailManager _jailManager;
        private readonly IWorkInfoManager _workInfoManager;

        public PlayerDisconnectManager() {}
        public PlayerDisconnectManager(
            IAccountsProvider accountsProvider,
            IPlayersProvider playersProvider,
            IVehiclesProvider vehiclesProvider,
            IPlayerInfoManager playerInfoManager,
            IPoliceManager policeManager,
            IJailManager jailManager,
            IWorkInfoManager workInfoManager) {
            _accountsProvider = accountsProvider;
            _playersProvider = playersProvider;
            _vehiclesProvider = vehiclesProvider;
            _playerInfoManager = playerInfoManager;
            _policeManager = policeManager;
            _jailManager = jailManager;
            _workInfoManager = workInfoManager;
        }

        /// <summary>
        /// Обработчик выхода игрока
        /// </summary>
        public void OnPlayerDisconnect(Client player, string reason) {
            if (player.hasData(ClanMission.BOOTY_OBJECT)) {
                ClanMissionManager.DetachBooty(player);
            }
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (playerInfo == null) {
                return;
            }
            if (player.hasData(PlayerData.ON_RACE) || player.hasSyncedData(PlayerData.IS_REGISTERED)) {
                ProcessRacer(player);
            }
            SyncWork(player);
            SyncPrisoner(player);
            SyncJail(player, playerInfo);
            SyncWeaponAmmo(player, playerInfo);
            SyncClanMissionVote(player, playerInfo);
            playerInfo.LastPosition = PositionConverter.VectorToString(player.position);
            _accountsProvider.UpdateTotalTime(playerInfo.AccountId);
            _playersProvider.UpdatePlayersInfos(new List<PlayerInfo> {playerInfo});
            _vehiclesProvider.Update(playerInfo.Vehicles.Values);
            _playerInfoManager.Remove(player);
        }

        /// <summary>
        /// Обрабатывает выход участника гонки
        /// </summary>
        private static void ProcessRacer(Client player) {
            var races = ServerKernel.Kernel.Get<Race[]>();
            foreach (var race in races) {
                if (race.Contains(player)) {
                    race.RemoveMember(player);
                }
            }
        }

        /// <summary>
        /// Синхронизировать активную работу игрока
        /// </summary>
        private void SyncWork(Client player) {
            var activeWork = _workInfoManager.GetActiveWork(player);
            if (activeWork == null) {
                return;
            }
            _playerInfoManager.SetBalance(player, activeWork.Salary);
            activeWork.Salary = 0;
            activeWork.Active = false;
            _workInfoManager.SetWorkInfo(player, activeWork);
        }

        /// <summary>
        /// Синхронизация задержанного игрока
        /// </summary>
        private void SyncPrisoner(Client prisoner) {
            var policeman = _policeManager.GetAttachedPlayer(prisoner);
            if (policeman == null) {
                return;
            }
            _jailManager.PutPrisonerInJail(policeman, prisoner);
        }

        /// <summary>
        /// Синхронизация заключенного
        /// </summary>
        private void SyncJail(Client prisoner, PlayerInfo info) {
            if (info.Wanted.JailTime == 0) {
                return;
            }
            _jailManager.OnPrisonerExit(prisoner);
        }

        /// <summary>
        /// Синхронизирует оружие игрока
        /// </summary>
        private void SyncWeaponAmmo(Client player, PlayerInfo playerInfo) {
            var weapons = API.getPlayerWeapons(player);
            foreach (var weapon in weapons) {
                var ammo = API.getPlayerWeaponAmmo(player, weapon);
                var playerAmmo = InventoryManager.GetAmmo(playerInfo, (int) weapon);
                if (playerAmmo != null) {
                    playerAmmo.Count = ammo;
                }
            }
        }

        /// <summary>
        /// Синхронизирует голосование за старт миссии банды
        /// </summary>
        private void SyncClanMissionVote(Client player, PlayerInfo playerInfo) {
            if (!player.hasData(MissionMenuHandler.MISSION_START_VOTE)) {
                return;
            }
            ClanMissionManager.Missions[playerInfo.Clan.ClanId].Members -= 1;
        }
    }
}