using System.Collections.Generic;
using System.Threading;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Helpers;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using Ninject;
using Vehicle = gta_mp_database.Entity.Vehicle;
using VehInst = GrandTheftMultiplayer.Server.Elements.Vehicle;

namespace gta_mp_server.Managers.Races.Interfaces {
    /// <summary>
    /// Базовый класс гонки
    /// </summary>
    internal abstract class Race : Script {
        private readonly Dictionary<int, WorkReward> _rewards = new Dictionary<int, WorkReward> {
            [1] = new WorkReward { Salary = 500, Exp = 150 },
            [2] = new WorkReward { Salary = 400, Exp = 100 },
            [3] = new WorkReward { Salary = 300, Exp = 50 }
        };

        protected IPlayerInfoManager PlayerInfoManager;
        protected IVehicleManager VehicleManager;

        protected Race() {
            PlayerInfoManager = ServerKernel.Kernel.Get<IPlayerInfoManager>();
            VehicleManager = ServerKernel.Kernel.Get<IVehicleManager>();
        }

        /// <summary>
        /// Тип гонки
        /// </summary>
        internal RaceType Type;

        /// <summary>
        /// Состояние гонки
        /// </summary>
        internal bool InProgress = false;

        /// <summary>
        /// Запустить гонку
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Завершить гонку
        /// </summary>
        public abstract void Finish();

        /// <summary>
        /// Проверяет, что гонку можно завершить
        /// </summary>
        public abstract bool CanFinish();

        /// <summary>
        /// Добавить участника
        /// </summary>
        public abstract void AddMember(Client player, Vehicle vehicle = null);

        /// <summary>
        /// Добавить участника
        /// </summary>
        public abstract void AddMember(Client player, int hash = 0);

        /// <summary>
        /// Убрать участника
        /// </summary>
        public abstract void RemoveMember(Client player);

        /// <summary>
        /// Добавляет победителя в очередь
        /// </summary>
        public abstract void AddWinner(Client player);

        /// <summary>
        /// Возвращает название гонки
        /// </summary>
        public abstract string GetName();

        /// <summary>
        /// Возвращает количество участников
        /// </summary>
        public abstract int GetMembersCount();

        /// <summary>
        /// Проверяет, находится ли игрок в списке участников
        /// </summary>
        public abstract bool Contains(Client player);

        /// <summary>
        /// Обработчик старта гонки
        /// </summary>
        protected void OnRaceStart(IEnumerable<VehInst> vehicles) {
            foreach (var vehicle in vehicles) {
                vehicle.freezePosition = false;
            }
        }

        /// <summary>
        /// Запретить движение до старта
        /// </summary>
        protected static void FreezeVehicles(IEnumerable<VehInst> vehicles) {
            Thread.Sleep(2000); // чтобы гта успела их создать и поставить на поверхность
            foreach (var vehicle in vehicles) {
                vehicle.freezePosition = true;
            }
        }

        /// <summary>
        /// Записывает данные игрока до эвента
        /// </summary>
        protected void SetPlayerData(Client player, int dimension) {
            player.setData(PlayerData.ON_RACE, true);
            player.setData(PlayerData.RACE_TYPE, Type);
            player.setData(PlayerData.LAST_POSITION, player.position);
            player.setData(PlayerData.LAST_DIMENSION, player.dimension);
            API.setEntityDimension(player, dimension);
        }

        /// <summary>
        /// Завершает гонку
        /// </summary>
        protected void Finish(List<Client> players, string message) {
            Thread.Sleep(5000);
            foreach (var player in players) {
                API.sendChatMessageToPlayer(player, message);
                API.triggerClientEvent(player, ServerEvent.HIDE_RACE_POINT);
                if (player.hasData(PlayerData.ON_RACE)) {
                    // note: если игрок выбыл из гонки, то его позиция уже восстановлена
                    PlayerHelper.RestorePosition(player);
                }
                player.resetData(PlayerData.ON_RACE);
                player.resetData(PlayerData.RACE_TYPE);
                player.resetSyncedData(PlayerData.IS_REGISTERED);
            }
        }

        /// <summary>
        /// Проверяет, что гонку можно завершить
        /// </summary>
        protected bool CanFinish(int activeMembers, int winnersCount) {
            return winnersCount == 3 || activeMembers == winnersCount;
        }

        /// <summary>
        /// Выдать награду победителям
        /// </summary>
        protected string SetReward(Queue<Client> winners) {
            var message = string.Empty;
            var counter = 0;
            while (winners.Count > 0 && counter < 3) {
                counter++;
                var player = winners.Dequeue();
                var reward = _rewards[counter];
                PlayerInfoManager.SetBalance(player, reward.Salary, true);
                PlayerInfoManager.SetExperience(player, reward.Exp);
                message += $"{counter}. {player.name}; ";
            }
            return message != string.Empty ? $"~g~Победители: {message}" : "~b~Победителей нет";
        }
    }
}