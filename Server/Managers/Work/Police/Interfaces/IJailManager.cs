using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Work.Police.Interfaces {
    public interface IJailManager {
        /// <summary>
        /// Инициализировать тюрьму
        /// </summary>
        void Initialize();

        /// <summary>
        /// Помещает нарушителя за решетку
        /// </summary>
        void PutPrisonerInJail(Client policeman, Client prisoner, bool copSuccess = true, Vector3 jailPosition = null);

        /// <summary>
        /// Заключенный игрок заходит в игру
        /// </summary>
        void SetInJail(Client player, Vector3 jailPosition = null);

        /// <summary>
        /// Заключенный игрок входит в игру
        /// </summary>
        void OnPrisonerEnter(Client player);

        /// <summary>
        /// Заключенный игрок выходит из игры
        /// </summary>
        void OnPrisonerExit(Client player);
    }
}