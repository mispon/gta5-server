using gta_mp_database.Models.Player;

namespace gta_mp_database.Converters {
    /// <summary>
    /// Конвертер данных о водительском удостоверении
    /// </summary>
    public class DriverInfoConverter {
        /// <summary>
        /// Конвертация модели в сущность
        /// </summary>
        public static gta_mp_data.Entity.DriverInfo ConvertToEntity(long accountId, DriverInfo model) {
            return new gta_mp_data.Entity.DriverInfo {
                AccountId = accountId,
                TheoryExamPaid = model.TheoryExamPaid,
                PassedTheory = model.PassedTheory,
                PracticeExamPaid = model.PracticeExamPaid,
                PassedPracticeB = model.PassedPracticeB,
                PassedPracticeC = model.PassedPracticeB,
                TimeToNextTry = model.TimeToNextTry
            };
        }

        /// <summary>
        /// Конвертация сущности в модель
        /// </summary>
        public static DriverInfo ConvertToModel(gta_mp_data.Entity.DriverInfo entity) {
            return new DriverInfo {
                TheoryExamPaid = entity.TheoryExamPaid,
                PassedTheory = entity.PassedTheory,
                PracticeExamPaid = entity.PracticeExamPaid,
                PassedPracticeB = entity.PassedPracticeB,
                PassedPracticeC = entity.PassedPracticeB,
                TimeToNextTry = entity.TimeToNextTry
            };
        }
    }
}