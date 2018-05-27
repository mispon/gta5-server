using System.Linq;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Global;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Races.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using Newtonsoft.Json;
using Vehicle = gta_mp_database.Entity.Vehicle;

namespace gta_mp_server.Managers.MenuHandlers {
    /// <summary>
    /// Обработчик меню гонок
    /// </summary>
    internal class RaceMenuHandler : Script, IMenu {
        private const int MIN_LEVEL = 5;
        private const int MAX_MEMBERS = 10;

        private readonly Race[] _races;
        private readonly IPlayerInfoManager _playerInfoManager;

        public RaceMenuHandler() {}
        public RaceMenuHandler(Race[] races, IPlayerInfoManager playerInfoManager) {
            _races = races;
            _playerInfoManager = playerInfoManager;
        }

        /// <summary>
        /// Инициализировать обработчик меню
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.REGISTER_ON_RACE, RegisterOnRace);
            ClientEventHandler.Add(ClientEvent.REGISTER_ON_RACE_WITH_RENT, RegisterOnRaceWithRent);
            ClientEventHandler.Add(ClientEvent.CANCEL_RACE, CancelRaceRegister);
        }

        /// <summary>
        /// Обработчик регистрации на гонку
        /// </summary>
        private void RegisterOnRace(Client player, object[] args) {
            if (AlreadyRegister(player)) {
                return;
            }
            var type = (RaceType) args[0];
            var race = GetRace(type);
            if (!CanRegister(player, race)) {
                return;
            }
            var vehicle = args.Length > 1 ? JsonConvert.DeserializeObject<Vehicle>(args[1].ToString()) : null;
            race.AddMember(player, vehicle);
            FinishRegistration(player, race.GetName());
        }

        /// <summary>
        /// Обработчик регистрации на гонку с арендованным транспортом
        /// </summary>
        private void RegisterOnRaceWithRent(Client player, object[] args) {
            if (AlreadyRegister(player)) {
                return;
            }
            var type = (RaceType) args[0];
            var rentCost = (int) args[2];
            var race = GetRace(type);
            if (!CanRegister(player, race) || !PayRent(player, rentCost)) {
                return;
            }
            var hash = args.Length > 1 ? (int) args[1] : 0;
            race.AddMember(player, hash);
            FinishRegistration(player, race.GetName());
        }

        /// <summary>
        /// Проверяет, что игрок может зарегистрироваться
        /// </summary>
        private bool CanRegister(Client player, Race race) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (playerInfo.Level < MIN_LEVEL) {
                API.sendNotificationToPlayer(player, "~r~Необходимо достигнуть 5-го уровня и выше", true);
                return false;
            }
            if (race.InProgress) {
                API.sendNotificationToPlayer(player, "~r~Дождитесь окончания активной гонки", true);
                return false;
            }
            if (race.GetMembersCount() >= MAX_MEMBERS) {
                API.sendNotificationToPlayer(player, "~r~Не осталось свободных мест", true);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Оплата аренды гоночного транспорта
        /// </summary>
        private bool PayRent(Client player, int cost) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (playerInfo.Balance < cost) {
                API.sendNotificationToPlayer(player, "~r~Недостаточно средств для аренды", true);
                return false;
            }
            playerInfo.Balance -= cost;
            _playerInfoManager.RefreshUI(player, playerInfo);
            return true;
        }

        /// <summary>
        /// Завершение регистрации в гонке
        /// </summary>
        private void FinishRegistration(Client player, string raceName) {
            player.setSyncedData(PlayerData.IS_REGISTERED, true);
            API.sendNotificationToPlayer(player, $"~b~Вы зарегистрировались в \"{raceName}\"");
            API.triggerClientEvent(player, ServerEvent.HIDE_RACE_MENU);
        }

        /// <summary>
        /// Отмена регистрации
        /// </summary>
        private void CancelRaceRegister(Client player, object[] args) {
            var result = false;
            foreach (var race in _races) {
                if (!race.Contains(player)) continue;
                race.RemoveMember(player);
                API.sendNotificationToPlayer(player, "~b~Участие в гонке отменено");
                player.resetSyncedData(PlayerData.IS_REGISTERED);
                result = true;
                break;
            }
            if (!result) {
                API.sendNotificationToPlayer(player, "~r~Вы не участвуете ни в одной гонке", true);
            }
        }

        /// <summary>
        /// Проверяет, зарегистрирован ли игрок в гонке
        /// </summary>
        private bool AlreadyRegister(Client player) {
            foreach (var race in _races) {
                if (!race.Contains(player)) continue;
                API.sendNotificationToPlayer(player, $"~r~Вы уже участвуете в \"{race.GetName()}\"", true);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Возвращает гонку по выбранному типу
        /// </summary>
        private Race GetRace(RaceType type) {
            return _races.First(e => e.Type == type);
        }
    }
}