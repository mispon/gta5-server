using System.Linq;
using gta_mp_database.Models.Player;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Events.Interfaces;
using gta_mp_server.Helpers;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Places.Hospitals;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Races.Interfaces;
using gta_mp_server.Managers.Work.Police.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using Ninject;

namespace gta_mp_server.Events {
    internal class PlayerRespawnManager : Script, IPlayerRespawnManager {
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IPoliceManager _policeManager;
        private readonly IJailManager _jailManager;
        private readonly IInventoryManager _inventoryManager;

        public PlayerRespawnManager() {}
        public PlayerRespawnManager(IPlayerInfoManager playerInfoManager, IPoliceManager policeManager, 
            IJailManager jailManager, IInventoryManager inventoryManager) {
            _playerInfoManager = playerInfoManager;
            _policeManager = policeManager;
            _jailManager = jailManager;
            _inventoryManager = inventoryManager;
        }

        /// <summary>
        /// Обработчик респавна игрока
        /// </summary>
        public void OnPlayerRespawn(Client player) {
            _inventoryManager.EquipWeapon(player);
            if (player.hasData(PlayerData.ON_EVENT) || player.hasData(PlayerData.FIGHTER)) {
                return;
            }
            if (ProcessRacer(player)) {
                return;
            }
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (ProcessPrisoner(player, playerInfo)) {
                return;
            }
            ProcessPlayer(player, playerInfo);
        }

        /// <summary>
        /// Обработать респавн гонщика
        /// </summary>
        private bool ProcessRacer(Client player) {
            if (!player.hasData(PlayerData.ON_RACE)) {
                return false;
            }
            API.sendNotificationToPlayer(player, "~b~Вы выбыли из гонки");
            player.resetData(PlayerData.ON_RACE);
            PlayerHelper.RestorePosition(player);
            var raceType = (RaceType) player.getData(PlayerData.RACE_TYPE);
            var race = ServerKernel.Kernel.Get<Race[]>().First(e => e.Type == raceType);
            if (race.CanFinish()) {
                race.Finish();
            }
            return true;
        }

        /// <summary>
        /// Обработка респавна заключенного или арестованного игрока
        /// </summary>
        private bool ProcessPrisoner(Client player, PlayerInfo playerInfo) {
            if (playerInfo.Wanted.JailTime > 0) {
                _jailManager.SetInJail(player);
                return true;
            }
            var policeman = _policeManager.GetAttachedPlayer(player);
            if (policeman != null) {
                _jailManager.PutPrisonerInJail(policeman, player, false);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Обработка респавна обычного игрока
        /// </summary>
        private void ProcessPlayer(Client player, PlayerInfo playerInfo) {
            const int health = 15;
            playerInfo.Health = health;
            _playerInfoManager.RefreshUI(player, playerInfo);
            API.setPlayerHealth(player, health);
            var hospital = HospitalHelper.GetNearestHospital(player.position);
            API.setEntityPosition(player, Hospital.SpawnPosition);
            _playerInfoManager.SetDimension(player, hospital.Dimension);
        }
    }
}