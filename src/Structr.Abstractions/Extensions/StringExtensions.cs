using System;
using System.Text;

namespace Structr.Abstractions.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Return default value if current string is empty, equals null or equals white space, otherwise current string.
        /// </summary>
        /// <param name="string">Current string.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>
        /// Return default value if current string is IsNullOrWhiteSpace, otherwise current string.
        /// </returns>
        public static string DefaultIfEmpty(this string @string, string defaultValue)
        {
            return !string.IsNullOrWhiteSpace(@string) ? @string : defaultValue;
        }

        /// <summary>
        /// Returns a value indicating whether a specified substring occurs within this 
        ///     string with using comparison.
        /// </summary>
        /// <param name="string">Target string value.</param>
        /// <param name="value">Required substring value.</param>
        /// <param name="comparison">Comparison flag.</param>
        /// <returns>
        /// True if the value parameter occurs within this string, or if value is 
        ///     the empty string, otherwise, false.
        /// </returns>
        public static bool Contains(this string @string, string value, StringComparison comparison)
        {
            if (string.IsNullOrEmpty(@string))
                return false;
            if (string.IsNullOrEmpty(value))
                return true;

            return @string.IndexOf(value, comparison) >= 0;
        }

        /// <summary>
        /// Cast string to another type with throwing error.
        /// </summary>
        /// <param name="string">Target string value.</param>
        /// <param name="type">Type to convert to.</param>
        /// <param name="throwIfInvalidCast">Throwing flag.</param>
        /// <returns>
        /// Return converted value as object.
        /// </returns>
        public static object Cast(this string @string, Type type, bool throwIfInvalidCast = false)
        {
            try
            {
                if (type == typeof(string))
                    return @string;

                if (string.IsNullOrWhiteSpace(@string))
                    return null;

                type = Nullable.GetUnderlyingType(type) ?? type;

                if (type.IsEnum)
                    return Enum.Parse(type, @string);

                var value = Convert.ChangeType(@string, type);
                return value;
            }
            catch (Exception ex)
            {
                if (throwIfInvalidCast)
                    throw new InvalidCastException($"Error with convert string \"{@string}\" to type \"{type.Name}\"", ex);

                return null;
            }
        }

        /// <summary>
        /// Cast string to another type with throwing error.
        /// </summary>
        /// <typeparam name="T">Type to convert to.</typeparam>
        /// <param name="string">Target string value.</param>
        /// <param name="throwIfInvalidCast">Throwing flag.</param>
        /// <returns>
        /// Return converted value as object.
        /// </returns>
        public static T Cast<T>(this string @string, bool throwIfInvalidCast = false)
        {
            return (T)Cast(@string, typeof(T), throwIfInvalidCast);
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified string in the current 
        ///     instance are replaced with another specified string with using comparison.
        /// </summary>
        /// <param name="string">Target string value.</param>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The string to replace all occurrences of oldValue.</param>
        /// <param name="comparisonType">Comparison flag.</param>
        /// <returns></returns>
        public static string Replace(this string @string, string oldValue, string newValue, StringComparison comparisonType)
        {
            if (@string == null)
                throw new ArgumentNullException(nameof(@string));
            if (@string.Length == 0)
                return @string;
            if (oldValue == null)
                throw new ArgumentNullException(nameof(oldValue));
            if (oldValue.Length == 0)
                throw new ArgumentException("String cannot be of zero length.");

            StringBuilder resultStringBuilder = new StringBuilder(@string.Length);

            bool isReplacementNullOrEmpty = string.IsNullOrEmpty(@newValue);

            const int valueNotFound = -1;
            int foundAt;
            int startSearchFromIndex = 0;
            while ((foundAt = @string.IndexOf(oldValue, startSearchFromIndex, comparisonType)) != valueNotFound)
            {

                int @charsUntilReplacment = foundAt - startSearchFromIndex;
                bool isNothingToAppend = @charsUntilReplacment == 0;
                if (!isNothingToAppend)
                {
                    resultStringBuilder.Append(@string, startSearchFromIndex, @charsUntilReplacment);
                }

                if (!isReplacementNullOrEmpty)
                {
                    resultStringBuilder.Append(@newValue);
                }

                startSearchFromIndex = foundAt + oldValue.Length;
                if (startSearchFromIndex == @string.Length)
                {
                    return resultStringBuilder.ToString();
                }
            }

            int @charsUntilStringEnd = @string.Length - startSearchFromIndex;
            resultStringBuilder.Append(@string, startSearchFromIndex, @charsUntilStringEnd);

            return resultStringBuilder.ToString();
        }
    }
}
