using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Abstractions
{
    public static class Ensure
    {
        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> when value of specified varible is null.
        /// </summary>
        /// <param name="value">Value of varible to be checked.</param>
        /// <param name="name">Name of varible to be checked.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void NotNull(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> when value of specified <see cref="string"/> varible is null or empty.
        /// </summary>
        /// <param name="value">Value of varible to be checked.</param>
        /// <param name="name">Name of varible to be checked.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void NotEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> when value of specified <see cref="IEnumerable{T}"/> collection is null or collection is empty.
        /// </summary>
        /// <param name="value">Value of varible to be checked.</param>
        /// <param name="name">Name of varible to be checked.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void NotEmpty<T>(IEnumerable<T> value, string name)
        {
            if (value == null || value.Any() == false)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> when specified <see cref="string"/> value length isn't between provided boundaries (with inclusion).
        /// </summary>
        /// <param name="value">Value of varible to be checked.</param>
        /// <param name="minLength">Lower length boundry.</param>
        /// <param name="maxLength">Upper length boundry.</param>
        /// <param name="name">Name of varible to be checked.</param>
        public static void InRange(string value, int minLength, int maxLength, string name)
        {
            if (Check.IsInRange(value, minLength, maxLength) == false)
            {
                throw new ArgumentOutOfRangeException(name, value, $"String length is out of range. The length must be between {minLength} and {maxLength}.");
            }
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> when specified <see cref="DateTime"/> value isn't between provided boundaries (with inclusion).
        /// </summary>
        /// <param name="value">Value of varible to be checked.</param>
        /// <param name="minValue">Lower length boundry.</param>
        /// <param name="maxValue">Upper length boundry.</param>
        /// <param name="name">Name of varible to be checked.</param>
        public static void InRange(DateTime value, DateTime minValue, DateTime maxValue, string name)
        {
            if (Check.IsInRange(value, minValue, maxValue) == false)
            {
                throw new ArgumentOutOfRangeException(name, value, $"Value is out of range. Value must be between {minValue} and {maxValue}.");
            }
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> when specified value isn't between provided boundaries (with inclusion).
        /// </summary>
        /// <param name="value">Value of varible to be checked.</param>
        /// <param name="minValue">Lower length boundry.</param>
        /// <param name="maxValue">Upper length boundry.</param>
        /// <param name="name">Name of varible to be checked.</param>
        public static void InRange(IComparable value, IComparable minValue, IComparable maxValue, string name)
        {
            var valueType = value.GetType();
            var minValueType = minValue.GetType();
            var maxValueType = maxValue.GetType();

            if ((valueType == minValueType && valueType == maxValueType) == false)
            {
                throw new InvalidOperationException($"Value type and {nameof(minValue)} type and {nameof(maxValue)} type must be equals.");
            }

            if ((value.CompareTo(minValue) >= 0 && value.CompareTo(maxValue) <= 0) == false)
            {
                throw new ArgumentOutOfRangeException(name, value, $"Value is out of range. Value must be between {minValue} and {maxValue}.");
            }
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> when specified <see cref="string"/> value length isn't greater than provided threshold.
        /// </summary>
        /// <param name="value">String to be checked.</param>
        /// <param name="minLength">Lower length threshold.</param>
        /// <param name="name">Name of varible to be checked.</param>
        public static void GreaterThan(string value, int minLength, string name)
        {
            if (Check.IsGreaterThan(value, minLength) == false)
            {
                throw new ArgumentOutOfRangeException(name, value, $"String length is out of range. The length must be greater than {minLength}.");
            }
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> when specified <see cref="DateTime"/> value isn't greater than provided threshold.
        /// </summary>
        /// <param name="value">DateTime value to be checked.</param>
        /// <param name="minValue">Lower length threshold.</param>
        /// <param name="name">Name of varible to be checked.</param>
        public static void GreaterThan(DateTime value, DateTime minValue, string name)
        {
            if (Check.IsGreaterThan(value, minValue) == false)
            {
                throw new ArgumentOutOfRangeException(name, value, $"Value is out of range. Value must be greater than {minValue}.");
            }
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> when specified value isn't greater than provided threshold.
        /// </summary>
        /// <param name="value">Value of varible to be checked.</param>
        /// <param name="minValue">Lower length threshold.</param>
        /// <param name="name">Name of varible to be checked.</param>
        public static void GreaterThan(IComparable value, IComparable minValue, string name)
        {
            if (value.GetType() != minValue.GetType())
            {
                throw new InvalidOperationException($"Value type and {nameof(minValue)} type must be equals.");
            }

            if ((value.CompareTo(minValue) > 0) == false)
            {
                throw new ArgumentOutOfRangeException(name, value, $"Value is out of range. Value must be greater than {minValue}.");
            }
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> when specified <see cref="string"/> value length isn't less than provided threshold.
        /// </summary>
        /// <param name="value">String to be checked.</param>
        /// <param name="maxLength">Lower length threshold.</param>
        /// <param name="name">Name of varible to be checked.</param>
        public static void LessThan(string value, int maxLength, string name)
        {
            if (Check.IsLessThan(value, maxLength) == false)
            {
                throw new ArgumentOutOfRangeException(name, value, $"String length is out of range. The length must be less than {maxLength}.");
            }
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> when specified <see cref="DateTime"/> value isn't less than provided threshold.
        /// </summary>
        /// <param name="value">DateTime to be checked.</param>
        /// <param name="maxValue">Lower length threshold.</param>
        /// <param name="name">Name of varible to be checked.</param>
        public static void LessThan(DateTime value, DateTime maxValue, string name)
        {
            if (Check.IsLessThan(value, maxValue) == false)
            {
                throw new ArgumentOutOfRangeException(name, value, $"Value is out of range. Value must be less than {maxValue}.");
            }
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> when specified value isn't less than provided threshold.
        /// </summary>
        /// <param name="value">Value of varible to be checked.</param>
        /// <param name="maxValue">Lower length threshold.</param>
        /// <param name="name">Name of varible to be checked.</param>
        public static void LessThan(IComparable value, IComparable maxValue, string name)
        {
            if (value.GetType() != maxValue.GetType())
            {
                throw new InvalidOperationException($"Value type and {nameof(maxValue)} type must be equals.");
            }

            if ((value.CompareTo(maxValue) < 0) == false)
            {
                throw new ArgumentOutOfRangeException(name, value, $"Value is out of range. Value must be less than {maxValue}.");
            }
        }
    }
}
