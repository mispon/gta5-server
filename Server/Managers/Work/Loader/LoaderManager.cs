using System.Collections.Generic;
using System.Linq;
using gta_mp_server.Constant;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.Work.Loader.Interfaces;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;

namespace gta_mp_server.Managers.Work.Loader {
    /// <summary>
    /// Логика работы грузчиком начального уровня
    /// </summary>
    internal class LoaderManager : Script, ILoaderManager {
        internal const string LOADER_TAKE_KEY = "LoaderTakePoint";
        internal const string LOADER_TAKE_VALUE = "LoaderTakePoint_{0}";
        internal const string LOADER_PUT_KEY = "LoaderPutPoint";
        internal const string LOADER_PUT_VALUE = "LoaderPutPoint_{0}";
        private const float COLSHAPE_RANGE = 2.0F;

        private HashSet<LoaderPoint> _takePoints;
        private HashSet<LoaderPoint> _putPoints;

        private readonly ILoaderEventHandler _eventHandler;

        public LoaderManager() {}
        public LoaderManager(ILoaderEventHandler eventHandler) {
            _eventHandler = eventHandler;
        }

        /// <summary>
        /// Инициализировать свойства
        /// </summary>
        public void Initialize() {
            CreateTakePoints();
            CreatePutPoints();
        }

        /// <summary>
        /// Инициализировать точки получения груза
        /// </summary>
        private void CreateTakePoints() {
            _takePoints = new HashSet<LoaderPoint>();
            for (var i = 0; i < LoaderDataGetter.TakePositions.Count; i++) {
                var colShape = API.createSphereColShape(LoaderDataGetter.TakePositions[i], COLSHAPE_RANGE);
                colShape.setData(LOADER_TAKE_KEY, string.Format(LOADER_TAKE_VALUE, i));
                colShape.onEntityEnterColShape += (shape, entity) => _eventHandler.OnTakeThing(shape, entity);
                var point = new LoaderPoint {
                    ColShape = colShape,
                    Position = LoaderDataGetter.TakePositions[i],
                    Number = i
                };
                _takePoints.Add(point);
            }
        }

        /// <summary>
        /// Инициализировать позиции точек сдачи груза
        /// </summary>
        private void CreatePutPoints() {
            _putPoints = new HashSet<LoaderPoint>();
            for (var i = 0; i < LoaderDataGetter.PutPositions.Count; i++) {
                var colShape = API.createSphereColShape(LoaderDataGetter.PutPositions[i], COLSHAPE_RANGE);
                colShape.setData(LOADER_PUT_KEY, string.Format(LOADER_PUT_VALUE, i));
                colShape.onEntityEnterColShape += OnEnterPutColShape;
                var point = new LoaderPoint {
                    ColShape = colShape,
                    Position = LoaderDataGetter.PutPositions[i],
                    Number = i
                };
                _putPoints.Add(point);
            }
        }

        /// <summary>
        /// Обработчик сдачи груза
        /// </summary>
        private void OnEnterPutColShape(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            var success = _eventHandler.OnPutThing(shape, player);
            if (success) {
                NextTakePoint(player);
                NextPutPoint(player);
            }
        }

        /// <summary>
        /// Показать следующую случайную точку получения
        /// </summary>
        private void NextTakePoint(Client player) {
            var takeNum = ActionHelper.Random.Next(_takePoints.Count);
            var takePoint = _takePoints.First(e => e.Number == takeNum);
            player.setData(LOADER_TAKE_KEY, string.Format(LOADER_TAKE_VALUE, takePoint.Number));
            API.triggerClientEvent(player, ServerEvent.SHOW_TAKE_LOADER_POINT, takePoint.Position);
        }

        /// <summary>
        /// Показать следующую случайную точку сдачи
        /// </summary>
        private void NextPutPoint(Client player) {
            var putNum = ActionHelper.Random.Next(_putPoints.Count);
            var putPoint = _putPoints.First(e => e.Number == putNum);
            player.setData(LOADER_PUT_KEY, string.Format(LOADER_PUT_VALUE, putPoint.Number));
            API.triggerClientEvent(player, ServerEvent.SHOW_PUT_LOADER_POINT, putPoint.Position);
        }
    }
}