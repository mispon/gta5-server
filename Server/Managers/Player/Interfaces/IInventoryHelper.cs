using System.Collections.Generic;
using gta_mp_data.Entity;
using gta_mp_server.Managers.Player.Helpers;

namespace gta_mp_server.Managers.Player.Interfaces {
    internal interface IInventoryHelper {
        /// <summary>
        /// Проверяет, что игрок / тс может столько унести / увезти
        /// </summary>
        bool CanCarry(IEnumerable<InventoryItem> inventory, InventoryItem newItem, int count, int carrying = InventoryHelper.MAX_PLAYER_CARRYING);
    }
}