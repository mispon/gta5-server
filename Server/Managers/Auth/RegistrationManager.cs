using gta_mp_database.Providers.Interfaces;
using gta_mp_server.Constant;
using gta_mp_server.Global;
using gta_mp_server.Managers.Auth.Interfaces;
using gta_mp_server.Managers.Interface.Interfaces;
using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.Auth {
    /// <summary>
    /// Логика регистрации нового аккаунта
    /// </summary>
    internal class RegistrationManager : Script, IInterfaceManager {
        private readonly IAccountsProvider _accountsProvider;
        private readonly IPlayersProvider _playersProvider;
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly ICreatingCharManager _creatingCharManager;

        public RegistrationManager() {}
        public RegistrationManager(IAccountsProvider accountsProvider, IPlayersProvider playersProvider, 
            IPlayerInfoManager playerInfoManager, ICreatingCharManager creatingCharManager) {
            _accountsProvider = accountsProvider;
            _playersProvider = playersProvider;
            _playerInfoManager = playerInfoManager;
            _creatingCharManager = creatingCharManager;
        }

        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.PLAYER_REGISTER, PlayerRegister);
        }

        /// <summary>
        /// Обработчик регистрации нового игрока 
        /// </summary>
        private void PlayerRegister(Client player, object[] args) {
            var email = args[0].ToString();
            var password = args[1].ToString();
            var friendReferal = args[2].ToString();
            var createResult = _accountsProvider.Create(email, password, friendReferal);
            if (!createResult) {
                API.triggerClientEvent(player, ServerEvent.BAD_REGISTER);
                return;
            }
            var account = _accountsProvider.Get(email, password);
            _playersProvider.Add(account);
            var playerInfo = _playersProvider.GetInfo(account.Id);
            _playerInfoManager.Add(player, playerInfo);
            API.triggerClientEvent(player, ServerEvent.HIDE_AUTH);
            API.setEntityDimension(player, (int) -account.Id);
            _creatingCharManager.ShowCreator(player);
        }
    }
}