using gta_mp_server.Managers.Player.Interfaces;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.Player {
    /// <summary>
    /// Вспомогательный класс для оповещения о новых возможностях
    /// </summary>
    internal class OpportunitiesNotifier : Script, IOpportunitiesNotifier {
        /// <summary>
        /// Оповещает об открывшыхся возможностях
        /// </summary>
        public void Notify(Client player, int level) {
            switch (level) {
                case 2:
                    API.sendNotificationToPlayer(player, "~b~Открыта автошкола");
                    API.sendNotificationToPlayer(player, "~b~Доступна новая работа: Водитель погрузчика");
                    break;
                case 3:
                    API.sendNotificationToPlayer(player, "~b~Доступна новая работа: Строитель");
                    API.sendNotificationToPlayer(player, "~b~Доступна новая работа: Водитель такси");
                    API.sendNotificationToPlayer(player, "~b~Открыт эвент: Уличные драки");
                    break;
                case 4:
                    API.sendNotificationToPlayer(player, "~b~Доступна новая работа: Рыбак");
                    API.sendNotificationToPlayer(player, "~b~Доступна новая работа: Водитель автобуса");
                    break;
                case 5:
                    API.sendNotificationToPlayer(player, "~b~Доступны 2 новые работы в Закусочной");
                    API.sendNotificationToPlayer(player, "~b~Открыт эвент: Гонки");
                    break;
                case 6:
                    API.sendNotificationToPlayer(player, "~b~Открыта покупка лицензии на оружие");
                    API.sendNotificationToPlayer(player, "~b~Доступна новая работа: Эвакуаторщик");
                    break;
                case 7:
                    API.sendNotificationToPlayer(player, "~b~Доступна новая работа: Дальнобойщик");
                    break;
                case 8:
                    API.sendNotificationToPlayer(player, "~b~Доступна новая работа: Пилот");
                    break;
                case 9:
                    API.sendNotificationToPlayer(player, "~b~Доступна новая работа: Полицейский");
                    break;
                case 10:
                    API.sendNotificationToPlayer(player, "~b~Открыта возможность присоединиться к банде");
                    break;
            }
        }
    }
}