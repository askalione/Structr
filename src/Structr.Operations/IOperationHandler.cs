using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations
{
    public interface IOperationHandler<in TOperation, TResult>
        where TOperation : IOperation<TResult>
    {
        Task<TResult> HandleAsync(TOperation operation, CancellationToken cancellationToken);
    }

    public interface IOperationHandler<in TOperation> : IOperationHandler<TOperation, VoidResult>
        where TOperation : IOperation
    {
    }
}
