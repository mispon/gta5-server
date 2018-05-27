using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_server.Constant;
using gta_mp_server.Managers.Places.FarmPlace;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models.Places.Instances {
    /// <summary>
    /// Модель данных фермы
    /// </summary>
    internal class FarmInstance : Script {
        internal const string LOADER_FARM_TRAILER = "LoadedFarmTrailer";
        private const int PACK_COUNT = 120;
        private const int MAX_TRAILERS = 2;

        /// <summary>
        /// Количество урожая, собранного фермарами
        /// </summary>
        private int HarvestCount { get; set; }

        /// <summary>
        /// Количество подготовленных прицепов
        /// </summary>
        private List<Vehicle> LoadedTrailers { get; set; } = new List<Vehicle>(MAX_TRAILERS);
        
        /// <summary>
        /// Дата окончания бафа
        /// </summary>
        private DateTime BuffEnd { get; set; }

        /// <summary>
        /// Увеличивает скорость сбора урожая
        /// </summary>
        public void BuffFarm() {
            BuffEnd = DateTime.Now.AddMinutes(5);
        }

        /// <summary>
        /// Проверяет, наложен ли баф в данный момент
        /// </summary>
        public bool HasBuff() {
            return DateTime.Now < BuffEnd;
        }

        /// <summary>
        /// Записывает количество собранного урожая
        /// </summary>
        public void AddHarvest() {
            HarvestCount += 12;
            if (HarvestCount >= PACK_COUNT && LoadedTrailers.Count < MAX_TRAILERS) {
                CreateHarvestTrailer();
                HarvestCount -= PACK_COUNT;
            }
        }

        /// <summary>
        /// Удаляет отвезенный трейлер
        /// </summary>
        public void RemoveTrailer() {
            LoadedTrailers = LoadedTrailers.Where(e => e.exists).ToList();
        }

        /// <summary>
        /// Создает загруженный урожаем трейлер
        /// </summary>
        private void CreateHarvestTrailer() {
            foreach (var trailerInfo in FarmDataGetter.LoadedTrailers) {
                if (LoadedTrailers.Any(e => Vector3.Distance(e.position, trailerInfo.Position) <= 1.7)) {
                    continue;
                }
                var trailer = API.createVehicle(trailerInfo.Hash, trailerInfo.Position, trailerInfo.Rotation, 0, 0);
                trailer.setData(LOADER_FARM_TRAILER, true);
                LoadedTrailers.Add(trailer);
                NotifyTractorDrivers(trailer.position);
                break;
            }
        }

        /// <summary>
        /// Оповещает водителей тракторов о загруженном прицепе
        /// </summary>
        private void NotifyTractorDrivers(Vector3 trailerPosition) {
            foreach (var player in API.getAllPlayers().Where(e => e != null && e.hasData(WorkData.IS_TRACTOR_DRIVER))) {
                API.sendNotificationToPlayer(player, "~p~Фермеры загрузили урожай", true);
                API.triggerClientEvent(player, ServerEvent.SHOW_LOADED_TRAILER, trailerPosition);
            }
        }
    }
}