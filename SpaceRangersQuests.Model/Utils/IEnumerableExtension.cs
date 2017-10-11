using System;
using System.Collections;
using System.Collections.Generic;

namespace SpaceRangersQuests.Model.Utils
{
    // ReSharper disable InconsistentNaming
    public static class IEnumerableExtension
    // ReSharper restore InconsistentNaming
    {
        public static void ForEach(this IEnumerable enumerable, Action<object> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            foreach (object value in enumerable)
            {
                action(value);
            }
        }

        public static void ForEach<T>(this IEnumerable enumerable, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            foreach (T value in enumerable)
            {
                action(value);
            }
        }

        public static void ForEach<T>(this IEnumerable enumerable, Action<T, int> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var index = 0;
            foreach (T value in enumerable)
            {
                action(value, index++);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            foreach (T value in enumerable)
            {
                action(value);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var index = 0;
            foreach (T value in enumerable)
            {
                action(value, index++);
            }
        }
    }
}
