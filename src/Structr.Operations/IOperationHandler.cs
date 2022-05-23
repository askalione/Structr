using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations
{
    /// <summary>
    /// Defines a handler for an operation.
    /// </summary>
    /// <typeparam name="TOperation">The type of operation being handled.</typeparam>
    /// <typeparam name="TResult">The type of result from the handler.</typeparam>
    public interface IOperationHandler<in TOperation, TResult>
        where TOperation : IOperation<TResult>
    {
        /// <summary>
        /// Handles an operation.
        /// </summary>
        /// <param name="operation">The operation to be handled.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>Result generated from operation handling.</returns>
        Task<TResult> HandleAsync(TOperation operation, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Defines a handler for an operation with no returning result.
    /// </summary>
    /// <typeparam name="TOperation">The type of operation being handled.</typeparam>
    public interface IOperationHandler<in TOperation> : IOperationHandler<TOperation, VoidResult>
        where TOperation : IOperation
    {
    }
}
