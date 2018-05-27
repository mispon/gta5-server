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
    /// Обработчик трассы мотогонок
    /// </summary>
    internal class MotoTrackHandler : TrackHandler {
        private readonly Race _motoRace;

        public MotoTrackHandler() {}
        public MotoTrackHandler([Named("MotoRace")] Race motoRace) {
            _motoRace = motoRace;
            LastPoint = 21;
        }

        /// <summary>
        /// Создать трассу
        /// </summary>
        public override void CreateTrack() {
            for (var i = 0; i < MotoRaceData.TrackPositions.Count; i++) {
                var position = MotoRaceData.TrackPositions[i];
                var point = API.createSphereColShape(position, 8f);
                point.dimension = Dimension.MOTO_RACE;
                var pointNumber = i;
                point.onEntityEnterColShape += (shape, entity) => PlayerEnterPoint(entity, pointNumber, Dimension.MOTO_RACE);
            }
        }

        /// <summary>
        /// Возвращает поворот последней точки
        /// </summary>
        protected override Vector3 GetFinishPointRotation() {
            return MotoRaceData.FinishPointRotation;
        }

        /// <summary>
        /// Возвращает позицию следующей точки трассы
        /// </summary>
        protected override Vector3 GetNextPosition(int index) {
            return MotoRaceData.TrackPositions[index];
        }

        /// <summary>
        /// Добавляет победителя
        /// </summary>
        protected override void AddWinner(Client player) {
            _motoRace.AddWinner(player);
        }

        /// <summary>
        /// Отображает первую точку трассы
        /// </summary>
        internal static void ShowFirstPoint(Client player) {
            player.setData(GetPointKey(0), true);
            API.shared.triggerClientEvent(player, ServerEvent.SHOW_RACE_POINT, MotoRaceData.TrackPositions[0], Dimension.MOTO_RACE, null);
        }
    }
}