using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_data.Entity;
using gta_mp_data.Enums;
using gta_mp_database.Converters;
using gta_mp_database.Providers.Interfaces;
using LinqToDB;
using LinqToDB.Data;
using PlayerInfo = gta_mp_database.Models.Player.PlayerInfo;

namespace gta_mp_database.Providers {
    /// <summary>
    /// Провайдер к таблице данных игроков
    /// </summary>
    public class PlayersProvider : IPlayersProvider {
        public PlayersProvider() {
            DataConnection.DefaultSettings = new AppSettings();
        }

        /// <summary>
        /// Добавляет данные нового игрока
        /// </summary>
        public void Add(Account account) {
            var playerInfo = new gta_mp_data.Entity.PlayerInfo {AccountId = account.Id, Health = 100, Satiety = 100};
            var additionalInfo = new PlayerAdditionalInfo {AccountId = account.Id};
            var driverInfo = new DriverInfo {AccountId = account.Id};
            var wantedInfo = new Wanted {AccountId = account.Id};
            var settings = new Settings {AccountId = account.Id};
            var inventory = new InventoryItem {OwnerId = account.Id, Name = "Деньги", Type = InventoryType.Money, Count = 100, CountInHouse = 0};
            using (var db = new Database()) {
                db.Insert(playerInfo);
                db.Insert(additionalInfo);
                db.Insert(driverInfo);
                db.Insert(wantedInfo);
                db.Insert(settings);
                db.Insert(inventory);
            }
        }

        /// <summary>
        /// Возвращает данные игрока
        /// </summary>
        public PlayerInfo GetInfo(long accountId) {
            using (var db = new Database()) {
                var playerInfo = db.PlayersInfo.First(e => e.AccountId == accountId);
                var additionalInfo  = db.PlayersAdditionalInfo.First(e => e.AccountId == accountId);
                var driverInfo = db.DriversInfo.First(e => e.AccountId == accountId);
                var worksInfo = db.PlayerWorks.Where(e => e.AccountId == accountId).ToList();
                var wantedInfo = db.Jail.First(e => e.AccountId == accountId);
                var settings = db.Settings.First(e => e.AccountId == accountId);
                var clothes = db.PlayerClothes.Where(e => e.AccountId == accountId).ToList(); 
                var result = PlayerInfoConverter.ConvertToModel(playerInfo, additionalInfo, driverInfo, worksInfo, wantedInfo, settings, clothes);
                result.Inventory = db.Inventory.Where(e => e.OwnerId == accountId).ToList();
                result.Clan = db.PlayerClanInfos.FirstOrDefault(e => e.AccountId == accountId);
                result.PhoneContacts = db.PhoneContacts.Where(e => e.AccountId == accountId).ToList();
                result.PremiumEnd = db.Accounts.First(e => e.Id == accountId).PremiumEnd;
                return result;
            }
        }

        /// <summary>
        /// Записать данные игрока
        /// todo: попробовать заюзать async
        /// </summary>
        public void SetInfo(PlayerInfo playerInfo) {
            var playerEntity = PlayerInfoConverter.ConvertToEntity(playerInfo);
            var additionalEntity = PlayerInfoConverter.ConvertToAdditionalEntity(playerInfo);
            var driverInfo = DriverInfoConverter.ConvertToEntity(playerInfo.AccountId, playerInfo.Driver);
            using (var db = new Database()) {
                db.Update(playerEntity);
                db.Update(additionalEntity);
                db.Update(driverInfo);
                db.Update(playerInfo.Wanted);
                db.Update(playerInfo.Settings);
                UpdateClanInfo(db, playerInfo.Clan);
                UpdateInventory(db, playerInfo);
                UpdateWorks(db, playerInfo);
                UpdateClothes(db, playerInfo);
                UpdatePhoneContacts(db, playerInfo);
            }
        }

        /// <summary>
        /// Обновляет данные игроков
        /// </summary>
        public void UpdatePlayersInfos(List<PlayerInfo> playersInfos) {
            foreach (var playerInfo in playersInfos) {
                SetInfo(playerInfo);
            }
        }

        /// <summary>
        /// Записывает имя игрока
        /// </summary>
        public bool SetName(long accountId, string name) {
            using (var db = new Database()) {
                var info = db.PlayersInfo.FirstOrDefault(e => e.AccountId == accountId && e.Name == name);
                if (info != null) {
                    return false;
                }
                db.PlayersInfo
                    .Where(e => e.AccountId == accountId)
                    .Set(e => e.Name, name)
                    .Update();
                return true;
            }
        }

        /// <summary>
        /// Установить измерение игрока
        /// </summary>
        public void SetDimension(long accountId, int dimension) {
            using (var db = new Database()) {
                db.PlayersInfo.Where(e => e.AccountId == accountId).Set(e => e.Dimension, dimension).Update();
            }
        }

        /// <summary>
        /// Зачисляет награду за реферала
        /// </summary>
        public void SetReferalReward(string name, int value) {
            using (var db = new Database()) {
                var playerInfo = db.PlayersInfo.FirstOrDefault(e => e.Name == name);
                if (playerInfo == null) {
                    return;
                }
                db.Inventory
                    .Where(e => e.OwnerId == playerInfo.AccountId && e.Type == InventoryType.Money)
                    .Set(e => e.Count, e => e.Count + value)
                    .Update();
            }
        }

        /// <summary>
        /// Возвращает следующий номер телефона
        /// </summary>
        public int GetPhoneNumber() {
            const int minNumber = 800000;
            using (var db = new Database()) {
                var currentMaxNumber = db.PlayersAdditionalInfo.OrderByDescending(e => e.PhoneNumber).First().PhoneNumber;
                return currentMaxNumber != 0 ? currentMaxNumber + 1 : minNumber;
            }
        }

        /// <summary>
        /// Обновляет информацию о клане игрока
        /// </summary>
        private static void UpdateClanInfo(Database db, PlayerClanInfo clanInfo) {
            if (clanInfo == null) {
                return;
            }
            var exist = db.PlayerClanInfos.Any(e => e.AccountId == clanInfo.AccountId);
            if (exist) {
                db.Update(clanInfo);
            }
            else {
                db.Insert(clanInfo);
            }
        }

        /// <summary>
        /// Обновляет данные инвентаря
        /// </summary>
        private static void UpdateInventory(Database db, PlayerInfo playerInfo) {
            foreach (var entity in playerInfo.Inventory) {
                var exist = db.Inventory.Any(e => e.Id == entity.Id);
                if (exist) {
                    db.Update(entity);
                }
                else {
                    entity.Id = Convert.ToInt32(db.InsertWithIdentity(entity));
                }
            }
        }

        /// <summary>
        /// Обновляет данные о работах игрока
        /// </summary>
        private static void UpdateWorks(Database db, PlayerInfo playerInfo) {
            var worksEntities = WorksInfoConverter.ConvertToEntities(playerInfo.AccountId, playerInfo.Works.Values);
            foreach (var entity in worksEntities) {
                var exist = db.PlayerWorks.Any(e => e.AccountId == entity.AccountId && e.Type == entity.Type);
                if (exist) {
                    db.Update(entity);
                }
                else {
                    db.Insert(entity);
                }
            }
        }

        /// <summary>
        /// Обновление информации об одеждах игрока
        /// </summary>
        private static void UpdateClothes(Database db, PlayerInfo playerInfo) {
            var clothesEntities = PlayerClothesConverter.ConvertToEntities(playerInfo.AccountId, playerInfo.Clothes);
            foreach (var entity in clothesEntities) {
                var exist = db.PlayerClothes.Any(e => e.AccountId == entity.AccountId && e.Slot == entity.Slot && e.Variation == entity.Variation);
                if (exist) {
                    db.Update(entity);
                }
                else {
                    db.Insert(entity);
                }
            }
        }

        /// <summary>
        /// Обновление информации об одеждах игрока
        /// </summary>
        private static void UpdatePhoneContacts(Database db, PlayerInfo playerInfo) {
            foreach (var entity in playerInfo.PhoneContacts) {
                var exist = db.PhoneContacts.Any(e => e.AccountId == entity.AccountId && e.Number == entity.Number);
                if (exist) {
                    db.Update(entity);
                }
                else {
                    db.Insert(entity);
                }
            }
        }
    }
}