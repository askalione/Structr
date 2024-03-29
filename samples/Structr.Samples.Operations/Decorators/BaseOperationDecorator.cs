using Structr.Operations;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Operations.Decorators
{
    public abstract class BaseOperationDecorator<TOperation, TResult>
        where TOperation : IOperation<TResult>
    {
        public abstract Task<TResult> DecorateAsync(TOperation operation, IOperationHandler<TOperation, TResult> handler, CancellationToken cancellationToken);
    }
}
