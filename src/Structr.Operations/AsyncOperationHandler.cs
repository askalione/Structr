using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations
{
    public abstract class AsyncOperationHandler<TOperation, TResult> : IOperationHandler<TOperation, TResult>
        where TOperation : IOperation<TResult>
    {
        public abstract Task<TResult> HandleAsync(TOperation operation, CancellationToken cancellationToken);
    }

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
