using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gta_mp_database.Entity;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Global;
using gta_mp_server.Managers.Races.Data;
using gta_mp_server.Managers.Races.Interfaces;
using gta_mp_server.Managers.Races.TrackHandlers;
using GrandTheftMultiplayer.Server.Elements;
using Vehicle = gta_mp_database.Entity.Vehicle;
using VehInst = GrandTheftMultiplayer.Server.Elements.Vehicle;

namespace gta_mp_server.Managers.Races {
    /// <summary>
    /// Гонка на мотоциклах
    /// </summary>
    internal class MotoRace : Race {
        private const string NAME = "Мотогонки";
        internal static Dictionary<Client, Vehicle> Members = new Dictionary<Client, Vehicle>();
        private static readonly Queue<Client> _winners = new Queue<Client>(10);
        private static readonly List<VehInst> _memberVehicles = new List<VehInst>();

        public MotoRace() {
            Type = RaceType.Moto;
        }

        /// <summary>
        /// Запустить гонку
        /// </summary>
        public override void Start() {
            const int driverSeat = -1;
            Members = Members.Where(e => API.isPlayerConnected(e.Key)).ToDictionary(e => e.Key, e => e.Value);
            if (Members.Count < 2) {
                return;
            }
            ClientEventHandler.Add(ServerEvent.START_MOTO_RACE, (player, args) => OnRaceStart(_memberVehicles));
            var index = 0;
            foreach (var member in Members) {
                SetPlayerData(member.Key, Dimension.MOTO_RACE);
                var moto = CreateMemberMoto(member.Value, index);
                API.setPlayerIntoVehicle(member.Key, moto, driverSeat);
                MotoTrackHandler.ShowFirstPoint(member.Key);
                API.triggerClientEvent(member.Key, ServerEvent.SET_TIMER, 10, ServerEvent.START_MOTO_RACE);
                index++;
            }
            FreezeVehicles(_memberVehicles);
            InProgress = true;
        }

        /// <summary>
        /// Создает транспорт игрока
        /// </summary>
        private VehInst CreateMemberMoto(Vehicle vehicleInfo, int index) {
            var startPosition = MotoRaceData.StartPositions[index];
            var moto = VehicleManager.CreateVehicle(vehicleInfo, startPosition.Item1, startPosition.Item2, Dimension.MOTO_RACE);
            moto.engineStatus = true;
            _memberVehicles.Add(moto);
            return moto;
        }

        /// <summary>
        /// Завершить гонку
        /// </summary>
        public override void Finish() {
            var message = SetReward(_winners);
            Finish(Members.Keys.ToList(), message);
            Dispose();
            API.sendChatMessageToAll($"~g~\"{NAME}\" завершились. Регистрация открыта!");
        }

        /// <summary>
        /// Очистить ресурсы гонки
        /// </summary>
        private void Dispose() {
            ClientEventHandler.Remove(ServerEvent.START_MOTO_RACE);
            InProgress = false;
            foreach (var moto in _memberVehicles) {
                API.deleteEntity(moto);
            }
            _memberVehicles.Clear();
            Members.Clear();
            _winners.Clear();
        }

        /// <summary>
        /// Добавить участника
        /// </summary>
        public override void AddMember(Client player, Vehicle vehicle = null) {
            Members.Add(player, vehicle);
        }

        /// <summary>
        /// Добавить участника
        /// </summary>
        public override void AddMember(Client player, int hash = 0) {
            var vehicle = new Vehicle {
                Hash = hash,
                Fuel = 50,
                Tuning = new VehicleTuning {PrimaryColor = 0, SecondColor = 0}
            };
            AddMember(player, vehicle);
        }

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
            var activeMembers = Members.Count(e => e.Key.hasData(PlayerData.ON_RACE));
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
            return Members.ContainsKey(player);
        }
    }
}