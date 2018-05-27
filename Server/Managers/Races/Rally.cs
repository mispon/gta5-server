using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using gta_mp_database.Entity;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Global;
using gta_mp_server.Managers.Races.Data;
using gta_mp_server.Managers.Races.Interfaces;
using gta_mp_server.Managers.Races.TrackHandlers;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using Vehicle = gta_mp_database.Entity.Vehicle;
using VehInst = GrandTheftMultiplayer.Server.Elements.Vehicle;

namespace gta_mp_server.Managers.Races {
    /// <summary>
    /// Гонка по бездорожью
    /// </summary>
    internal class Rally : Race {
        private const string NAME = "Ралли";
        internal static List<Client> Members = new List<Client>();
        private static readonly List<VehInst> _memberVehicles = new List<VehInst>();
        private static readonly Queue<Client> _winners = new Queue<Client>(10);

        public Rally() {
            Type = RaceType.Rally;
        }

        /// <summary>
        /// Запустить гонку
        /// </summary>
        public override void Start() {
            const int driverSeat = -1;
            Members = Members.Where(e => API.isPlayerConnected(e)).ToList();
            if (Members.Count < 2) {
                return;
            }
            ClientEventHandler.Add(ServerEvent.START_RALLY, (player, args) => OnRaceStart(_memberVehicles));
            var index = 0;
            foreach (var member in Members) {
                SetPlayerData(member, Dimension.RALLY);
                var car = CreateMemberCar(index);
                Thread.Sleep(300); // чтобы машина успела появиться
                API.setPlayerIntoVehicle(member, car, driverSeat);
                RallyTrackHandler.ShowFirstPoint(member);
                API.triggerClientEvent(member, ServerEvent.SET_TIMER, 10, ServerEvent.START_RALLY);
                index++;
            }
            FreezeVehicles(_memberVehicles);
            InProgress = true;
        }

        /// <summary>
        /// Создает транспорт игрока
        /// </summary>
        private VehInst CreateMemberCar(int index) {
            var vehicleInfo = new Vehicle {
                Hash = (int) VehicleHash.Brawler,
                Fuel = 70,
                Tuning = new VehicleTuning {PrimaryColor = 160, SecondColor = 0}
            };
            var startPosition = RallyRaceData.StartPositions[index];
            var car = VehicleManager.CreateVehicle(vehicleInfo, startPosition.Item1, startPosition.Item2, Dimension.RALLY);
            car.engineStatus = true;
            _memberVehicles.Add(car);
            return car;
        }


        /// <summary>
        /// Завершить гонку
        /// </summary>
        public override void Finish() {
            var message = SetReward(_winners);
            Finish(Members, message);
            Dispose();
            API.sendChatMessageToAll($"~g~\"{NAME}\" завершилось. Регистрация открыта!");
        }

        /// <summary>
        /// Очистить ресурсы гонки
        /// </summary>
        private void Dispose() {
            ClientEventHandler.Remove(ServerEvent.START_RALLY);
            InProgress = false;
            foreach (var car in _memberVehicles) {
                API.deleteEntity(car);
            }
            _memberVehicles.Clear();
            Members.Clear();
            _winners.Clear();
        }

        /// <summary>
        /// Добавить участника
        /// </summary>
        public override void AddMember(Client player, Vehicle vehicle = null) {
            Members.Add(player);
        }

        /// <summary>
        /// Добавить участника
        /// </summary>
        public override void AddMember(Client player, int hash = 0) {/* IGNORED */}

        /// <summary>
        /// Убрать участника
        /// </summary>
        public override void RemoveMember(Client player) {
            Members.Remove(player);
        }

        /// <summary>
        /// Добавляет победителя в очередь
        /// </summary>
        public override void AddWinner(Client player) {
            _winners.Enqueue(player);
            if (CanFinish()) {
                Task.Run(() => Finish());
            }
        }

        /// <summary>
        /// Проверяет, что гонку можно завершить
        /// </summary>
        public override bool CanFinish() {
            var activeMembers = Members.Count(e => e.hasData(PlayerData.ON_RACE));
            return CanFinish(activeMembers, _winners.Count);
        }

        /// <summary>
        /// Возвращает название гонки
        /// </summary>
        public override string GetName() {
            return NAME;
        }

        /// <summary>
        /// Возвращает количество участников
        /// </summary>
        public override int GetMembersCount() {
            return Members.Count;
        }

        /// <summary>
        /// Проверяет, находится ли игрок в списке участников
        /// </summary>
        public override bool Contains(Client player) {
            return Members.Contains(player);
        }
    }
}