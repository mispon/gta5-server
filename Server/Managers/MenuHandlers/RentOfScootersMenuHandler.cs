using System.Collections.Generic;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System.Linq;
using gta_mp_data.Entity;
using gta_mp_server.Helpers;

namespace gta_mp_server.Managers.MenuHandlers {
    /// <summary>
    /// Обработчик меню аренды скутеров
    /// </summary>
    internal class RentOfScootersMenuHandler : Script, IMenu {
        private const int FUEL_LEVEL = 50;

        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IClanManager _clanManager;

        private static readonly Dictionary<int, VehicleHash> _scooters = new Dictionary<int, VehicleHash> {
            [0] = VehicleHash.Scorcher,
            [1] = VehicleHash.Faggio,
            [2] = VehicleHash.Fcr
        };

        public RentOfScootersMenuHandler() {}
        public RentOfScootersMenuHandler(IPlayerInfoManager playerInfoManager, IClanManager clanManager) {
            _playerInfoManager = playerInfoManager;
            _clanManager = clanManager;
        }

        /// <summary>
        /// Инициализировать меню
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.RENT_SCOOTER, RentScooter);
        }

        /// <summary>
        /// Проверяет, является ли транспорт арендованным
        /// </summary>
        internal static bool IsScooter(int model) {
            return _scooters.Values.Contains((VehicleHash) model);
        }

        /// <summary>
        /// Обработчик аренды велосипедов и скутеров
        /// </summary>
        private void RentScooter(Client player, object[] args) {
            var scooterInfo = GetScooterInfo(args);
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (playerInfo.Balance < scooterInfo.Price) {
                API.sendNotificationToPlayer(player, "~r~У вас недостаточно денег!", true);
                return;
            }
            CreateScooter(scooterInfo, playerInfo.AccountId);
            playerInfo.Balance -= scooterInfo.Price;
            _playerInfoManager.RefreshUI(player, playerInfo);
            var district = (int) args[0];
            _clanManager.ReplenishClanBalance(district, scooterInfo.Price);
            API.sendNotificationToPlayer(player, $"~b~Списано {scooterInfo.Price}$");
            API.triggerClientEvent(player, ServerEvent.HIDE_SCOOTERS_MENU);
        }

        /// <summary>
        /// Создает скутер
        /// </summary>
        private void CreateScooter(ScooterInfo scooterInfo, long ownerId) {
            var scooterHash = _scooters[scooterInfo.Type];
            var vehicle = API.createVehicle(
                scooterHash, scooterInfo.Position, scooterInfo.Rotation, 
                ActionHelper.Random.Next(159), ActionHelper.Random.Next(159)
            );
            vehicle.setData(VehicleManager.OWNER_ID, ownerId);
            vehicle.setData(VehicleManager.MAX_FUEL, FUEL_LEVEL);
            vehicle.setData(VehicleManager.DONT_RESTORE, true);
            API.setVehicleFuelLevel(vehicle, FUEL_LEVEL);
        }

        /// <summary>
        /// Распарсить входные параметры в модель
        /// </summary>
        private ScooterInfo GetScooterInfo(object[] args) {
            return new ScooterInfo {
                Type = (int) args[0],
                Price = (int) args[1],
                Position = (Vector3) args[2],
                Rotation = (Vector3) args[3]
            };
        }

        /// <summary>
        /// Информация о выбранном ТС
        /// </summary>
        private class ScooterInfo {
            public int Type { get; set; }
            public int Price { get; set; }
            public Vector3 Position { get; set; }
            public Vector3 Rotation { get; set; }
        }
    }
}