using System;

namespace Structr.Abstractions.Providers
{
    /// <summary>
    /// Simple timestamp returning current UTC date and time value.
    /// </summary>
    public class UtcTimestampProvider : ITimestampProvider
    {
        public DateTime GetTimestamp()
        {
            return DateTime.UtcNow;
        }
    }
}
