using System;

namespace Structr.Abstractions.Providers.SequentialGuid
{
    public interface ISequentialGuidProvider
    {
        Guid GetSequentialGuid(SequentialGuidType type = SequentialGuidType.String);
    }
}
