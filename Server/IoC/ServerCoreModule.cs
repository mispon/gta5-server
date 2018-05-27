using gta_mp_server.Clan;
using gta_mp_server.Clan.DistrictWar;
using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Clan.Mission;
using gta_mp_server.Events;
using gta_mp_server.Events.Interfaces;
using gta_mp_server.GameEvents;
using gta_mp_server.GameEvents.Interfaces;
using gta_mp_server.Global;
using gta_mp_server.Global.Interfaces;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers;
using gta_mp_server.Managers.Auth;
using gta_mp_server.Managers.Auth.Interfaces;
using gta_mp_server.Managers.DrivingSchool;
using gta_mp_server.Managers.DrivingSchool.Interfaces;
using gta_mp_server.Managers.Fun;
using gta_mp_server.Managers.House;
using gta_mp_server.Managers.House.Interfaces;
using gta_mp_server.Managers.Interface;
using gta_mp_server.Managers.Interface.Interfaces;
using gta_mp_server.Managers.MenuHandlers;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.MenuHandlers.Work;
using gta_mp_server.Managers.NPC;
using gta_mp_server.Managers.NPC.Interfaces;
using gta_mp_server.Managers.Phone;
using gta_mp_server.Managers.Phone.Interfaces;
using gta_mp_server.Managers.Places;
using gta_mp_server.Managers.Places.AirPorts;
using gta_mp_server.Managers.Places.ClothesShop;
using gta_mp_server.Managers.Places.FarmPlace;
using gta_mp_server.Managers.Places.Hospitals;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Places.VehicleShowroom;
using gta_mp_server.Managers.Places.Weapon;
using gta_mp_server.Managers.Player;
using gta_mp_server.Managers.Player.Helpers;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Races;
using gta_mp_server.Managers.Races.Interfaces;
using gta_mp_server.Managers.Races.TrackHandlers;
using gta_mp_server.Managers.Races.TrackHandlers.Interfaces;
using gta_mp_server.Managers.Tuning;
using gta_mp_server.Managers.Vehicles;
using gta_mp_server.Managers.Vehicles.Interfaces;
using gta_mp_server.Managers.Work;
using gta_mp_server.Managers.Work.Bistro;
using gta_mp_server.Managers.Work.Bistro.Interfaces;
using gta_mp_server.Managers.Work.Builder;
using gta_mp_server.Managers.Work.Builder.Interfaces;
using gta_mp_server.Managers.Work.Farmer;
using gta_mp_server.Managers.Work.Farmer.Interfaces;
using gta_mp_server.Managers.Work.Fisherman;
using gta_mp_server.Managers.Work.Fisherman.Interfaces;
using gta_mp_server.Managers.Work.Forklift;
using gta_mp_server.Managers.Work.Forklift.Interfaces;
using gta_mp_server.Managers.Work.Interfaces;
using gta_mp_server.Managers.Work.Loader;
using gta_mp_server.Managers.Work.Loader.Interfaces;
using gta_mp_server.Managers.Work.Pilot;
using gta_mp_server.Managers.Work.Pilot.Interfaces;
using gta_mp_server.Managers.Work.Police;
using gta_mp_server.Managers.Work.Police.Interfaces;
using gta_mp_server.Managers.Work.Taxi;
using gta_mp_server.Managers.Work.Taxi.Interfaces;
using gta_mp_server.Managers.Work.Trucker;
using gta_mp_server.Managers.Work.Trucker.Interfaces;
using gta_mp_server.Managers.Work.Wrecker;
using gta_mp_server.Managers.Work.Wrecker.Interfaces;
using gta_mp_server.Quests.ClanQuests;
using gta_mp_server.Quests.Interfaces;
using gta_mp_server.Voice;
using gta_mp_server.Voice.Interfaces;
using Ninject.Modules;

namespace gta_mp_server.IoC {
    /// <summary>
    /// Загрузчик всех модулей сервера
    /// </summary>
    internal class ServerCoreModule : NinjectModule {
        /// <summary>
        /// Загрузка модуля
        /// </summary>
        public override void Load() {
            LoadEvents();
            LoadGameEvents();
            LoadRaces();
            LoadHelpers();
            LoadManagers();
            LoadPlaces();
            LoadMenus();
            LoadGlobal();
            LoadClans();
            LoadQuests();
        }

        /// <summary>
        /// Глобальные обработчики
        /// </summary>
        private void LoadGlobal(){
            Bind<IWorldInitializer>().To<WorldInitializer>().InSingletonScope();
            Bind<IDatabaseSynchronizer>().To<DatabaseSynchronizer>().InSingletonScope();
        }

        /// <summary>
        /// Вспомогательные классы
        /// </summary>
        private void LoadHelpers() {
            Bind<IPointCreator>().To<PointCreator>().InSingletonScope();
            Bind<IDoormanager>().To<Doormanager>().InSingletonScope();
            Bind<IGtaCharacter>().To<GtaCharacter>().InSingletonScope();
        }

        /// <summary>
        /// Загрузить менеджеры
        /// </summary>
        private void LoadManagers() {
            LoadWorksManagers();
            LoadNpcs();

            // Менеджеры сущностей
            Bind<IPlayerManager>().To<PlayerManager>().InSingletonScope();
            Bind<IPlayerInfoManager>().To<PlayerInfoManager>().InSingletonScope();
            Bind<IWorkInfoManager>().To<WorkInfoManager>().InSingletonScope();
            Bind<IVehicleManager>().To<VehicleManager>().InSingletonScope();
            Bind<IVehicleInfoManager>().To<VehicleInfoManager>().InSingletonScope();
            Bind<IVehicleTrunkManager>().To<VehicleTrunkManager>().InSingletonScope();
            Bind<IHouseManager>().To<HouseManager>().InSingletonScope();
            Bind<IHouseEventManager>().To<HouseEventManager>().InSingletonScope();
            Bind<IStorageManager>().To<StorageManager>().InSingletonScope();
            Bind<IInventoryManager>().To<InventoryManager>().InSingletonScope();
            Bind<IInventoryHelper>().To<InventoryHelper>().InSingletonScope();
            Bind<IOpportunitiesNotifier>().To<OpportunitiesNotifier>().InSingletonScope();
            Bind<IChatHandler>().To<ChatHandler>().InSingletonScope();
            Bind<IVoiceManager>().To<VoiceManager>().InSingletonScope();
            Bind<IPhoneManager>().To<PhoneManager>().InSingletonScope();
            Bind<IWorkEquipmentManager>().To<WorkEquipmentManager>().InSingletonScope();
            Bind<IGiftsManager>().To<GiftsManager>().InSingletonScope();

            // Всякое
            Bind<IInterfaceManager>().To<LoginManager>().InSingletonScope();
            Bind<IInterfaceManager>().To<RegistrationManager>().InSingletonScope();
            Bind<IInterfaceManager>().To<InterfaceManager>().InSingletonScope();
            Bind<ICreatingCharManager>().To<CreatingCharManager>().InSingletonScope();
            Bind<IDriverPracticeExamManager>().To<DriverPracticeExamManager>().InSingletonScope();
        }

        /// <summary>
        /// Модули работ
        /// </summary>
        private void LoadWorksManagers() {
            Bind<ILoaderManager>().To<LoaderManager>().InSingletonScope();
            Bind<ILoaderEventHandler>().To<LoaderEventHandler>().InSingletonScope();
            Bind<IForkliftManager>().To<ForkliftManager>().InSingletonScope();
            Bind<IForkliftEventHandler>().To<ForkliftEventHandler>().InSingletonScope();
            Bind<IBuilderManager>().To<BuilderManager>().InSingletonScope();
            Bind<ITaxiDriverManager>().To<TaxiDriverManager>().InSingletonScope();
            Bind<IPoliceManager>().To<PoliceManager>().InSingletonScope();
            Bind<IPoliceRewardManager>().To<PoliceRewardManager>().InSingletonScope();
            Bind<IPoliceAlertManager>().To<PoliceAlertManager>().InSingletonScope();
            Bind<IJailManager>().To<JailManager>().InSingletonScope();
            Bind<IBistroManager>().To<BistroManager>().InSingletonScope();
            Bind<ITruckersManager>().To<TruckersManager>().InSingletonScope();
            Bind<IWreckerManager>().To<WreckerManager>().InSingletonScope();
            Bind<IPilotManager>().To<PilotManager>().InSingletonScope();
            Bind<IFishermanManager>().To<FishermanManager>().InSingletonScope();
            Bind<IFarmerManager>().To<FarmerManager>().InSingletonScope();
            Bind<ITractorDriverManager>().To<TractorDriverManager>().InSingletonScope();
        }

        /// <summary>
        /// Нпс
        /// </summary>
        private void LoadNpcs() {
            Bind<INpc>().To<LoaderNpc>();
            Bind<INpc>().To<ForkliftNpc>();
            Bind<INpc>().To<BuilderNpc>();
            Bind<INpc>().To<DrivingSchoolNpc>();
            Bind<INpc>().To<BusDriverNpc>();
            Bind<INpc>().To<TaxiDriverNpc>();
            Bind<INpc>().To<PolicemanNpc>();
            Bind<INpc>().To<RentOfScootersNpc>();
            Bind<INpc>().To<ParkingNpc>();
            Bind<INpc>().To<ParkingFineNpc>();
            Bind<INpc>().To<BistroNpc>();
            Bind<INpc>().To<RaceNpc>();
            Bind<INpc>().To<StreetFightNpc>();
            Bind<INpc>().To<FishermanNpc>();
            Bind<INpc>().To<FarmNpc>();
        }

        /// <summary>
        /// Различные места
        /// </summary>
        private void LoadPlaces() {
            Bind<Place>().To<ScrapMetalDump>();
            Bind<Place>().To<TextileMill>();
            Bind<Place>().To<Building>();
            Bind<Place>().To<DrivingSchool>();
            Bind<Place>().To<BusDepot>();
            Bind<Place>().To<TaxiDepot>();
            Bind<Place>().To<PoliceDepartment>();
            Bind<Place>().To<FillingStation>();
            Bind<Place>().To<Shop>();
            Bind<Place>().To<Hospital>();
            Bind<Place>().To<ClothesShop>();
            Bind<Place>().To<VehicleShowroom>();
            Bind<Place>().To<Parking>();
            Bind<Place>().To<ParkingFine>();
            Bind<Place>().To<Bistro>();
            Bind<Place>().To<Port>();
            Bind<Place>().To<Parachute>();
            Bind<Place>().To<StreetFights>();
            Bind<Place>().To<AmmuNation>();
            Bind<Place>().To<TuningGarage>();
            Bind<Place>().To<AirPort>();
            Bind<Place>().To<FishingVillage>();
            Bind<Place>().To<Farm>();
        }

        /// <summary>
        /// Обработчики меню
        /// </summary>
        private void LoadMenus() {
            Bind<IMenu>().To<AdminMenuHandler>();
            Bind<IMenu>().To<GpsMenuHandler>();
            Bind<IMenu>().To<ForkliftMenuHandler>();
            Bind<IMenu>().To<LoaderMenuHandler>();
            Bind<IMenu>().To<DrivingSchoolMenuHandler>();
            Bind<IMenu>().To<BusDriverMenuHandler>();
            Bind<IMenu>().To<TaxiDriverMenuHandler>();
            Bind<IMenu>().To<PoliceMenuHandler>();
            Bind<IMenu>().To<HouseMenuHandler>();
            Bind<IMenu>().To<FillingMenuHandler>();
            Bind<IMenu>().To<ShopMenuHandler>();
            Bind<IMenu>().To<RentOfScootersMenuHandler>();
            Bind<IMenu>().To<NurseMenuHandler>();
            Bind<IMenu>().To<ClothesMenuHandler>();
            Bind<IMenu>().To<DressingRoomMenuHandler>();
            Bind<IMenu>().To<VehicleShowroomMenuHandler>();
            Bind<IMenu>().To<BistroMenuHandler>();
            Bind<IMenu>().To<TruckersMenuHandler>();
            Bind<IMenu>().To<ParkingFineMenuHandler>();
            Bind<IMenu>().To<RaceMenuHandler>();
            Bind<IMenu>().To<PlayerActionsMenuHandler>();
            Bind<IMenu>().To<VehicleActionsMenuHandler>();
            Bind<IMenu>().To<TuningMenuHandler>();
            Bind<IMenu>().To<PilotMenuHandler>();
            Bind<IMenu>().To<BuilderMenuHandler>();
            Bind<IMenu>().To<FishermanMenuHandler>();
            Bind<IMenu>().To<FarmMenuHandler>();

            Bind<IMenu>().To<RacecourseMenuHandler>();
        }

        /// <summary>
        /// Менеджеры серверных событий игроков
        /// </summary>
        private void LoadEvents() {
            Bind<IPlayerFinishDownloadManager>().To<PlayerFinishDownloadManager>().InSingletonScope();
            Bind<IPlayerConnectManager>().To<PlayerConnectManager>().InSingletonScope();
            Bind<IPlayerDisconnectManager>().To<PlayerDisconnectManager>().InSingletonScope();
            Bind<IPlayerRespawnManager>().To<PlayerRespawnManager>().InSingletonScope();
            Bind<IPlayerDamagedManager>().To<PlayerDamagedManager>().InSingletonScope();
            Bind<IPlayerDeathManager>().To<PlayerDeathManager>().InSingletonScope();
            Bind<IVehicleEnterManager>().To<VehicleEventsManager>().InSingletonScope();
        }

        /// <summary>
        /// Игровые эвенты
        /// </summary>
        private void LoadGameEvents() {
            Bind<IScheduler>().To<EventsScheduler>().InSingletonScope();
            Bind<GameEvent>().To<SniperBattle>().InSingletonScope();
            Bind<GameEvent>().To<PrisonRiot>().InSingletonScope();
            Bind<GameEvent>().To<CountryBreakdown>().InSingletonScope();
        }

        /// <summary>
        /// Гонки
        /// </summary>
        private void LoadRaces() {
            Bind<IScheduler>().To<RaceScheduler>().InSingletonScope();
            Bind<Race>().To<CarRace>().InSingletonScope().Named("CarRace");
            Bind<Race>().To<MotoRace>().InSingletonScope().Named("MotoRace");
            Bind<Race>().To<Rally>().InSingletonScope().Named("Rally");
            Bind<Race>().To<MountainRace>().InSingletonScope().Named("MountainRace");
            Bind<TrackHandler>().To<CarTrackHandler>().InSingletonScope();
            Bind<TrackHandler>().To<MotoTrackHandler>().InSingletonScope();
            Bind<TrackHandler>().To<MountainTrackHandler>().InSingletonScope();
            Bind<TrackHandler>().To<RallyTrackHandler>().InSingletonScope();
        }

        /// <summary>
        /// Кланы
        /// </summary>
        private void LoadClans() {
            Bind<IScheduler>().To<DistrictWarsScheduler>().InSingletonScope();
            Bind<IMenu>().To<ClanMenuHandler>().InSingletonScope();
            Bind<IMenu>().To<MissionMenuHandler>().InSingletonScope();
            Bind<Place>().To<ClanHall>().InSingletonScope();
            Bind<IClanManager>().To<ClanManager>().InSingletonScope();
            Bind<IDistrictWarsManager>().To<DistrictWarsManager>().InSingletonScope();
            Bind<IClanMissionManager>().To<ClanMissionManager>().InSingletonScope();
            Bind<IClanCourtyard>().To<ClanCourtyard>().InSingletonScope();
        }

        /// <summary>
        /// Квесты
        /// </summary>
        private void LoadQuests() {
            Bind<ClanQuest>().To<VehicleTheft>().InSingletonScope();
            Bind<ClanQuest>().To<ClanRacket>().InSingletonScope();
            Bind<ClanQuest>().To<DrugDelivery>().InSingletonScope();
            Bind<ClanQuest>().To<CaseFalsification>().InSingletonScope();
        }
    }
}