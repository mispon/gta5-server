using System.Collections.Generic;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.IoC;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Places;
using gta_mp_server.Managers.Work.Taxi.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Ninject;
using Ninject.Syntax;

namespace gta_mp_server.Managers.MenuHandlers {
    /// <summary>
    /// Обработчик событий от GPS
    /// </summary>
    internal class GpsMenuHandler : Script, IMenu {
        private readonly ITaxiDriverManager _taxiManager;

        /// <summary>
        /// Основные места сервера
        /// </summary>
        private static readonly Dictionary<int, Vector3> _targets = new Dictionary<int, Vector3> {
            [0] = MainPosition.DrivingSchool,
            [1] = MainPosition.TextileMill,
            [2] = MainPosition.ScrapMetalDump,
            [3] = MainPosition.Building,
            [4] = MainPosition.BusDriver,
            [5] = MainPosition.Parking,
            [6] = MainPosition.ParkingFine,
            [7] = MainPosition.Bistro,
            [8] = MainPosition.Hospital,
            [9] = MainPosition.TaxiDriver,
            [10] = MainPosition.PoliceDepartment,
            [11] = MainPosition.Port,
            [12] = MainPosition.Race,
            [13] = MainPosition.StreetFights,
            [14] = MainPosition.Tuning,
            [15] = MainPosition.AirPort,
            [16] = MainPosition.FishingVillage,
            [17] = MainPosition.Farm
        };

        public GpsMenuHandler() : this(ServerKernel.Kernel) {}
        public GpsMenuHandler(IResolutionRoot kernel) {
            _taxiManager = kernel.Get<ITaxiDriverManager>();
        }

        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.GPS_POINT, ProcessTarget);
        }

        /// <summary>
        /// Обработка выбора маршрута
        /// </summary>
        private void ProcessTarget(Client player, object[] args) {
            var type = (int) args[0];
            var target = _targets[type];
            if (target == null) {
                return;
            }
            if (IsTaxiPassenger(player)) {
                _taxiManager.ProcessPassengerTarget(player, target);
                return;
            }
            API.triggerClientEvent(player, ServerEvent.SHOW_GPS_TARGET, target);
            API.triggerClientEvent(player, ServerEvent.HIDE_GPS_MENU);
        }

        /// <summary>
        /// Является ли игрок пассажиром в такси
        /// </summary>
        private bool IsTaxiPassenger(Client player) {
            var vehicle = API.getPlayerVehicle(player);
            return API.getEntityData(vehicle, TaxiDepot.TAXI_KEY) != null && API.getPlayerVehicleSeat(player) >= 0;
        }
    }
}