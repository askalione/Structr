using System;

namespace Structr.Abstractions.Providers
{
    /// <summary>
    /// Simple DateTime provider service intended to generate timestamp.
    /// </summary>
    public interface ITimestampProvider
    {
        /// <summary>
        /// Returns timestamp.
        /// </summary>
        DateTime GetTimestamp();
    }
}
