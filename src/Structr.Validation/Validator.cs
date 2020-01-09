using System.Threading;
using System.Threading.Tasks;

namespace Structr.Validation
{
    public abstract class Validator<T> : IValidator<T>
    {
        Task<IValidationResult> IValidator<T>.ValidateAsync(T instance, CancellationToken cancellationToken)
        {
            return Task.FromResult(Validate(instance));
        }

        protected abstract IValidationResult Validate(T instance);
    }
}
