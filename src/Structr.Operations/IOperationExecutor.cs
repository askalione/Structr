using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations
{
    /// <summary>
    /// Defines an operation executor encapsulating operation processing.
    /// </summary>
    public interface IOperationExecutor
    {
        /// <summary>
        /// Execute an operation buy passing it to pipeline to be handled by a single handler.
        /// </summary>
        /// <typeparam name="TResult">Result type.</typeparam>
        /// <param name="operation">The <see cref="IOperation{TResult}"/>.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the execution. The task result contains the handler's response.</returns>
        Task<TResult> ExecuteAsync<TResult>(IOperation<TResult> operation, CancellationToken cancellationToken = default);
    }
}