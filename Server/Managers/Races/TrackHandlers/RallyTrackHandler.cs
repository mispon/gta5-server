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
    /// Обработчик трассы ралли
    /// </summary>
    internal class RallyTrackHandler : TrackHandler {
        private readonly Race _rallyRace;

        public RallyTrackHandler() {}
        public RallyTrackHandler([Named("Rally")] Race rallyRace) {
            LastPoint = 27;
            _rallyRace = rallyRace;
        }

        /// <summary>
        /// Создать трассу
        /// </summary>
        public override void CreateTrack() {
            for (var i = 0; i < RallyRaceData.TrackPositions.Count; i++) {
                var position = RallyRaceData.TrackPositions[i];
                var point = API.createSphereColShape(position, 8f);
                point.dimension = Dimension.RALLY;
                var pointNumber = i;
                point.onEntityEnterColShape += (shape, entity) => PlayerEnterPoint(entity, pointNumber, Dimension.RALLY);
            }
        }

        /// <summary>
        /// Возвращает поворот последней точки
        /// </summary>
        protected override Vector3 GetFinishPointRotation() {
            return RallyRaceData.FinishPointRotation;
        }

        /// <summary>
        /// Возвращает позицию следующей точки трассы
        /// </summary>
        protected override Vector3 GetNextPosition(int index) {
            return RallyRaceData.TrackPositions[index];
        }

        /// <summary>
        /// Добавляет победителя
        /// </summary>
        protected override void AddWinner(Client player) {
            _rallyRace.AddWinner(player);
        }

        /// <summary>
        /// Отображает первую точку трассы
        /// </summary>
        internal static void ShowFirstPoint(Client player) {
            player.setData(GetPointKey(0), true);
            API.shared.triggerClientEvent(player, ServerEvent.SHOW_RACE_POINT, RallyRaceData.TrackPositions[0], Dimension.RALLY, null);
        }
    }
}