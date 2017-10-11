using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SpaceRangersQuests.Model.Utils
{
    /// <summary>
    /// Неизвестные значения
    /// </summary>
    public class UnknownValues
    {
        private const string UnknownValueName = "Unknown";

        /// <summary>
        /// Неизвестное значение
        /// </summary>
        [DebuggerDisplay("{Description} : {Value}")]
        public class UnknownValue
        {
            /// <summary>
            /// Конструктор неизвестного значения
            /// </summary>
            /// <param name="value">Значение</param>
            public UnknownValue(object value)
                : this(null, value)
            { }

            /// <summary>
            /// Конструктор неизвестного значения
            /// </summary>
            /// <param name="description">Описание</param>
            /// <param name="value">Значение</param>
            public UnknownValue(string description, object value)
            {
                Description = description;
                Value = value;
            }

            /// <summary>
            /// Описание
            /// </summary>
            public string Description { get; }
            /// <summary>
            /// Значение
            /// </summary>
            public object Value { get; }
        }

        /// <summary>
        /// Неизвестное значение
        /// </summary>
        public class UnknownValue<T> : UnknownValue
        {
            /// <summary>
            /// Конструктор неизвестного значения
            /// </summary>
            /// <param name="description">Описание</param>
            /// <param name="value">Значение</param>
            public UnknownValue(string description, T value)
                : base(description, value)
            {
            }

            /// <summary>
            /// Значение
            /// </summary>
            public new T Value { get { return (T)base.Value; } }
        }

        private readonly IList<UnknownValue> _unknownValues;

        /// <summary>
        /// Конструктор списка неизвестных значений
        /// </summary>
        public UnknownValues()
        {
            _unknownValues = new List<UnknownValue>();
        }
        /// <summary>
        /// Число значений
        /// </summary>
        public int CountValues { get { return _unknownValues.Count; } }
        /// <summary>
        /// Добавить неизвестного значения
        /// </summary>
        /// <typeparam name="T">Тип неизвестного значения</typeparam>
        /// <param name="value">Значение</param>
        public void Add<T>(T value)
        {
            var index = 1;
            var description = $"{UnknownValueName}{index++}";
            while (_unknownValues.Any(v => v.Description == description))
            {
                description = $"{UnknownValueName}{index++}";
            }
            Add(description, value);
        }
        /// <summary>
        /// Добавить неизвестного значения
        /// </summary>
        /// <typeparam name="T">Тип неизвестного значения</typeparam>
        /// <param name="description">Описание</param>
        /// <param name="value">Значение</param>
        public void Add<T>(string description, T value)
        {
            _unknownValues.Add(new UnknownValue<T>(description, value));
        }

        public object GetValue(int index)
        {
            return _unknownValues[index].Value;
        }

        public T GetValue<T>(int index)
        {
            return (T) _unknownValues[index].Value;
        }
    }
}
