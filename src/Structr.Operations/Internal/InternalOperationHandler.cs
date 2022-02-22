using System;
using System.Linq;
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
            Task HandleAsync() => HandlerProvider.GetHandler<IOperationHandler<TOperation>>(serviceProvider).HandleAsync((TOperation)operation, cancellationToken);
            var filters = serviceProvider.GetServices<IOperationFilter<TOperation>>();
            return filters
                .Reverse()
                .Aggregate((OperationHandlerDelegate)HandleAsync,
                    (next, filter) => () => filter.HandleAsync((TOperation)operation, cancellationToken, next))();
        }
    }

    internal class InternalOperationHandler<TOperation, TResult> : InternalHandler<TResult> where TOperation : IOperation<TResult>
    {
        public override Task<TResult> HandleAsync(IOperation operation,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            Task<TResult> HandleAsync() => HandlerProvider.GetHandler<IOperationHandler<TOperation, TResult>>(serviceProvider).HandleAsync((TOperation)operation, cancellationToken);
            var filters = serviceProvider.GetServices<IOperationFilter<TOperation, TResult>>();
            return filters
                .Reverse()
                .Aggregate((OperationHandlerDelegate<TResult>)HandleAsync,
                    (next, filter) => () => filter.HandleAsync((TOperation)operation, cancellationToken, next))();
        }
    }
}
