using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations
{
    public interface IOperationExecutor
    {
        Task<TResult> ExecuteAsync<TResult>(IOperation<TResult> operation, CancellationToken cancellationToken = default);
    }
}
