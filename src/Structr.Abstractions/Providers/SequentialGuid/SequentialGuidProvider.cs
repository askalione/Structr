using System;

namespace Structr.Abstractions.Providers.SequentialGuid
{
    public class SequentialGuidProvider : ISequentialGuidProvider
    {
        private SequentialGuidInitializer _initializer;
        private SequentialGuidTimestampProvider _timestampProvider;

        private DateTime _minTimestamp { get; } = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public SequentialGuidProvider(SequentialGuidInitializer guidInitializer, SequentialGuidTimestampProvider timestampProvider)
        {
            Ensure.NotNull(guidInitializer, nameof(guidInitializer));
            Ensure.NotNull(timestampProvider, nameof(timestampProvider));

            _initializer = guidInitializer;
            _timestampProvider = timestampProvider;
        }

        public Guid GetSequentialGuid(SequentialGuidType type = SequentialGuidType.String)
        {
            byte[] guidBytes = GetGuidBytes();
            byte[] timestampBytes = GetTimestampBytes();

            switch (type)
            {
                case SequentialGuidType.String:
                case SequentialGuidType.Binary:
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);

                    if (type is SequentialGuidType.String && BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(guidBytes, 0, 4);
                        Array.Reverse(guidBytes, 4, 2);
                    }

                    break;
                case SequentialGuidType.Ending:
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                    break;
                default:
                    throw new NotSupportedException($"Not supported sequiential GUID type \"{type}\".");
            }

            return new Guid(guidBytes);
        }

        private byte[] GetGuidBytes()
        {
            Guid guid = _initializer();
            return guid.ToByteArray();
        }

        private byte[] GetTimestampBytes()
        {
            DateTime timestamp = _timestampProvider();
            var timestampBytes = BitConverter.GetBytes((timestamp.Ticks - _minTimestamp.Ticks) / 10000L);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }

            return timestampBytes;
        }
    }
}
