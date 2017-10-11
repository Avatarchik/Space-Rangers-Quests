namespace SpaceRangersQuests.Model.Entity
{
    /// <summary>
    /// Тип локации
    /// </summary>
    public enum LocationType
    {
        /// <summary>
        /// Промежуточная локация
        /// </summary>
        LOCATION_NORMAL,
        /// <summary>
        /// Стартовая локация
        /// </summary>
        LOCATION_START,
        /// <summary>
        /// Пустая локация
        /// </summary>
        LOCATION_EMPTY,
        /// <summary>
        /// Победная локация
        /// </summary>
        LOCATION_SUCCESS,
        /// <summary>
        /// Провальная локация
        /// </summary>
        LOCATION_FAIL,
        /// <summary>
        /// Смертельная локация
        /// </summary>
        LOCATION_DEATH
    }
}