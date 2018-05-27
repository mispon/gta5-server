using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using gta_mp_database.Models.Player;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.IoC;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Interfaces;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Ninject;
using Ninject.Syntax;

namespace gta_mp_server.Managers.Fun {
    /// <summary>
    /// Бойцовский клуб
    /// </summary>
    internal class StreetFights : Place {
        private const int MIN_LEVEL = 3;
        private const int BET = 100;
        private const int WINNER_EXP = 30;

        internal static Queue<Client> Members = new Queue<Client>();
        private static Client _firstFighter;
        private static Client _secondFighter;
        private static readonly Tuple<Vector3, Vector3> _firstPlayerPositions = 
            new Tuple<Vector3, Vector3>(new Vector3(-9.33, -1242.60, 29.50), new Vector3(0.00, 0.00, -4.14));
        private static readonly Tuple<Vector3, Vector3> _secondPlayerPositions = 
            new Tuple<Vector3, Vector3>(new Vector3(-8.22, -1228.80, 29.30), new Vector3(0.00, 0.00, 172.74));

        private readonly IGtaCharacter _gtaCharacter;
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IInventoryManager _inventoryManager;
        private readonly IWorkEquipmentManager _workEquipmentManager;

        public StreetFights() : this(ServerKernel.Kernel) {
            API.onPlayerRespawn += OnPlayerRespawn;
            API.onPlayerDeath += (player, killer, weapon) => HandleFinishEvent(player);
            API.onPlayerDisconnected += (player, reason) => HandleFinishEvent(player);
        }

        public StreetFights(IResolutionRoot kernel) {
            _gtaCharacter = kernel.Get<IGtaCharacter>();
            _playerInfoManager = kernel.Get<IPlayerInfoManager>();
            _inventoryManager = kernel.Get<IInventoryManager>();
            _workEquipmentManager = kernel.Get<IWorkEquipmentManager>();
        }

        /// <summary>
        /// Инизиализировать место
        /// </summary>
        public override void Initialize() {
            ClientEventHandler.Add(ClientEvent.REGISTER_ON_FIGHTING, (entity, args) => PlayerHelper.ProcessAction(entity, Register));
            ClientEventHandler.Add(ClientEvent.CANCEL_REGISTER_ON_FIGHTING, (entity, args) => PlayerHelper.ProcessAction(entity, CancelRegistration));
            ClientEventHandler.Add(ServerEvent.START_FIGHT, (entity, args) => PlayerHelper.ProcessAction(entity, player => player.freeze(false)));
            FightsBoxCreator.CreateBox();
        }

        /// <summary>
        /// Обработчик воскрешения игрока
        /// </summary>
        private static void OnPlayerRespawn(Client player) {
            if (!player.hasData(PlayerData.FIGHTER)) {
                return;
            }
            PlayerHelper.RestorePosition(player);
        }

        /// <summary>
        /// Обработчик смерти игрока
        /// </summary>
        private void HandleFinishEvent(Client player) {
            if (!player.hasData(PlayerData.FIGHTER)) {
                return;
            }
            var winner = GetWinner(player);
            Task.Run(() => Finish(winner));
        }

        /// <summary>
        /// Возвращает победителя
        /// </summary>
        private static Client GetWinner(Client player) {
            return player == _firstFighter ? _secondFighter : _firstFighter;
        }

        /// <summary>
        /// Регистрация в поединках
        /// </summary>
        private void Register(Client player) {
            if (Members.Contains(player)) {
                API.sendNotificationToPlayer(player, "~r~Вы уже зарегистрировались", true);
                return;
            }
            if (!TakeBet(player)) {
                return;
            }
            Members.Enqueue(player);
            player.setSyncedData(PlayerData.IS_REGISTERED, true);
            if (Members.Count > 1 && _firstFighter == null && _secondFighter == null) {
                Start();
            }
            API.sendNotificationToPlayer(player, "~b~Вы успешно зарегистрировались на поединок");
            API.triggerClientEvent(player, ServerEvent.HIDE_FIGHT_MENU);
        }

        /// <summary>
        /// Забирает у игрока ставку на поединок
        /// </summary>
        private bool TakeBet(Client player) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (playerInfo.Level < MIN_LEVEL) {
                API.sendNotificationToPlayer(player, "~r~Необходимо достигнуть 3-го уровня и выше", true);
                return false;
            }
            if (playerInfo.Balance < BET) {
                API.sendNotificationToPlayer(player, "~r~Недостаточно денег для взноса на поединок", true);
                return false;
            }
            playerInfo.Balance -= BET;
            API.sendNotificationToPlayer(player, $"~b~Списано {BET}$", true);
            _playerInfoManager.RefreshUI(player, playerInfo);
            return true;
        }

        /// <summary>
        /// Отмена регистрации в поединках
        /// </summary>
        private void CancelRegistration(Client player) {
            if (!Members.Contains(player)) {
                API.sendNotificationToPlayer(player, "~r~Вы не были зарегистрированы", true);
                return;
            }
            _playerInfoManager.SetBalance(player, BET);
            Members = RemoveMember(player);
            player.resetSyncedData(PlayerData.IS_REGISTERED);
            API.sendNotificationToPlayer(player, "~b~Регистрация отменена");
            API.triggerClientEvent(player, ServerEvent.HIDE_FIGHT_MENU);
        }

        /// <summary>
        /// Удаляет игрока из очереди
        /// </summary>
        private static Queue<Client> RemoveMember(Client player) {
            var result = new Queue<Client>();
            while (Members.Count > 0) {
                var member = Members.Dequeue();
                if (member != player) {
                    result.Enqueue(member);
                }
            }
            return result;
        }

        /// <summary>
        /// Запустить поединок
        /// </summary>
        private void Start() {
            _firstFighter = Members.Dequeue();
            _secondFighter = Members.Dequeue();
            PrepareFighter(_firstFighter, _firstPlayerPositions.Item1, _firstPlayerPositions.Item2);
            PrepareFighter(_secondFighter, _secondPlayerPositions.Item1, _secondPlayerPositions.Item2);
        }

        /// <summary>
        /// Устанавливает данные бойца
        /// </summary>
        private void PrepareFighter(Client player, Vector3 position, Vector3 rotation) {
            player.setData(PlayerData.FIGHTER, true);
            player.setData(PlayerData.LAST_POSITION, player.position);
            player.setData(PlayerData.LAST_DIMENSION, player.dimension);
            player.freeze(true);
            API.setEntityPosition(player, position);
            API.setEntityRotation(player, rotation);
            API.setPlayerHealth(player, PlayerInfo.MAX_VALUE);
            _gtaCharacter.SetClothes(player, GetFighterClothes(player));
            API.removeAllPlayerWeapons(player);
            API.triggerClientEvent(player, ServerEvent.SET_TIMER, 10, ServerEvent.START_FIGHT);
        }

        /// <summary>
        /// Возвращает одежду бойца
        /// </summary>
        private List<ClothesModel> GetFighterClothes(Client player) {
            var isMale = _playerInfoManager.IsMale(player);
            return new List<ClothesModel> {
                new ClothesModel {Slot = 0, Variation = isMale ? 11 : 57, Texture = 0, IsClothes = false},
                new ClothesModel {
                    Slot = 11, Variation = isMale ? 15 : 5, Torso = isMale ? 15 : 4,
                    Texture = 0, Undershirt = isMale ? 57 : 2, IsClothes = true
                },
                new ClothesModel {Slot = 4, Variation = isMale ? 55 : 44, Texture = 0, IsClothes = true},
                new ClothesModel {Slot = 6, Variation = isMale ? 7 : 27, Texture = 0, IsClothes = true},
            };
        }

        /// <summary>
        /// Завершить поединок
        /// </summary>
        private void Finish(Client winner) {
            Thread.Sleep(10000);
            _playerInfoManager.SetBalance(winner, BET * 2, true);
            _playerInfoManager.SetExperience(winner, WINNER_EXP);
            PlayerHelper.RestorePosition(winner);
            ResetData(_firstFighter);
            ResetData(_secondFighter);
            _playerInfoManager.SetPlayerClothes(_firstFighter);
            _playerInfoManager.SetPlayerClothes(_secondFighter);
            _firstFighter = null;
            _secondFighter = null;
            if (Members.Count > 1) {
                Start();
            }
        }

        /// <summary>
        /// Сбросить данные игрока
        /// </summary>
        private void ResetData(Client player) {
            player.resetData(PlayerData.FIGHTER);
            player.resetData(PlayerData.LAST_POSITION);
            player.resetData(PlayerData.LAST_DIMENSION);
            API.setPlayerHealth(player, PlayerInfo.MAX_VALUE);
            _inventoryManager.EquipWeapon(player);
            _workEquipmentManager.SetEquipment(player);
        }
    }
}