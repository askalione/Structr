using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations
{
    public interface IOperationExecutor
    {
        Task ExecuteAsync(IOperation operation, CancellationToken cancellationToken = default(CancellationToken));
        Task<TResult> ExecuteAsync<TResult>(IOperation<TResult> operation, CancellationToken cancellationToken = default(CancellationToken));
    }
}
