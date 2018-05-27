using System;
using System.Collections.Generic;
using System.Linq;
using gta_mp_database.Models.Player;
using gta_mp_server.Constant;
using gta_mp_server.Helpers;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Police.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Work.Police {
    /// <summary>
    /// Логика тюремного заключения
    /// </summary>
    internal class JailManager : Script, IJailManager {
        private const string CHAT = "[Тюрьма]";
        private const int CHECK_PRISONERS_TIMEOUT = 60000;
        private const float JAIL_DOOR_RANGE = 2f;

        private readonly HashSet<Client> _prisoners = new HashSet<Client>();
        /// <summary>
        /// Позиции тюрьмы
        /// Item1 - дверь
        /// Item2 - центр камеры
        /// </summary>
        private readonly List<Tuple<Vector3, Vector3>> _jailPositions = new List<Tuple<Vector3, Vector3>> {
            new Tuple<Vector3, Vector3>(new Vector3(461.81, -994.41, 25.1), new Vector3(460.34, -994.20, 24.91)),
            new Tuple<Vector3, Vector3>(new Vector3(461.81, -997.66, 25.1), new Vector3(459.50, -997.81, 24.91)),
            new Tuple<Vector3, Vector3>(new Vector3(461.81, -1001.3, 25.1), new Vector3(459.67, -1001.44, 24.91))
        };

        private readonly IDoormanager _doormanager;
        private readonly IPoliceManager _policeManager;
        private readonly IPoliceRewardManager _policeRewardManager;
        private readonly IPlayerInfoManager _playerInfoManager;

        public JailManager() {}
        public JailManager(IDoormanager doormanager, IPoliceManager policeManager,
            IPoliceRewardManager policeRewardManager, IPlayerInfoManager playerInfoManager) {
            _doormanager = doormanager;
            _policeManager = policeManager;
            _policeRewardManager = policeRewardManager;
            _playerInfoManager = playerInfoManager;
        }

        /// <summary>
        /// Инициализировать тюрьму
        /// </summary>
        public void Initialize() {
            foreach (var position in _jailPositions) {
                var doorId = _doormanager.Register(631614199, position.Item1);
                _doormanager.SetDoorState(doorId, true, 0);
                API.createSphereColShape(position.Item1, JAIL_DOOR_RANGE).onEntityEnterColShape += (shape, entity) => {
                    OnPutPrisonerInJail(entity, position.Item2);
                };
            }
            ActionHelper.StartTimer(CHECK_PRISONERS_TIMEOUT, CheckPrisoners);
        }

        /// <summary>
        /// Помещает нарушителя за решетку
        /// </summary>
        public void PutPrisonerInJail(Client policeman, Client prisoner, bool copSuccess = true, Vector3 jailPosition = null) {
            _policeManager.DetachPrisoner(policeman, prisoner, true);
            SetPolicemanReward(policeman, prisoner, copSuccess);
            UpdatePrisonerInfo(prisoner);
            _playerInfoManager.ClearWanted(prisoner);
            SetInJail(prisoner, jailPosition);
        }

        /// <summary>
        /// Заключенный игрок заходит в игру
        /// </summary>
        public void SetInJail(Client player, Vector3 jailPosition = null) {
            var position = jailPosition ?? GetFreestRoom();
            API.setEntityPosition(player, position);
            _prisoners.Add(player);
        }

        /// <summary>
        /// Заключенный игрок входит в игру
        /// </summary>
        public void OnPrisonerEnter(Client player) {
            _prisoners.Add(player);
        }

        /// <summary>
        /// Заключенный игрок выходит из игры
        /// </summary>
        public void OnPrisonerExit(Client player) {
            _prisoners.Remove(player);
        }

        /// <summary>
        /// Проверяет время ареста заключенных
        /// </summary>
        private void CheckPrisoners() {
            var playersToRelease = new List<Client>();
            foreach (var prisoner in _prisoners) {
                var info = _playerInfoManager.GetInfo(prisoner);
                if (info.Wanted.WantedLevel > 0) {
                    var timeToAdd = CalculateJailTime(info);
                    info.Wanted.JailTime += timeToAdd;
                    _playerInfoManager.ClearWanted(prisoner);
                    API.sendNotificationToPlayer(prisoner, $"~b~{CHAT} Добавлено время за плохое поведение: {info.Wanted.JailTime} мин.");
                }
                info.Wanted.JailTime -= 1;
                _playerInfoManager.RefreshUI(prisoner, info);
                if (info.Wanted.JailTime > 0) {
                    API.sendNotificationToPlayer(prisoner, $"~b~{CHAT} До освобождения осталось: {info.Wanted.JailTime} мин.");
                    continue;
                }
                playersToRelease.Add(prisoner);
            }
            ReleasePrisoner(playersToRelease);
        }

        /// <summary>
        /// Обработчик входа в тюремную камеру
        /// </summary>
        private void OnPutPrisonerInJail(NetHandle entity, Vector3 jailPosition) {
            var policeman = API.getPlayerFromHandle(entity);
            if (policeman == null || policeman.getData(WorkData.IS_POLICEMAN) == null) {
                return;
            }
            var prisoner = _policeManager.GetAttachedPlayer(policeman);
            if (prisoner == null) {
                return;
            }
            PutPrisonerInJail(policeman, prisoner, true, jailPosition);
        }

        /// <summary>
        /// Выдает награду полицейскому
        /// </summary>
        private void SetPolicemanReward(Client policeman, Client prisoner, bool copSuccess) {
            if (copSuccess) {
                _policeRewardManager.SetEffortReward(policeman, prisoner);
            }
            else {
                API.sendNotificationToPlayer(policeman, "~r~Разыскиваемый игрок погиб");
                _policeRewardManager.SetPatrolReward(policeman);
            }
        }

        /// <summary>
        /// Обновляет данные заключенного
        /// </summary>
        private void UpdatePrisonerInfo(Client prisoner) {
            var info = _playerInfoManager.GetInfo(prisoner);
            info.Wanted.Jails += 1;
            info.Wanted.JailTime = CalculateJailTime(info);
            // todo: Отобрать всё оружие
            // todo: если у игрока было оружие, сообщить:
            //API.sendNotificationToPlayer(prisoner, "~b~Ваше оружие изъято");
            _playerInfoManager.RefreshUI(prisoner, info);
            API.sendNotificationToPlayer(prisoner, $"~b~{CHAT} Срок вашего заключения: {info.Wanted.JailTime} мин.");
        }

        /// <summary>
        /// Вычисляет срок заключения
        /// </summary>
        private static int CalculateJailTime(PlayerInfo info) {
            var result = Math.Round(info.Wanted.WantedLevel * 0.5f);
            return (int) result; 
        }

        /// <summary>
        /// Выпускает заключенного на свободу
        /// </summary>
        private void ReleasePrisoner(IEnumerable<Client> prisoners) {
            foreach (var prisoner in prisoners) {
                _prisoners.Remove(prisoner);
                API.setEntityPosition(prisoner, MainPosition.Jail);
                API.sendNotificationToPlayer(prisoner, $"~g~{CHAT} Срок вашего заключения истек");
            }
        }

        /// <summary>
        /// Возвращает менее заполненную камеру
        /// </summary>
        private Vector3 GetFreestRoom() {
            var jailPositions = _jailPositions.Select(e => e.Item2).ToList();
            var result = jailPositions.First();
            var countInLastRoom = int.MaxValue;
            foreach (var position in jailPositions) {
                var prisonersCount = API.getPlayersInRadiusOfPosition(4f, position).Count;
                if (prisonersCount >= countInLastRoom) {
                    continue;
                }
                result = position;
                countInLastRoom = prisonersCount;
            }
            return result;
        }
    }
}