using gta_mp_database.Models.Player;

namespace gta_mp_database.Providers.Interfaces {
    public interface IPlayersAppearanceProvider {
        /// <summary>
        /// Сохраняет настройки внешнего вида игрока
        /// </summary>
        void Save(long accountId, PlayerAppearance model);

        /// <summary>
        /// Возвращает настройки внешнего вида игрока
        /// </summary>
        PlayerAppearance Get(long accountId);
    }
}