using System;

namespace Structr.Abstractions.Providers
{
    /// <summary>
    /// Simple timestamp returning current date and time value.
    /// </summary>
    public class LocalTimestampProvider : ITimestampProvider
    {
        public DateTime GetTimestamp()
        {
            return DateTime.Now;
        }
    }
}
