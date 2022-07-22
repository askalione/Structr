using System.Threading;
using System.Threading.Tasks;

namespace Structr.Validation
{
    /// <summary>
    /// Defines asynchronous validation method for insatances of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Class which instances will be validated.</typeparam>
    public interface IValidator<in T>
    {
        /// <summary>
        /// Asynchronously validates instance of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="instance">The instance of <typeparamref name="T"/> to be validated.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The <see cref="ValidationResult"/>.</returns>
        Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellationToken);
    }
}
