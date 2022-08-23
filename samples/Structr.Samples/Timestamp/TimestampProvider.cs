using System;

namespace Structr.Samples.Timestamp
{
    public class TimestampProvider : ITimestampProvider
    {
        public DateTime GetTimestamp()
            => DateTime.Now;
    }
}
