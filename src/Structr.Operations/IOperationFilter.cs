using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations
{
    public interface IOperationFilter<in TOperation, TResult>
        where TOperation : IOperation<TResult>
    {
        Task<TResult> FilterAsync(TOperation operation, CancellationToken cancellationToken, OperationHandlerDelegate<TResult> next);
    }
}
