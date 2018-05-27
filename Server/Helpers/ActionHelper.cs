using System;
using System.Threading;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using Timer = System.Timers.Timer;
// ReSharper disable All

namespace gta_mp_server.Helpers {
    /// <summary>
    /// Вспомогательный класс для обработки различных событий
    /// </summary>
    internal class ActionHelper : Script {
        private const string CANCEL_ACTION_KEY = "CancelPlayerAction";

        internal static Random Random = new Random();

        /// <summary>
        /// Создает и запускает таймер переданного действия
        /// </summary>
        public static Timer StartTimer(int timeout, Action action) {
            var timer = new Timer(timeout) {AutoReset = true};
            timer.Elapsed += (sender, args) => action();
            timer.Start();
            return timer;
        }

        /// <summary>
        /// Создает и запускает таймер переданного действия
        /// </summary>
        public static Timer StartTimer(int timeout, Action action, Func<bool> predicate) {
            var timer = new Timer(timeout) { AutoReset = true };
            timer.Elapsed += (sender, args) => action();
            timer.Start();
            while (timer.Enabled) {
                if (predicate()) {
                    timer.Dispose();
                }
            }
            return timer;
        }

        /// <summary>
        /// Выполнить действие после паузы
        /// </summary>
        public static async void SetAction(int timeout, Action action) {
            await Task.Delay(timeout);
            action();
        }

        /// <summary>
        /// Выполнить действие после паузы
        /// </summary>
        public static async void SetAction(Client player, int timeout, Action action, string cancelKey = CANCEL_ACTION_KEY) {
            var source = new CancellationTokenSource();
            player.setData(cancelKey, source);
            await Task.Run(
                async () => {
                    try {
                        await Task.Delay(timeout, source.Token);
                        action();
                    }
                    catch (Exception) {/* ignore */}
                });
        }

        /// <summary>
        /// Отменить выполнение действия 
        /// </summary>
        public static void CancelAction(Client player, string cancelKey = CANCEL_ACTION_KEY) {
            if (!player.hasData(cancelKey)) return;
            var source = (CancellationTokenSource) player.getData(cancelKey);
            source.Cancel();
            player.resetData(cancelKey);
        }
    }
}