using System.Collections.Generic;
using System.Linq;
using gta_mp_data.Enums;
using gta_mp_server.Clan.Data;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Clan.Mission;
using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Places.VehicleShowroom;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Models.Clan;
using gta_mp_server.Models.Shops;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using Marker = gta_mp_server.Enums.Marker;

namespace gta_mp_server.Clan {
    /// <summary>
    /// Менеджер клановых помещений
    /// </summary>
    internal class ClanHall : Place {
        private readonly IPointCreator _pointCreator;
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IClanCourtyard _clanCourtyard;

        public ClanHall() {}
        public ClanHall(IPointCreator pointCreator, IPlayerInfoManager playerInfoManager, IClanCourtyard clanCourtyard) {
            _pointCreator = pointCreator;
            _playerInfoManager = playerInfoManager;
            _clanCourtyard = clanCourtyard;
        }

        /// <summary>
        /// Инизиализировать место
        /// </summary>
        public override void Initialize() {
            LoadIteriors();
            foreach (var clanInfo in ClanDataGetter.ClansInfo) {
                _pointCreator.CreateBlip(clanInfo.Enter, 181, clanInfo.BlipColor, name: $"{clanInfo.ClanName} ({clanInfo.LeaderName})");
                var enter = _pointCreator.CreateMarker(Marker.UpsideDownCone, clanInfo.Enter, Colors.Yellow, 1f);
                enter.ColShape.onEntityEnterColShape += (shape, entity) => PlayerEnterClanHall(entity, clanInfo.AfterEnter);
                CreateLeader(clanInfo);
                CreateAdmin(clanInfo);
                CreateGunsmith(clanInfo);
                CreateMechanic(clanInfo);
                CreateDressingRoom(clanInfo);
                var exit = _pointCreator.CreateMarker(Marker.UpsideDownCone, clanInfo.Exit, Colors.Yellow, 1f);
                exit.ColShape.onEntityEnterColShape += (shape, entity) => PlayerExitClanHall(entity, clanInfo.AfterExit);
                _clanCourtyard.Initialize(clanInfo.ClanId, clanInfo.Courtyard);
            }
        }

        /// <summary>
        /// Загружает интерьеры кланхолов
        /// </summary>
        private void LoadIteriors() {
            // клан Майкла
            API.requestIpl("ex_sm_13_office_02b");
            API.requestIpl("imp_sm_13_cargarage_b");
            // Клан Тревора
            API.requestIpl("ex_dt1_11_office_02b");
            API.requestIpl("imp_dt1_11_cargarage_b");
            // Клан Франклина
            API.requestIpl("ex_dt1_02_office_02b");
            API.requestIpl("imp_dt1_02_cargarage_b");
        }

        /// <summary>
        /// Игрок входит в кланхол
        /// </summary>
        private void PlayerEnterClanHall(NetHandle entity, Vector3 positionAfter) {
            PlayerHelper.ProcessAction(entity, player => {
                API.setEntityPosition(player, positionAfter);
                API.sendNativeToPlayer(player, Hash.DISPLAY_RADAR, false);
            });
        }

        /// <summary>
        /// Игрок выходит из кланхола
        /// </summary>
        private void PlayerExitClanHall(NetHandle entity, Vector3 positionAfter) {
            PlayerHelper.ProcessAction(entity, player => {
                API.setEntityPosition(player, positionAfter);
                API.sendNativeToPlayer(player, Hash.DISPLAY_RADAR, true);
            });
        }

        /// <summary>
        /// Создает лидера клана
        /// </summary>
        private void CreateLeader(ClanInfo clanInfo) {
            var ped = _pointCreator.CreatePed(
                clanInfo.LeaderHash, clanInfo.LeaderName, clanInfo.LeaderPosition,
                clanInfo.LeaderRotation, clanInfo.LeaderMarker, Colors.VividCyan
            );
            if (clanInfo.ClanId == 3) {
                var bitches = API.createPed(PedHash.Tanisha, new Vector3(-124.01, -642.23, 168.82), 0);
                API.setEntityRotation(bitches, new Vector3(0.00, 0.00, 41.92));
            }
            ped.ColShape.onEntityEnterColShape += (shape, entity) => {
                PlayerHelper.ProcessAction(entity, player => {
                    if (!HasRight(player, clanInfo.ClanId, ClanRank.Lowest)) {
                        return;
                    }
                    API.triggerClientEvent(
                        player, ServerEvent.SHOW_CLAN_LEADER_MENU, (int) clanInfo.ClanId,
                        ClanMissionManager.GetMissionVotes(clanInfo.ClanId),
                        ClanManager.GetAuthority(clanInfo.ClanId)
                    );
                });
            };
            ped.ColShape.onEntityExitColShape += (shape, entity) => {
                var player = API.getPlayerFromHandle(entity);
                API.triggerClientEvent(player, ServerEvent.HIDE_CLAN_LEADER_MENU);
            };
        }

        /// <summary>
        /// Создает админа на ресепшене
        /// </summary>
        private void CreateAdmin(ClanInfo clanInfo) {
            var ped = _pointCreator.CreatePed(
                clanInfo.AdminHash, "Администратор", clanInfo.AdminPosition,
                clanInfo.AdminRotation, clanInfo.AdminMarker, Colors.VividCyan
            );
            ped.ColShape.onEntityEnterColShape += (shape, entity) =>
                PlayerHelper.ProcessAction(entity, player => {
                    var playerInfo = _playerInfoManager.GetInfo(player);
                    if (playerInfo.Clan != null && playerInfo.Clan.ClanId != clanInfo.ClanId) {
                        API.sendColoredNotificationToPlayer(player, "Вы состоите в другой банде", 0, 6, true);
                        return;
                    }
                    API.triggerClientEvent(player, ServerEvent.SHOW_CLAN_MENU, (int) clanInfo.ClanId);
                });
            ped.ColShape.onEntityExitColShape += (shape, entity) => 
                PlayerHelper.ProcessAction(entity, player => API.triggerClientEvent(player, ServerEvent.HIDE_CLAN_MENU));
        }

        /// <summary>
        /// Создает оружейника
        /// </summary>
        private void CreateGunsmith(ClanInfo clanInfo) {
            var ped = _pointCreator.CreatePed(
                clanInfo.GunsmithHash, "Оружейник", clanInfo.GunsmithPosition,
                clanInfo.GunsmithRotation, clanInfo.GunsmithMarker, Colors.VividCyan
            );
            ped.ColShape.onEntityEnterColShape += (shape, entity) => PlayerHelper.ProcessAction(entity, player => {
                if (!HasRight(player, clanInfo.ClanId, ClanRank.Middle)) {
                    return;
                }
                API.triggerClientEvent(player, ServerEvent.SHOW_AMMU_NATION_MENU,
                    JsonConvert.SerializeObject(ClanDataGetter.Weapons),
                    JsonConvert.SerializeObject(ClanDataGetter.Ammo),
                    (int) Validator.INVALID_ID
                );
            });
            ped.ColShape.onEntityExitColShape += (shape, entity) =>
                PlayerHelper.ProcessAction(entity, player => API.triggerClientEvent(player, ServerEvent.HIDE_AMMU_NATION_MENU));
        }

        /// <summary>
        /// Создает автомеханика
        /// </summary>
        private void CreateMechanic(ClanInfo clanInfo) {
            var ped = _pointCreator.CreatePed(
                clanInfo.MechHash, "Механик", clanInfo.MechPosition,
                clanInfo.MechRotation, clanInfo.MechMarker, Colors.VividCyan
            );
            ped.ColShape.onEntityEnterColShape += (shape, entity) => PlayerHelper.ProcessAction(entity, player => {
                if (!HasRight(player, clanInfo.ClanId, ClanRank.Middle)) {
                    return;
                }
                API.triggerClientEvent(player, ServerEvent.SHOW_SHOWROOM_MENU,
                    JsonConvert.SerializeObject(ClanDataGetter.ClanVehicles[clanInfo.ClanId]),
                    GetPlayerVehiclesData(player),
                    JsonConvert.SerializeObject(ClanDataGetter.ClanVehicleShowroom[clanInfo.ClanId]),
                    (int) ShowroomType.Clan, (int) Validator.INVALID_ID
                );
            });
            ped.ColShape.onEntityExitColShape += (shape, entity) => PlayerHelper.ProcessAction(entity, player => {
                API.triggerClientEvent(player, ServerEvent.HIDE_SHOWROOM_MENU);
            });
        }

        /// <summary>
        /// Возвращает сериализованные данные транспорта
        /// </summary>
        private string GetPlayerVehiclesData(Client player) {
            var vehicles = new List<ShowroomVehicle>();
            foreach (var vehicle in _playerInfoManager.GetInfo(player).Vehicles.Values) {
                var price = ShowroomsGetter.GetSellPrice(vehicle.Hash);
                if (Validator.IsValid(price)) {
                    vehicles.Add(new ShowroomVehicle {Id = (int) vehicle.Id, Hash = vehicle.Hash, Price = price});
                }
            }
            return JsonConvert.SerializeObject(vehicles);
        }

        /// <summary>
        /// Создать гардероб
        /// </summary>
        private void CreateDressingRoom(ClanInfo clanInfo) {
            var dressing = _pointCreator.CreateMarker(Marker.VerticalCylinder, clanInfo.DressingMarker, Colors.VividCyan, 1.3f, "Гардероб");
            dressing.ColShape.onEntityEnterColShape += (shape, entity) => PlayerHelper.ProcessAction(entity, player => {
                if (!HasRight(player, clanInfo.ClanId, ClanRank.Low)) {
                    return;
                }
                var playerInfo = _playerInfoManager.GetInfo(player);
                var clothes = CopyHelper.DeepCopy(playerInfo.Clothes);
                var isMale = playerInfo.Skin == Skin.Male;
                foreach (var clanClothes in ClanDataGetter.GetClanClothes(clanInfo.ClanId, isMale)) {
                    if (clothes.Any(e => e.Variation == clanClothes.Variation && e.Slot == clanClothes.Slot)) {
                        continue;
                    }
                    clothes.Add(clanClothes);
                }
                API.triggerClientEvent(
                    player, ServerEvent.SHOW_CLOTHES_MENU, 0,
                    JsonConvert.SerializeObject(clanInfo.DressingRoomPositions),
                    JsonConvert.SerializeObject(clothes), (int) Validator.INVALID_ID
                );
            });
            dressing.ColShape.onEntityExitColShape += (shape, entity) =>
                PlayerHelper.ProcessAction(entity, player => {
                    if (HasRight(player, clanInfo.ClanId, ClanRank.Low)) {
                        API.triggerClientEvent(player, ServerEvent.HIDE_CLOTHES_MENU);
                    }
                }
            );
        }

        /// <summary>
        /// Проверяет, что игроку разрешено взаимодействовать с нпс
        /// </summary>
        private bool HasRight(Client player, long clanId, ClanRank rank) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (playerInfo.Clan == null) {
                API.sendColoredNotificationToPlayer(player, "Вы не состоите в банде", 0, 6, true);
                return false;
            }
            if (playerInfo.Clan.ClanId != clanId) {
                API.sendColoredNotificationToPlayer(player, "Вы состоите в другой банде", 0, 6, true);
                return false;
            }
            if (playerInfo.Clan.Rank < rank) {
                API.sendNotificationToPlayer(player, $"~r~Необходим ранг \"{rank.GetDescription()}\" и выше", true);
                return false;
            }
            return true;
        }
    }
}