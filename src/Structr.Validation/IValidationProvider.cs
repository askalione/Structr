using System.Threading;
using System.Threading.Tasks;

namespace Structr.Validation
{
    /// <summary>
    /// Provides functionality for validation.
    /// </summary>
    public interface IValidationProvider
    {
        /// <summary>
        /// Asynchronously validates the <paramref name="instance"/> and returns the <see cref="ValidationResult"/>.
        /// </summary>
        /// <param name="instance">The instance of some class.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The <see cref="ValidationResult"/>.</returns>
        Task<ValidationResult> ValidateAsync(object instance, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Asynchronously validates the <paramref name="instance"/> and throws <see cref="ValidationException"/> if validation result has failures.
        /// </summary>
        /// <param name="instance">The instance of some class.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        Task ValidateAndThrowAsync(object instance, CancellationToken cancellationToken = default(CancellationToken));
    }
}
