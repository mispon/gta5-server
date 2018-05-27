using gta_mp_server.Enums;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Models.Work {
    /// <summary>
    /// Контракт на перевозку груза для дальнобойщиков и летчиков
    /// </summary>
    internal class DeliveryContract {
        private const int DEFAULT_REWARD = 200;

        private readonly int _from;
        private readonly int _to;

        public DeliveryContract(int from = 250, int to = 350) {
            _from = from;
            _to = to;
        }

        /// <summary>
        /// Название груза
        /// </summary>
        public string Name => Type.GetDescription();

        /// <summary>
        /// Позиция точки доставки
        /// </summary>
        public Vector3 TargetPosition { get; set; }

        /// <summary>
        /// Тип контракта
        /// </summary>
        public DeliveryContractType Type { get; set; }

        /// <summary>
        /// Денежное вознаграждение
        /// </summary>
        public int Reward { get; set; } = DEFAULT_REWARD;

        /// <summary>
        /// Выставить рандомное значение награды
        /// </summary>
        internal void ChangeReward() {
            Reward = ActionHelper.Random.Next(_from, _to);
        }
    }
}