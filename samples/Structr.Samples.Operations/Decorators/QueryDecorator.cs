using Structr.Operations;
using Structr.Samples.IO;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Operations.Decorators
{
    public class QueryDecorator<TQuery, TResult> : IQueryDecorator<TQuery, TResult> where TQuery : IOperation<TResult>
    {
        private readonly IStringWriter _writer;

        public QueryDecorator(IStringWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _writer = writer;
        }

        public async Task<TResult> DecorateAsync(TQuery query, IOperationHandler<TQuery, TResult> handler, CancellationToken cancellationToken)
        {
            await _writer.WriteLineAsync($"Preprocess query `{typeof(TQuery).Name}` by `{GetType().Name}`");

            var result = await handler.HandleAsync(query, cancellationToken);

            await _writer.WriteLineAsync($"Postprocess query `{typeof(TQuery).Name}` by `{GetType().Name}`");

            return result;
        }
    }
}
