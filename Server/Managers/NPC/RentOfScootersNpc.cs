using System.Collections.Generic;
using gta_mp_server.Constant;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.NPC.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.NPC {
    /// <summary>
    /// Аренда велосипедов и скутеров
    /// </summary>
    internal class RentOfScootersNpc : Script, INpc {
        private readonly IPointCreator _pointCreator;

        public RentOfScootersNpc() {}
        public RentOfScootersNpc(IPointCreator pointCreator) {
            _pointCreator = pointCreator;
        }

        /// <summary>
        /// Создать нпс
        /// </summary>
        public void Initialize() {
            var npcs = GetNpcsPositions();
            foreach (var npc in npcs) {
                _pointCreator.CreateBlip(npc.Position, 512, 9, name: "Аренда скутеров");
                var point = _pointCreator.CreatePed(
                    PedHash.ChiGoon01GMM, "Аренда скутеров", npc.Position, 
                    npc.Rotation, npc.MarkerPosition.Add(new Vector3(0, 0, 0.1f)), Colors.VividCyan
                );
                point.ColShape.onEntityEnterColShape += (shape, entity) => {
                    OnEntityEnterColShape(entity, npc.ScooterPosition, npc.ScooterRotation, npc.District);
                };
                point.ColShape.onEntityExitColShape += OnEntityExitColShape;
            }
        }

        /// <summary>
        /// Игрок подошел к нпс
        /// </summary>
        private void OnEntityEnterColShape(NetHandle entity, Vector3 scooterPos, Vector3 scooterRot, int district) {
            var player = API.getPlayerFromHandle(entity);
            if (PlayerHelper.PlayerCorrect(player)) {
                API.triggerClientEvent(player, ServerEvent.SHOW_SCOOTERS_MENU, scooterPos, scooterRot, district);
            }
        }

        /// <summary>
        /// Игрок отошел от нпс
        /// </summary>
        private void OnEntityExitColShape(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (PlayerHelper.PlayerCorrect(player)) {
                API.triggerClientEvent(player, ServerEvent.HIDE_SCOOTERS_MENU);
            }
        }

        /// <summary>
        /// Информация о позиции нпс
        /// </summary>
        private class NpcPositions {
            public Vector3 Position { get; set; }
            public Vector3 Rotation { get; set; }
            public Vector3 MarkerPosition { get; set; }
            public Vector3 ScooterPosition { get; set; }
            public Vector3 ScooterRotation { get; set; }
            public int District { get; set; }
        }

        /// <summary>
        /// Возвращает позиции нпц
        /// </summary>
        private static IEnumerable<NpcPositions> GetNpcsPositions() {
            return new List<NpcPositions> {
                new NpcPositions {
                    Position = new Vector3(-332.71, -1515.09, 27.56),
                    Rotation = new Vector3(0.00, 0.00, -170.38),
                    MarkerPosition = new Vector3(-332.64, -1515.80, 26.55),
                    ScooterPosition = new Vector3(-329.19, -1516.44, 27.54),
                    ScooterRotation = new Vector3(0.00, 0.00, 175.39),
                    District = 10
                },
                new NpcPositions {
                    Position = new Vector3(102.89, -1958.95, 20.79),
                    Rotation = new Vector3(0.00, 0.00, -7.31),
                    MarkerPosition = new Vector3(102.95, -1958.23, 19.74),
                    ScooterPosition = new Vector3(101.09, -1956.62, 20.75),
                    ScooterRotation = new Vector3(0.00, 0.00, -13.08),
                    District = 7
                },
                new NpcPositions {
                    Position = new Vector3(276.98, -1347.75, 31.94),
                    Rotation = new Vector3(0.00, 0.00, 141.03),
                    MarkerPosition = new Vector3(276.49, -1348.37, 30.94),
                    ScooterPosition = new Vector3(279.49, -1350.65, 31.94),
                    ScooterRotation = new Vector3(0.00, 0.00, 147.47),
                    District = 1
                },
                new NpcPositions {
                    Position = new Vector3(-728.81, -1051.52, 12.46),
                    Rotation = new Vector3(0.00, 0.00, 121.57),
                    MarkerPosition = new Vector3(-729.51, -1051.82, 11.45),
                    ScooterPosition = new Vector3(-728.10, -1054.27, 12.40),
                    ScooterRotation = new Vector3(0.00, 0.00, 123.48),
                    District = 2
                },
                new NpcPositions {
                    Position = new Vector3(-1339.01, -389.72, 36.69),
                    Rotation = new Vector3(0.00, 0.00, -144.12),
                    MarkerPosition = new Vector3(-1338.62, -390.41, 35.68),
                    ScooterPosition = new Vector3(-1338.40, -392.40, 36.65),
                    ScooterRotation = new Vector3(0.00, 0.00, 140.72),
                    District = 3
                },
                new NpcPositions {
                    Position = new Vector3(-419.39, 143.22, 65.12),
                    Rotation = new Vector3(0.00, 0.00, 170.35),
                    MarkerPosition = new Vector3(-419.50, 142.40, 64.09),
                    ScooterPosition = new Vector3(-421.68, 140.85, 65.04),
                    ScooterRotation = new Vector3(0.00, 0.00, -177.89),
                    District = 4
                },
                new NpcPositions {
                    Position = new Vector3(596.74, 85.52, 92.79),
                    Rotation = new Vector3(0.00, 0.00, -113.58),
                    MarkerPosition = new Vector3(597.60, 85.27, 91.76),
                    ScooterPosition = new Vector3(600.07, 86.03, 92.75),
                    ScooterRotation = new Vector3(0.00, 0.00, -114.05),
                    District = 6
                },
                new NpcPositions {
                    Position = new Vector3(1784.62, 3644.79, 34.41),
                    Rotation = new Vector3(0.00, 0.00, 26.28),
                    MarkerPosition = new Vector3(1784.10, 3645.63, 33.40),
                    ScooterPosition = new Vector3(1779.05, 3648.46, 34.47),
                    ScooterRotation = new Vector3(0.00, 0.00, -65.30),
                    District = 9
                },
                new NpcPositions {
                    Position = new Vector3(-221.63, 6352.49, 31.74),
                    Rotation = new Vector3(0.00, 0.00, -139.88),
                    MarkerPosition = new Vector3(-221.05, 6351.84, 30.75),
                    ScooterPosition = new Vector3(-216.59, 6354.54, 31.49),
                    ScooterRotation = new Vector3(0.00, 0.00, 135.35),
                    District = 9
                }
            };
        }
    }
}