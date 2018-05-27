using System.Collections.Generic;
using System.Linq;
using gta_mp_server.Constant;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Managers.Places.Hospitals {
    public class HospitalHelper {
        /// <summary>
        /// Госпитали
        /// </summary>
        internal static List<HospitalInfo> Hospitals = new List<HospitalInfo> {
            new HospitalInfo {
                Position = MainPosition.Hospital,
                PositionAfterExit = new Vector3(339.76, -1395.61, 32.51),
                Dimension = Dimension.HOSPITAL_CITY
            },
            new HospitalInfo {
                Position = new Vector3(1838.96, 3673.51, 34.28),
                PositionAfterExit = new Vector3(1840.67, 3670.37, 33.68),
                Dimension = Dimension.HOSPITAL_COUNTRY_1
            },
            new HospitalInfo {
                Position = new Vector3(-243.95, 6325.43, 32.43),
                PositionAfterExit = new Vector3(-240.44, 6324.13, 32.43),
                Dimension = Dimension.HOSPITAL_COUNTRY_2
            }
        };

        /// <summary>
        /// Возвращает ближайший госпиталь
        /// </summary>
        internal static HospitalInfo GetNearestHospital(Vector3 playerPosition) {
            var result = Hospitals.First();
            foreach (var hospital in Hospitals) {
                var distanceToHospital = Vector3.Distance(playerPosition, hospital.Position);
                var resultDistance = Vector3.Distance(playerPosition, result.Position);
                if (distanceToHospital < resultDistance) {
                    result = hospital;
                }
            }
            return result;
        }

        internal class HospitalInfo {
            public Vector3 Position { get; set; }
            public Vector3 PositionAfterExit { get; set; }
            public int Dimension { get; set; }
        }
    }
}