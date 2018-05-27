using System.Linq;
using gta_mp_server.Constant;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.NPC.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Races;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;

namespace gta_mp_server.Managers.NPC {
    /// <summary>
    /// Нпс гонок
    /// </summary>
    internal class RaceNpc : Script, INpc {
        private readonly IPointCreator _pointCreator;
        private readonly IPlayerInfoManager _playerInfoManager;

        public RaceNpc() {}
        public RaceNpc(IPointCreator pointCreator, IPlayerInfoManager playerInfoManager) {
            _pointCreator = pointCreator;
            _playerInfoManager = playerInfoManager;
        }

        /// <summary>
        /// Создать нпс
        /// </summary>
        public void Initialize() {
            _pointCreator.CreateBlip(MainPosition.Race, 315, 69, name: "Гонки");
            var ped = _pointCreator.CreatePed(
                PedHash.Bevhills02AMM, "Пол", MainPosition.Race, new Vector3(0.00, 0.00, -33.72),
                new Vector3(-1650.48, -957.17, 6.79), Colors.VividCyan
            );
            ped.ColShape.onEntityEnterColShape += (shape, entity) => PlayerComeToNpc(entity);
            ped.ColShape.onEntityExitColShape += (shape, entity) =>
                PlayerHelper.ProcessAction(entity, player => API.triggerClientEvent(player, ServerEvent.HIDE_RACE_MENU));
        }

        /// <summary>
        /// Обработчик вызова меню нпс
        /// </summary>
        private void PlayerComeToNpc(NetHandle entity) {
            PlayerHelper.ProcessAction(entity, player => {
                var vehicles = _playerInfoManager.GetInfo(player).Vehicles.Values;
                var cars = vehicles.Where(e => API.isVehicleACar((VehicleHash) e.Hash));
                var motos = vehicles.Where(e => IsMotocicle((VehicleHash) e.Hash));
                var membersInfo = GetMembersInfo();
                API.triggerClientEvent(
                    player, ServerEvent.SHOW_RACE_MENU, JsonConvert.SerializeObject(cars),
                    JsonConvert.SerializeObject(motos), JsonConvert.SerializeObject(membersInfo)
                );
            });
        }

        /// <summary>
        /// Информация об зарегистрированных
        /// </summary>
        private static object GetMembersInfo() {
            return new {
                cars = CarRace.Members.Count,
                moto = MotoRace.Members.Count,
                rally = Rally.Members.Count,
                mountain = MountainRace.Members.Count
            };
        }

        /// <summary>
        /// Отсеивает
        /// </summary>
        private bool IsMotocicle(VehicleHash model) {
            const int motoClass = 8;
            return API.getVehicleClass(model) == motoClass;
        }
    }
}