using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations.Internal
{
    internal abstract class InternalHandler
    {
        public abstract Task HandleAsync(IOperation operation,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken);
    }

    internal abstract class InternalHandler<TResult>
    {
        public abstract Task<TResult> HandleAsync(IOperation operation,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken);
    }
}
