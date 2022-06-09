using System.Threading;
using System.Threading.Tasks;

namespace Structr.Validation
{
    /// <summary>
    /// Defines asynchronous validation method for insatances of <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">Class which instances will be validated.</typeparam>
    public interface IValidator<in T>
    {
        /// <summary>
        /// Asynchronously validates instance of <see cref="T"/>.
        /// </summary>
        /// <param name="instance">The instance of <see cref="T"/> to be validated.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The <see cref="ValidationResult"/>.</returns>
        Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellationToken);
    }
}
