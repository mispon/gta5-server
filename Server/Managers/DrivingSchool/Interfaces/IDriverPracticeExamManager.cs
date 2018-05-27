using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.DrivingSchool.Interfaces {
    internal interface IDriverPracticeExamManager {
        /// <summary>
        /// Начать экзамен
        /// </summary>
        void Start(Client player);

        /// <summary>
        /// Завершить экзамен
        /// </summary>
        void Finish(Client player, bool success);
    }
}