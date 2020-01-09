using System;

namespace Structr.Abstractions.Providers
{
    public class LocalTimestampProvider : ITimestampProvider
    {
        public DateTime GetTimestamp()
        {
            return DateTime.Now;
        }
    }
}
