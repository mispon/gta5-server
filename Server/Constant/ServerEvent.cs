namespace gta_mp_server.Constant {
    public class ServerEvent {
        /// <summary>
        /// Авторизация
        /// </summary>
        public const string SHOW_LOGIN = "ShowLogin";
        public const string BAD_LOGIN = "BadLogin";
        public const string HIDE_AUTH = "HideAuth";
        public const string BAD_REGISTER = "BadRegister";

        /// <summary>
        /// Меню действий игрока
        /// </summary>
        public const string SHOW_PLAYER_ACTION_MENU = "ShowPlayerActionMenu";
        public const string HIDE_PLAYER_ACTION_MENU = "HidePlayerActionMenu";

        /// <summary>
        /// Меню действий над транспортом
        /// </summary>
        public const string SHOW_VEHICLE_ACTION_MENU = "ShowVehicleActionMenu";
        public const string HIDE_VEHICLE_ACTION_MENU = "HideVehicleActionMenu";

        /// <summary>
        /// Грузчики
        /// </summary>
        public const string SHOW_LOADER_MENU = "ShowLoaderMenu";
        public const string HIDE_LOADER_MENU = "HideLoaderMenu";

        /// <summary>
        /// Погрузчики
        /// </summary>
        public const string SHOW_FORKLIFT_MENU = "ShowForkliftMenu";
        public const string HIDE_FORKLIFT_MENU = "HideForkliftMenu";

        /// <summary>
        /// Эвенты грузчиков / погрузчиков
        /// </summary>
        public const string SHOW_TAKE_LOADER_POINT = "ShowTakeLoaderPoint";
        public const string SHOW_PUT_LOADER_POINT = "ShowPutLoaderPoint";
        public const string HIDE_LOADER_POINTS = "HideLoaderPoints";

        /// <summary>
        /// Строители
        /// </summary>
        public const string SHOW_BUILDER_MENU = "ShowBuilderMenu";
        public const string HIDE_BUILDER_MENU = "HideBuilderMenu";
        public const string SHOW_BUILDER_POINT = "ShowBuilderPoint";
        public const string HIDE_BUILDER_POINT = "HideBuilderPoint";

        /// <summary>
        /// Автошкола
        /// </summary>
        public const string SHOW_ANDREAS_MENU = "ShowAndreasMenu";
        public const string HIDE_ANDREAS_MENU = "HideAndreasMenu";
        public const string SHOW_THEORY_EXAM = "ShowTheoryExam";

        /// <summary>
        /// Автобусный парк
        /// </summary>
        public const string SHOW_ONEIL_MENU = "ShowOneilMenu";
        public const string HIDE_ONEIL_MENU = "HideOneilMenu";

        /// <summary>
        /// Такси
        /// </summary>
        public const string SHOW_SIEMON_MENU = "ShowSiemonMenu";
        public const string HIDE_SIEMON_MENU = "HideSiemonMenu";
        public const string SHOW_GPS_TARGET = "ShowGPSTarget";
        public const string SHOW_GPS_MENU = "ShowGPSMenu";
        public const string HIDE_GPS_MENU = "HideGPSMenu";

        /// <summary>
        /// Полиция
        /// </summary>
        public const string SHOW_SARAH_MENU = "ShowSarahMenu";
        public const string HIDE_SARAH_MENU = "HideSarahMenu";
        public const string HIDE_POLICEMAN_MENU = "HidePolicemanMenu";
        public const string TRIGGER_POLICE_ACTION_MENU = "TriggerPoliceActionMenu";
        public const string CREATE_ALERT = "CreatePoliceAlert";
        public const string UPDATE_ALERT = "UpdatePoliceAlert";
        public const string FINISH_ALERT = "FinishPoliceAlert";
        public const string SHOW_ALL_ALERTS = "ShowPoliceAlerts";
        public const string HIDE_ALL_ALERTS = "HidePoliceAlerts";

        /// <summary>
        /// Дома
        /// </summary>
        public const string SHOW_HOUSE_MENU = "ShowHouseMenu";
        public const string HIDE_HOUSE_MENU = "HideHouseMenu";
        public const string SHOW_HOUSE_STORAGE_MENU = "ShowHouseStorageMenu";
        public const string HIDE_HOUSE_STORAGE_MENU = "HideHouseStorageMenu";

        /// <summary>
        /// Заправки
        /// </summary>
        public const string SHOW_FILLING_MENU = "ShowFillingMenu";
        public const string HIDE_FILLING_MENU = "HideFillingMenu";

        /// <summary>
        /// Магазины
        /// </summary>
        public const string SHOW_SHOP_MENU = "ShowShopMenu";
        public const string HIDE_SHOP_MENU = "HideShopMenu";

        /// <summary>
        /// Аренда скутеров
        /// </summary>
        public const string SHOW_SCOOTERS_MENU = "ShowScootersMenu";
        public const string HIDE_SCOOTERS_MENU = "HideScootersMenu";

        /// <summary>
        /// Медсестра
        /// </summary>
        public const string SHOW_NURSE_MENU = "ShowNurseMenu";
        public const string HIDE_NURSE_MENU = "HideNurseMenu";

        /// <summary>
        /// Интерфейсные эвенты
        /// </summary>
        public const string SET_TIMER = "SetTimer";
        public const string STOP_TIMER = "StopTimer";
        public const string SET_PROGRESS_ACTION = "SetProgressAction";
        public const string STOP_PROGRESS_ACTION = "StopProgressAction";
        public const string UPDATE_INFO = "UpdateInfo";
        public const string SHOW_INTERFACE = "ShowInterface";
        public const string SHOW_SPEEDOMETER = "ShowSpeedometer";
        public const string HIDE_SPEEDOMETER = "HideSpeedometer";
        public const string SHOW_PLAYER_INFO = "ShowPlayerInfo";
        public const string SHOW_HINT = "ShowHint";
        public const string HIDE_HINT = "HideHint";
        public const string SHOW_SUBTITLE = "ShowSubtitle";
        public const string SET_GIFTS_TIMER = "SetGiftsTimer";

        /// <summary>
        /// Телефон
        /// </summary>
        public const string HIDE_PHONE = "HidePhone";
        public const string SET_PHONE_INFO = "SetPhoneInfo";
        public const string SHOW_MAIN_DISPLAY = "ShowMainDisplay";

        /// <summary>
        /// Настройка внешности персонажа
        /// </summary>
        public const string SHOW_CHAR_CREATE = "ShowCharCreate";
        public const string HIDE_CHAR_CREATE = "HideCharCreate";
        public const string NAME_ALREADY_EXIST = "NameAlreadyExist";
        public const string UPDATE_APPEARANCE = "UpdateAppearance";

        /// <summary>
        /// Магазин одежды и примерочная
        /// </summary>
        public const string SHOW_CLOTHES_LIST = "ShowClothesList";
        public const string SHOW_CLOTHES_MENU = "ShowClothesMenu";
        public const string HIDE_CLOTHES_MENU = "HideClothesMenu";

        /// <summary>
        /// Автосалон
        /// </summary>
        public const string SHOW_SHOWROOM_MENU = "ShowShowroomMenu";
        public const string HIDE_SHOWROOM_MENU = "HideShowroomMenu";

        /// <summary>
        /// Парковка
        /// </summary>
        public const string SHOW_PARKING_MENU = "ShowParkingMenu";
        public const string HIDE_PARKING_MENU = "HideParkingMenu";

        /// <summary>
        /// Закусочная
        /// </summary>
        public const string SHOW_BISTRO_MENU = "ShowBistroMenu";
        public const string HIDE_BISTRO_MENU = "HideBistroMenu";
        public const string SHOW_BISTRO_FOOD_MENU = "ShowBistroFoodMenu";
        public const string HIDE_BISTRO_FOOD_MENU = "HideBistroFoodMenu";
        public const string SHOW_DELIVERY_POINT = "ShowDeliveryPoint";

        /// <summary>
        /// Дальнобойщики
        /// </summary>
        public const string SHOW_TRUCKERS_MENU = "ShowTruckersMenu";
        public const string HIDE_TRUCKERS_MENU = "HideTruckersMenu";
        public const string SHOW_TRUCKER_TARGET_POINT = "ShowTruckerTargetPoint";
        public const string HIDE_TRUCKER_TARGET_POINT = "HideTruckerTargetPoint";

        /// <summary>
        /// Летчики
        /// </summary>
        public const string SHOW_PILOT_MENU = "ShowPilotMenu";
        public const string HIDE_PILOT_MENU = "HidePilotMenu";
        public const string SHOW_PILOT_TARGET_POINT = "ShowPilotTargetPoint";
        public const string HIDE_PILOT_TARGET_POINT = "HidePilotTargetPoint";

        /// <summary>
        /// Прыжок с парашютом
        /// </summary>
        public const string SHOW_PARACHUTE_MENU = "ShowParachuteMenu";
        public const string HIDE_PARACHUTE_MENU = "HideParachuteMenu";

        /// <summary>
        /// Штрафстоянка
        /// </summary>
        public const string SHOW_PARKING_FINE_MENU = "ShowParkingFineMenu";
        public const string HIDE_PARKING_FINE_MENU = "HideParkingFineMenu";
        public const string SHOW_AFK_VEHICLES = "ShowAfkVehicles";
        public const string HIDE_AFK_VEHICLES = "HideAfkVehicles";
        public const string SHOW_DROP_ZONE = "ShowDropZone";
        public const string HIDE_DROP_ZONE = "HideDropZone";
        public const string ADD_AFK_VEHICLE = "AddAfkVehicle";
        public const string REMOVE_AFK_VEHICLE = "RemoveAfkVehicle";

        /// <summary>
        /// Гонки
        /// </summary>
        public const string SHOW_RACE_MENU = "ShowRaceMenu";
        public const string HIDE_RACE_MENU = "HideRaceMenu";
        public const string SHOW_RACE_POINT = "ShowRacePoint";
        public const string HIDE_RACE_POINT = "HideRacePoint";
        public const string START_CAR_RACE = "StartCarRace";
        public const string START_MOTO_RACE = "StartMotoRace";
        public const string START_RALLY = "StartRally";
        public const string START_MOUNTAIN_RACE = "StartMountainRace";

        /// <summary>
        /// Уличные драки
        /// </summary>
        public const string SHOW_FIGHT_MENU = "ShowFightMenu";
        public const string HIDE_FIGHT_MENU = "HideFightMenu";
        public const string START_FIGHT = "StartFight";

        /// <summary>
        /// Инвентарь
        /// </summary>
        public const string SHOW_INVENTORY = "ShowInventory";

        /// <summary>
        /// Магазин оружия
        /// </summary>
        public const string SHOW_AMMU_NATION_MENU = "ShowAmmuNationMenu";
        public const string HIDE_AMMU_NATION_MENU = "HideAmmuNationMenu";

        /// <summary>
        /// Кланы
        /// </summary>
        public const string SHOW_CLAN_MENU = "ShowClanMenu";
        public const string HIDE_CLAN_MENU = "HideClanMenu";
        public const string SHOW_CLAN_LEADER_MENU = "ShowClanLeaderMenu";
        public const string HIDE_CLAN_LEADER_MENU = "HideClanLeaderMenu";
        public const string SHOW_CLAN_VANS_MENU = "ShowClanVansMenu";
        public const string HIDE_CLAN_VANS_MENU = "HideClanVansMenu";

        /// <summary>
        /// Клановые задания
        /// </summary>
        public const string SHOW_CLAN_QUEST_TARGET = "ShowClanQuestTarget";
        public const string SHOW_CLAN_QUEST_END_POINT = "ShowClanQuestEndPoint";
        public const string HIDE_CLAN_QUEST_POINTS = "HideClanQuestPoints";

        /// <summary>
        /// Тюнинг
        /// </summary>
        public const string SHOW_TUNING_MENU = "ShowTuningMenu";
        public const string HIDE_TUNING_MENU = "HideTuningMenu";

        /// <summary>
        /// Рыбаки
        /// </summary>
        public const string SHOW_FISHERMAN_MENU = "ShowFishermansMenu";
        public const string HIDE_FISHERMAN_MENU = "HideFishermansMenu";
        public const string SHOW_FISHERMAN_POINT = "ShowFishermanPoint";
        public const string HIDE_FISHERMAN_POINT = "HideFishermanPoint";

        /// <summary>
        /// Фермеры
        /// </summary>
        public const string SHOW_FARM_MENU = "ShowFarmMenu";
        public const string HIDE_FARM_MENU = "HideFarmMenu";
        public const string SHOW_FARMER_POINT = "ShowFarmerPoint";
        public const string HIDE_FARMER_POINT = "HideFarmerPoint";
        public const string SHOW_FARMER_END_POINT = "ShowFarmerEndPoint";
        public const string HIDE_FARMER_END_POINT = "HideFarmerEndPoint";

        /// <summary>
        /// Трактористы
        /// </summary>
        public const string SHOW_TRACTOR_POINT = "ShowTractorPoint";
        public const string HIDE_TRACTOR_POINT = "HideTractorPoint";
        public const string SHOW_LOADED_TRAILER = "ShowLoadedTrailer";
        public const string HIDE_LOADED_TRAILER = "HideLoadedTrailer";
        public const string SHOW_HARVEST_DELIVERY_POINT = "ShowHarvestDeliveryPoint";
        public const string HIDE_HARVEST_DELIVERY_POINT = "HideHarvestDeliveryPoint";
    }
}