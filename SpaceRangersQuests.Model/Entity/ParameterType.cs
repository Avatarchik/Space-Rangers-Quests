namespace SpaceRangersQuests.Model.Entity
{
    /// <summary>
    /// Тип параметра
    /// </summary>
    public enum ParameterType : byte
    {
        /// <summary>
        /// Обычный
        /// </summary>
        PARAMETER_NORMAL = 0,
        /// <summary>
        /// Провальный
        /// </summary>
        PARAMETER_FAIL = 1,
        /// <summary>
        /// Успешный
        /// </summary>
        PARAMETER_SUCCESS = 2,
        /// <summary>
        /// Смертельный
        /// </summary>
        PARAMETER_DEATH = 3
    };
}