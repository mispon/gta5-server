using gta_mp_server.Constant;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Work.Loader.Interfaces;

namespace gta_mp_server.Managers.Places {
    /// <summary>
    /// Свалка металлолома
    /// Работа грузчиком
    /// </summary>
    internal class ScrapMetalDump : Place {
        private readonly IPointCreator _pointCreator;
        private readonly ILoaderManager _loaderManager;

        public ScrapMetalDump() {}
        public ScrapMetalDump(IPointCreator pointCreator, ILoaderManager loaderManager) {
            _pointCreator = pointCreator;
            _loaderManager = loaderManager;
        }

        /// <summary>
        /// Инизиализировать место
        /// </summary>
        public override void Initialize() {
            _pointCreator.CreateBlip(MainPosition.ScrapMetalDump, 478, 21, name: "Свалка металлолома");
            _loaderManager.Initialize();
        }
    }
}