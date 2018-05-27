using System.Collections.Generic;
using gta_mp_data.Entity;
using gta_mp_data.Enums;
using gta_mp_database.Models.Work;

namespace gta_mp_database.Converters {
    /// <summary>
    /// Конвертер данных о работе
    /// </summary>
    public class WorksInfoConverter {
        /// <summary>
        /// Конвертация в модели
        /// </summary>
        public static Dictionary<WorkType, WorkInfo> ConvertToModels(IEnumerable<PlayerWork> workEntities) {
            var result = new Dictionary<WorkType, WorkInfo>();
            foreach (var entity in workEntities) {
                var model = new WorkInfo {
                    Type = entity.Type,
                    Level = entity.Level,
                    Experience = entity.Experience,
                    Salary = entity.Salary,
                    Active = entity.Active
                };
                result.Add(model.Type, model);
            }
            return result;
        }

        /// <summary>
        /// Конвертация в сущности
        /// </summary>
        public static IEnumerable<PlayerWork> ConvertToEntities(long accountId, IEnumerable<WorkInfo> models) {
            var result = new List<PlayerWork>();
            foreach (var model in models) {
                var entity = new PlayerWork {
                   AccountId = accountId,
                   Type = model.Type,
                   Level = model.Level,
                   Experience = model.Experience,
                   Salary = model.Salary,
                   Active = model.Active
                };
                result.Add(entity);
            }
            return result;
        }
    }
}