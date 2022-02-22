using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations
{
    public interface IOperationFilter<in TOperation> where TOperation : IOperation
    {
        Task HandleAsync(TOperation operation, CancellationToken cancellationToken, OperationHandlerDelegate next);
    }

    public interface IOperationFilter<in TOperation, TResult> where TOperation : IOperation<TResult>
    {
        Task<TResult> HandleAsync(TOperation operation, CancellationToken cancellationToken, OperationHandlerDelegate<TResult> next);
    }
}
