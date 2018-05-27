using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.GameEvents.Data;
using gta_mp_server.GameEvents.Interfaces;
using gta_mp_server.Global;
using gta_mp_server.Models;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;

namespace gta_mp_server.GameEvents {
    /// <summary>
    /// Эвент "Деревенская разборка"
    /// </summary>
    internal class CountryBreakdown : GameEvent {
        private const string EVENT_NAME = "Деревенская разборка";
        private const int MAX_MEMBERS = 50;
        private const int EVENT_DIMENSION = -120;
        private const int WINNER_MONEY = 500;
        private const int WINNER_EXP = 100;

        public CountryBreakdown() {
            API.onPlayerRespawn += OnPlayerRespawn;
            API.onPlayerDeath += OnPlayerDeath;
            API.onPlayerDisconnected += OnPlayerDisconnected;
        }

        /// <summary>
        /// Обработчик воскрешения игрока
        /// </summary>
        private void OnPlayerRespawn(Client player) {
            if (!player.hasData(PlayerData.ON_EVENT) || (string) player.getData(PlayerData.ON_EVENT) != EVENT_NAME) {
                return;
            }
            OnPlayerRespawn(player, CountryBreakdownData.DeadPosition);
        }

        /// <summary>
        /// Обработчик смерти игрока
        /// </summary>
        private void OnPlayerDeath(Client player, NetHandle entitykiller, int weapon) {
            if (!player.hasData(PlayerData.ON_EVENT) || (string) player.getData(PlayerData.ON_EVENT) != EVENT_NAME || Winners != EventTeam.None) {
                return;
            }
            OnPlayerDeath(player, entitykiller);
        }

        /// <summary>
        /// Возвращает название эвента
        /// </summary>
        protected override EventInfo GetEventInfo() {
            return new EventInfo {Name = EVENT_NAME, MaxMembers = MAX_MEMBERS, Type = EventType.Commands.GetDescription()};
        }

        /// <summary>
        /// Создает зону эвента
        /// </summary>
        protected override ColShape CreateEventZone() {
            var result = API.createSphereColShape(CountryBreakdownData.EventZonePosition, 230f);
            result.dimension = EVENT_DIMENSION;
            return result;
        }

        /// <summary>
        /// Устанавливает параметры игрока перед эвентом
        /// </summary>
        protected override void SetEventData(Client player, EventTeam team, int index) {
            var position = team == EventTeam.Red ? CountryBreakdownData.RedTeamPositions[index] : CountryBreakdownData.BlueTeamPositions[index];
            API.setEntityPosition(player, position);
            API.setEntityDimension(player, EVENT_DIMENSION);
            API.givePlayerWeapon(player, CountryBreakdownData.GetWeapon(), 300, true);
            var clothes = CountryBreakdownData.GetEventClothes(team, PlayerInfoManager.IsMale(player));
            GtaCharacter.SetClothes(player, clothes);
        }

        /// <summary>
        /// Сообщение о завершении эвента
        /// </summary>
        protected override void SendFinishNotify(EventTeam winners) {
            SendMessageForMembers($"~g~Победила {winners.GetDescription()} команда! Эвент завершится через 30 секунд");
        }

        /// <summary>
        /// Выдает награду победителю
        /// </summary>
        protected override void SetReward(Client player) {
            PlayerInfoManager.SetBalance(player, WINNER_MONEY, true);
            PlayerInfoManager.SetExperience(player, WINNER_EXP);
        }
    }
}