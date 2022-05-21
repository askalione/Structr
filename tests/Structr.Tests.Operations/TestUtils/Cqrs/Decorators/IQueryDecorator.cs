using Structr.Operations;
using Structr.Tests.Operations.TestUtils.Cqrs.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Tests.Operations.TestUtils.Cqrs.Decorators
{
    public interface IQueryDecorator<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> DecorateAsync(TQuery query, IOperationHandler<TQuery, TResult> handler, CancellationToken cancellationToken);
    }
}
