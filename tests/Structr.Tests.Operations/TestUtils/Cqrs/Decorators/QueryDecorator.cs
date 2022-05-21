using Structr.Operations;
using Structr.Tests.Operations.TestUtils.Cqrs.Queries;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Tests.Operations.TestUtils.Cqrs.Decorators
{
    public class QueryDecorator<TQuery, TResult> : BaseOperationDecorator<TQuery, TResult>, IQueryDecorator<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private readonly IStringWriter _writer;

        public QueryDecorator(IStringWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            _writer = writer;
        }

        public override async Task<TResult> DecorateAsync(TQuery query, IOperationHandler<TQuery, TResult> handler, CancellationToken cancellationToken)
        {
            await _writer.WriteAsync($"Preprocess query `{typeof(TQuery).Name}` by `{GetType().Name}`");

            var result = await handler.HandleAsync(query, cancellationToken);

            await _writer.WriteAsync($"Postprocess query `{typeof(TQuery).Name}` by `{GetType().Name}`");

            return result;
        }
    }
}
