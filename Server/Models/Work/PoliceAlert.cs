using System;
using gta_mp_server.Constant;
using gta_mp_server.Helpers;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;

namespace gta_mp_server.Models.Work {
    /// <summary>
    /// Модель вызова полиции
    /// </summary>
    internal class PoliceAlert {
        public PoliceAlert(int id, Vector3 position, string name) {
            Id = id;
            Position = position;
            Name = name;
            Date = DateTime.Now;
        }

        /// <summary>
        /// Уникальный идентификатор вызова
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Позиция вызова
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Тип вызова
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Время создания вызова
        /// </summary>
        [JsonIgnore]
        public DateTime Date { get; set; }

        /// <summary>
        /// Обработчик зоны вызова
        /// </summary>
        [JsonIgnore]
        public ColShape Zone { get; set; }

        /// <summary>
        /// Создает зону вызова
        /// </summary>
        public void CreateZone() {
            Zone = API.shared.createSphereColShape(Position, 25f);
            Zone.onEntityEnterColShape += (shape, entity) => PlayerHelper.ProcessAction(entity, player => {
                if (player.hasData(WorkData.IS_POLICEMAN)) {
                    player.setData(WorkData.ALERT_ZONE_KEY, Id);
                }
            }, true);
            Zone.onEntityExitColShape += (shape, entity) => PlayerHelper.ProcessAction(entity, player => player.resetData(WorkData.ALERT_ZONE_KEY));
        }
    }
}