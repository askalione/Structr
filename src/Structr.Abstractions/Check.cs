using System;

namespace Structr.Abstractions
{
    /// <summary>
    /// Provides functionality for determining if some variable's value meets specified conditions.
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Determines whether specified <see cref="string"/> value length lays in provided boundaries with inclusion.
        /// </summary>
        /// <param name="value">String to be checked.</param>
        /// <param name="minLength">Lower length boundry.</param>
        /// <param name="maxLength">Upper length boundry.</param>
        /// <returns><see langword="true"/> if string length lays between two boundaries, overwise <see langword="false"/></returns>
        public static bool IsInRange(string value, int minLength, int maxLength)
        {
            int length = value?.Length ?? 0;
            return length >= minLength && length <= maxLength;
        }

        /// <summary>
        /// Determines whether specified <see cref="DateTime"/> value lays in provided boundaries with inclusion.
        /// </summary>
        /// <param name="value">DateTime to be checked.</param>
        /// <param name="minValue">Lower boundry.</param>
        /// <param name="maxValue">Upper boundry.</param>
        /// <returns><see langword="true"/> if value lays between two boundaries, overwise <see langword="false"/></returns>
        public static bool IsInRange(DateTime value, DateTime minValue, DateTime maxValue)
        {
            var utcValue = value.ToUniversalTime();
            return utcValue >= minValue.ToUniversalTime() && value <= maxValue.ToUniversalTime();
        }

        /// <summary>
        /// Determines whether specified <see cref="string"/> value length is greater than provided threshold.
        /// </summary>
        /// <param name="value">String to be checked.</param>
        /// <param name="threshold">Lower length threshold.</param>
        /// <returns><see langword="true"/> if string's length is greater than threshold, overwise <see langword="false"/></returns>
        public static bool IsGreaterThan(string value, int threshold)
        {
            int length = value?.Length ?? 0;
            return length > threshold;
        }

        /// <summary>
        /// Determines whether specified <see cref="DateTime"/> value is greater than provided threshold.
        /// </summary>
        /// <param name="value">DateTime to be checked.</param>
        /// <param name="minValue">Lower threshold.</param>
        /// <returns><see langword="true"/> if value is greater than threshold, overwise <see langword="false"/></returns>
        public static bool IsGreaterThan(DateTime value, DateTime minValue)
        {
            var utcValue = value.ToUniversalTime();
            return utcValue > minValue.ToUniversalTime();
        }

        /// <summary>
        /// Determines whether specified <see cref="string"/> value length is less than provided threshold.
        /// </summary>
        /// <param name="value">String to be checked.</param>
        /// <param name="threshold">Upper length threshold.</param>
        /// <returns><see langword="true"/> if string length is less than threshold, overwise <see langword="false"/>.</returns>
        public static bool IsLessThan(string value, int threshold)
        {
            int length = value?.Length ?? 0;
            return length < threshold;
        }

        /// <summary>
        /// Determines whether specified <see cref="DateTime"/> value is less than provided threshold.
        /// </summary>
        /// <param name="value">DateTime to be checked.</param>
        /// <param name="maxValue">Upper threshold.</param>
        /// <returns><see langword="true"/> if value is less than threshold, overwise <see langword="false"/>.</returns>
        public static bool IsLessThan(DateTime value, DateTime maxValue)
        {
            var utcValue = value.ToUniversalTime();
            return utcValue < maxValue.ToUniversalTime();
        }
    }
}
