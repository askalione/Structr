using System;

namespace Structr.Abstractions.Extensions
{
    public static class LongExtensions
    {
        public static string ToFileSizeString(this long value)
        {
            string[] sizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            if (value < 0) { return "-" + (-value).ToFileSizeString(); }
            if (value == 0) { return "0.0 bytes"; }

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, sizeSuffixes[mag]);
        }
    }
}
