using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Fun {
    /// <summary>
    /// Вспомогательный класс для создания зоны поединков
    /// </summary>
    internal class FightsBoxCreator {
        /// <summary>
        /// Создает коробку для бойцов
        /// </summary>
        internal static void CreateBox() {
            var rotation = new Vector3(0.00, 0.00, -88.37);
            API.shared.createObject(1035808898, new Vector3(-9.96, -1240.57, 33.80), rotation);
            API.shared.createObject(1035808898, new Vector3(-9.96, -1230.57, 33.80), rotation);
            CreateWalls(rotation);
            CreateInvisibleWalls(rotation);
        }

        /// <summary>
        /// Создает небольшие стены
        /// </summary>
        private static void CreateWalls(Vector3 rotation) {
            API.shared.createObject(-709723927, new Vector3(-0.46, -1248.57, 28.90), rotation);
            API.shared.createObject(-345463719, new Vector3(-0.46, -1240.47, 28.30), rotation);
            API.shared.createObject(-345463719, new Vector3(-0.46, -1230.77, 28.30), rotation);
            API.shared.createObject(-709723927, new Vector3(-0.46, -1222.33, 28.90), rotation);
            API.shared.createObject(-709723927, new Vector3(-18.68, -1248.57, 28.90), rotation);
            API.shared.createObject(-345463719, new Vector3(-18.68, -1230.96, 28.30), rotation);
            API.shared.createObject(-345463719, new Vector3(-18.68, -1240.37, 28.30), rotation);
            API.shared.createObject(-709723927, new Vector3(-18.98, -1222.45, 28.90), rotation);
        }

        /// <summary>
        /// Создает большие невидимые стены
        /// </summary>
        private static void CreateInvisibleWalls(Vector3 rotation) {
            var walls = new List<Object> {
                API.shared.createObject(2108146567, new Vector3(-0.46, -1248.57, 29.80), rotation),
                API.shared.createObject(779917859, new Vector3(-0.46, -1240.57, 29.80), rotation),
                API.shared.createObject(779917859, new Vector3(-0.46, -1230.60, 29.80), rotation),
                API.shared.createObject(779917859, new Vector3(-0.46, -1222.33, 29.80), rotation),
                API.shared.createObject(2108146567, new Vector3(-18.68, -1247.90, 29.80), rotation),
                API.shared.createObject(779917859, new Vector3(-18.68, -1230.96, 29.80), rotation),
                API.shared.createObject(779917859, new Vector3(-18.68, -1240.37, 29.80), rotation),
                API.shared.createObject(779917859, new Vector3(-18.68, -1221.45, 29.80), rotation),
                API.shared.createObject(2108146567, new Vector3(-0.46, -1247.57, 32.00), rotation),
                API.shared.createObject(779917859, new Vector3(-4.30, -1250.22, 31.00), new Vector3(3.00, 0.00, 177.00)),
                API.shared.createObject(779917859, new Vector3(-4.30, -1250.22, 33.20), new Vector3(21.00, 0.00, 177.00)),
                API.shared.createObject(779917859, new Vector3(-10.72, -1249.72, 32.50), new Vector3(5.00, 0.00, 177.00)),
                API.shared.createObject(779917859, new Vector3(-16.00, -1249.52, 33.00), new Vector3(3.00, 0.00, 175.00))
            };
            foreach (var wall in walls) {
                API.shared.setEntityTransparency(wall, 0);
            }
        }
    }
}