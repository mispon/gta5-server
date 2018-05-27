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
    /// Горная гонка на байках
    /// </summary>
    internal class MountainRace : Race {
        private const string NAME = "Горный трeк";
        internal static List<Client> Members = new List<Client>();
        private static readonly List<VehInst> _memberVehicles = new List<VehInst>();
        private static readonly Queue<Client> _winners = new Queue<Client>(10);

        public MountainRace() {
            Type = RaceType.Mountain;
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
            ClientEventHandler.Add(ServerEvent.START_MOUNTAIN_RACE, (player, args) => OnRaceStart(_memberVehicles));
            var index = 0;
            foreach (var member in Members) {
                SetPlayerData(member, Dimension.MOUNT_RACE);
                var moto = CreateMemberMoto(index);
                Thread.Sleep(300); // чтобы машина успела появиться
                API.setPlayerIntoVehicle(member, moto, driverSeat);
                MountainTrackHandler.ShowFirstPoint(member);
                API.triggerClientEvent(member, ServerEvent.SET_TIMER, 10, ServerEvent.START_MOUNTAIN_RACE);
                index++;
            }
            FreezeVehicles(_memberVehicles);
            InProgress = true;
        }

        /// <summary>
        /// Создает транспорт игрока
        /// </summary>
        private VehInst CreateMemberMoto(int index) {
            var vehicleInfo = new Vehicle {
                Hash = (int) VehicleHash.Sanchez,
                Fuel = 50,
                Tuning = new VehicleTuning {PrimaryColor = 0, SecondColor = 0}
            };
            var startPosition = MountainRaceData.StartPositions[index];
            var moto = VehicleManager.CreateVehicle(vehicleInfo, startPosition.Item1, startPosition.Item2, Dimension.MOUNT_RACE);
            moto.engineStatus = true;
            _memberVehicles.Add(moto);
            return moto;
        }

        /// <summary>
        /// Завершить гонку
        /// </summary>
        public override void Finish() {
            var message = SetReward(_winners);
            Finish(Members, message);
            Dispose();
            API.sendChatMessageToAll($"~g~\"{NAME}\" завершился. Регистрация открыта!");
        }

        /// <summary>
        /// Очистить ресурсы гонки
        /// </summary>
        private void Dispose() {
            ClientEventHandler.Remove(ServerEvent.START_MOUNTAIN_RACE);
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