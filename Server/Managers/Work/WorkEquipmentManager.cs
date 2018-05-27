using System;
using System.Collections.Generic;
using gta_mp_data.Enums;
using gta_mp_database.Models.Player;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Interfaces;
using gta_mp_server.Managers.Work.Police.Data;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.Work {
    /// <summary>
    /// Логика управления рабочим снаряжением игрока
    /// </summary>
    internal class WorkEquipmentManager : Script, IWorkEquipmentManager {
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IWorkInfoManager _workInfoManager;
        private readonly IGtaCharacter _gtaCharacter;

        public WorkEquipmentManager() {}
        public WorkEquipmentManager(IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager, IGtaCharacter gtaCharacter) {
            _playerInfoManager = playerInfoManager;
            _workInfoManager = workInfoManager;
            _gtaCharacter = gtaCharacter;
        }

        /// <summary>
        /// Устанавливает рабочее снаряжение игрока
        /// </summary>
        public void SetEquipment(Client player) {
            var activeWork = _workInfoManager.GetActiveWork(player);
            if (activeWork == null) {
                return;
            }
            var isMale = _playerInfoManager.IsMale(player);
            switch (activeWork.Type) {
                case WorkType.Loader:
                case WorkType.Forklift:
                case WorkType.Builder:
                    SetLoaderClothes(player, isMale);
                    break;
                case WorkType.BusDriver:
                case WorkType.Trucker:
                case WorkType.TaxiDriver:
                    SetDriverClothes(player, isMale);
                    break;
                case WorkType.Police:
                    SetPoliceEquipment(player, isMale, activeWork.Level);
                    break;
                case WorkType.FoodTrunk:
                    SetBistroClothes(player, isMale, false);
                    break;
                case WorkType.FoodDeliveryMan:
                    SetBistroClothes(player, isMale, true);
                    break;
                case WorkType.Wrecker:
                    SetWreckerClothes(player, isMale);
                    break;
                case WorkType.Pilot:
                    SetPilotEquipment(player, isMale);
                    break;
                case WorkType.Fisherman:
                    SetFishermanEquipment(player, isMale);
                    break;
                case WorkType.Farmer:
                case WorkType.TractorDriver:
                    SetFarmerEquipment(player, isMale);
                    break;
                default:
                    throw new ArgumentException("Неизвестный тип работы!");
            }
        }

        /// <summary>
        /// Одежда грузчика
        /// </summary>
        private void SetLoaderClothes(Client player, bool isMale) {
            var clothes = new List<ClothesModel> {
                new ClothesModel {
                    Slot = 11, Variation = isMale ? 237 : 74, Torso = isMale ? 126 : 84,
                    Undershirt = isMale ? 59 : 36, Texture = 0, IsClothes = true
                },
                new ClothesModel {Slot = 4, Variation = isMale ? 98 : 101, Texture = 0, IsClothes = true},
                new ClothesModel {Slot = 6, Variation = isMale ? 27: 26, Texture = 0, IsClothes = true}
            };
            _gtaCharacter.SetClothes(player, clothes);
        }

        /// <summary>
        /// Общая одежда для водителей такси, автобусов, дальнобойщиков
        /// </summary>
        private void SetDriverClothes(Client player, bool isMale) {
            var clothes = new List<ClothesModel> {
                new ClothesModel {Slot = 0, Variation = isMale ? 7 : 5, Texture = 0, IsClothes = false},
                new ClothesModel {
                    Slot = 11, Variation = isMale ? 6 : 69, Torso = isMale ? 14 : 0,
                    Undershirt = isMale ? 24 : 58, Texture = 0, IsClothes = true
                },
                new ClothesModel {Slot = 4, Variation = isMale ? 5 : 1, Texture = 0, IsClothes = true},
                new ClothesModel {Slot = 6, Variation = isMale ? 6 : 4, Texture = 0, IsClothes = true}
            };
            _gtaCharacter.SetClothes(player, clothes);
        }

        /// <summary>
        /// Одежда работников закусочной
        /// </summary>
        private void SetBistroClothes(Client player, bool isMale, bool needBag) {
            var clothes = new List<ClothesModel> {
                new ClothesModel {Slot = 0, Variation = isMale ? 45 : 44, Texture = 0, IsClothes = false},
                new ClothesModel {
                    Slot = 11, Variation = isMale ? 1 : 14, Torso = isMale ? 0 : 14,
                    Undershirt = isMale ? 57: 3, Texture = 4, IsClothes = true
                },
                new ClothesModel {Slot = 4, Variation = isMale ? 12 : 25, Texture = 0, IsClothes = true},
                new ClothesModel {Slot = 6, Variation = isMale ? 1: 3, Texture = 0, IsClothes = true}
            };
            if (needBag) {
                API.setPlayerClothes(player, 5, 45, 0);
            }
            _gtaCharacter.SetClothes(player, clothes);
        }

        /// <summary>
        /// Одежда эвакуаторщиков
        /// </summary>
        private void SetWreckerClothes(Client player, bool isMale) {
            var clothes = new List<ClothesModel> {
                new ClothesModel {Slot = 0, Variation = isMale ? 5 : 12, Texture = 0, IsClothes = false},
                new ClothesModel {
                    Slot = 11, Variation = isMale ? 111 : 103, Torso = isMale ? 4 : 3,
                    Undershirt = isMale ? 59 : 36, Texture = 0, IsClothes = true
                },
                new ClothesModel {Slot = 4, Variation = isMale ? 30 : 29, Texture = 0, IsClothes = true},
                new ClothesModel {Slot = 6, Variation = isMale ? 27: 26, Texture = 0, IsClothes = true}
            };
            _gtaCharacter.SetClothes(player, clothes);
        }

        /// <summary>
        /// Снаряжение полицеского
        /// </summary>
        private void SetPoliceEquipment(Client player, bool isMale, int workLevel) {
            var clothes = new List<ClothesModel> {
                new ClothesModel {
                    Variation = isMale ? 55 : 48, Slot = 11, Torso = isMale ? 30 : 31,
                    Undershirt = isMale ? 58 : 35, IsClothes = true
                },
                new ClothesModel {Variation = isMale ? 46 : 45, Slot = 0, IsClothes = false},
                new ClothesModel {Variation = isMale ? 35 : 34, Slot = 4, IsClothes = true},
                new ClothesModel {Variation = 25, Slot = 6, IsClothes = true}
            };
            _gtaCharacter.SetClothes(player, clothes);
            API.removeAllPlayerWeapons(player);
            foreach (var weaponHash in PoliceDataGetter.Ammo[workLevel]) {
                API.givePlayerWeapon(player, weaponHash, 400, false);
            }
            API.sendNotificationToPlayer(player, "~b~Вам выдано табельное оружие");
        }

        /// <summary>
        /// Снаряжение летчика
        /// </summary>
        private void SetPilotEquipment(Client player, bool isMale) {
            var clothes = new List<ClothesModel> {
                new ClothesModel {
                    Variation = isMale ? 54 : 47, Slot = 11, Torso = isMale ? 1 : 3,
                    Undershirt = isMale ? 57 : 3, IsClothes = true
                },
                new ClothesModel {Variation = isMale ? 47 : 37, Slot = 0, IsClothes = false},
                new ClothesModel {Variation = isMale ? 41 : 42, Slot = 4, IsClothes = true},
                new ClothesModel {Variation = 24, Slot = 6, IsClothes = true}
            };
            _gtaCharacter.SetClothes(player, clothes);
        }

        /// <summary>
        /// Снаряжение рыбака
        /// </summary>
        private void SetFishermanEquipment(Client player, bool isMale) {
            var clothes = new List<ClothesModel> {
                new ClothesModel {
                    Variation = isMale ? 244 : 252, Slot = 11, Torso = isMale ? 14 : 5,
                    Undershirt = isMale ? 57 : 3, IsClothes = true
                },
                new ClothesModel {Variation = isMale ? 94 : 22, Slot = 0, IsClothes = false},
                new ClothesModel {Variation = isMale ? 33 : 32, Slot = 4, IsClothes = true},
                new ClothesModel {Variation = isMale ? 50 : 56, Slot = 6, IsClothes = true}
            };
            _gtaCharacter.SetClothes(player, clothes);
        }

        /// <summary>
        /// Снаряжение фермера
        /// </summary>
        private void SetFarmerEquipment(Client player, bool isMale) {
            var clothes = new List<ClothesModel> {
                new ClothesModel {
                    Variation = isMale ? 56 : 23, Slot = 11, Torso = isMale ? 0 : 4,
                    Undershirt = isMale ? 57 : 3, IsClothes = true
                },
                new ClothesModel {Variation = isMale ? 14 : 21, Slot = 0, IsClothes = false},
                new ClothesModel {Variation = isMale ? 90 : 93, Slot = 4, IsClothes = true},
                new ClothesModel {Variation = isMale ? 50 : 56, Slot = 6, IsClothes = true}
            };
            _gtaCharacter.SetClothes(player, clothes);
        }
    }
}