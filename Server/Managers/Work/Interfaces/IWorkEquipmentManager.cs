using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.Work.Interfaces {
    internal interface IWorkEquipmentManager {
        /// <summary>
        /// Устанавливает рабочее снаряжение игрока
        /// </summary>
        void SetEquipment(Client player);
    }
}