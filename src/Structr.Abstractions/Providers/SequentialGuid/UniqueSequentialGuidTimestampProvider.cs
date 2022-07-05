using System;

namespace Structr.Abstractions.Providers.SequentialGuid
{
    public class UniqueSequentialGuidTimestampProvider
    {
        public const double IncrementMs = 4;

        private DateTime _lastValue = DateTime.MinValue;
        private object locker = new object();

        public DateTime GetTimestamp()
        {
            var timestamp = DateTime.UtcNow;
            lock (locker)
            {
                if ((timestamp - _lastValue).TotalMilliseconds < IncrementMs)
                {
                    timestamp = _lastValue.AddMilliseconds(IncrementMs);
                }
                _lastValue = timestamp;
            }
            return timestamp;
        }
    }
}
