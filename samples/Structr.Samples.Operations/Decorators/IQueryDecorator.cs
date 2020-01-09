using Structr.Operations;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Operations.Decorators
{
    public interface IQueryDecorator<TQuery, TResult> where TQuery : IOperation<TResult>
    {
        Task<TResult> DecorateAsync(TQuery query, IOperationHandler<TQuery, TResult> handler, CancellationToken cancellationToken);
    }
}
