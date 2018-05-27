using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.Player.Interfaces {
    internal interface IOpportunitiesNotifier {
        /// <summary>
        /// 
        /// </summary>
        void Notify(Client player, int level);
    }
}