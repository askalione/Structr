using System.Threading;
using System.Threading.Tasks;

namespace Structr.Operations
{
    /// <summary>
    /// Defines a filter method to be applied before an operation execution.
    /// </summary>
    /// <typeparam name="TOperation">The type of operation being handled.</typeparam>
    /// <typeparam name="TResult">The type of result from the handler.</typeparam>
    public interface IOperationFilter<in TOperation, TResult>
        where TOperation : IOperation<TResult>
    {
        Task<TResult> FilterAsync(TOperation operation, CancellationToken cancellationToken, OperationHandlerDelegate<TResult> next);
    }
}
