using System;
using gta_mp_server.Helpers.Interfaces;
using gta_mp_server.Models.Utils;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Color = gta_mp_server.Models.Utils.Color;
using Marker = gta_mp_server.Enums.Marker;

namespace gta_mp_server.Helpers {
    /// <summary>
    /// Хелпер создания маркеров
    /// </summary>
    internal class PointCreator : Script, IPointCreator {
        /// <summary>
        /// Создать маркер
        /// </summary>
        public PointResult CreateMarker(Marker type, Vector3 position, Color color, float range, string label = null, int dimention = 0) {
            var markerType = Convert.ToInt32(type);
            var result = new PointResult {
                ColShape = API.createCylinderColShape(position, range, 3f),
                Marker = API.createMarker(
                    markerType, position, new Vector3(), new Vector3(), new Vector3(1, 1, 0.7),
                    color.Bright, color.Red, color.Green, color.Blue, dimention
                )
            };
            if (label != null) {
                result.Label = API.createTextLabel(label, position.Add(new Vector3(0, 0, 1.2)), 10, 0.5F, dimension: dimention);
                result.Label.dimension = dimention;
            }
            if (dimention != 0) {
                result.ColShape.dimension = dimention;
                result.Marker.dimension = dimention;
            }
            return result;
        }

        /// <summary>
        /// Создать НПС
        /// </summary>
        public PointResult CreatePed(PedHash hash, string name, Vector3 pedPosition, Vector3 pedRotation,
            Vector3 markerPosition, Color markerColor, int dimention = 0) {
            var markerType = Convert.ToInt32(Marker.HorizontalSplitArrowCircle);
            var result = new PointResult {
                Npc = CreatePed(hash, pedPosition, pedRotation, dimention),
                Label = API.createTextLabel(name, pedPosition.Add(new Vector3(0, 0, 1.2)), 10, 0.5F),
                ColShape = API.createSphereColShape(markerPosition, 1.4F),
                Marker = API.createMarker(
                    markerType, markerPosition, new Vector3(), new Vector3(), new Vector3(1, 1, 1),
                    markerColor.Bright, markerColor.Red, markerColor.Green, markerColor.Blue
                )
            };
            if (dimention != 0) {
                result.ColShape.dimension = dimention;
                result.Label.dimension = dimention;
                result.Marker.dimension = dimention;
            }
            return result;
        }

        /// <summary>
        /// Создать НПС
        /// </summary>
        public Ped CreatePed(PedHash hash, Vector3 position, Vector3 rotation, int dimension = 0) {
            var ped = API.createPed(hash, position, 0, dimension);
            API.setEntityRotation(ped, rotation);
            API.setEntityPosition(ped, position);
            return ped;
        }

        /// <summary>
        /// Создать метку на карте
        /// </summary>
        public Blip CreateBlip(Vector3 position, int sprite, int color, int dimention = 0, float scale = 1, string name = "") {
            var result = API.createBlip(position, 0, dimention);
            API.setBlipColor(result, color);
            API.setBlipScale(result, scale);
            API.setBlipSprite(result, sprite);
            API.setBlipShortRange(result, true);
            if (!string.IsNullOrEmpty(name)) {
                API.setBlipName(result, name);
            }
            return result;
        }
    }
}