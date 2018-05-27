using System.ComponentModel;

namespace gta_mp_server.Enums {
    /// <summary>
    /// Вид груза в контракте
    /// </summary>
    internal enum DeliveryContractType {
        #region Дальнобойщики

        [Description("Зерно")]
        Corn = 1,

        [Description("Сырье")]
        Crude,

        [Description("Инструменты")]
        Tools,

        [Description("Древесина в мастерскую")]
        WorkshopWood,

        [Description("Древесина на стройку")]
        BuildingsWood,

        [Description("Красители")]
        Dyes,

        [Description("Бензин")]
        Fuel,

        [Description("Продукты")]
        Products,

        [Description("Металлолом")]
        ScrapMetal,

        #endregion


        #region Летчики

        [Description("Чертежи механизмов")]
        MechanicalDrawings,

        [Description("Рассада и удобрения")]
        Fertilizer,

        [Description("Военное снаряжение")]
        MilitaryEquipment,

        [Description("Собранные прототипы")]
        CollectedPrototypes,

        [Description("Урожай")]
        Crop,

        [Description("Военные отчеты")]
        MilitaryReports,

        [Description("Деньги на фeрму (Банда)")]
        MoneyToFarm,

        [Description("Марихуана (Банда)")]
        Marijuana,

        [Description("Взятка военным (Банда)")]
        MoneyToMilitary,

        [Description("Контрабанда оружия (Банда)")]
        SmugglingWeapon

        #endregion
    }
}