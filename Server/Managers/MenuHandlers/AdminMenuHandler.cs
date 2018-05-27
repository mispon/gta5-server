using System;
using System.IO;
using gta_mp_server.Constant;
using gta_mp_server.Enums.Vehicles;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Vehicles;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.MenuHandlers {
    internal class AdminMenuHandler : Script, IMenu {
        /// <summary>
        /// Инициализировать админское меню
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.GET_POSITION, GetCurrentPosition);
            ClientEventHandler.Add(ClientEvent.GET_ROTATION, GetCurrentRotation);
            ClientEventHandler.Add(ClientEvent.GET_CAR, GetCar);
            ClientEventHandler.Add(ClientEvent.GET_WEAPON, GetWeapon);
        }

        /// <summary>
        /// Вывести текущую позицию
        /// </summary>
        private void GetCurrentPosition(Client player, object[] args) {
            var position = player.position;
            API.sendChatMessageToPlayer(player, $"~b~X:{position.X:F}, Y:{position.Y:F}, Z:{position.Z - 1:F}");
            File.AppendAllText("positions.txt", $"{position.X:F};{position.Y:F};{position.Z - 1:F}\r\n");
        }

        /// <summary>
        /// Вывести текущий поворот
        /// </summary>
        private void GetCurrentRotation(Client player, object[] args) {
            var rotation = player.rotation;
            API.sendChatMessageToPlayer(player, $"~b~X:{rotation.X:F}, Y:{rotation.Y:F}, Z:{rotation.Z:F}");
            File.AppendAllText("rotation.txt", $"{rotation.X:F};{rotation.Y:F};{rotation.Z:F}\r\n");
        }

        /// <summary>
        /// Присуммонить кашерную машину
        /// </summary>
        private void GetCar(Client player, object[] args) {
            var values = Enum.GetValues(typeof(SuperCar));
            var model = (VehicleHash) values.GetValue(ActionHelper.Random.Next(values.Length));
            var position = player.position.Add(new Vector3(-2, 0, 0));
            var vehicle = API.createVehicle(model, position, new Vector3(), ActionHelper.Random.Next(159), ActionHelper.Random.Next(159));
            vehicle.setData(VehicleManager.MAX_FUEL, 100);
            API.setPlayerIntoVehicle(player, vehicle, -1);
            API.setVehicleFuelLevel(vehicle, 100);
        }

        /// <summary>
        /// Взять случайное оружие
        /// </summary>
        private void GetWeapon(Client player, object[] objects) {
            API.givePlayerWeapon(player, WeaponHash.RPG, int.MaxValue, true);
        }
    }
}