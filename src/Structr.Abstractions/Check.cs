using System;

namespace Structr.Abstractions
{
    public static class Check
    {
        public static bool IsInRange(string value, int minLength, int maxLength)
        {
            int length = value?.Length ?? 0;
            return length >= minLength && length <= maxLength;
        }

        public static bool IsInRange(DateTime value, DateTime minValue, DateTime maxValue)
        {
            var utcValue = value.ToUniversalTime();
            return utcValue >= minValue.ToUniversalTime() && value <= maxValue.ToUniversalTime();
        }

        public static bool IsGreaterThan(string value, int threshold)
        {
            int length = value?.Length ?? 0;
            return length > threshold;
        }

        public static bool IsGreaterThan(DateTime value, DateTime minValue)
        {
            var utcValue = value.ToUniversalTime();
            return utcValue > minValue.ToUniversalTime();
        }

        public static bool IsLessThan(string value, int minLength)
        {
            int length = value?.Length ?? 0;
            return length < minLength;
        }

        public static bool IsLessThan(DateTime value, DateTime maxValue)
        {
            var utcValue = value.ToUniversalTime();
            return utcValue < maxValue.ToUniversalTime();
        }
    }
}
