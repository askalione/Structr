using System;

namespace Structr.Samples.Timestamp
{
    public interface ITimestampProvider
    {
        DateTime GetTimestamp();
    }
}
