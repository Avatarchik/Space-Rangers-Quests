using System.Collections.Generic;

namespace SpaceRangersQuests.Model.Entity
{
    /// <summary>
    /// Класс со занчениями для видимости
    /// </summary>
    public class IncludeValue
    {
        public IncludeValue()
        {
            IncludeValues = new List<int>();
        }
        /// <summary>
        /// Количество значений определяющих видимость
        /// </summary>
        public int CountIncludeValues;
        /// <summary>
        /// Дип сравнения значений, равен/не равен
        /// </summary>
        public IncludeValueType IncludeType;
        /// <summary>
        /// Значения определяющие видимость
        /// </summary>
        public IList<int> IncludeValues { get; private set; }
    }
}