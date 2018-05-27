using gta_mp_server.Constant;
using gta_mp_server.Helpers;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Races.TrackHandlers.Interfaces {
    internal abstract class TrackHandler : Script {
        protected int LastPoint = 8;

        /// <summary>
        /// Создать трассу
        /// </summary>
        public abstract void CreateTrack();

        /// <summary>
        /// Возвращает поворот последней точки
        /// </summary>
        protected abstract Vector3 GetFinishPointRotation();

        /// <summary>
        /// Возвращает позицию следующей точки трассы
        /// </summary>
        protected abstract Vector3 GetNextPosition(int index);

        /// <summary>
        /// Добавляет победителя
        /// </summary>
        protected abstract void AddWinner(Client player);

        /// <summary>
        /// Обработчик точки маршрута
        /// </summary>
        protected void PlayerEnterPoint(NetHandle entity, int pointNumber, int dimension) {
            PlayerHelper.ProcessAction(entity, player => {
                if (!player.hasData(PlayerData.ON_RACE) || !player.hasData(GetPointKey(pointNumber))) {
                    return;
                }
                player.resetData(GetPointKey(pointNumber));
                if (pointNumber != LastPoint) {
                    ShowNextPoint(player, ++pointNumber, dimension);
                }
                else {
                    AddWinner(player);
                    API.triggerClientEvent(player, ServerEvent.HIDE_RACE_POINT);
                }
            }, true);
        }

        /// <summary>
        /// Показать следующую точку трассы
        /// </summary>
        internal void ShowNextPoint(Client player, int nextPointNumber, int dimension) {
            player.setData(GetPointKey(nextPointNumber), true);
            var position = GetNextPosition(nextPointNumber);
            var finishRotation = nextPointNumber == LastPoint ? GetFinishPointRotation() : null;
            API.triggerClientEvent(player, ServerEvent.SHOW_RACE_POINT, position, dimension, finishRotation);
        }

        /// <summary>
        /// Возвращает ключ точки трассы
        /// </summary>
        protected static string GetPointKey(int number) {
            return $"point_{number}";
        }
    }
}