using System.Threading;
using System.Threading.Tasks;

namespace Structr.Validation
{
    /// <summary>
    /// Class to be used as base for all synchronous validators.
    /// </summary>
    /// <typeparam name="T">Class which instances will be validated.</typeparam>
    public abstract class Validator<T> : IValidator<T>
    {
        Task<ValidationResult> IValidator<T>.ValidateAsync(T instance, CancellationToken cancellationToken)
        {
            return Task.FromResult(Validate(instance));
        }

        /// <summary>
        /// Synchronously validates instance of <see cref="T"/>.
        /// </summary>
        /// <param name="instance">The instance of <see cref="T"/> to be validated.</param>
        /// <returns>The <see cref="ValidationResult"/>.</returns>
        protected abstract ValidationResult Validate(T instance);
    }
}
