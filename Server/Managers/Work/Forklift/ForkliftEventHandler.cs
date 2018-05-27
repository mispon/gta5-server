using System;
using System.Collections.Generic;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Helpers;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Forklift.Interfaces;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Ninject;
using Ninject.Syntax;
using Object = GrandTheftMultiplayer.Server.Elements.Object;

namespace gta_mp_server.Managers.Work.Forklift {
    /// <summary>
    /// Обработчик работы на погрузчике
    /// </summary>
    internal class ForkliftEventHandler : Script, IForkliftEventHandler {
        protected const string THING_KEY = "ForkliftThing";

        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IWorkInfoManager _workInfoManager;

        private static readonly Dictionary<long, WorkReward> _workRewards = new Dictionary<long, WorkReward> {
            [1] = new WorkReward {Salary = 9, Exp = 5, WorkExp = 12},
            [2] = new WorkReward {Salary = 12, Exp = 7, WorkExp = 13},
            [3] = new WorkReward {Salary = 15, Exp = 9, WorkExp = 14},
            [4] = new WorkReward {Salary = 19, Exp = 11, WorkExp = 15},
            [5] = new WorkReward {Salary = 24, Exp = 13, WorkExp = 0}
        };

        public ForkliftEventHandler() : this(ServerKernel.Kernel) {
            API.onPlayerEnterVehicle += (player, vehicle, seat) => {
                var hash = (VehicleHash) API.getEntityModel(vehicle);
                if (hash != VehicleHash.Forklift || player.hasData(WorkData.IS_FORKLIFT)) {
                    return;
                }
                API.sendNotificationToPlayer(player, "~r~Вы не работаете водителем погрузчика", true);
                API.warpPlayerOutOfVehicle(player);
            };
        }

        public ForkliftEventHandler(IResolutionRoot kernel) {
            _playerInfoManager = kernel.Get<IPlayerInfoManager>();
            _workInfoManager = kernel.Get<IWorkInfoManager>();
        }

        /// <summary>
        /// Обработчик получения предмета
        /// </summary>
        public void OnTakeThing(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!(ForkliftCorrect(player, () => !player.hasData(THING_KEY)) && IsRightTakePoint(shape, player))) {
                return;
            }
            var thing = TakeThing(player, -1513883840);
            var vehicle = API.getPlayerVehicle(player);
            API.attachEntityToEntity(thing, vehicle, "forks_attach", new Vector3(0.0, -0.15, 0.17), new Vector3());
        }

        /// <summary>
        /// Взять груз
        /// </summary>
        private Object TakeThing(Client player, int model) {
            var result = API.createObject(model, player.position, new Quaternion());
            player.setData(THING_KEY, result);
            return result;
        }

        /// <summary>
        /// Обработчик сдачи предмета
        /// </summary>
        public bool OnPutThing(ColShape shape, Client player) {
            if (!(ForkliftCorrect(player, () => player.hasData(THING_KEY)) && IsRightPutPoint(shape, player))) {
                return false;
            }
            PutThing(player);
            var workLevel = _workInfoManager.GetWorkInfo(player, WorkType.Forklift).Level;
            SetPlayerReward(player, _workRewards[workLevel]);
            return true;
        }


        /// <summary>
        /// Положить груз
        /// </summary>
        private void PutThing(Client player) {
            if (!player.hasData(THING_KEY)) {
                return;
            }
            var thing = (Object) player.getData(THING_KEY);
            API.deleteEntity(thing);
            player.resetData(THING_KEY);
            API.stopPlayerAnimation(player);
        }

        /// <summary>
        /// Проверка, что водитель погрузчика может положить предмет
        /// </summary>
        private bool ForkliftCorrect(Client player, Func<bool> thingChecker) {
            return PlayerHelper.PlayerCorrect(player, true) && player.hasData(WorkData.IS_FORKLIFT) && thingChecker() && PlayerInForklift(player);
        }

        /// <summary>
        /// Проверка, что игрок находится в погрузчике
        /// </summary>
        private bool PlayerInForklift(Client player) {
            var vehicle = API.getPlayerVehicle(player);
            if (VehicleHash.Forklift == (VehicleHash) API.getEntityModel(vehicle)) {
                return true;
            }
            API.sendNotificationToPlayer(player, "~r~Вы не находитесь в погрузчике", true);
            return false;
        }

        /// <summary>
        /// Проверяет, что игрок на нужной точке получения
        /// </summary>
        private static bool IsRightTakePoint(ColShape shape, Client player) {
            return shape.getData(ForkliftManager.FORKLIFT_TAKE_KEY) == player.getData(ForkliftManager.FORKLIFT_TAKE_KEY);
        }

        /// <summary>
        /// Проверяет, что игрок на нужной точке сдачи
        /// </summary>
        private static bool IsRightPutPoint(ColShape shape, Client player) {
            return shape.getData(ForkliftManager.FORKLIFT_PUT_KEY) == player.getData(ForkliftManager.FORKLIFT_PUT_KEY);
        }

        /// <summary>
        /// Обновить прогресс текущей рабочей смены
        /// </summary>
        private void SetPlayerReward(Client player, WorkReward reward) {
            _workInfoManager.SetSalary(player, WorkType.Forklift, reward.Salary);
            _workInfoManager.SetExperience(player, WorkType.Forklift, reward.WorkExp);
            _playerInfoManager.SetExperience(player, reward.Exp);
        }
    }
}