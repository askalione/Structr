using System;

namespace Structr.Abstractions
{
    public static class Ensure
    {
        public static void NotNull(object value, string name)
        {
            if (Check.IsNull(value))
                throw new ArgumentNullException(name);
        }

        public static void NotEmpty(string value, string name)
        {
            if (Check.IsEmpty(value))
                throw new ArgumentNullException(name);
        }

        public static void InRange(string value, int minLength, int maxLength, string name)
        {
            if (!Check.IsInRange(value, minLength, maxLength))
                throw new ArgumentOutOfRangeException(name, value, $"String length is out of range. The length must be between {minLength} and {maxLength}.");
        }

        public static void InRange(int value, int minValue, int maxValue, string name)
        {
            if (!Check.IsInRange(value, minValue, maxValue))
                throw new ArgumentOutOfRangeException(name, value, $"Value is out of range. Value must be between {minValue} and {maxValue}.");
        }

        public static void InRange(DateTime value, DateTime minValue, DateTime maxValue, string name)
        {
            if (!Check.IsInRange(value, minValue, maxValue))
                throw new ArgumentOutOfRangeException(name, value, $"Value is out of range. Value must be between {minValue} and {maxValue}.");
        }
    }
}
