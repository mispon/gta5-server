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
    /// Эвент "Терюмный бунт"
    /// </summary>
    internal class PrisonRiot : GameEvent {
        private const string EVENT_NAME = "Тюремный бунт";
        private const int MAX_MEMBERS = 30;
        private const int EVENT_DIMENSION = -110;
        private const int WINNER_MONEY = 400;
        private const int WINNER_EXP = 50;

        public PrisonRiot() {
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
            OnPlayerRespawn(player, PrisonRiotData.DeadPosition);
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
            var result = API.createSphereColShape(PrisonRiotData.EventZonePosition, 65f);
            result.dimension = EVENT_DIMENSION;
            return result;
        }

        /// <summary>
        /// Устанавливает параметры игрока перед эвентом
        /// </summary>
        protected override void SetEventData(Client player, EventTeam team, int index) {
            var isPrisoner = team == EventTeam.Red;
            var position = isPrisoner ? PrisonRiotData.PrisonersPositions[index] : PrisonRiotData.OfficersPositions[index];
            API.setEntityPosition(player, position);
            API.setEntityDimension(player, EVENT_DIMENSION);
            API.setPlayerSkin(player, PrisonRiotData.GetSkin(isPrisoner));
            API.givePlayerWeapon(player, PrisonRiotData.GetWeapon(isPrisoner), 0, true);
        }

        /// <summary>
        /// Сообщение о завершении эвента
        /// </summary>
        protected override void SendFinishNotify(EventTeam winners) {
            var winnersName = winners == EventTeam.Red ? "заключенные" : "охранники";
            SendMessageForMembers($"~g~Победили {winnersName}! Эвент завершится через 30 секунд");
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