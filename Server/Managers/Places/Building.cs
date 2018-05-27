using gta_mp_server.Constant;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Managers.Places.Interfaces;
using gta_mp_server.Managers.Work.Builder.Interfaces;

namespace gta_mp_server.Managers.Places {
    /// <summary>
    /// Стройка
    /// </summary>
    internal class Building : Place {
        private readonly IPointCreator _pointCreator;
        private readonly IBuilderManager _builderManager;

        public Building() {}
        public Building(IPointCreator pointCreator, IBuilderManager builderManager) {
            _pointCreator = pointCreator;
            _builderManager = builderManager;
        }

        /// <summary>
        /// Инизиализировать место
        /// </summary>
        public override void Initialize() {
            _pointCreator.CreateBlip(MainPosition.Building, 475, 56, name: "Стройка");
            _builderManager.Initialize();
        }
    }
}