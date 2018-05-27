namespace gta_mp_server.Constant {
    public class ClientEvent {
        /// <summary>
        /// Меню админа
        /// </summary>
        public const string GET_POSITION = "GetPosition";
        public const string GET_ROTATION = "GetRotation";
        public const string GET_CAR = "GetCar";
        public const string GET_WEAPON = "GetWeapon";
        public const string ADMIN_TELEPORT = "Teleport";

        /// <summary>
        /// Авторизация
        /// </summary>
        public const string PLAYER_LOGIN = "Login";
        public const string PLAYER_REGISTER = "Register";

        /// <summary>
        /// События игрока
        /// </summary>
        public const string ON_PLAYER_MELEE_HIT = "OnPlayerMeleeHit";
        public const string ON_PLAYER_DAMAGED = "OnPlayerDamaged";
        public const string GIVE_ONLINE_GIFT = "GiveOnlineGift";

        /// <summary>
        /// Звуковые события
        /// </summary>
        public const string CAR_LOCK_SOUND = "CarLockSound";

        /// <summary>
        /// Меню действий игрока
        /// </summary>
        public const string TRIGGER_PLAYER_ACTION_MENU = "TriggerPlayerActionMenu";
        public const string PLAY_PLAYER_ANIM = "PlayPlayerAnimation";
        public const string STOP_PLAYER_ANIM = "StopPlayerAnimation";
        public const string SEND_MONEY_TO_PLAYER = "SendMoneyToPlayer";
        public const string TELEPORT_TO_HOUSE = "TeleportToHouse";

        /// <summary>
        /// Управление машиной
        /// </summary>
        public const string TRIGGER_VEHICLE_ACTION_MENU = "TriggerVehicleActionMenu";
        public const string TURN_ENGINE = "TurnEngine";
        public const string LOCK_VEHICLE = "LockVehicle";
        public const string CHANGE_DOOR_STATE = "ChangeDoorState";
        public const string PUT_IN_TRUNK = "PutInTrunk";
        public const string TAKE_FROM_TRUNK = "TakeFromTrunk";
        public const string TRIGGER_VANS_TRUNK = "TriggerVansTrunk";

        /// <summary>
        /// Работа грузчиком
        /// </summary>
        public const string WORK_AS_LOADER = "LoaderWork";
        public const string LOADER_SALARY = "LoaderSalary";

        /// <summary>
        /// Работа на погрузчике
        /// </summary>
        public const string WORK_ON_FORKLIFT = "ForkliftWork";
        public const string FORKLIFT_SALARY = "ForkliftSalary";

        /// <summary>
        /// Работа на стройке
        /// </summary>
        public const string WORK_AS_BUILDER = "BuilderWork";
        public const string BUILDER_SALARY = "BuilderSalary";
        public const string PROCESS_BUILDER_POINT = "ProcessBuilderPoint";
        public const string FINISH_BUILDER_POINT = "FinishBuilderPoint";

        /// <summary>
        /// Автошкола
        /// </summary>
        public const string THEORY_EXAM = "TheoryExam";
        public const string FINISH_THEORY_EXAM = "FinishTheoryExam";
        public const string PRACTICE_EXAM = "PracticeExam";
        public const string FINISH_PRACTICE_EXAM = "FinishPracticeExam";
        public const string SHOW_NEXT_DRIVING_EXAM_POINT = "ShowNextDrivingExamPoint";
        public const string BRIBE_LICENCE = "BribeLicence";
        public const string TIME_OF_EXAM_WAS = "TimeOfExamWas";
        public const string DAMAGE_COUNT_EXCEEDED = "DamageCountExceeded";

        /// <summary>
        /// GPS
        /// </summary>
        public const string GPS_POINT = "GPSPoint";

        /// <summary>
        /// Водитель автобуса
        /// </summary>
        public const string SHOW_NEXT_BUS_STOP = "ShowNextBusStop";
        public const string HIDE_BUS_ROUTE = "HideBusRoute";
        public const string WORK_ON_BUS = "WorkOnBus";
        public const string BUS_DRIVER_SALARY = "BusDriverSalary";

        /// <summary>
        /// Таксист
        /// </summary>
        public const string WORK_IN_TAXI = "WorkInTaxi";
        public const string TAXI_DRIVER_SALARY = "TaxiDriverSalary";

        /// <summary>
        /// Полицейский
        /// </summary>
        public const string WORK_IN_POLICE = "WorkInPolice";
        public const string PAY_PENALTY = "PayPenalty";
        public const string BUY_WEAPON_LICENSE = "BuyWeaponLicense";
        public const string POLICE_SALARY = "PoliceSalary";
        public const string CHECK_PLAYER = "CheckPlayer";
        public const string GIVE_PENALTY = "GivePenalty";
        public const string FINISH_POLICE_ALERT = "FinishPoliceAlert";
        public const string ARREST_PLAYER = "ArrestPlayer";
        public const string PUT_IN_CAR = "PutPlayerInCar";
        public const string TAKE_FROM_CAR = "TakePlayerFromCar";
        public const string RELEASE_PLAYER = "ReleasePlayer";
        public const string GET_POLICE_MENU = "GetPoliceMenu";

        /// <summary>
        /// Дома
        /// </summary>
        public const string GET_HOUSE_RENT = "GetHouseRent";
        public const string ENTER_HOUSE = "EnterHouse";
        public const string EXIT_HOUSE = "ExitHouse";
        public const string LOCK_HOUSE = "LockHouse";
        public const string ENTER_GARAGE = "EnterGarage";
        public const string EXIT_GARAGE = "ExitGarage";
        public const string PUT_ITEM_TO_STORAGE = "PutItemToStorage";
        public const string TAKE_ITEM_TO_STORAGE = "TakeItemToStorage";

        /// <summary>
        /// Заправки
        /// </summary>
        public const string FILL_VEHICLE = "FillVehicle";
        public const string FILL_CANISTER = "FillCanister";
        public const string BUY_CANISTER = "BuyCanister";

        /// <summary>
        /// Магазины
        /// </summary>
        public const string BUY_FOOD = "BuyFood";
        public const string BUY_THING = "BuyThing";

        /// <summary>
        /// Аренда скутеров
        /// </summary>
        public const string RENT_SCOOTER = "RentScooter";

        /// <summary>
        /// Лечение медсестры
        /// </summary>
        public const string RESTORE_HEALTH = "RestoreHealth";

        /// <summary>
        /// Интерфейс
        /// </summary>
        public const string GET_PLAYER_INFO = "GetPlayerInfo";
        public const string SAVE_SETTINGS = "SaveSettings";

        /// <summary>
        /// Настройки внешности персонажа
        /// </summary>
        public const string SAVE_CHARACTER = "SaveCharacter";

        /// <summary>
        /// Покупка / смена одежды
        /// </summary>
        public const string DRESS_OR_BUY_GOOD = "DressOrBuyGood";
        public const string DRESS_PLAYER_CLOTHES = "DressPlayerClothes";
        public const string GO_TO_DRESSING_ROOM = "GoToDressingRoom";
        public const string EXIT_FROM_DRESSING_ROOM = "ExitFromDressingRoom";

        /// <summary>
        /// Автосалон
        /// </summary>
        public const string ENTER_TO_VEHICLE_PREVIEW = "EnterToVehiclePreview";
        public const string EXIT_FROM_VEHICLE_PREVIEW = "ExitFromVehiclePreview";
        public const string BUY_VEHICLE = "BuyVehicle";
        public const string SELL_VEHICLE = "SellVehicle";
        
        /// <summary>
        /// Парковка
        /// </summary>
        public const string GET_VEHICLE_FROM_PARKING = "GetVehicleFromParking";
        public const string PARK_VEHICLE = "ParkVehicle";

        /// <summary>
        /// Закусочная
        /// </summary>
        public const string FOOD_TRUNK_DRIVER = "FoodTrunkDriver";
        public const string FOOD_DELIVERYMAN = "FoodDeliveryman";
        public const string BISTRO_SALARY = "BistroSalary";
        public const string BUY_BISTRO_FOOD = "BuyBistroFood";
        public const string SET_DELIVERY_FAIL = "SetDeliveryFail";
        public const string COMPLETE_DELIVERY = "CompleteDelivery";

        /// <summary>
        /// Дальнобойщики
        /// </summary>
        public const string CHOOSE_TRUCKER_CONTRACT = "ChooseTruckerContract";
        public const string TRUCKER_SALARY = "TruckerSalary";

        /// <summary>
        /// Дальнобойщики
        /// </summary>
        public const string CHOOSE_PILOT_CONTRACT = "ChoosePilotContract";
        public const string PROCESS_PILOT_DELIVERY = "ProcessPilotDelivery";
        public const string PILOT_SALARY = "PilotSalary";

        /// <summary>
        /// Покупка парашюта
        /// </summary>
        public const string BUY_PARACHUTE = "BuyParachute";

        /// <summary>
        /// Штрафстоянка
        /// </summary>
        public const string GET_VEHICLE_FROM_PARKING_FINE = "GetVehicleFromParkingFine";
        public const string WRECKER = "Wrecker";
        public const string WRECKER_SALARY = "WreckerSalary";

        /// <summary>
        /// Игровые эвенты
        /// </summary>
        public const string EVENT_PARTICIPATION = "EventParticipation";
        public const string START_EVENT = "StartEvent";

        /// <summary>
        /// Гонки
        /// </summary>
        public const string REGISTER_ON_RACE = "RegisterOnRace";
        public const string REGISTER_ON_RACE_WITH_RENT = "RegisterOnRaceWithRent";
        public const string CANCEL_RACE = "CancelRace";

        /// <summary>
        /// Уличные драки
        /// </summary>
        public const string REGISTER_ON_FIGHTING = "RegisterOnFighting";
        public const string CANCEL_REGISTER_ON_FIGHTING = "CancelRegisterOnFighting";

        /// <summary>
        /// Инвентарь
        /// </summary>
        public const string SHOW_INVENTORY = "ShowInventory";
        public const string USE_INVENTORY_ITEM = "UseInventoryItem";

        /// <summary>
        /// Магазин оружия
        /// </summary>
        public const string BUY_WEAPON = "BuyWeapon";
        public const string BUY_AMMO = "BuyAmmo";

        /// <summary>
        /// Кланы
        /// </summary>
        public const string JOIN_CLAN = "JoinClan";
        public const string LEFT_CLAN = "LeftClan";
        public const string CAPTURE_DISTRICT = "CaptureDistrict";
        public const string JOIN_CLAN_MISSION = "JoinClanMission";
        public const string LEFT_CLAN_MISSION = "LeftClanMission";
        public const string SPAWN_MISSION_VANS = "SpawnMissionVans";
        public const string ACCEPT_CLAN_QUEST = "AcceptClanQuest";
        public const string FINISH_RACKET_POINT = "FinishRacketPoint";
        public const string FINISH_DRUG_DELIVERY_POINT = "FinishDrugDeliveryPoint";
        public const string ASK_CLAN_QUEST_KEY = "AskClanQuestKey";
        public const string CHANGE_INTO_POLICE = "ChangeIntoPolice";
        public const string FINISH_CASE_FALSIFICATION = "FinishCaseFalsification";

        /// <summary>
        /// Тюнинг
        /// </summary>
        public const string SET_VEHICLE_MOD = "SetVehicleMod";
        public const string SET_NEON = "SetNeon";
        public const string SET_ENGINE_POWER = "SetEnginePower";
        public const string SET_VEHICLE_COLOR = "SetVehicleColor";
        public const string REPAIR_VEHICLE = "RepairVehicle";
        public const string EXIT_FROM_TUNING_GARAGE = "ExitFromTuningGarage";

        /// <summary>
        /// Телефон
        /// </summary>
        public const string GET_PHONE_INFO = "GetPhoneInfo";
        public const string START_CALL = "StartCall";
        public const string ANSWER_CALL = "AnswerCall";
        public const string HANGUP_CALL = "HangupCall";
        public const string ADD_PHONE_CONTACT = "AddPhoneContact";
        public const string REPLENISH_PHONE_BALANCE = "ReplenishPhoneBalance";
        public const string BUY_PHONE = "BuyPhone";
        public const string TRIGGER_PHONE_VISIBLE = "TriggerPhoneVisible";

        /// <summary>
        /// Работа рыбаком
        /// </summary>
        public const string WORK_AS_FISHERMAN = "WorkAsFisherman";
        public const string BUY_FISH_BAITS = "BuyFishBaits";
        public const string FISHERMAN_SALARY = "FishermanSalary";
        public const string PROCESS_FISHERMAN_POINT = "ProcessFishermanPoint";
        public const string FINISH_FISHERMAN_POINT = "FinishFishermanPoint";

        /// <summary>
        /// Работа рыбаком
        /// </summary>
        public const string WORK_AS_FARMER = "WorkAsFarmer";
        public const string WORK_AS_TRACTOR_DRIVER = "WorkAsTractorDriver";
        public const string FARMER_SALARY = "FarmerSalary";
        public const string PROCESS_FARMER_POINT = "ProcessFarmerPoint";
    }
}