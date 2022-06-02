using System.Threading;
using System.Threading.Tasks;

namespace Structr.Validation
{
    /// <summary>
    /// Represents a type that validate some instance of class <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">Some class to validation.</typeparam>
    public interface IValidator<in T>
    {
        /// <summary>
        /// Asynchronously validates instance of <see cref="T"/>.
        /// </summary>
        /// <param name="instance">The instance of <see cref="T"/>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The <see cref="ValidationResult"/>.</returns>
        Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellationToken);
    }
}
