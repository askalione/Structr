using System.Threading;
using System.Threading.Tasks;

namespace Structr.Validation
{
    public abstract class Validator<T> : IValidator<T>
    {
        Task<ValidationResult> IValidator<T>.ValidateAsync(T instance, CancellationToken cancellationToken)
        {
            var failure = new ValidationFailure("")
            {
                ActualValue = 1
            };

            return Task.FromResult(Validate(instance));
        }

        protected abstract ValidationResult Validate(T instance);
    }
}
