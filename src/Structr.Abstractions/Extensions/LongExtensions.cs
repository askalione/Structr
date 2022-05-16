using System;

namespace Structr.Abstractions.Extensions
{
    public static class LongExtensions
    {
        /// <summary>
        /// Creates a human readable file size string from <see cref="long"/> value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>String with human readable file size info.</returns>
        public static string ToFileSizeString(this long value)
        {
            string[] sizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" }; // "ZB", "YB" is unreachable for "long" type
            if (value < 0) { return "-" + (-value).ToFileSizeString(); }
            if (value == 0) { return string.Format("{0:n1} {1}", 0, sizeSuffixes[0]); }

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, sizeSuffixes[mag]);
        }
    }
}
