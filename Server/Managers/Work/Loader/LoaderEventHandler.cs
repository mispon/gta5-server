using System.Collections.Generic;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Helpers;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Loader.Interfaces;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Ninject;
using Ninject.Syntax;

namespace gta_mp_server.Managers.Work.Loader {
    /// <summary>
    /// Обработчик работы грузчиком
    /// </summary>
    internal class LoaderEventHandler : Script, ILoaderEventHandler {
        private const string THING_KEY = "LoaderThing";

        private static readonly Dictionary<long, WorkReward> _workRewards = new Dictionary<long, WorkReward> {
            [1] = new WorkReward { Salary = 5, Exp = 3, WorkExp = 12},
            [2] = new WorkReward { Salary = 7, Exp = 4, WorkExp = 14},
            [3] = new WorkReward { Salary = 9, Exp = 5, WorkExp = 15},
            [4] = new WorkReward { Salary = 12, Exp = 6, WorkExp = 16},
            [5] = new WorkReward { Salary = 15, Exp = 7, WorkExp = 0}
        };

        private readonly IPlayerInfoManager _playerManager;
        private readonly IWorkInfoManager _workInfoManager;

        public LoaderEventHandler() : this(ServerKernel.Kernel) {
            API.onPlayerEnterVehicle += OnLoaderEnterVehicle;
        }

        public LoaderEventHandler(IResolutionRoot kernel) {
            _playerManager = kernel.Get<IPlayerInfoManager>();
            _workInfoManager = kernel.Get<IWorkInfoManager>();
        }

        /// <summary>
        /// Обработчик входа грузчика в машину
        /// </summary>
        private void OnLoaderEnterVehicle(Client player, NetHandle vehicle, int seat) {
            if (!(PlayerHelper.PlayerCorrect(player, true) && IsLoaderWork(player) && player.hasData(THING_KEY))) {
                return;
            }
            PutThing(player);
            API.sendNotificationToPlayer(player, "~r~Вы уронили груз!", true);
        }

        /// <summary>
        /// Обработчик получения предмета
        /// </summary>
        public void OnTakeThing(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!IsRightTakePoint(shape, player) || !LoaderCanTake(player)) {
                return;
            }
            var thing = TakeThing(player, 377646791);
            API.attachEntityToEntity(thing, player, "SKEL_R_HAND", new Vector3(0.1, 0.3, -0.2), new Vector3());
            API.playPlayerAnimation(player, PlayerHelper.LOADER_FLAGS, "anim@heists@box_carry@", "run");
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
            if (!(IsRightPutPoint(shape, player) && LoaderCanPut(player))) {
                return false;
            }
            var workLevel = _workInfoManager.GetWorkInfo(player, WorkType.Loader).Level;
            PutThing(player);
            SetPlayerReward(player, _workRewards[workLevel]);
            return true;
        }

        /// <summary>
        /// Положить груз
        /// </summary>
        internal static void PutThing(Client player) {
            if (!player.hasData(THING_KEY)) return;
            var thing = (Object) player.getData(THING_KEY);
            API.shared.deleteEntity(thing);
            player.resetData(THING_KEY);
            API.shared.stopPlayerAnimation(player);
        }

        /// <summary>
        /// Проверка, что грузчик может взять новый предмет
        /// </summary>
        private bool LoaderCanTake(Client player) {
            return PlayerHelper.PlayerCorrect(player) && IsLoaderWork(player) && PlayerIsNotInVehicle(player) && !player.hasData(THING_KEY);
        }

        /// <summary>
        /// Проверка, что грузчик может положить предмет
        /// </summary>
        private bool LoaderCanPut(Client player) {
            return PlayerHelper.PlayerCorrect(player) && IsLoaderWork(player) && PlayerIsNotInVehicle(player) && player.hasData(THING_KEY);
        }

        /// <summary>
        /// Проверка, что игрок не находится в транспортном средстве
        /// </summary>
        private bool PlayerIsNotInVehicle(Client player) {
            var playerVehicle = API.getPlayerVehicle(player);
            if (playerVehicle.Value != 0) {
                API.sendNotificationToPlayer(player, "~r~Покиньте транспортное средство", true);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Проверка, что игрок работает грузчиком
        /// </summary>
        private static bool IsLoaderWork(Client player) {
            var isLoader = player.getData(WorkData.IS_LOADER);
            return isLoader != null && isLoader;
        }

        /// <summary>
        /// Проверяет, что игрок на нужной точке получения
        /// </summary>
        private static bool IsRightTakePoint(ColShape shape, Client player) {
            return shape.getData(LoaderManager.LOADER_TAKE_KEY) == player?.getData(LoaderManager.LOADER_TAKE_KEY);
        }

        /// <summary>
        /// Проверяет, что игрок на нужной точке сдачи
        /// </summary>
        private static bool IsRightPutPoint(ColShape shape, Client player) {
            return shape.getData(LoaderManager.LOADER_PUT_KEY) == player?.getData(LoaderManager.LOADER_PUT_KEY);
        }

        /// <summary>
        /// Обновить прогресс текущей рабочей смены
        /// </summary>
        private void SetPlayerReward(Client player, WorkReward reward) {
            _workInfoManager.SetSalary(player, WorkType.Loader, reward.Salary);
            _workInfoManager.SetExperience(player, WorkType.Loader, reward.WorkExp);
            _playerManager.SetExperience(player, reward.Exp);
        }
    }
}