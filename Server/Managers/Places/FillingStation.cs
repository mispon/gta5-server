using System;
using System.Collections.Generic;
using gta_mp_server.Constant;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Models.Places;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Marker = gta_mp_server.Enums.Marker;
using Vehicle = GrandTheftMultiplayer.Server.Elements.Vehicle;

namespace gta_mp_server.Managers.Places {
    /// <summary>
    /// Заправка
    /// </summary>
    internal class FillingStation : Place {
        internal const string STATION_KEY = "filling_{0}";
        private static readonly Vector3 _pointDowngrade = new Vector3(0, 0, -0.55);
        private readonly IPointCreator _pointCreator;

        public FillingStation() {}
        public FillingStation(IPointCreator pointCreator) {
            _pointCreator = pointCreator;
        }

        /// <summary>
        /// Инизиализировать заправки
        /// </summary>
        public override void Initialize() {
            foreach (var station in GetFillingPositions()) {
                var position = station.Position;
                _pointCreator.CreateBlip(position, 361, 12, name: "Заправка");
                InitializeInside(station);
                CreateFillingPoints(station);
            }
        }

        /// <summary>
        /// Инициализировать заправку внутри
        /// </summary>
        private void InitializeInside(FillingModel station) {
            var pedPositions = PositionConverter.ToListVector3(station.NpcPositions);
            var ped = _pointCreator.CreatePed(PedHash.GuadalopeCutscene, "Продавец", pedPositions[0], pedPositions[1], pedPositions[2], Colors.VividCyan);
            ped.ColShape.onEntityEnterColShape += (shape, entity) => PlayerComeToSeller(entity, station.Id, station.District);
            ped.ColShape.onEntityExitColShape += PlayerComeAwayFromSeller;
        }

        /// <summary>
        /// Инициализировать бензоколонки на заправке
        /// </summary>
        private void CreateFillingPoints(FillingModel station) {
            var positions = PositionConverter.ToListVector3(station.FillingPoints);
            var stationKey = string.Format(STATION_KEY, station.Id);
            foreach (var position in positions) {
                _pointCreator.CreateMarker(Marker.HorizontalCircleFlat, position.Add(_pointDowngrade), Colors.VividCyan, 1f);
                var point = API.createSphereColShape(position, 2f);
                point.onEntityEnterColShape += (shape, entity) => PlayerEnterFillingPoint(entity, stationKey);
                point.onEntityExitColShape += (shape, entity) => PlayerExitFillingPoint(entity, stationKey);
            }
        }

        /// <summary>
        /// Игрок подошел к продавцу
        /// </summary>
        private void PlayerComeToSeller(NetHandle entity, long stationId, int district) {
            var player = API.getPlayerFromHandle(entity);
            if (PlayerHelper.PlayerCorrect(player)) {
                API.triggerClientEvent(player, ServerEvent.SHOW_FILLING_MENU, (int) stationId, district);
            }
        }

        /// <summary>
        /// Игрок отошел от продавца
        /// </summary>
        private void PlayerComeAwayFromSeller(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (PlayerHelper.PlayerCorrect(player)) {
                API.triggerClientEvent(player, ServerEvent.HIDE_FILLING_MENU);
            }
        }

        /// <summary>
        /// Игрок подъехал к бензоколонке
        /// </summary>
        private void PlayerEnterFillingPoint(NetHandle entity, string stationKey) {
            ProcessFillingPoint(entity, stationKey, (vehicle, player, key) => {
                player.setData(stationKey, vehicle);
            });
        }

        /// <summary>
        /// Игрок отъехал от бензоколонки
        /// </summary>
        private void PlayerExitFillingPoint(NetHandle entity, string stationKey) {
            ProcessFillingPoint(entity, stationKey, (vehicle, player, key) => {
                player.resetData(key);
            });
        }

        /// <summary>
        /// Обработать событие возле бензоколонки
        /// </summary>
        private void ProcessFillingPoint(NetHandle entity, string stationKey, Action<Vehicle, Client, string> action) {
            var vehicle = API.getEntityFromHandle<Vehicle>(entity);
            if (vehicle == null) {
                return;
            }
            var player = API.getVehicleDriver(vehicle);
            if (PlayerHelper.PlayerCorrect(player, true) && player.isInVehicle) {
                action(vehicle, player, stationKey);
            }
        }

        /// <summary>
        /// Метод-заглушка получения заправок
        /// </summary>
        private static IEnumerable<FillingModel> GetFillingPositions() {
            return new List<FillingModel> {
                new FillingModel {
                    Id = 1,
                    Position = new Vector3(1698.38, 4929.64, 42.10),
                    NpcPositions = "1698,16;4922,79;42,06|0,00;0,00;-25,20|1698,93;4924,04;41,16",
                    FillingPoints = "1691,62;4926,38;41,66|1688,42;4928,99;41,66|1686,09;4930,38;41,66|1682,93;4932,68;41,65",
                    District = 9
                },
                new FillingModel {
                    Id = 2,
                    Position = new Vector3(-1821.92, 787.74, 138.18),
                    NpcPositions = "-1820,17;794,33;138,09|0,00;0,00;135,76|-1821,31;793,38;137,22",
                    FillingPoints = "-1802,07;796,06;138,14|-1806,88;801,53;138,14|-1810,34;798,28;138,14|-1805,62;793,16;138,14" +
                                    "|-1798,84;798,93;138,14|-1804,02;804,29;138,14|-1800,78;807,67;138,14|-1795,65;802,13;138,14" +
                                    "|-1792,41;804,86;138,14|-1797,36;810,17;138,14|-1794,21;813,38;138,14|-1789,25;807,94;138,14",
                    District = 8
                },
                new FillingModel {
                    Id = 3,
                    Position = new Vector3(1159.86, -327.31, 69.21),
                    NpcPositions = "1164,73;-322,58;69,21|0,00;0,00;96,69|1163,29;-322,93;68,31",
                    FillingPoints = "1175,24;-319,94;68,77|1182,62;-318,56;68,77|1183,61;-323,23;68,77|1176,18;-324,54;68,77" +
                                    "|1177,99;-333,37;68,77|1185,41;-332,08;68,77|1185,78;-335,73;68,77|1178,46;-337,10;68,77" +
                                    "|1179,38;-342,02;68,77|1187,03;-340,99;68,77",
                    District = 6
                },
                new FillingModel {
                    Id = 4,
                    Position = new Vector3(-711.76, -917.27, 19.21),
                    NpcPositions = "-706,08;-913,49;19,22|0,00;0,00;91,47|-707,64;-913,62;18,32",
                    FillingPoints = "-713,21;-932,55;18,60|-713,12;-939,19;18,60|-717,46;-939,46;18,60|-717,44;-932,63;18,60" +
                                    "|-721,89;-932,44;18,60|-722,02;-939,12;18,60|-726,13;-939,42;18,60|-726,11;-932,66;18,60" +
                                    "|-730,58;-932,47;18,60|-730,56;-939,09;18,60|-734,72;-939,24;18,60|-734,75;-932,53;18,60",
                    District = 2
                },
                new FillingModel {
                    Id = 5,
                    Position = new Vector3(-53.29, -1757.10, 29.44),
                    NpcPositions = "-46,69;-1757,82;29,42|0,00;0,00;43,89|-47,98;-1756,87;28,52",
                    FillingPoints = "-59,00;-1761,39;28,72|-61,45;-1768,52;28,72|-65,47;-1767,37;28,76|-62,78;-1760,26;28,75" +
                                    "|-67,37;-1758,73;28,97|-70,09;-1765,58;28,97|-73,88;-1764,66;29,02|-71,26;-1757,49;29,05" +
                                    "|-75,58;-1755,57;29,27|-78,38;-1762,72;29,26|-82,09;-1761,43;29,30|-79,71;-1754,43;29,26",
                    District = 10
                }
            };
        }
    }
}