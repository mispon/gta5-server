using gta_mp_database.Providers;
using gta_mp_database.Providers.Interfaces;
using Ninject.Modules;

namespace gta_mp_server.IoC {
    public class DatabaseCoreModule : NinjectModule {
        /// <summary>
        /// Загрузка модуля
        /// </summary>
        public override void Load() {
            Bind<IAccountsProvider>().To<AccountsProvider>().InSingletonScope();
            Bind<IPlayersProvider>().To<PlayersProvider>().InSingletonScope();
            Bind<IHousesProvider>().To<HousesProvider>().InSingletonScope();
            Bind<IVehiclesProvider>().To<VehiclesProvider>().InSingletonScope();
            Bind<IPlayersAppearanceProvider>().To<PlayersAppearanceProvider>().InSingletonScope();
            Bind<IClanProvider>().To<ClanProvider>().InSingletonScope();
            Bind<IDistrictsProvider>().To<DistrictsProvider>().InSingletonScope();
        }
    }
}