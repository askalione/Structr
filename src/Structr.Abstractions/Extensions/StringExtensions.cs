using System;
using System.Text;

namespace Structr.Abstractions.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Return default value if current string is empty, equals null or equals white space, otherwise current string.
        /// </summary>
        /// <param name="string">Current string.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>
        /// Default value if current string is IsNullOrWhiteSpace, otherwise current string.
        /// </returns>
        public static string DefaultIfEmpty(this string @string, string defaultValue)
        {
            return string.IsNullOrWhiteSpace(@string) == false ? @string : defaultValue;
        }

        /// <summary>
        /// Returns a value indicating whether a specified string occurs within this string, using the specified comparison rules.
        /// </summary>
        /// <param name="string">Target string value.</param>
        /// <param name="value">Required substring value.</param>
        /// <param name="comparison">Comparison rule.</param>
        /// <returns>
        /// <see langword="true"/> if the provided value occurs within this string or if value is 
        /// the empty, otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>Similar method added in .net standard 2.1</remarks>
        public static bool Contains(this string @string, string value, StringComparison comparison)
        {
            if (string.IsNullOrEmpty(@string))
            {
                return false;
            }
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            return @string.IndexOf(value, comparison) >= 0;
        }

        /// <summary>
        /// Casts string to specified type or throws if cast is impossible and <paramref name="throwIfInvalidCast"/> set to <see langword="true"/>.
        /// </summary>
        /// <param name="string">Source string value.</param>
        /// <param name="type">Type to convert to.</param>
        /// <param name="throwIfInvalidCast">If <see langword="true"/> than exception will be thrown when failing the cast.</param>
        /// <returns>
        /// Converted value as object.
        /// </returns>
        /// <exception cref="InvalidCastException"></exception>
        public static object Cast(this string @string, Type type, bool throwIfInvalidCast = false)
        {
            try
            {
                if (type == typeof(string))
                {
                    return @string;
                }
                if (string.IsNullOrWhiteSpace(@string) && Nullable.GetUnderlyingType(type) != null)
                {
                    return null;
                }

                type = Nullable.GetUnderlyingType(type) ?? type;

                if (type.IsEnum)
                {
                    return Enum.Parse(type, @string);
                }

                var value = Convert.ChangeType(@string, type);
                return value;
            }
            catch (Exception ex)
            {
                if (throwIfInvalidCast)
                {
                    throw new InvalidCastException($"Error with converting string \"{@string}\" to type \"{type.Name}\"", ex);
                }

                return null;
            }
        }

        /// <summary>
        /// Cast string to another type.
        /// </summary>
        /// <typeparam name="T">Type to convert to.</typeparam>
        /// <param name="string">Source string value.</param>
        /// <param name="throwIfInvalidCast">If <see langword="true"/> than exception will be thrown when failing the cast.</param>
        /// <returns>
        /// Converted value as object.
        /// </returns>
        /// <exception cref="InvalidCastException"></exception>
        public static T Cast<T>(this string @string, bool throwIfInvalidCast = false)
        {
            return (T)Cast(@string, typeof(T), throwIfInvalidCast);
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified string in the current 
        /// instance are replaced with another specified string using provided comparison rule.
        /// </summary>
        /// <param name="string">Target string value.</param>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The string to replace all occurrences of oldValue.</param>
        /// <param name="comparisonType">Comparison flag.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <remarks>Similar method added in .net standard 2.1</remarks>
        public static string Replace(this string @string, string oldValue, string newValue, StringComparison comparisonType)
        {
            Ensure.NotNull(@string, nameof(@string));
            if (@string.Length == 0)
            {
                return @string;
            }
            Ensure.NotEmpty(oldValue, nameof(oldValue));

            StringBuilder resultStringBuilder = new StringBuilder(@string.Length);

            bool isReplacementNullOrEmpty = string.IsNullOrEmpty(@newValue);

            const int valueNotFound = -1;
            int foundAt;
            int startSearchFromIndex = 0;
            while ((foundAt = @string.IndexOf(oldValue, startSearchFromIndex, comparisonType)) != valueNotFound)
            {

                int @charsUntilReplacment = foundAt - startSearchFromIndex;
                bool isNothingToAppend = @charsUntilReplacment == 0;
                if (isNothingToAppend == false)
                {
                    resultStringBuilder.Append(@string, startSearchFromIndex, @charsUntilReplacment);
                }

                if (isReplacementNullOrEmpty == false)
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

        /// <summary>
        /// Formats string value into hyphen case format. For example "ToHyphenCase" will be formated into "to-hyphen-case".
        /// </summary>
        /// <param name="value">Value to be proccessed.</param>
        /// <returns>String value formatted in hyphen case</returns> 
        public static string ToHyphenCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            var result = "";
            for (var i = 0; i < value.Length; i++)
            {
                if (i == 0)
                {
                    result += char.ToLower(value[i]);
                }
                else
                {
                    if (char.IsUpper(value[i]))
                    {
                        result += "-";
                    }
                    result += char.ToLower(value[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// Formats string value into camel case format. For example "ToCamelCase" will be formated into "toCamelCase".
        /// </summary>
        /// <param name="value">Value to be proccessed.</param>
        /// <returns>String value formatted in camel case</returns>
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            var result = value;
            if (string.IsNullOrEmpty(result) == false && result.Length > 1)
            {
                result = char.ToLowerInvariant(result[0]) + result.Substring(1);
            }
            return result;
        }
    }
}
