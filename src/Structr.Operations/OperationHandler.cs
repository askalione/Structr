using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations
{
    /// <summary>
    /// Class to be used as base for all synchronous operation handlers.
    /// </summary>
    /// <typeparam name="TOperation">The type of operation being handled.</typeparam>
    /// <typeparam name="TResult">The type of result from the handler.</typeparam>
    public abstract class OperationHandler<TOperation, TResult> : IOperationHandler<TOperation, TResult>
        where TOperation : IOperation<TResult>
    {
        Task<TResult> IOperationHandler<TOperation, TResult>.HandleAsync(TOperation operation, CancellationToken cancellationToken)
        {
            return Task.FromResult(Handle(operation));
        }

        protected abstract TResult Handle(TOperation operation);
    }

    /// <summary>
    /// Class to be used as base for all synchronous operation handlers in case if no result is implied. 
    /// </summary>
    /// <typeparam name="TOperation">The type of operation being handled.</typeparam>
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
