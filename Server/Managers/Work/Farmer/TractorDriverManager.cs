using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Helpers;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Places.FarmPlace;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Vehicles;
using gta_mp_server.Managers.Work.Farmer.Interfaces;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using Ninject;
using Ninject.Syntax;

namespace gta_mp_server.Managers.Work.Farmer {
    /// <summary>
    /// Логика работы трактористом
    /// </summary>
    internal class TractorDriverManager : Script, ITractorDriverManager {
        internal const string TRACTOR_POINT_KEY = "TractorPoint";
        private const string TRACTOR_POINT_VALUE = "TractorPoint_{0}";
        private const string POINT_INDEX = "TractorPointIndex";
        private const string POINTS_PROCESSED = "PointsProcessed";
        private const string HARVEST_TRAILER_KEY = "HarvestTrailer";
        private const string RAKE_TRAILER_KEY = "RakeTrailer";

        private readonly Dictionary<int, WorkReward> _rewards = new Dictionary<int, WorkReward> {
            [1] = new WorkReward {Salary = 8, Exp = 4, WorkExp = 12},
            [2] = new WorkReward {Salary = 10, Exp = 6, WorkExp = 13},
            [3] = new WorkReward {Salary = 12, Exp = 8, WorkExp = 14},
            [4] = new WorkReward {Salary = 14, Exp = 10, WorkExp = 15},
            [5] = new WorkReward {Salary = 16, Exp = 12, WorkExp = 0}
        };

        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IWorkInfoManager _workInfoManager;

        public TractorDriverManager() : this(ServerKernel.Kernel) {
            API.onPlayerEnterVehicle += OnPlayerEnterTractor;
            API.onPlayerExitVehicle += OnPlayerExitTractor;
            API.onVehicleTrailerChange += OnTractorTrailerChange;
        }

        public TractorDriverManager(IResolutionRoot kernel) {
            _playerInfoManager = kernel.Get<IPlayerInfoManager>();
            _workInfoManager = kernel.Get<IWorkInfoManager>();
        }

        /// <summary>
        /// Обработчик входа в трактор
        /// </summary>
        private void OnPlayerEnterTractor(Client player, NetHandle vehicle, int seat) {
            if (!API.hasEntityData(vehicle, Farm.TRACTOR_KEY)) {
                return;
            }
            if (!player.hasData(WorkData.IS_TRACTOR_DRIVER)) {
                API.sendNotificationToPlayer(player, "~r~Вы не работаете трактористом", true);
                player.warpOutOfVehicle();
                return;
            }
            ActionHelper.SetAction(player, 5100, () => 
                API.triggerClientEvent(player, ServerEvent.SHOW_SUBTITLE, "Найдите ~y~прицеп ~w~для рыхления почвы")
            );
        }

        /// <summary>
        /// Обработчик выхода из трактора
        /// </summary>
        private void OnPlayerExitTractor(Client player, NetHandle vehicle, int seat) {
            if (!API.hasEntityData(vehicle, Farm.TRACTOR_KEY)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_TRACTOR_POINT);
        }

        /// <summary>
        /// Обработчик изменения наличия прицепа
        /// </summary>
        private void OnTractorTrailerChange(NetHandle vehicleHandle, NetHandle trailerHandle) {
            if (!API.hasEntityData(vehicleHandle, Farm.TRACTOR_KEY)) {
                return;
            }
            var tractor = API.getEntityFromHandle<Vehicle>(vehicleHandle);
            var player = API.getVehicleDriver(tractor);
            if (!trailerHandle.IsNull) {
                OnTrailerAttach(player, trailerHandle);
            }
            else {
                OnTrailerDetach(player);
            }
        }

        /// <summary>
        /// Обработка сцепления
        /// </summary>
        private void OnTrailerAttach(Client player, NetHandle trailerHandle) {
            var hash = (VehicleHash)API.getEntityModel(trailerHandle);
            switch (hash) {
                case VehicleHash.RakeTrailer:
                    var currentPoint = player.hasData(POINT_INDEX) ? (int) player.getData(POINT_INDEX) : -1;
                    ShowNextPoit(player, currentPoint);
                    var trailer = API.getEntityFromHandle<Vehicle>(trailerHandle);
                    trailer.setData(VehicleManager.DONT_RESTORE, true);
                    player.setData(RAKE_TRAILER_KEY, trailer);
                    break;
                case VehicleHash.BaleTrailer:
                    player.setData(HARVEST_TRAILER_KEY, trailerHandle);
                    API.triggerClientEvent(player, ServerEvent.SHOW_HARVEST_DELIVERY_POINT, FarmDataGetter.HarvestDeliveryPoint);
                    NotifyTractorDrivers(tractorDriver => API.triggerClientEvent(tractorDriver, ServerEvent.HIDE_LOADED_TRAILER));
                    break;
            }
        }

        /// <summary>
        /// Обработка отцепления
        /// </summary>
        private void OnTrailerDetach(Client player) {
            API.triggerClientEvent(player, ServerEvent.HIDE_TRACTOR_POINT);
            if (player.hasData(HARVEST_TRAILER_KEY)) {
                var trailer = (NetHandle)player.getData(HARVEST_TRAILER_KEY);
                NotifyTractorDrivers(
                    tractorDriver => API.triggerClientEvent(tractorDriver, ServerEvent.SHOW_LOADED_TRAILER, API.getEntityPosition(trailer))
                );
                player.resetData(HARVEST_TRAILER_KEY);
                API.triggerClientEvent(player, ServerEvent.HIDE_HARVEST_DELIVERY_POINT);
            }
            else if (player.hasData(RAKE_TRAILER_KEY)) {
                var trailer = (Vehicle) player.getData(RAKE_TRAILER_KEY);
                trailer.resetData(VehicleManager.DONT_RESTORE);
            }
        }

        /// <summary>
        /// Оповещение трактористов об изменениях
        /// </summary>
        private void NotifyTractorDrivers(Action<Client> action) {
            foreach (var tractorDriver in API.getAllPlayers().Where(e => e != null && e.hasData(WorkData.IS_TRACTOR_DRIVER))) {
                action(tractorDriver);
            }
        }

        /// <summary>
        /// Инизиализировать работу
        /// </summary>
        public void Initialize() {
            CreateRoute();
            CreateHarvestDeliveryPoint();
        }

        /// <summary>
        /// Создать точки маршрута трактора
        /// </summary>
        private void CreateRoute() {
            for (var i = 0; i < FarmDataGetter.TractorRoute.Count; i++) {
                var shape = API.createSphereColShape(FarmDataGetter.TractorRoute[i], 3f);
                shape.setData(TRACTOR_POINT_KEY, string.Format(TRACTOR_POINT_VALUE, i));
                shape.setData(POINT_INDEX, i);
                shape.onEntityEnterColShape += OnPlayerEnterPoint;
            }
        }

        /// <summary>
        /// Обработчик входа в точку сбора
        /// </summary>
        private void OnPlayerEnterPoint(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PointCorrect(shape, player)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_TRACTOR_POINT);
            var pointsCount = player.hasData(POINTS_PROCESSED) ? (int) player.getData(POINTS_PROCESSED) : 0;
            pointsCount++;
            if (pointsCount == 5) {
                SetReward(player);
                pointsCount = 0;
            }
            player.setData(POINTS_PROCESSED, pointsCount);
            player.resetData(TRACTOR_POINT_KEY);
            ShowNextPoit(player, (int) shape.getData(POINT_INDEX));
        }

        /// <summary>
        /// Проверяет корректность точки
        /// </summary>
        private static bool PointCorrect(ColShape shape, Client player) {
            if (!(PlayerHelper.PlayerCorrect(player, true) && player.hasData(TRACTOR_POINT_KEY))) {
                return false;
            }
            return shape.getData(TRACTOR_POINT_KEY) == player.getData(TRACTOR_POINT_KEY);
        }

        /// <summary>
        /// Показать следующий маркер
        /// </summary>
        private void ShowNextPoit(Client player, int currentIndex) {
            var nextPointIndex = currentIndex + 1;
            if (nextPointIndex == FarmDataGetter.TractorRoute.Count - 1) {
                Farm.Instance.BuffFarm();
                nextPointIndex = 0;
            }
            player.setData(POINT_INDEX, nextPointIndex);
            player.setData(TRACTOR_POINT_KEY, string.Format(TRACTOR_POINT_VALUE, nextPointIndex));
            API.triggerClientEvent(player, ServerEvent.SHOW_TRACTOR_POINT, FarmDataGetter.TractorRoute[nextPointIndex]);
        }

        /// <summary>
        /// Создает точку доставки урожая
        /// </summary>
        private void CreateHarvestDeliveryPoint() {
            var point = API.createSphereColShape(FarmDataGetter.HarvestDeliveryPoint, 3f);
            point.onEntityEnterColShape += (shape, entity) => {
                var player = API.getPlayerFromHandle(entity);
                if (!(PlayerHelper.PlayerCorrect(player, true) && player.hasData(HARVEST_TRAILER_KEY))) {
                    return;
                }
                var trailer = (NetHandle) player.getData(HARVEST_TRAILER_KEY);
                player.resetData(HARVEST_TRAILER_KEY);
                API.deleteEntity(trailer);
                Farm.Instance.RemoveTrailer();
                SetReward(player, 10);
                API.triggerClientEvent(player, ServerEvent.HIDE_HARVEST_DELIVERY_POINT);
            };
        }

        /// <summary>
        /// Выдать награду
        /// </summary>
        private void SetReward(Client player, int salaryCoef = 1) {
            var workLevel = _workInfoManager.GetWorkInfo(player, WorkType.TractorDriver).Level;
            var reward = _rewards[workLevel];
            _playerInfoManager.SetExperience(player, reward.Exp);
            _workInfoManager.SetSalary(player, WorkType.TractorDriver, reward.Salary * salaryCoef);
            _workInfoManager.SetExperience(player, WorkType.TractorDriver, reward.WorkExp);
        }
    }
}