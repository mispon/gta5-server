using gta_mp_data.Entity;
using gta_mp_database.Entity;
using LinqToDB;
using LinqToDB.Data;

namespace gta_mp_database {
    /// <summary>
    /// Класс представляет собой источник данных из базы
    /// </summary>
    public class Database : DataConnection {
        public Database() : base("GtaVServer") {}

        public ITable<Account> Accounts => GetTable<Account>();
        public ITable<PlayerInfo> PlayersInfo => GetTable<PlayerInfo>();
        public ITable<PlayerAdditionalInfo> PlayersAdditionalInfo => GetTable<PlayerAdditionalInfo>();
        public ITable<PlayerAppearance> PlayersAppearance => GetTable<PlayerAppearance>();
        public ITable<PlayerWork> PlayerWorks => GetTable<PlayerWork>();
        public ITable<DriverInfo> DriversInfo => GetTable<DriverInfo>();
        public ITable<Wanted> Jail => GetTable<Wanted>();
        public ITable<PlayerClothes> PlayerClothes => GetTable<PlayerClothes>();
        public ITable<PlayerClanInfo> PlayerClanInfos => GetTable<PlayerClanInfo>();
        public ITable<Settings> Settings => GetTable<Settings>();
        public ITable<House> Houses => GetTable<House>();
        public ITable<Vehicle> Vehicles => GetTable<Vehicle>();
        public ITable<VehicleTuning> VehiclesTuning => GetTable<VehicleTuning>();
        public ITable<InventoryItem> Inventory => GetTable<InventoryItem>();
        public ITable<PhoneContact> PhoneContacts => GetTable<PhoneContact>();

        public ITable<Clan> Clans => GetTable<Clan>();
        public ITable<District> Districts => GetTable<District>();
    }
}