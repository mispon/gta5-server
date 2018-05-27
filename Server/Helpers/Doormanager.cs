using System.Collections.Generic;
using gta_mp_server.Helpers.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Helpers {
    public class Doormanager : Script, IDoormanager {
        public const ulong SET_STATE_OF_CLOSEST_DOOR_OF_TYPE = 0xF82D8F1926A02C3D;

        private readonly Dictionary<int, ColShape> _doorColShapes = new Dictionary<int, ColShape>();
        private int _doorCounter;

        public Doormanager() {
            API.onEntityEnterColShape += ColShapeTrigger;
        }

        /// <summary>
        /// Зарегистрировать дверь
        /// </summary>
        public int Register(int modelHash, Vector3 position) {
            var colShapeId = ++ _doorCounter;
            var info = new DoorInfo {
                Hash = modelHash,
                Position = position,
                Locked = false,
                Id = colShapeId,
                State = 0
            };

            var colShape = API.createSphereColShape(position, 3f);
            colShape.setData("DOOR_INFO", info);
            colShape.setData("DOOR_ID", colShapeId);
            colShape.setData("IS_DOOR_TRIGGER", true);
            _doorColShapes.Add(colShapeId, colShape);
            return colShapeId;
        }

        /// <summary>
        /// Установить состояние двери
        /// </summary>
        public void SetDoorState(int doorId, bool locked, float heading) {
            if (!_doorColShapes.ContainsKey(doorId)) {
                return;
            }
            var door = _doorColShapes[doorId];
            var data = door.getData("DOOR_INFO");
            data.Locked = locked;
            data.State = heading;
            door.setData("DOOR_INFO", data);
            foreach (var entity in door.getAllEntities()) {
                var player = API.getPlayerFromHandle(entity);
                if (player == null) {
                    continue;
                }
                float cH = data.State;
                API.sendNativeToPlayer(player, SET_STATE_OF_CLOSEST_DOOR_OF_TYPE,
                    data.Hash, data.Position.X, data.Position.Y, data.Position.Z,
                    data.Locked, cH, false);
            }
        }

        /// <summary>
        /// Обработчик открытия двери
        /// </summary>
        private void ColShapeTrigger(ColShape colshape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (player == null || colshape == null) {
                return;
            }
            if (colshape.getData("IS_DOOR_TRIGGER") != true) {
                return;
            }
            colshape.getData("DOOR_ID");
            var info = colshape.getData("DOOR_INFO");
            var heading = 0f;
            if (info.State != null) heading = info.State;
            API.sendNativeToPlayer(player, SET_STATE_OF_CLOSEST_DOOR_OF_TYPE,
                info.Hash, info.Position.X, info.Position.Y, info.Position.Z,
                info.Locked, heading, false);
        }
    }

    public struct DoorInfo {
        public int Hash;
        public Vector3 Position;
        public int Id;
        public bool Locked;
        public float State;
    }
}