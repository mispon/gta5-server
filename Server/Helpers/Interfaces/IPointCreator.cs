using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Color = gta_mp_server.Models.Utils.Color;
using Marker = gta_mp_server.Enums.Marker;

namespace gta_mp_server.Helpers.Interfaces {
    internal interface IPointCreator {
        /// <summary>
        /// Создать маркер
        /// </summary>
        PointResult CreateMarker(Marker type, Vector3 position, Color color, float range, string label = null, int dimention = 0);

        /// <summary>
        /// Создать НПС
        /// </summary>
        PointResult CreatePed(PedHash hash, string name, Vector3 pedPosition, Vector3 pedRotation, Vector3 markerPosition, Color markerColor, int dimention = 0);

        /// <summary>
        /// Создать НПС
        /// </summary>
        Ped CreatePed(PedHash hash, Vector3 position, Vector3 rotation, int dimension = 0);

        /// <summary>
        /// Создать метку на карте
        /// </summary>
        Blip CreateBlip(Vector3 position, int sprite, int color, int dimention = 0, float scale = 1, string name = "");
    }
}