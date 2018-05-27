using System.Linq;
using JustAnotherVoiceChat.Server.GTMP.Interfaces;
using JustAnotherVoiceChat.Server.Wrapper.Interfaces;

namespace gta_mp_server.Voice {
    public class PositionalTask : IVoiceTask<IGtmpVoiceClient> {
        private readonly int _sleepTime;

        public PositionalTask(int sleep = 125) {
            _sleepTime = sleep;
        }

        /// <summary>
        /// Отслежевание позиций игроков
        /// </summary>
        public int RunVoiceTask(IVoiceServer<IGtmpVoiceClient> server) {
            var playersPositions = server.GetClients().Select(player => 
                player.Player.vehicle == null 
                    ? player.MakeClientPosition() 
                    : player.MakeClientPosition(player.Player.vehicle.position, player.CameraRotation)
            );
            server.SetPlayerPositions(playersPositions);
            return _sleepTime;
        }

        public void Dispose() {}
    }
}