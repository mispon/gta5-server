using gta_mp_database.Models.Player;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.Player.Interfaces {
    internal interface IInventoryManager {
        /// <summary>
        /// Проинициализировать инвентарь
        /// </summary>
        void Initialize();

        /// <summary>
        /// Одеть оружие на игрока
        /// </summary>
        void EquipWeapon(Client player);

        /// <summary>
        /// Обновить патроны игрока
        /// </summary>
        void RefreshAmmo(Client player, PlayerInfo playerInfo = null);
    }
}