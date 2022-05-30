using System.Threading;
using System.Threading.Tasks;

namespace Structr.Validation
{
    /// <inheritdoc cref="IValidator{}"/>
    public abstract class Validator<T> : IValidator<T>
    {
        Task<ValidationResult> IValidator<T>.ValidateAsync(T instance, CancellationToken cancellationToken)
        {
            return Task.FromResult(Validate(instance));
        }

        protected abstract ValidationResult Validate(T instance);
    }
}
