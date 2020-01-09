using System;

namespace Structr.Abstractions
{
    public static class Check
    {
        public static bool IsNull(object value)
        {
            return value == null;
        }
        
        public static bool IsEmpty(string value)
        {
            return value == null || value.Trim().Length == 0;
        }

        public static bool IsInRange(string value, int minLength, int maxLength)
        {
            int length = value?.Length ?? 0;
            return length >= minLength && length <= maxLength;
        }

        public static bool IsInRange(int value , int minValue, int maxValue)
        {
            return value >= minValue && value <= maxValue;
        }

        public static bool IsInRange(DateTime value, DateTime minValue, DateTime maxValue)
        {
            var utcValue = value.ToUniversalTime();
            return utcValue >= minValue.ToUniversalTime() && value <= maxValue.ToUniversalTime();
        }
    }
}
