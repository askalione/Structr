using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations
{
    public abstract class OperationHandler<TOperation, TResult> : IOperationHandler<TOperation, TResult>
        where TOperation : IOperation<TResult>
    {
        Task<TResult> IOperationHandler<TOperation, TResult>.HandleAsync(TOperation operation, CancellationToken cancellationToken)
        {
            return Task.FromResult(Handle(operation));
        }

        protected abstract TResult Handle(TOperation operation);
    }

    public abstract class OperationHandler<TOperation> : IOperationHandler<TOperation>
        where TOperation : IOperation
    {
        Task<VoidResult> IOperationHandler<TOperation, VoidResult>.HandleAsync(TOperation operation, CancellationToken cancellationToken)
        {
            Handle(operation);
            return VoidResult.TaskValue;
        }

        protected abstract void Handle(TOperation operation);
    }
}
