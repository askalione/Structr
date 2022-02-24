using Structr.Operations;
using Structr.Samples.Operations.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Operations.Decorators
{
    public interface IQueryDecorator<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> DecorateAsync(TQuery query, IOperationHandler<TQuery, TResult> handler, CancellationToken cancellationToken);
    }
}
