using System.Collections.Generic;
using System.Linq;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Vehicles;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Models.Places;
using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.MenuHandlers {
    internal class RacecourseMenuHandler : Script, IMenu {
        private const string RACECOURSE_MEMBER = "RacecourseMember";
        private const string RACE_VEHICLE_KEY = "RacecourseCar";
        private const string RACE_CIRCLE_NUBMER = "RaceCircleNumber";
        private const int CIRCLES_COUNT = 10;
        private const int TIMEOUT = 5000;

        private static readonly List<SpawnVehicleInfo> _raceVehicles = new List<SpawnVehicleInfo> {
            new SpawnVehicleInfo {Hash = VehicleHash.Cheburek, Position = new Vector3(1131.07, 202.46, 81.51), Rotation = new Vector3(-0.21, 3.63, 146.85)},
            new SpawnVehicleInfo {Hash = VehicleHash.Cheburek, Position = new Vector3(1126.55, 204.83, 81.28), Rotation = new Vector3(-0.12, 1.56, 148.67)},
            new SpawnVehicleInfo {Hash = VehicleHash.Cheburek, Position = new Vector3(1122.73, 207.56, 81.30), Rotation = new Vector3(-0.11, -1.40, 147.73)},
            new SpawnVehicleInfo {Hash = VehicleHash.Cheburek, Position = new Vector3(1119.12, 209.71, 81.51), Rotation = new Vector3(-0.09, -3.54, 147.46)}
        };

        private static readonly Dictionary<int, int> _colors = new Dictionary<int, int> {
            [1] = 28, [2] = 55, [3] = 81, [4] = 134
        };

        private static readonly Queue<Client> _finished = new Queue<Client>(4);

        private readonly IVehicleManager _vehicleManager;

        public RacecourseMenuHandler() {}
        public RacecourseMenuHandler(IVehicleManager vehicleManager) {
            _vehicleManager = vehicleManager;
        }

        public void Initialize() {
            ClientEventHandler.Add("OnStartRacecourseRace", OnStartRace);
            ClientEventHandler.Add("StartRacecourseRace", StartRace);
            ClientEventHandler.Add("StopRacecourseRace", (client, objects) => FinishRace());
            ClientEventHandler.Add("SpawnRacecourseCars", SpawnRaceVehicles);
            ClientEventHandler.Add("RemoveRacecourseCars", (client, objects) => RemoveRaceVehicles());
            CreateRaceCircleCounter();
            CreateRaceObjects();
        }

        /// <summary>
        /// Обработчик запуска стартового отсчета
        /// </summary>
        private void OnStartRace(Client player, object[] args) {
            var vehicles = GetRaceVehicles();
            var members = vehicles.Select(e => API.getVehicleDriver(e)).Where(e => e != null && API.isPlayerConnected(e)).ToList();
            if (members.Count < 2) return;
            members.ForEach(e => API.triggerClientEvent(e, ServerEvent.SET_TIMER, 10, "StartRacecourseRace"));
            ActionHelper.SetAction(TIMEOUT * 3, () => {
                foreach (var vehicle in vehicles) {
                    if (API.getVehicleDriver(vehicle) != null) continue;
                    API.deleteEntity(vehicle);
                }
            });
        }

        /// <summary>
        /// Запуск гонки
        /// </summary>
        private void StartRace(Client player, object[] args) {
            player.setData(RACECOURSE_MEMBER, true);
            var vehicle = API.getEntityFromHandle<Vehicle>(API.getPlayerVehicle(player));
            while (vehicle.freezePosition) vehicle.freezePosition = false;
        }

        /// <summary>
        /// Завершает гонку
        /// </summary>
        private void FinishRace() {
            _finished.Clear();
            RemoveRaceVehicles();
        }

        /// <summary>
        /// Заспавнить машины для заезда
        /// </summary>
        private void SpawnRaceVehicles(Client player, object[] args) {
            var counter = 1;
            foreach (var spawnVehicleInfo in _raceVehicles) {
                var color = _colors[counter];
                var vehicle = _vehicleManager.CreateVehicle(new CommonVehicle {
                    Hash = spawnVehicleInfo.Hash,
                    SpawnPosition = spawnVehicleInfo.Position,
                    SpawnRotation = spawnVehicleInfo.Rotation,
                    Fuel = 50,
                    MaxFuel = 50,
                    VehicleType = RACE_VEHICLE_KEY,
                    MainColor = color,
                    SecondColor = color
                });
                vehicle.setData(VehicleManager.DONT_RESTORE, true);
                vehicle.engineStatus = true;
                API.setVehicleEnginePowerMultiplier(vehicle, 25);
                while (!vehicle.freezePosition) vehicle.freezePosition = true;
                counter++;
            }
        }

        /// <summary>
        /// Удалить машины с заезда
        /// </summary>
        private void RemoveRaceVehicles() {
            GetRaceVehicles().ForEach(e => API.deleteEntity(e));
        }

        /// <summary>
        /// Создает шейп отсчета кругов
        /// </summary>
        private void CreateRaceCircleCounter() {
            var counter = API.createCylinderColShape(new Vector3(1118.15, 193.02, 80.87), 10f, 5);
            counter.onEntityEnterColShape += (shape, entity) => PlayerHelper.ProcessAction(entity, player => {
                if (!player.hasData(RACECOURSE_MEMBER)) return;
                var circleNumber = player.hasData(RACE_CIRCLE_NUBMER) ? (int) player.getData(RACE_CIRCLE_NUBMER) : 0;
                if (circleNumber == CIRCLES_COUNT) {
                    _finished.Enqueue(player);
                    API.triggerClientEvent(player, "HideRacecourseFinish");
                    player.resetData(RACE_CIRCLE_NUBMER);
                    if (_finished.Count >= API.getAllPlayers().Count(e => e.hasData(RACECOURSE_MEMBER))) {
                        ActionHelper.SetAction(TIMEOUT, () => {
                            DisplayResults();
                            FinishRace();
                        });
                    }
                    ActionHelper.SetAction(TIMEOUT, () => API.deleteEntity(API.getPlayerVehicle(player)));
                    return;
                }
                if (circleNumber == CIRCLES_COUNT - 1) {
                    ActionHelper.SetAction(player, TIMEOUT, () => API.triggerClientEvent(player, "ShowRacecourseFinish"));
                }
                circleNumber++;
                player.setData(RACE_CIRCLE_NUBMER, circleNumber);
                API.sendNotificationToPlayer(player, $"Круги:~b~ {circleNumber} / {CIRCLES_COUNT}");
            }, true);
        }

        /// <summary>
        /// Выводит результаты гонки
        /// </summary>
        private void DisplayResults() {
            API.sendChatMessageToAll("~b~-======== РЕЗУЛЬТАТ ЗАЕЗДА ========-");
            var place = 1;
            while (_finished.Count > 0) {
                var player = _finished.Dequeue();
                player.resetData(RACECOURSE_MEMBER);
                API.sendChatMessageToAll($"~g~{place}. {player.name}");
                place++;
            }
            API.sendChatMessageToAll("~b~-=====================================-");
        }

        /// <summary>
        /// Возвращает гоночные машины
        /// </summary>
        private List<NetHandle> GetRaceVehicles() {
            return API.getAllVehicles().Where(e => API.hasEntityData(e, RACE_VEHICLE_KEY)).ToList();
        }

        /// <summary>
        /// Создает объекты трассы
        /// </summary>
        private void CreateRaceObjects() {
            // ворота
            API.createObject(-1813095810, new Vector3(1122, 200, 80), new Vector3(0, 0, 57));
            // перегородки
            API.createObject(-905357089, new Vector3(1156.5, 279, 81), new Vector3(0, 0, 57));
            API.createObject(-905357089, new Vector3(1158.9, 283, 81), new Vector3(0, 0, 60));
            API.createObject(-905357089, new Vector3(1184.9, 298.8, 81), new Vector3(0, 0, 10));
            API.createObject(-905357089, new Vector3(1178.5, 297.5, 81), new Vector3(0, 0, 12.5));
            API.createObject(-666143389, new Vector3(1172.1, 296.0, 81), new Vector3(0, 0, 13));
            API.createObject(-666143389, new Vector3(1084.0, -100.5, 81), new Vector3(0, 0, -157.0));
            API.createObject(-666143389, new Vector3(1095.5, -95.42, 81), new Vector3(0, 0, -155.0));
            API.createObject(-905357089, new Vector3(1103.0, -91.9, 81), new Vector3(0, 0, -155.0));
        }
    }
}