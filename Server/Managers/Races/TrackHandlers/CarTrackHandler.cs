using gta_mp_server.Constant;
using gta_mp_server.Managers.Races.Data;
using gta_mp_server.Managers.Races.Interfaces;
using gta_mp_server.Managers.Races.TrackHandlers.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Ninject;

namespace gta_mp_server.Managers.Races.TrackHandlers {
    /// <summary>
    /// Обработчик трассы автогонок
    /// </summary>
    internal class CarTrackHandler : TrackHandler {
        private readonly Race _carRace;

        public CarTrackHandler() {}
        public CarTrackHandler([Named("CarRace")] Race carRace) {
            _carRace = carRace;
            LastPoint = 42;
        }

        /// <summary>
        /// Создать трассу
        /// </summary>
        public override void CreateTrack() {
            for (var i = 0; i < CarRaceData.TrackPositions.Count; i++) {
                var position = CarRaceData.TrackPositions[i];
                var point = API.createSphereColShape(position, 8f);
                point.dimension = Dimension.CAR_RACE;
                var pointNumber = i;
                point.onEntityEnterColShape += (shape, entity) => PlayerEnterPoint(entity, pointNumber, Dimension.CAR_RACE);
            }
        }

        /// <summary>
        /// Возвращает поворот последней точки
        /// </summary>
        protected override Vector3 GetFinishPointRotation() {
            return CarRaceData.FinishPointRotation;
        }

        /// <summary>
        /// Возвращает позицию следующей точки трассы
        /// </summary>
        protected override Vector3 GetNextPosition(int index) {
            return CarRaceData.TrackPositions[index];
        }

        /// <summary>
        /// Добавляет победителя
        /// </summary>
        protected override void AddWinner(Client player) {
            _carRace.AddWinner(player);
        }

        /// <summary>
        /// Отображает первую точку трассы
        /// </summary>
        internal static void ShowFirstPoint(Client player) {
            player.setData(GetPointKey(0), true);
            API.shared.triggerClientEvent(player, ServerEvent.SHOW_RACE_POINT, CarRaceData.TrackPositions[0], Dimension.CAR_RACE, null);
        }
    }
}