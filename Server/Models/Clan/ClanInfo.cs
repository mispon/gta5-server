using gta_mp_server.Models.Shops;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models.Clan {
    internal class ClanInfo {
        /// <summary>
        /// Идентификатор аналогичный тому, что в базе
        /// </summary>
        public long ClanId { get; set; }

        /// <summary>
        /// Название клана
        /// </summary>
        public string ClanName { get; set; }

        /// <summary>
        /// Цвет маркера
        /// </summary>
        public int BlipColor { get; set; }

        /// <summary>
        /// Позиции входа и после входа
        /// </summary>
        public Vector3 Enter { get; set; }
        public Vector3 AfterEnter { get; set; }

        /// <summary>
        /// Позиции входа и после входа
        /// </summary>
        public Vector3 Exit { get; set; }
        public Vector3 AfterExit { get; set; }

        /// <summary>
        /// Данные лидера
        /// </summary>
        public string LeaderName { get; set; }
        public Vector3 LeaderPosition { get; set; }
        public Vector3 LeaderRotation { get; set; }
        public Vector3 LeaderMarker { get; set; }
        public PedHash LeaderHash { get; set; }

        /// <summary>
        /// Данные администратора
        /// </summary>
        public Vector3 AdminPosition { get; set; }
        public Vector3 AdminRotation { get; set; }
        public Vector3 AdminMarker { get; set; }
        public PedHash AdminHash { get; set; }

        /// <summary>
        /// Данные оружейника
        /// </summary>
        public Vector3 GunsmithPosition { get; set; }
        public Vector3 GunsmithRotation { get; set; }
        public Vector3 GunsmithMarker { get; set; }
        public PedHash GunsmithHash { get; set; }

        /// <summary>
        /// Данные автомеханика
        /// </summary>
        public Vector3 MechPosition { get; set; }
        public Vector3 MechRotation { get; set; }
        public Vector3 MechMarker { get; set; }
        public PedHash MechHash { get; set; }

        /// <summary>
        /// Позиции гардероба
        /// </summary>
        public Vector3 DressingMarker { get; set; }
        public DressingRoomPositions DressingRoomPositions { get; set; }

        /// <summary>
        /// Данные внутреннего дворика
        /// </summary>
        public ClanCourtyardInfo Courtyard { get; set; }
    }
}