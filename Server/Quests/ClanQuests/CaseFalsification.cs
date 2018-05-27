using gta_mp_server.Clan.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Helpers;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Quests.Interfaces;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Quests.ClanQuests {
    /// <summary>
    /// Фальсификация полицейского дела
    /// </summary>
    internal class CaseFalsification : ClanQuest {
        private const string IN_WARDROBE = "InPoliceWardrobe";
        private const string IN_REPOSITORY = "InPoliceRepository";
        private const string REPOSITORY_KEY = "PoliceRepositoryKey";

        private static readonly Vector3 _wardrobe = new Vector3(456.44, -988.93, 29.69);
        private static readonly Vector3 _reseption = new Vector3(441.03, -981.27, 29.79);
        private static readonly Vector3 _documentRepository = new Vector3(459.67, -987.88, 23.91);

        private readonly IInventoryManager _inventoryManager;

        public CaseFalsification() {}
        public CaseFalsification(IPlayerInfoManager playerInfoManager, IClanManager clanManager, IInventoryManager inventoryManager)
            : base(playerInfoManager, clanManager) {
            _inventoryManager = inventoryManager;
        }

        /// <summary>
        /// Инициализирует квест
        /// </summary>
        public override void Initialize() {
            ClientEventHandler.Add(ClientEvent.ASK_CLAN_QUEST_KEY, AskQuestKey);
            ClientEventHandler.Add(ClientEvent.CHANGE_INTO_POLICE, ChangeIntoPolice);
            ClientEventHandler.Add(ClientEvent.FINISH_CASE_FALSIFICATION, FinishFalsification);
            CreateWardrobe();
            CreateRepository();
        }

        /// <summary>
        /// Переодеться в полицейского
        /// </summary>
        private void ChangeIntoPolice(Client player, object[] args) {
            player.resetSyncedData(IN_WARDROBE);
            API.setPlayerSkin(player, PedHash.Cop01SMY);
            API.triggerClientEvent(player, ServerEvent.SHOW_CLAN_QUEST_TARGET, _reseption, false);
            API.triggerClientEvent(player, ServerEvent.SHOW_SUBTITLE, "Достаньте ~y~ключ ~w~от шкафа");
        }

        /// <summary>
        /// Попросить ключ от хранилища документов
        /// </summary>
        private void AskQuestKey(Client player, object[] args) {
            var playerSkin = (PedHash) API.getEntityModel(player);
            if (playerSkin == PedHash.Cop01SMY) {
                API.sendChatMessageToPlayer(player, "~b~Сара: ~w~Я тебя раньше не видела...Новенький? Вот ключ, держи");
                player.setData(REPOSITORY_KEY, true);
                API.triggerClientEvent(player, ServerEvent.SHOW_CLAN_QUEST_TARGET, _documentRepository, true);
                API.triggerClientEvent(player, ServerEvent.SHOW_SUBTITLE, "Замените ~y~документы ~w~на подделку");
            }
            else {
                API.sendChatMessageToPlayer(player, "~b~Сара: ~w~С чего бы мне раздавать ключи кому попало?");
            }
            API.triggerClientEvent(player, ServerEvent.HIDE_POLICEMAN_MENU);
        }

        /// <summary>
        /// Обработчик подмены документов
        /// </summary>
        private void FinishFalsification(Client player, object[] args) {
            PlayActionAnimation(player);
            player.resetSyncedData(IN_REPOSITORY);
            player.resetData(REPOSITORY_KEY);
            player.resetData(IN_REPOSITORY);
            SetQuestReward(player);
            ActionHelper.SetAction(3000, () => {
                PlayerInfoManager.SetPlayerClothes(player, true);
                _inventoryManager.EquipWeapon(player);
            });
            API.triggerClientEvent(player, ServerEvent.HIDE_CLAN_QUEST_POINTS);
        }

        /// <summary>
        /// Направляет на место задания
        /// </summary>
        public override void ShowTarget(Client player) {
            player.setData(PlayerData.CLAN_QUEST, true);
            API.triggerClientEvent(player, ServerEvent.SHOW_CLAN_QUEST_TARGET, _wardrobe, true);
            API.triggerClientEvent(player, ServerEvent.SHOW_SUBTITLE, "Подмените ~y~документы ~w~в полиции");
        }

        /// <summary>
        /// Создает полицейскую раздевалку
        /// </summary>
        private void CreateWardrobe() {
            var wardrobe = API.createCylinderColShape(_wardrobe, 1.5f, 2f);
            wardrobe.onEntityEnterColShape += (shape, entity) => 
                PlayerHelper.ProcessAction(entity, player => player.setSyncedData(IN_WARDROBE, true));
            wardrobe.onEntityExitColShape += (shape, entity) =>
                PlayerHelper.ProcessAction(entity, player => player.resetSyncedData(IN_WARDROBE));
        }

        /// <summary>
        /// Создает хранилище документов
        /// </summary>
        private void CreateRepository() {
            var repository = API.createCylinderColShape(_documentRepository, 1.5f, 2f);
            repository.onEntityEnterColShape += (shape, entity) =>
                PlayerHelper.ProcessAction(entity, player => {
                    if (player.hasData(REPOSITORY_KEY)) {
                        player.setSyncedData(IN_REPOSITORY, true);
                    }
                });
            repository.onEntityExitColShape += (shape, entity) =>
                PlayerHelper.ProcessAction(entity, player => player.resetSyncedData(IN_REPOSITORY));
        }
    }
}