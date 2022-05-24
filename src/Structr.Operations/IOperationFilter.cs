using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations
{
    /// <summary>
    /// Defines a filter method to be applied before an operation execution.
    /// </summary>
    /// <typeparam name="TOperation">The type of operation being handled.</typeparam>
    /// <typeparam name="TResult">The type of result from the handler.</typeparam>
    public interface IOperationFilter<in TOperation, TResult>
        where TOperation : IOperation<TResult>
    {
        /// <summary>
        /// Filter an executed operation.
        /// </summary>
        /// <param name="operation">The <see cref="IOperation{TResult}"/>.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <param name="next">Delegate representing next filter to be applied or operation handler itself.</param>
        /// <returns>A task that represents the execution. The task result contains the handler's response.</returns>
        Task<TResult> FilterAsync(TOperation operation, CancellationToken cancellationToken, OperationHandlerDelegate<TResult> next);
    }
}
