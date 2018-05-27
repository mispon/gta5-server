using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Global.Interfaces;
using gta_mp_server.Managers;
using gta_mp_server.Managers.House.Interfaces;
using gta_mp_server.Managers.Interface.Interfaces;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.NPC.Interfaces;
using gta_mp_server.Managers.Phone.Interfaces;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Quests.Interfaces;

namespace gta_mp_server.Global {
    /// <summary>
    /// Инициализирует все объекты, при запуске сервера
    /// </summary>
    internal class WorldInitializer : IWorldInitializer {
        private readonly IHouseManager _houseManager;
        private readonly IInventoryManager _inventoryManager;
        private readonly IClanManager _clanManager;
        private readonly IPhoneManager _phoneManager;
        private readonly IInterfaceManager[] _interfaceManagers;
        private readonly INpc[] _npcs;
        private readonly IMenu[] _menus;
        private readonly Place[] _places;
        private readonly IScheduler[] _schedulers;
        private readonly ClanQuest[] _clanQuests;

        public WorldInitializer(
            IHouseManager houseManager,
            IInventoryManager inventoryManager,
            IClanManager clanManager,
            IPhoneManager phoneManager,
            IInterfaceManager[] interfaceManagers,
            INpc[] npcs, 
            IMenu[] menus, 
            Place[] places,
            IScheduler[] schedulers,
            ClanQuest[] clanQuests) {
            _houseManager = houseManager;
            _inventoryManager = inventoryManager;
            _interfaceManagers = interfaceManagers;
            _clanManager = clanManager;
            _phoneManager = phoneManager;
            _npcs = npcs;
            _menus = menus;
            _places = places;
            _schedulers = schedulers;
            _clanQuests = clanQuests;
        }

        /// <summary>
        /// Инициализовать игровой мир
        /// </summary>
        public void Initialize() {
            _houseManager.Initialize();
            _inventoryManager.Initialize();
            _clanManager.Initialize();
            _phoneManager.Initialize();
            foreach (var interfaceManager in _interfaceManagers) interfaceManager.Initialize();
            foreach (var npc in _npcs) npc.Initialize();
            foreach (var menu in _menus) menu.Initialize();
            foreach (var place in _places) place.Initialize();
            foreach (var scheduler in _schedulers) scheduler.Initialize();
            foreach (var clanQuest in _clanQuests) clanQuest.Initialize();
        }
    }
}