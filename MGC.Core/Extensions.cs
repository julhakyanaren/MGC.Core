using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGC.Core
{
    public static class Extensions
    {
        public static T[] Add<T>(this T[] array, T item)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            Array.Resize(ref array, array.Length + 1);
            array[array.Length - 1] = item;
            return array;
        }

        public static T[] InsertAt<T>(this T[] array, int index, T item)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if (index < 0 || index > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            int oldLength = array.Length;
            Array.Resize(ref array, oldLength + 1);
            if (index < oldLength)
            {

                Array.Copy(array, index, array, index + 1, oldLength - index);
            }
            array[index] = item;

            return array;

        }

        public static T[] RemoveAt<T>(this T[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (index < 0 || index >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (array.Length == 1)
            {
                return Array.Empty<T>();
            }

            T[] newArray = new T[array.Length - 1];
            if (index > 0)
            {
                Array.Copy(array, 0, newArray, 0, index);
            }
            int rightCount = array.Length - index - 1;
            if (rightCount > 0)
            {
                Array.Copy(array, index + 1, newArray, index, rightCount);
            }
            return newArray;
        }

        public static T[] AddRange<T>(this T[] array, IEnumerable<T> items)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            T[] append = items as T[] ?? new List<T>(items).ToArray();
            int appendCount = append.Length;

            if (appendCount == 0)
            {
                return array;
            }

            T[] newArray = new T[array.Length + appendCount];

            Array.Copy(array, newArray, array.Length);

            Array.Copy(append, 0, newArray, array.Length, appendCount);

            return newArray;
        }

        public static T[] Remove<T>(this T[] array, T item)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            int index = Array.IndexOf(array, item);
            if (index < 0)
            {
                return array;
            }
            return array.RemoveAt(index);
        }

        public static T[] EnsureCapacity<T>(this T[] array, int minCapacity)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if (minCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minCapacity));
            }

            if (array.Length >= minCapacity)
            {
                return array;
            }
            T[] newArray = new T[minCapacity];

            Array.Copy(array, newArray, array.Length);

            return newArray;
        }

        public static T[] ShiftLeft<T>(this T[] array, int count = 1)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            int len = array.Length;
            if (len == 0 || count == 0)
            {
                return array;
            }

            if (count >= len)
            {
                return new T[len];
            }

            T[] result = new T[len];
            Array.Copy(array, count, result, 0, len - count);
            return result;
        }

        public static T[] ShiftRight<T>(this T[] array, int count = 1)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            int len = array.Length;
            if (len == 0 || count == 0)
            {
                return array;
            }

            if (count >= len)
            {
                return new T[len];
            }

            T[] result = new T[len];
            Array.Copy(array, 0, result, count, len - count);
            return result;
        }

        public static T[] Unique<T>(this T[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            return array.ToHashSet().ToArray();
        }
    }
}
