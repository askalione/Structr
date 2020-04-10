using System;

namespace Structr.Abstractions
{
    public static class Ensure
    {
        public static void NotNull(object value, string name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }

        public static void NotEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(name);
        }

        public static void InRange(string value, int minLength, int maxLength, string name)
        {
            if (!Check.IsInRange(value, minLength, maxLength))
                throw new ArgumentOutOfRangeException(name, value, $"String length is out of range. The length must be between {minLength} and {maxLength}.");
        }

        public static void InRange(DateTime value, DateTime minValue, DateTime maxValue, string name)
        {
            if (!Check.IsInRange(value, minValue, maxValue))
                throw new ArgumentOutOfRangeException(name, value, $"Value is out of range. Value must be between {minValue} and {maxValue}.");
        }

        public static void InRange(IComparable value, IComparable minValue, IComparable maxValue, string name)
        {
            if (!(value.CompareTo(minValue) >= 0 && value.CompareTo(maxValue) <= 0))
                throw new ArgumentOutOfRangeException(name, value, $"Value is out of range. Value must be between {minValue} and {maxValue}.");
        }

        public static void GreaterThan(string value, int minLength, string name)
        {
            if (!Check.IsGreaterThan(value, minLength))
                throw new ArgumentOutOfRangeException(name, value, $"String length is out of range. The length must be greater than {minLength}.");
        }

        public static void GreaterThan(DateTime value, DateTime minValue, string name)
        {
            if (!Check.IsGreaterThan(value, minValue))
                throw new ArgumentOutOfRangeException(name, value, $"Value is out of range. Value must be greater than {minValue}.");
        }

        public static void GreaterThan(IComparable value, IComparable minValue, string name)
        {
            if (!(value.CompareTo(minValue) > 0))
                throw new ArgumentOutOfRangeException(name, value, $"Value is out of range. Value must be greater than {minValue}.");
        }

        public static void LessThan(string value, int maxLength, string name)
        {
            if (!Check.IsLessThan(value, maxLength))
                throw new ArgumentOutOfRangeException(name, value, $"String length is out of range. The length must be less than {maxLength}.");
        }

        public static void LessThan(DateTime value, DateTime maxValue, string name)
        {
            if (!Check.IsLessThan(value, maxValue))
                throw new ArgumentOutOfRangeException(name, value, $"Value is out of range. Value must be less than {maxValue}.");
        }

        public static void LessThan(IComparable value, IComparable maxValue, string name)
        {
            if (!(value.CompareTo(maxValue) < 0))
                throw new ArgumentOutOfRangeException(name, value, $"Value is out of range. Value must be less than {maxValue}.");
        }
    }
}
