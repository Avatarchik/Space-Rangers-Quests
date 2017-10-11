using System.Collections.Generic;

namespace SpaceRangersQuests.Model.Entity
{
    /// <summary>
    /// Класс определяющий видимость по кртности/не кратности
    /// </summary>
    public class ModValue
    {
        public ModValue()
        {
            ModValues = new List<int>();
        }
        /// <summary>
        /// Количество значений которым кртно/не кратно значение
        /// </summary>
        public int CountModValues;
        /// <summary>
        /// Тип кратности. Должен/не должен быть кратен
        /// </summary>
        public ModValueType ModType;
        /// <summary>
        /// Значения кратности
        /// </summary>
        public IList<int> ModValues { get; private set; }
    };
}