using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations
{
    /// <summary>
    /// Defines an operation executor encapsulating operation (command or query) processing.
    /// </summary>
    public interface IOperationExecutor
    {
        /// <summary>
        /// Asynchronously execute an operation buy passing it to pipeline to be handled by a single handler.
        /// </summary>
        /// <typeparam name="TResult">Result type.</typeparam>
        /// <param name="operation">Operation object.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A task that represents the execution. The task result contains the handler's response.</returns>
        Task<TResult> ExecuteAsync<TResult>(IOperation<TResult> operation, CancellationToken cancellationToken = default);
    }
}