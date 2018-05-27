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
    /// Обработчик горной трассы
    /// </summary>
    internal class MountainTrackHandler : TrackHandler {
        private readonly Race _mountainRace;

        public MountainTrackHandler() { }
        public MountainTrackHandler([Named("MountainRace")] Race mountainRace) {
            _mountainRace = mountainRace;
        }

        /// <summary>
        /// Создать трассу
        /// </summary>
        public override void CreateTrack() {
            for (var i = 0; i < MountainRaceData.TrackPositions.Count; i++) {
                var position = MountainRaceData.TrackPositions[i];
                var point = API.createSphereColShape(position, 6f);
                point.dimension = Dimension.MOUNT_RACE;
                var pointNumber = i;
                point.onEntityEnterColShape += (shape, entity) => PlayerEnterPoint(entity, pointNumber, Dimension.MOUNT_RACE);
            }
        }

        /// <summary>
        /// Возвращает поворот последней точки
        /// </summary>
        protected override Vector3 GetFinishPointRotation() {
            return MountainRaceData.FinishPointRotation;
        }

        /// <summary>
        /// Возвращает позицию следующей точки трассы
        /// </summary>
        protected override Vector3 GetNextPosition(int index) {
            return MountainRaceData.TrackPositions[index];
        }

        /// <summary>
        /// Добавляет победителя
        /// </summary>
        protected override void AddWinner(Client player) {
            _mountainRace.AddWinner(player);
        }

        /// <summary>
        /// Отображает первую точку трассы
        /// </summary>
        internal static void ShowFirstPoint(Client player) {
            player.setData(GetPointKey(0), true);
            API.shared.triggerClientEvent(player, ServerEvent.SHOW_RACE_POINT, MountainRaceData.TrackPositions[0], Dimension.MOUNT_RACE, null);
        }
    }
}