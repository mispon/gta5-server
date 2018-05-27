using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.Work.Police.Interfaces {
    /// <summary>
    /// Логика вознаграждений полицейских
    /// </summary>
    internal interface IPoliceRewardManager {
        /// <summary>
        /// Начислить награду за патрулирование
        /// </summary>
        void SetPatrolReward(Client player);

        /// <summary>
        /// Начислить награду за арест
        /// </summary>
        void SetEffortReward(Client policeman, Client prisoner);
    }
}