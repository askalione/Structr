using System.Collections.Generic;

namespace Structr.Validation
{
    public interface IValidationResult : IEnumerable<IValidationFailure>
    {
        bool IsValid { get; }
        string ToString(string separator);
    }
}
