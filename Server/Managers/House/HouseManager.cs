using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_database.Providers.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.House.Interfaces;
using gta_mp_server.Managers.MenuHandlers;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Models;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Marker = gta_mp_server.Enums.Marker;
using HouseInfo = gta_mp_database.Entity.House;
using VehicleInfo = gta_mp_database.Entity.Vehicle;

namespace gta_mp_server.Managers.House {
    /// <summary>
    /// Логика обработки действий с домами
    /// </summary>
    internal class HouseManager : Script, IHouseManager {
        private const int NO_OWNER = (int) Validator.INVALID_ID;
        private const int PLAYER_HEAL_TIMEOUT = 20000; // 20сек.
        private const int RENT_CHECK_TIMEOUT = 900000; // 15мин.
        private const float SHAPE_RANGE = 0.7f;
        private const int FREE_HOUSE_COLOR = 69;
        private const int BUSY_HOUSE_COLOR = 38;
        private const int RECOVER_AMOUNT = 10;

        private static readonly Dictionary<long, Blip> _blipsByHouses = new Dictionary<long, Blip>();
        private readonly Vector3 _innerMarkerScale = new Vector3(0.7, 0.7, 0.7);
        private readonly Vector3 _displacement = new Vector3(0.0, 0.0, -1.0);

        private readonly IHousesProvider _housesProvider;
        private readonly IPointCreator _pointCreator;
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IHouseEventManager _houseEventManager;
        private readonly IStorageManager _storageManager;
        private readonly IVehicleInfoManager _vehicleInfoManager;
        
        public HouseManager() {}
        public HouseManager(
            IHousesProvider housesProvider,
            IPointCreator pointCreator,
            IPlayerInfoManager playerInfoManager,
            IHouseEventManager houseEventManager,
            IStorageManager storageManager,
            IVehicleInfoManager vehicleInfoManager) {
            _housesProvider = housesProvider;
            _pointCreator = pointCreator;
            _playerInfoManager = playerInfoManager;
            _houseEventManager = houseEventManager;
            _storageManager = storageManager;
            _vehicleInfoManager = vehicleInfoManager;
        }

        /// <summary>
        /// Загрузить дома
        /// </summary>
        public void Initialize() {
            API.requestIpl("apa_v_mp_h_04_a");
            API.requestIpl("TrevorsTrailerTidy");
            ServerState.Houses = _housesProvider.GetHouses().ToDictionary(e => e.Id);
            foreach (var house in ServerState.Houses.Values) {
                var position = PositionConverter.ToVector3(house.Position);
                CreateBlips(house, position);
                var enter = _pointCreator.CreateMarker(Marker.UpsideDownCone, position, Colors.Yellow, SHAPE_RANGE);
                enter.ColShape.onEntityEnterColShape += (shape, entity) => _houseEventManager.OnPlayerWentToEnter(entity, house.Id);
                enter.ColShape.onEntityExitColShape += (shape, entity) => _houseEventManager.OnPlayerAway(entity);
                InitializeHouseInside(house);
                InitializeGarage(house);
            }
            _storageManager.Initialize();
            ActionHelper.StartTimer(PLAYER_HEAL_TIMEOUT, HealPlayersInHouses);
            ActionHelper.StartTimer(RENT_CHECK_TIMEOUT, SyncroizeRent);
        }

        /// <summary>
        /// Восстановить здоровье игроков в домах
        /// </summary>
        private void HealPlayersInHouses() {
            var players = API.getAllPlayers().Where(e => e.hasData(HouseMenuHandler.IN_HOUSE));
            foreach (var player in players) {
                if (player.health + RECOVER_AMOUNT > PlayerHelper.MAX_HEALTH) {
                    continue;
                }
                player.health += RECOVER_AMOUNT;
            }
        }

        /// <summary>
        /// Обновить информацию об аренде домов
        /// </summary>
        private void SyncroizeRent() {
            var houses = ServerState.Houses.Values.Where(e => Validator.IsValid(e.EndOfRenting)).ToList();
            foreach (var house in houses) {
                if (house.EndOfRenting > DateTime.Now) {
                    continue;
                }
                ProcessOwner(house);
                house.OwnerId = NO_OWNER;
                house.EndOfRenting = DateTime.MinValue;
                SetHouse(house);
                UpdateBlip(house);
            }
        }

        /// <summary>
        /// Обработать данные владельца дома
        /// </summary>
        private void ProcessOwner(HouseInfo house) {
            var player = _playerInfoManager.GetWithData(house.OwnerId);
            if (PlayerHelper.PlayerCorrect(player.Player)) {
                var houseId = PlayerHelper.GetData(player.Player, HouseMenuHandler.IN_HOUSE, NO_OWNER);
                if (houseId == house.Id) {
                    player.Player.resetData(HouseMenuHandler.IN_HOUSE);
                    API.setEntityPosition(player.Player, PositionConverter.ToVector3(house.Position));
                    API.setEntityDimension(player.Player, 0);
                }
                MoveVehiclesOnParking(player.PlayerInfo.Vehicles.Values, player.Player);
                _playerInfoManager.RefreshUI(player.Player, player.PlayerInfo);
                API.sendNotificationToPlayer(player.Player, "~b~Аренда дома подошла к концу");
            }
            else {
                MoveVehiclesOnParking(player.PlayerInfo.Vehicles.Values);
                player.PlayerInfo.LastPosition = house.Position;
                player.PlayerInfo.Dimension = 0;
                _playerInfoManager.Set(player.PlayerInfo);
            }
        }

        /// <summary>
        /// Переместить транспорт игрока на парковку
        /// </summary>
        private void MoveVehiclesOnParking(IEnumerable<VehicleInfo> vehicles, Client player = null) {
            foreach (var vehicle in vehicles.ToList()) {
                if (vehicle.HouseId == NO_OWNER) continue;
                vehicle.HouseId = Validator.INVALID_ID;
                vehicle.IsSpawned = false;
                if (player != null) {
                    _vehicleInfoManager.SetInfo(player, vehicle);
                }
                else {
                    _vehicleInfoManager.SetInfo(vehicle);
                }
            }
        }

        /// <summary>
        /// Возвращает информацию о доме
        /// </summary>
        public HouseInfo GetHouse(long houseId) {
            ServerState.Houses.TryGetValue(houseId, out var result);
            return result;
        }

        /// <summary>
        /// Записать информацию о доме
        /// </summary>
        public void SetHouse(HouseInfo house) {
            ServerState.Houses[house.Id] = house;
        }

        /// <summary>
        /// Возвращает дома игрока
        /// </summary>
        public List<HouseInfo> GetPlayerHouses(long accountId) {
            return ServerState.Houses.Values.Where(e => e.OwnerId == accountId).ToList();
        }

        /// <summary>
        /// Обновить иконку дома на карте
        /// </summary>
        public void UpdateBlip(HouseInfo house, string playerName = "") {
            var blip = _blipsByHouses[house.Id];
            var hasOwner = !string.IsNullOrEmpty(playerName);
            var color = hasOwner ? BUSY_HOUSE_COLOR : FREE_HOUSE_COLOR;
            var sprite = hasOwner ? 40 : 374;
            var label = $"{HouseHelper.GetHouseLabel(house.Type, playerName)}";
            API.setBlipColor(blip, color);
            API.setBlipSprite(blip, sprite);
            API.setBlipName(blip, label);
        }

        /// <summary>
        /// Создает значек дома на карте
        /// </summary>
        private void CreateBlips(HouseInfo house, Vector3 position) {
            var hasOwner = house.OwnerId != NO_OWNER;
            var playerName = hasOwner
                ? _playerInfoManager.Get(house.OwnerId)?.Name
                : string.Empty;
            var blip = _pointCreator.CreateBlip(position, hasOwner ? 40 : 374, 0, scale: 0.6f);
            _blipsByHouses.Add(house.Id, blip);
            UpdateBlip(house, playerName);
        }

        /// <summary>
        /// Проинициализировать дом внутри
        /// </summary>
        private void InitializeHouseInside(HouseInfo house) {
            var positions = HousesPositionsGetter.InnerPositions[house.Type];
            CreateWardrobe(house, positions);
            CreateStorage(house, positions);
            CreateExitFromHouse(house, positions);
        }

        /// <summary>
        /// Создает гардероб
        /// </summary>
        private void CreateWardrobe(HouseInfo house, HousesInnerPositions positions) {
            var wardrobe = _pointCreator.CreateMarker(
                Marker.VerticalCylinder, positions.Wardrobe, Colors.Yellow,
                SHAPE_RANGE, dimention: (int) -house.Id
            );
            wardrobe.ColShape.onEntityEnterColShape += (shape, entity) => _houseEventManager.OnPlayerEnterWardrobe(entity, house.Type);
            wardrobe.ColShape.onEntityExitColShape += (shape, entity) => {
                var player = API.getPlayerFromHandle(entity);
                API.triggerClientEvent(player, ServerEvent.HIDE_CLOTHES_MENU);
            };
            API.setMarkerScale(wardrobe.Marker, _innerMarkerScale);
            API.setEntityPosition(wardrobe.Marker, positions.Wardrobe.Add(_displacement));
        }

        /// <summary>
        /// Создает хранилище
        /// </summary>
        private void CreateStorage(HouseInfo house, HousesInnerPositions positions) {
            var storage = _pointCreator.CreateMarker(
                Marker.VerticalCylinder, positions.Storage, Colors.VividCyan,
                SHAPE_RANGE, dimention: (int) -house.Id
            );
            storage.ColShape.onEntityEnterColShape += (shape, entity) => _houseEventManager.OnPlayerEnterStorage(entity, house.Type);
            storage.ColShape.onEntityExitColShape += (shape, entity) =>
                PlayerHelper.ProcessAction(entity, player => API.triggerClientEvent(player, ServerEvent.HIDE_HOUSE_STORAGE_MENU));
            API.setMarkerScale(storage.Marker, _innerMarkerScale);
            API.setEntityPosition(storage.Marker, positions.Storage.Add(_displacement));
        }

        /// <summary>
        /// Создает выход из дома
        /// </summary>
        private void CreateExitFromHouse(HouseInfo house, HousesInnerPositions positions) {
            var exit = _pointCreator.CreateMarker(
                Marker.UpsideDownCone, positions.Exit, Colors.Yellow,
                SHAPE_RANGE, dimention: (int) -house.Id
            );
            exit.ColShape.dimension = (int) -house.Id;
            exit.ColShape.onEntityEnterColShape += (shape, entity) => { _houseEventManager.OnPlayerExit(entity, house.Id); };
            exit.ColShape.onEntityExitColShape += (shape, entity) => _houseEventManager.OnPlayerAway(entity);
        }

        /// <summary>
        /// Проинициализировать гараж
        /// </summary>
        private void InitializeGarage(HouseInfo house) {
            if (string.IsNullOrEmpty(house.GaragePosition)) {
                // не у всех домов может быть гараж?
                return;
            }
            // вход в гараж
            var enterPos = PositionConverter.ToVector3(house.GaragePosition);
            var enter = _pointCreator.CreateMarker(Marker.Stripes, enterPos, Colors.Yellow, 2f);
            API.setEntityRotation(enter.Marker, PositionConverter.ToVector3(house.RotationAfterExit));
            enter.ColShape.onEntityEnterColShape += (shape, entity) => {
                _houseEventManager.OnPlayerWentToGarageEnter(entity, house.Id);
            };
            enter.ColShape.onEntityExitColShape += (shape, entity) => _houseEventManager.OnPlayerAway(entity);
            // выход
            var exits = HousesPositionsGetter.GetGarageInnerPositions(house.Type).GarageExits;
            foreach (var exit in exits) {
                var garageExit = API.createSphereColShape(exit, 2f);
                garageExit.dimension = (int) -house.Id;
                garageExit.onEntityEnterColShape += (shape, entity) => {
                    _houseEventManager.OnPlayerExitGarage(entity, house.Id);
                };
                garageExit.onEntityExitColShape += (shape, entity) => _houseEventManager.OnPlayerAway(entity);
            }
        }
    }
}