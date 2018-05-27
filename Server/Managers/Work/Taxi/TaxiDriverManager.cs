using System;
using System.Collections.Generic;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Places;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles;
using gta_mp_server.Managers.Work.Taxi.Interfaces;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Ninject;
using Ninject.Syntax;

namespace gta_mp_server.Managers.Work.Taxi {
    /// <summary>
    /// Логика работы таксистом
    /// </summary>
    public class TaxiDriverManager : Script, ITaxiDriverManager {
        private const int PRICE_FOR_KM = 10;
        private const string RIDE_COST = "RideCost";
        private const string TAXI_DRIVER = "CurrentTaxiDriver";

        private readonly Dictionary<long, WorkReward> _workRewards = new Dictionary<long, WorkReward> {
            [1] = new WorkReward {Salary = 2, Exp = 1, WorkExp = 12},
            [2] = new WorkReward {Salary = 3, Exp = 1, WorkExp = 12},
            [3] = new WorkReward {Salary = 4, Exp = 2, WorkExp = 10},
            [4] = new WorkReward {Salary = 6, Exp = 3, WorkExp = 12},
            [5] = new WorkReward {Salary = 8, Exp = 3, WorkExp = 0}
        };

        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IWorkInfoManager _workInfoManager;

        public TaxiDriverManager() : this(ServerKernel.Kernel) {
            API.onPlayerEnterVehicle += OnEnterTaxi;
            API.onPlayerExitVehicle += OnExitTaxi;
            ClientEventHandler.Add("CallTaxi", OnTaxiCall);
        }

        public TaxiDriverManager(IResolutionRoot kernel) {
            _playerInfoManager = kernel.Get<IPlayerInfoManager>();
            _workInfoManager = kernel.Get<IWorkInfoManager>();
        }

        /// <summary>
        /// Обработчик входа в таки
        /// </summary>
        private void OnEnterTaxi(Client player, NetHandle vehicle, int seat) {
            if (API.getEntityData(vehicle, TaxiDepot.TAXI_KEY) == null) {
                return;
            }
            if (seat < 0) {
                OnDriverEnter(player);
            }
            else {
                OnPassengerEnter(player);
            }
        }

        /// <summary>
        /// Обработчик выхода из такси
        /// </summary>
        private void OnExitTaxi(Client player, NetHandle vehicle, int seat) {
            if (API.getEntityData(vehicle, TaxiDepot.TAXI_KEY) == null) {
                return;
            }
            if (seat < 0) {
                OnDriverExit(player);
            }
            else {
                OnPassengerExit(player);
            }
        }

        /// <summary>
        /// Обрабочик входа водителя
        /// </summary>
        private void OnDriverEnter(Client player) {
            if (player.getData(WorkData.IS_TAXI_DIVER) == null) {
                API.sendNotificationToPlayer(player, "~r~Вы не работаете водителем такси");
                API.warpPlayerOutOfVehicle(player);
                return;
            }
            API.triggerClientEvent(player, ServerEvent.SHOW_SUBTITLE, "Ожидайте заказов или ищите людей по городу");
        }

        /// <summary>
        /// Обработчик выхода водителя
        /// </summary>
        private void OnDriverExit(Client player) {
            if (player.getData(WorkData.IS_TAXI_DIVER) == null) {
                return;
            }
            var position = API.getEntityPosition(player);
            var distance = Vector3.Distance(MainPosition.TaxiDriver, position);
            if (distance >= 30) {
                API.triggerClientEvent(player, ServerEvent.SHOW_SUBTITLE, "Чтобы получить ~y~зарплату~w~, вернитесь на парковку");
            }
        }

        /// <summary>
        /// Обработчик входа пассажира
        /// </summary>
        private void OnPassengerEnter(Client player) {
            API.triggerClientEvent(player, ServerEvent.SHOW_GPS_MENU);
        }

        /// <summary>
        /// Обработчик выхода пассажира
        /// </summary>
        private void OnPassengerExit(Client passenger) {
            API.triggerClientEvent(passenger, ServerEvent.HIDE_GPS_MENU);
            var driver = (Client) passenger.getData(TAXI_DRIVER);
            if (driver == null || !passenger.hasData(TAXI_DRIVER)) {
                return;
            }
            var cost = (int) passenger.getData(RIDE_COST);
            SetDriverReward(driver, cost);
            _playerInfoManager.SetBalance(passenger, -cost);
            passenger.resetData(RIDE_COST);
            passenger.resetData(TAXI_DRIVER);
            API.sendNotificationToPlayer(passenger, $"Списано ~b~{cost}$");
        }

        /// <summary>
        /// Зачислить зарплату таксисту
        /// </summary>
        private void SetDriverReward(Client player, int taxiFee) {
            var workInfo = _workInfoManager.GetWorkInfo(player, WorkType.TaxiDriver);
            var reward = _workRewards[workInfo.Level];
            _workInfoManager.SetSalary(player, WorkType.TaxiDriver, taxiFee * reward.Salary);
            _workInfoManager.SetExperience(player, WorkType.TaxiDriver, reward.WorkExp);
            _playerInfoManager.SetExperience(player, taxiFee * reward.Exp);
        }

        /// <summary>
        /// Возвращает водителя такси
        /// </summary>
        private Client GetTaxiDriver(Client player) {
            var vehicle = API.getPlayerVehicle(player);
            return API.getVehicleDriver(vehicle);
        }

        /// <summary>
        /// Обработчик выбранного клиентом маршрута
        /// </summary>
        public void ProcessPassengerTarget(Client player, Vector3 target) {
            var driver = GetTaxiDriver(player);
            if (driver == null) {
                API.sendNotificationToPlayer(player, "~r~В машине нет водителя", true);
                return;
            }
            var balance = _playerInfoManager.GetInfo(player).Balance;
            var cost = CalculateRideCost(player, target);
            if (balance < cost) {
                API.sendNotificationToPlayer(player, $"~r~У вас не хватает денег на данную поездку", true);
                return;
            }
            player.setData(TAXI_DRIVER, driver);
            player.setData(RIDE_COST, cost);
            API.sendNotificationToPlayer(player, $"Стоимость поездки ~y~{cost}$");
            API.triggerClientEvent(player, ServerEvent.SHOW_GPS_TARGET, target);
            API.triggerClientEvent(driver, ServerEvent.SHOW_GPS_TARGET, target);
            API.triggerClientEvent(player, ServerEvent.HIDE_GPS_MENU);
        }

        /// <summary>
        /// Рассчитать стоимось поездки
        /// </summary>
        private int CalculateRideCost(Client passenger, Vector3 target) {
            var currentPosition = API.getEntityPosition(passenger);
            var distance = Vector3.Distance(target, currentPosition);
            return (int) Math.Round(distance / VehicleManager.ONE_KILOMETER * PRICE_FOR_KM);
        }

        /// <summary>
        /// Обработчик события вызова такси
        /// </summary>
        private void OnTaxiCall(Client player, object[] args) {
            // todo: отобразить всем таксистам поступивший заказ и дать его тому, кто первый принял
        }
    }
}