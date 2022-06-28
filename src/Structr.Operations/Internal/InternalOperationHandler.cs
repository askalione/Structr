using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations.Internal
{
    internal class InternalOperationHandler<TOperation, TResult> : InternalHandler<TResult> where TOperation : IOperation<TResult>
    {
        public override Task<TResult> HandleAsync(IBaseOperation operation,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            Task<TResult> HandleAsync() => serviceProvider
                .GetService<IOperationHandler<TOperation, TResult>>()?
                .HandleAsync((TOperation)operation, cancellationToken)
                ?? throw new InvalidOperationException($"Operation handler for operation '{typeof(TOperation).FullName}' was not found");
            var filters = serviceProvider.GetServices<IOperationFilter<TOperation, TResult>>();
            return filters
                .Reverse()
                .Aggregate((OperationHandlerDelegate<TResult>)HandleAsync,
                    (next, filter) => () => filter.FilterAsync((TOperation)operation, cancellationToken, next))();
        }
    }
}
