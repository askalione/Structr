using System.Threading;
using System.Threading.Tasks;

namespace Structr.Validation
{
    public interface IValidator<in T>
    {
        Task<IValidationResult> ValidateAsync(T instance, CancellationToken cancellationToken);
    }
}
