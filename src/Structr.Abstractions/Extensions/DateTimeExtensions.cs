using System;
using System.Threading;

namespace Structr.Abstractions.Extensions
{
    public static class DateTimeExtensions
    {
        private const string _defaultDateString = "";

        public static string ToShortDateString(this DateTime? dateTime, string defaultValue = _defaultDateString)
        {
            return ToString(dateTime, GetFormat(@short: true), defaultValue);
        }

        public static string ToLongDateString(this DateTime? dateTime, string defaultValue = _defaultDateString)
        {
            return ToString(dateTime, GetFormat(@short: false), defaultValue);
        }

        public static string ToString(this DateTime? dateTime, string format, string defaultValue = _defaultDateString)
        {
            if (dateTime.HasValue)
                return dateTime.Value.ToString(format);

            return defaultValue;
        }

        public static string ToLocalShortDateString(this DateTime dateTime)
        {
            return ToLocalDateString(dateTime, @short: true);
        }

        public static string ToLocalShortDateString(this DateTime? dateTime, string defaultValue = _defaultDateString)
        {
            if (dateTime.HasValue)
                return ToLocalDateString(dateTime.Value, @short: true);

            return defaultValue;
        }

        public static string ToLocalLongDateString(this DateTime dateTime)
        {
            return ToLocalDateString(dateTime, @short: false);
        }

        public static string ToLocalLongDateString(this DateTime? dateTime, string defaultValue = _defaultDateString)
        {
            if (dateTime.HasValue)
                return ToLocalDateString(dateTime.Value, @short: false);

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
