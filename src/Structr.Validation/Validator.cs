using System.Threading;
using System.Threading.Tasks;

namespace Structr.Validation
{
    /// <summary>
    /// Class to be used as base for all synchronous validators.
    /// </summary>
    /// <typeparam name="T">Some class or record to validation.</typeparam>
    public abstract class Validator<T> : IValidator<T>
    {
        Task<ValidationResult> IValidator<T>.ValidateAsync(T instance, CancellationToken cancellationToken)
        {
            return Task.FromResult(Validate(instance));
        }

        /// <summary>
        /// Synchronously validate instance of T.
        /// </summary>
        /// <param name="instance">Some class or record to validation.</param>
        /// <returns>The <see cref="ValidationResult"/>.</returns>
        protected abstract ValidationResult Validate(T instance);
    }
}
