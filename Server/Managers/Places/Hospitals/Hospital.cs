using gta_mp_server.Constant;
using gta_mp_server.Enums;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Places.Hospitals {
    /// <summary>
    /// Больница
    /// </summary>
    internal class Hospital : Place {
        internal static Vector3 SpawnPosition = new Vector3(264.39, -1352.10, 24.54);

        private static readonly Vector3 _exitPosition = new Vector3(275.85, -1361.49, 24.54);
        private static readonly Vector3 _afterExitPosition = new Vector3(274.00, -1359.97, 24.54);
        private static readonly Vector3 _nursePosition = new Vector3(264.63, -1357.38, 24.54);
        private static readonly Vector3 _nurseRotation = new Vector3(0.00, 0.00, 6.55);
        private static readonly Vector3 _nurseMarker = new Vector3(264.15, -1355.51, 23.64);

        private readonly IPointCreator _pointCreator;
        private readonly IPlayerInfoManager _playerInfoManager;

        public Hospital() {}
        public Hospital(IPointCreator pointCreator, IPlayerInfoManager playerInfoManager) {
            _pointCreator = pointCreator;
            _playerInfoManager = playerInfoManager;
        }

        /// <summary>
        /// Инизиализировать больницу
        /// </summary>
        public override void Initialize() {
            API.requestIpl("Coroner_Int_on");
            foreach (var hospital in HospitalHelper.Hospitals) {
                _pointCreator.CreateBlip(hospital.Position, 153, 11, scale: 1.3f, name: "Госпиталь");
                var enter = _pointCreator.CreateMarker(Marker.UpsideDownCone, hospital.Position, Colors.Yellow, 1.5f);
                enter.ColShape.onEntityEnterColShape += (shape, entity) => PlayerEnterIntoHospital(entity, hospital.Dimension);
                var exit = _pointCreator.CreateMarker(Marker.UpsideDownCone, _exitPosition, Colors.Yellow, 1.5f, dimention: hospital.Dimension);
                exit.ColShape.onEntityEnterColShape += (shape, entity) => PlayerExitFromHospital(entity, hospital.PositionAfterExit);
                exit.ColShape.dimension = hospital.Dimension;
                CreateNurse(hospital.Dimension);
                CreateSafeZone(SpawnPosition.Add(new Vector3(0, 0, -15)), 20f, hospital.Dimension);
                CreateSafeZone(hospital.PositionAfterExit, 27f);
            }
        }

        /// <summary>
        /// Игрок входит в госпиталь
        /// </summary>
        private void PlayerEnterIntoHospital(NetHandle entity, int dimension) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.setEntityPosition(player, _afterExitPosition);
            _playerInfoManager.SetDimension(player, dimension);
        }

        /// <summary>
        /// Игрок выходит из госпиталя
        /// </summary>
        private void PlayerExitFromHospital(NetHandle entity, Vector3 position) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.setEntityPosition(player, position);
            _playerInfoManager.SetDimension(player, 0);
        }

        /// <summary>
        /// Создает зону, где игрок неуязвим
        /// </summary>
        private void CreateSafeZone(Vector3 position, float range, int dimension = 0) {
            var safeZone = API.createSphereColShape(position, range);
            safeZone.dimension = dimension;
            safeZone.onEntityEnterColShape += (shape, entity) => PlayerHelper.ProcessAction(entity, player => {
                ActionHelper.SetAction(player, 500, () => API.setEntityInvincible(player, true));
            }, true);
            safeZone.onEntityExitColShape += (shape, entity) => PlayerHelper.ProcessAction(entity, player => API.setEntityInvincible(player, false), true);
        }

        /// <summary>
        /// Создает медсестру
        /// </summary>
        private void CreateNurse(int dimension) {
            var ped = _pointCreator.CreatePed(PedHash.Soucentmc01AFM, "Медсестра", _nursePosition, _nurseRotation, _nurseMarker, Colors.VividCyan, dimension);
            ped.ColShape.onEntityEnterColShape += PlayerComeToNurse;
            ped.ColShape.onEntityExitColShape += PlayerAwayFromNurse;
        }

        /// <summary>
        /// Игрок подошел к медсестре
        /// </summary>
        private void PlayerComeToNurse(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.SHOW_NURSE_MENU);
        }

        /// <summary>
        /// Игрок отошел от медсестры
        /// </summary>
        private void PlayerAwayFromNurse(ColShape shape, NetHandle entity) {
            var player = API.getPlayerFromHandle(entity);
            if (!PlayerHelper.PlayerCorrect(player)) {
                return;
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_NURSE_MENU);
        }
    }
}