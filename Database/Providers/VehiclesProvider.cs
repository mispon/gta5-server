using System.Collections.Generic;
using System.Linq;
using gta_mp_database.Entity;
using gta_mp_database.Providers.Interfaces;
using LinqToDB;
using LinqToDB.Data;

namespace gta_mp_database.Providers {
    /// <summary>
    /// Провайдер к таблице транспортных средств игроков
    /// </summary>
    public class VehiclesProvider : IVehiclesProvider {
        public VehiclesProvider() {
            DataConnection.DefaultSettings = new AppSettings();
        }

        /// <summary>
        /// Получить информацию о транспорте по идентификатору
        /// </summary>
        public Vehicle Get(long vehicleId) {
            using (var db = new Database()) {
                return db.Vehicles.FirstOrDefault(e => e.Id == vehicleId);
            }
        }

        /// <summary>
        /// Получить транспортные средства игрока
        /// </summary>
        public IEnumerable<Vehicle> GetVehicles(long accountId) {
            using (var db = new Database()) {
                var playerVehicles = db.Vehicles.Where(e => e.OwnerId == accountId).ToList();
                foreach (var vehicle in playerVehicles) {
                    vehicle.Tuning = db.VehiclesTuning.FirstOrDefault(e => e.VehicleId == vehicle.Id);
                }
                return playerVehicles;
            }
        }

        /// <summary>
        /// Добавить новый транспорт
        /// </summary>
        public void Add(Vehicle vehicle) {
            using (var db = new Database()) {
                var id = db.InsertWithInt64Identity(vehicle);
                vehicle.Id = id;
                vehicle.Tuning.VehicleId = id;
                db.Insert(vehicle.Tuning);
            }
        }

        /// <summary>
        /// Обновить данные транспортных средств
        /// </summary>
        public void Update(IEnumerable<Vehicle> vehicles) {
            foreach (var vehicle in vehicles) {
                using (var db = new Database()) {
                    db.Update(vehicle);
                    db.Update(vehicle.Tuning);
                }
            }
        }

        /// <summary>
        /// Обновить состояние спавна
        /// </summary>
        public void SetSpawn(long vehicleId, bool isSpawned) {
            using (var db = new Database()) {
                db.Vehicles
                    .Where(e => e.Id == vehicleId)
                    .Set(e => e.IsSpawned, isSpawned)
                    .Update();
            }
        }

        /// <summary>
        /// Удалить транспорт
        /// </summary>
        public void Remove(long vehicleId) {
            using (var db = new Database()) {
                db.Vehicles.Where(e => e.Id == vehicleId).Delete();
            }
        }

        /// <summary>
        /// Припарковать весь вызванный транспорт
        /// </summary>
        public void ParkAll() {
            using (var db = new Database()) {
                db.Vehicles
                    .Set(e => e.IsSpawned, false)
                    .Update();
            }
        }
    }
}