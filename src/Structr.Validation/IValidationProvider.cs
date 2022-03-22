using System.Threading;
using System.Threading.Tasks;

namespace Structr.Validation
{
    public interface IValidationProvider
    {
        Task<ValidationResult> ValidateAsync(object instance, CancellationToken cancellationToken = default(CancellationToken));
        Task ValidateAndThrowAsync(object instance, CancellationToken cancellationToken = default(CancellationToken));
    }
}
