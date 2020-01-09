using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations.Internal
{

    internal class InternalOperationHandler<TOperation> : InternalHandler where TOperation : IOperation
    {
        public override Task HandleAsync(IOperation operation,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            var handler = HandlerProvider.GetHandler<IOperationHandler<TOperation>>(serviceProvider);
            return handler.HandleAsync((TOperation)operation, cancellationToken);
        }
    }

    internal class InternalOperationHandler<TOperation, TResult> : InternalHandler<TResult> where TOperation : IOperation<TResult>
    {
        public override Task<TResult> HandleAsync(IOperation operation,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            var handler = HandlerProvider.GetHandler<IOperationHandler<TOperation, TResult>>(serviceProvider);
            return handler.HandleAsync((TOperation)operation, cancellationToken);
        }
    }
}
