using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Models.Shops;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using Marker = gta_mp_server.Enums.Marker;

namespace gta_mp_server.Managers.Places.VehicleShowroom {
    /// <summary>
    /// Автосалон
    /// </summary>
    internal class VehicleShowroom : Place {
        private readonly IDoormanager _doormanager;
        private readonly IPointCreator _pointCreator;
        private readonly IPlayerInfoManager _playerInfoManager;

        public VehicleShowroom() {}
        public VehicleShowroom(IDoormanager doormanager, IPointCreator pointCreator, IPlayerInfoManager playerInfoManager) {
            _doormanager = doormanager;
            _pointCreator = pointCreator;
            _playerInfoManager = playerInfoManager;
        }

        /// <summary>
        /// Инизиализировать место
        /// </summary>
        public override void Initialize() {
            LoadInteriorsAndDoors();
            foreach (var showroom in ShowroomsGetter.GetShowrooms()) {
                _pointCreator.CreateBlip(showroom.Position, showroom.Blip, 30, name: showroom.Name);
                CreateSeller(showroom);
                if (showroom.Type == ShowroomType.Expensive) {
                    var enter = _pointCreator.CreateMarker(Marker.UpsideDownCone, showroom.Position, Colors.Yellow, 1.5f);
                    enter.ColShape.onEntityEnterColShape += (shape, entity) => TriggerShowroomEnter(entity, showroom, true);
                    var exit = _pointCreator.CreateMarker(Marker.UpsideDownCone, showroom.ExitPosition, Colors.Yellow, 1.5f);
                    exit.ColShape.onEntityEnterColShape += (shape, entity) => TriggerShowroomEnter(entity, showroom, false);
                }
            }
        }

        /// <summary>
        /// Загружает текстуры салонов и двери
        /// </summary>
        private void LoadInteriorsAndDoors() {
            API.requestIpl("imp_dt1_02_modgarage");
            var mainEnterLeft = _doormanager.Register(2059227086, new Vector3(-59.89302, -1092.952, 26.88362));
            _doormanager.SetDoorState(mainEnterLeft, false, 1);
            var mainEnterRight = _doormanager.Register(1417577297, new Vector3(-60.54582, -1094.749, 26.88872));
            _doormanager.SetDoorState(mainEnterRight, false, 1);
            var parkingEnterLeft = _doormanager.Register(2059227086, new Vector3(-39.13366, -1108.218, 26.7198));
            _doormanager.SetDoorState(parkingEnterLeft, false, 1);
            var parkingEnterRight = _doormanager.Register(1417577297, new Vector3(-37.33113, -1108.873, 26.7198));
            _doormanager.SetDoorState(parkingEnterRight, false, 1);
        }

        /// <summary>
        /// Создать продавца
        /// </summary>
        private void CreateSeller(VehicleShowroomModel showroom) {
            var name = showroom.Type == ShowroomType.Cheap ? "Саймон" : "Консультант";
            var seller = _pointCreator.CreatePed(
                showroom.Seller, name, showroom.SellerPosition, showroom.SellerRotation,
                showroom.SellerMarkerPosition, Colors.VividCyan
            );
            seller.ColShape.onEntityEnterColShape += (shape, entity) => PlayerComeToSeller(entity, showroom);
            seller.ColShape.onEntityExitColShape += PlayerAwayFromSeller;
        }

        /// <summary>
        /// Игрок подошел к консультанту
        /// </summary>
        private async void PlayerComeToSeller(NetHandle entity, VehicleShowroomModel showroom) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            var vehicles = await SerializeVehicles(player, showroom.Type);
            API.triggerClientEvent(player, 
                ServerEvent.SHOW_SHOWROOM_MENU, vehicles.Item1, vehicles.Item2,
                JsonConvert.SerializeObject(showroom.ShowroomPositions), (int) showroom.Type, showroom.District
            );
        }

        /// <summary>
        /// Возвращает данные о транспорте салона и игрока
        /// </summary>
        private Task<Tuple<string, string>> SerializeVehicles(Client player, ShowroomType type) {
            var showroomVehiclesData = JsonConvert.SerializeObject(ShowroomsGetter.GetVehicles(type));
            var playerVehicles = new List<ShowroomVehicle>();
            foreach (var vehicle in _playerInfoManager.GetInfo(player).Vehicles.Values) {
                var price = ShowroomsGetter.GetSellPrice(vehicle.Hash);
                if (Validator.IsValid(price)) {
                    playerVehicles.Add(new ShowroomVehicle {Id = (int) vehicle.Id, Hash = vehicle.Hash, Price = price});
                }
            }
            var playerVehiclesData = JsonConvert.SerializeObject(playerVehicles);
            return Task.FromResult(new Tuple<string, string>(showroomVehiclesData, playerVehiclesData));
        }

        /// <summary>
        /// Игрок отошел от консультанта
        /// </summary>
        private void PlayerAwayFromSeller(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_SHOWROOM_MENU);
        }

        /// <summary>
        /// Обработчик двери салона
        /// </summary>
        private void TriggerShowroomEnter(NetHandle entity, VehicleShowroomModel showroom, bool enter) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.sendNativeToPlayer(player, Hash.DISPLAY_RADAR, !enter);
            API.setEntityPosition(player, enter ? showroom.PositionAfterEnter : showroom.PositionAfterExit);
        }
    }
}