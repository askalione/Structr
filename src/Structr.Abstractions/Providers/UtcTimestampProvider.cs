using System;

namespace Structr.Abstractions.Providers
{
    public class UtcTimestampProvider : ITimestampProvider
    {
        public DateTime GetTimestamp()
        {
            return DateTime.UtcNow;
        }
    }
}
