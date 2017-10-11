using System.Collections.Generic;

namespace SpaceRangersQuests.Model.Entity
{
    /// <summary>
    /// Класс со занчениями для видимости
    /// </summary>
    public class AcceptValue
    {
        public AcceptValue()
        {
            AcceptValues = new List<int>();
        }
        /// <summary>
        /// Количество значений определяющих видимость
        /// </summary>
        public int CountAcceptValues;
        /// <summary>
        /// Дип сравнения значений, равен/не равен
        /// </summary>
        public AcceptValueType AcceptType;
        /// <summary>
        /// Значения определяющие видимость
        /// </summary>
        public IList<int> AcceptValues { get; private set; }
    }
}