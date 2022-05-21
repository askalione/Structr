using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations
{
    /// <summary>
    /// Class to be used as base for all asynchronous operation handlers.
    /// </summary>
    /// <typeparam name="TOperation">The type of operation being handled.</typeparam>
    /// <typeparam name="TResult">The type of result from the handler.</typeparam>
    public abstract class AsyncOperationHandler<TOperation, TResult> : IOperationHandler<TOperation, TResult>
        where TOperation : IOperation<TResult>
    {
        public abstract Task<TResult> HandleAsync(TOperation operation, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Class to be used as base for all synchronous operation handlers in case if no result is implied. 
    /// </summary>
    /// <typeparam name="TOperation">The type of operation being handled.</typeparam>
    public abstract class AsyncOperationHandler<TOperation> : IOperationHandler<TOperation>
        where TOperation : IOperation
    {
        async Task<VoidResult> IOperationHandler<TOperation, VoidResult>.HandleAsync(TOperation operation, CancellationToken cancellationToken)
        {
            await HandleAsync(operation, cancellationToken).ConfigureAwait(false);
            return VoidResult.Value;
        }

        protected abstract Task HandleAsync(TOperation operation, CancellationToken cancellationToken);
    }
}
