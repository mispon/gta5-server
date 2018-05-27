using System;
using System.Collections.Generic;
using gta_mp_data.Enums;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Interfaces;
using gta_mp_server.Managers.Work.Loader;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.MenuHandlers.Work {
    /// <summary>
    /// Обработчик меню работы грузчиком
    /// </summary>
    internal class LoaderMenuHandler : BaseWorkMenu {
        private readonly IWorkEquipmentManager _workEquipmentManager;

        public LoaderMenuHandler() {}
        public LoaderMenuHandler(IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager,
            IWorkEquipmentManager workEquipmentManager): base(playerInfoManager, workInfoManager) {
            _workEquipmentManager = workEquipmentManager;
        }

        /// <summary>
        /// Инициализировать меню работы грузчиком
        /// </summary>
        public override void Initialize() {
            ClientEventHandler.Add(ClientEvent.WORK_AS_LOADER, StartWork);
            ClientEventHandler.Add(ClientEvent.LOADER_SALARY, GetSalary);
        }

        /// <summary>
        /// Начать работу грузчиком
        /// </summary>
        protected void StartWork(Client player, object[] args) {
            WorkInfoManager.CreateInfoIfNeed(player, WorkType.Loader);
            if (HasActiveWork(player)) {
                return;
            }
            WorkInfoManager.SetActivity(player, WorkType.Loader, true);
            player.setData(WorkData.IS_LOADER, true);
            ShowLoaderPoints(player);
            _workEquipmentManager.SetEquipment(player);
            API.sendNotificationToPlayer(player, "Вы начали работу ~b~грузчиком");
            API.triggerClientEvent(player, ServerEvent.HIDE_LOADER_MENU);
        }

        /// <summary>
        /// Показать начальные позиции грузчика
        /// </summary>
        protected void ShowLoaderPoints(Client player) {
            var takePosition = GetRandomPosition(LoaderDataGetter.TakePositions);
            player.setData(LoaderManager.LOADER_TAKE_KEY, string.Format(LoaderManager.LOADER_TAKE_VALUE, takePosition.Item1));
            API.triggerClientEvent(player, ServerEvent.SHOW_TAKE_LOADER_POINT, takePosition.Item2);
            var putPosition = GetRandomPosition(LoaderDataGetter.PutPositions);
            player.setData(LoaderManager.LOADER_PUT_KEY, string.Format(LoaderManager.LOADER_PUT_VALUE, putPosition.Item1));
            API.triggerClientEvent(player, ServerEvent.SHOW_PUT_LOADER_POINT, putPosition.Item2);
        }

        /// <summary>
        /// Возвращает рандомную позицию
        /// Item1 - порядковый номер
        /// Item2 - координаты
        /// </summary>
        private static Tuple<int, Vector3> GetRandomPosition(IReadOnlyList<Vector3> positions) {
            var index = ActionHelper.Random.Next(positions.Count);
            return new Tuple<int, Vector3>(index, positions[index]);
        }

        /// <summary>
        /// Получить зарплату
        /// </summary>
        private void GetSalary(Client player, object[] args) {
            var activeWork = WorkInfoManager.GetActiveWork(player);
            bool TypeChecker() => activeWork.Type == WorkType.Loader;
            if (!WorkIsCorrect(player, activeWork, TypeChecker)) {
                return;
            }
            LoaderEventHandler.PutThing(player);
            WorkInfoManager.SetActivity(player, activeWork.Type, false);
            player.resetData(WorkData.IS_LOADER);
            player.resetData(LoaderManager.LOADER_TAKE_KEY);
            player.resetData(LoaderManager.LOADER_PUT_KEY);
            PlayerInfoManager.SetPlayerClothes(player);
            PayOut(player, activeWork);
            API.triggerClientEvent(player, ServerEvent.HIDE_LOADER_POINTS);
            API.triggerClientEvent(player, ServerEvent.HIDE_LOADER_MENU);
        }
    }
}