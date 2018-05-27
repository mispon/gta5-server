using System;
using gta_mp_database.Models.Player;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Managers.MenuHandlers.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.MenuHandlers {
    /// <summary>
    /// Обработчик пунктов в меню автошколы
    /// </summary>
    internal class DrivingSchoolMenuHandler : Script, IMenu {
        internal const string PRACTICE_EXAM_KEY = "OnPracticeExam";

        private readonly IPlayerInfoManager _playerInfoManager;

        public DrivingSchoolMenuHandler() { }
        public DrivingSchoolMenuHandler(IPlayerInfoManager playerInfoManager) {
            _playerInfoManager = playerInfoManager;
        }

        /// <summary>
        /// Инициализировать меню
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.THEORY_EXAM, TheoryExam);
            ClientEventHandler.Add(ClientEvent.PRACTICE_EXAM, PracticeExam);
            ClientEventHandler.Add(ClientEvent.BRIBE_LICENCE, BribeLicence);
            ClientEventHandler.Add(ClientEvent.FINISH_THEORY_EXAM, FinishTheoryExam);
        }

        /// <summary>
        /// Теоретический экзамен
        /// </summary>
        private void TheoryExam(Client player, object[] args) {
            var theoryPrice = (int) args[0];
            var info = _playerInfoManager.GetInfo(player);
            if (!CanTakeTheTheory(player, info, theoryPrice)) {
                return;
            }
            if (!info.Driver.TheoryExamPaid) {
                info.Balance -= theoryPrice;
                info.Driver.TheoryExamPaid = true;
                _playerInfoManager.RefreshUI(player, info);
                API.sendNotificationToPlayer(player, $"~b~Списано {theoryPrice}$");
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_ANDREAS_MENU);
            API.triggerClientEvent(player, ServerEvent.SHOW_THEORY_EXAM);
        }

        /// <summary>
        /// Практический экзамен
        /// </summary>
        private void PracticeExam(Client player, object[] args) {
            var practicePrice = (int) args[0];
            var info = _playerInfoManager.GetInfo(player);
            if (!CanTakeThePractice(player, info, practicePrice)) {
                return;
            }
            if (!info.Driver.PracticeExamPaid) {
                info.Balance -= practicePrice;
                info.Driver.PracticeExamPaid = true;
                _playerInfoManager.RefreshUI(player, info);
                API.sendNotificationToPlayer(player, $"~b~Списано {practicePrice}$");
            }
            player.setData(PRACTICE_EXAM_KEY, true);
            API.sendNotificationToPlayer(player, "~b~Чтобы начать экзамен, сядьте в машину на парковке", true);
            API.triggerClientEvent(player, ServerEvent.HIDE_ANDREAS_MENU);
        }

        /// <summary>
        /// Обработчик завершения теоретического экзамена
        /// </summary>
        private void FinishTheoryExam(Client player, object[] args) {
            var info = _playerInfoManager.GetInfo(player);
            info.Driver.PassedTheory = true;
            _playerInfoManager.RefreshUI(player, info);
            API.sendNotificationToPlayer(player, "~b~Поздравляем! Вы были допущены к практике");
        }

        /// <summary>
        /// Дать взятку и получить права
        /// </summary>
        private void BribeLicence(Client player, object[] args) {
            var info = _playerInfoManager.GetInfo(player);
            if (AlreadyDriver(player, info)) {
                return;
            }
            var bribe = (int) args[0];
            if (info.Balance < bribe) {
                API.sendNotificationToPlayer(player, "~r~Вам не хватает денег на взятку", true);
                return;
            }
            info.Balance -= bribe;
            info.Driver.TheoryExamPaid = true;
            info.Driver.PassedTheory = true;
            info.Driver.PracticeExamPaid = true;
            info.Driver.PassedPracticeB = true;
            _playerInfoManager.RefreshUI(player, info);
            API.sendNotificationToPlayer(player, "~g~Вы получили водительскую лицензию");
            API.triggerClientEvent(player, ServerEvent.HIDE_ANDREAS_MENU);
        }

        /// <summary>
        /// Проверяет, что игрок может сдавать теоретический экзамен
        /// </summary>
        private bool CanTakeTheTheory(Client player, PlayerInfo info, int price) {
            if (AlreadyDriver(player, info)) {
                return false;
            }
            if (info.Driver.PassedTheory) {
                API.sendNotificationToPlayer(player, "~r~Вы уже сдали теорию", true);
                return false;
            }
            if (!info.Driver.TheoryExamPaid && !EnoughMoney(player, info.Balance, price)) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Проверяет, что игрок может сдавать практический экзамен
        /// </summary>
        private bool CanTakeThePractice(Client player, PlayerInfo info, int price) {
            if (info.Level < 2) {
                API.sendNotificationToPlayer(player, "~r~Для получения лицензии необходим 2й уровень", true);
                return false;
            }
            if (AlreadyDriver(player, info)) {
                return false;
            }
            if (!info.Driver.PassedTheory) {
                API.sendNotificationToPlayer(player, "~r~Вы еще не сдали теоретический экзамен", true);
                return false;
            }
            if (!info.Driver.PracticeExamPaid && !EnoughMoney(player, info.Balance, price)) {
                return false;
            }
            if (info.Driver.TimeToNextTry > DateTime.Now) {
                API.sendNotificationToPlayer(player, $"~r~Следующая попытка станет доступна {info.Driver.TimeToNextTry:g}", true);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Проверяет, что у игрока достаточно денег
        /// </summary>
        private bool EnoughMoney(Client player, int balance, int price) {
            if (balance >= price) {
                return true;
            }
            API.sendNotificationToPlayer(player, "~r~Вам не хватает денег на оплату экзамена", true);
            return false;
        }

        /// <summary>
        /// Проверка, что у игрока еще нет лицензии
        /// </summary>
        private bool AlreadyDriver(Client player, PlayerInfo info) {
            if (!info.Driver.CanDriveB) {
                return false;
            }
            API.sendNotificationToPlayer(player, "~r~У вас уже есть лицензия", true);
            return true;
        }
    }
}