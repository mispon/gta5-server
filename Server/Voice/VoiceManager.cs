using gta_mp_server.Global;
using gta_mp_server.Voice.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared.Gta.Tasks;
using JustAnotherVoiceChat.Server.GTMP.Extensions;
using JustAnotherVoiceChat.Server.GTMP.Factories;
using JustAnotherVoiceChat.Server.GTMP.Interfaces;
using JustAnotherVoiceChat.Server.Wrapper.Elements.Models;
using JustAnotherVoiceChat.Server.Wrapper.Enums;

namespace gta_mp_server.Voice {
    public class VoiceManager : Script, IVoiceManager {
        private const int VOICE_RANGE = 15;
        private const string VOICE_HANDSHAKE = "VoiceSetHandshake";
        private const string VOICE_ROTATION = "ChangeVoiceRotation";

        internal static IGtmpVoiceServer VoiceServer;

        public VoiceManager() {
            API.onResourceStart += OnStart;
            API.onResourceStop += OnStop;
        }

        /// <summary>
        /// Инициализация голосового чата
        /// </summary>
        private void OnStart() {
            var teamSpeakConfig = new VoiceServerConfiguration("194.87.101.165", 23332, "8nwm0jHKWfrtSCTc1JYXsded55A=", 3, "GtaVGrimeVoice"); // prod
            //var teamSpeakConfig = new VoiceServerConfiguration("localhost", 23332, "Av0yWcX9rmJg6QWS1Hw1NG7HNK0=", 3, "qwe123"); // local
            VoiceServer = GtmpVoice.CreateServer(API, teamSpeakConfig);
            VoiceServer.AddTask(new PositionalTask());
            AttachToVoiceServerEvents();
            ClientEventHandler.Add(VOICE_ROTATION, (client, args) => client.SetVoiceRotation((float) args[0]));
            VoiceServer.Start();
        }

        /// <summary>
        /// Остановка голосового чата
        /// </summary>
        private void OnStop() {
            VoiceServer.Stop();
            VoiceServer.Dispose();
        }

        /// <summary>
        /// Привязка обработчиков голосового сервера
        /// </summary>
        private void AttachToVoiceServerEvents() {
            VoiceServer.OnClientPrepared += OnHandshakeShouldResend;
            VoiceServer.OnClientConnected += OnClientConnected;
            VoiceServer.OnClientRejected += OnClientRejected;
            VoiceServer.OnClientDisconnected += OnHandshakeShouldResend;
            VoiceServer.OnClientTalkingChanged += OnPlayerTalkingChanged;
            VoiceServer.OnClientMicrophoneMuteChanged += (client, newStatus) => {
                client.Player.sendChatMessage($"~b~[Voice]: ~s~Вы {(newStatus ? "~r~выключили" : "~g~включили")} ~s~свой микрофон");
            };
            VoiceServer.OnClientSpeakersMuteChanged += (client, newStatus) => {
                client.Player.sendChatMessage($"~b~[Voice]: ~s~Вы {(newStatus ? "~r~выключили" : "~g~включили")} ~s~свои динамики");
            };
        }

        /// <summary>
        /// Обработчик подключения игрока к голосовому каналу
        /// </summary>
        private void OnClientConnected(IGtmpVoiceClient client) {
            client.SetVoiceRange(VOICE_RANGE);
            client.Player.triggerEvent(VOICE_HANDSHAKE, false);
            client.SetNickname(client.Player.name);
        }

        /// <summary>
        /// Кикает игрока при отсутствии возможности подключения к TeamSpeak'у
        /// </summary>
        private void OnClientRejected(IGtmpVoiceClient client, StatusCode statusCode) {
            client.Player.sendChatMessage("~b~Для активации голосового чата, подключитесь к серверу: 194.87.101.165:9987");
        }

        /// <summary>
        /// Обработчик соединения с голосовым каналом комнаты в TeamSpeak
        /// </summary>
        private void OnHandshakeShouldResend(IGtmpVoiceClient client) {
            client.Player.triggerEvent(VOICE_HANDSHAKE, true, client.HandshakeUrl);
        }

        /// <summary>
        /// Обработчик анимации говорящего игрока
        /// </summary>
        private void OnPlayerTalkingChanged(IGtmpVoiceClient speakingClient, bool newStatus) {
            if (newStatus) {
                API.playPlayerAnimation(speakingClient.Player, (int) (AnimationFlag.Loop | AnimationFlag.AllowRotation), "mp_facial", "mic_chatter");
            }
            else {
                API.stopPlayerAnimation(speakingClient.Player);
            }
        }

        [Command("mute")]
        public void MuteMe(Client sender, bool status) {
            sender.MuteForAll(status);
            sender.sendChatMessage($"You have been {(status ? "muted" : "unmuted")} for all.");
        }
    }
}