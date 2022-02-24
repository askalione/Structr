using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations.Internal
{
    internal abstract class InternalHandler<TResult>
    {
        public abstract Task<TResult> HandleAsync(IBaseOperation operation,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken);
    }
}
