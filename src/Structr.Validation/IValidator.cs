using System.Threading;
using System.Threading.Tasks;

namespace Structr.Validation
{
    public interface IValidator<in T>
    {
        Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellationToken);
    }
}
