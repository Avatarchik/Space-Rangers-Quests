using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceRangersQuests.Model.Utils
{
    public static class ArrayExtensions
    {
        public static T[] Concat<T>(this T[] first, T second)
        {
            var baseLength = first.Length;
            var result = new T[baseLength + 1];
            Array.Copy(first, result, baseLength);
            result[baseLength] = second;
            return result;
        }

        /// <summary>
        /// Произвести объединение массивов в один.
        /// </summary>
        /// <typeparam name="T">тип элемента массива</typeparam>
        /// <param name="first">первый массив</param>
        /// <param name="second">второй массив</param>
        /// <param name="tail">последующие массивы</param>
        /// <returns>объединённый массив</returns>
        public static T[] Concat<T>(this T[] first, T[] second, params T[][] tail)
        {
            var totalLength = first.Length + second.Length + tail.Sum(o => o.Length);
            var result = new T[totalLength];
            Array.Copy(first, 0, result, 0, first.Length);
            Array.Copy(second, 0, result, first.Length, second.Length);
            var offset = first.Length + second.Length;
            foreach (var t in tail)
            {
                Array.Copy(t, 0, result, offset, t.Length);
                offset += t.Length;
            }
            return result;
        }

        public static T[] Copy<T>(this T[] sourceArray, int startIndex, int size)
        {
            var destantionArray = new T[sourceArray.Length - startIndex];
            Array.Copy(sourceArray, startIndex, destantionArray, 0, destantionArray.Length);
            return destantionArray;
        }

        public static T[] Copy<T>(this T[] sourceArray, int startIndex)
        {
            return sourceArray.Copy(startIndex, sourceArray.Length - startIndex);
        }

        /// <summary>
        /// Произвести проверку массивов на равенство содержимого.
        /// </summary>
        /// <typeparam name="T">тип элемента массива</typeparam>
        /// <param name="a">первый массив</param>
        /// <param name="b">второй массив</param>
        /// <returns>признак равенства содержимого</returns>
        public static bool HasEqualContent<T>(this T[] a, T[] b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if (a == null || b == null)
                return false;
            if (a.Length != b.Length)
                return false;
            var comparer = EqualityComparer<T>.Default;
            for (var i = 0; i < a.Length; i++)
            {
                if (!comparer.Equals(a[i], b[i]))
                    return false;
            }
            return true;
        }
    }
}
