using System;
using System.Threading;

namespace Structr.Abstractions.Extensions
{
    public static class DateTimeExtensions
    {
        private const string _defaultDateString = "";

        /// <summary>
        /// Converts the value of the current <see cref="DateTime"/> object to its equivalent short date string representation.
        /// If value is null then returns specified <paramref name="defaultValue"/>
        /// </summary>
        /// <param name="dateTime">Nullable DateTime value to convert.</param>
        /// <param name="defaultValue">Default value to place instead of null DateTime value.</param>
        /// <returns>A string that contains the short date string representation of the current <see cref="DateTime"/>
        /// object or default string in case of null source date value.</returns>
        public static string ToShortDateString(this DateTime? dateTime, string defaultValue = _defaultDateString)
        {
            return ToString(dateTime, GetFormat(@short: true), defaultValue);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateTime"/> object to its equivalent long date string representation.
        /// If value is null then returns specified <paramref name="defaultValue"/>
        /// </summary>
        /// <param name="dateTime">Nullable DateTime value to convert.</param>
        /// <param name="defaultValue">Default value to place instead of null DateTime value.</param>
        /// <returns>A string that contains the long date string representation of the current <see cref="DateTime"/>
        /// object or default string in case of null source date value.</returns>
        public static string ToLongDateString(this DateTime? dateTime, string defaultValue = _defaultDateString)
        {
            return ToString(dateTime, GetFormat(@short: false), defaultValue);
        }

        /// <summary>
        /// Converts the value of the current <see cref="DateTime"/> object to its equivalent string representation
        /// using the specified format and the formatting conventions of the current culture.
        /// If value is null then returns specified <paramref name="defaultValue"/>
        /// </summary>
        /// <param name="dateTime">Nullable DateTime value to convert.</param>
        /// <param name="format">A standard or custom date and time format string.</param>
        /// <param name="defaultValue">Default value to place instead of null DateTime value.</param>
        /// <returns></returns>
        public static string ToString(this DateTime? dateTime, string format, string defaultValue = _defaultDateString)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToString(format);
            }

            return defaultValue;
        }

        public static string ToLocalShortDateString(this DateTime dateTime)
        {
            return ToLocalDateString(dateTime, @short: true);
        }

        public static string ToLocalShortDateString(this DateTime? dateTime, string defaultValue = _defaultDateString)
        {
            if (dateTime.HasValue)
            {
                return ToLocalDateString(dateTime.Value, @short: true);
            }

            return defaultValue;
        }

        public static string ToLocalLongDateString(this DateTime dateTime)
        {
            return ToLocalDateString(dateTime, @short: false);
        }

        public static string ToLocalLongDateString(this DateTime? dateTime, string defaultValue = _defaultDateString)
        {
            if (dateTime.HasValue)
            {
                return ToLocalDateString(dateTime.Value, @short: false);
            }

            return defaultValue;
        }

        private static string ToLocalDateString(this DateTime dateTime, bool @short)
        {
            var format = GetFormat(@short);
            return dateTime.ToLocalTime().ToString(format);
        }

        private static string GetFormat(bool @short)
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            var cultureDateTimeFormat = culture.DateTimeFormat;
            return @short ?
                cultureDateTimeFormat.ShortDatePattern :
                cultureDateTimeFormat.LongDatePattern;
        }
    }
}
