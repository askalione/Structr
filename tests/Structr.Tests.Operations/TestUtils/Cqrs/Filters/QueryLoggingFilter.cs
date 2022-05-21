using Structr.Operations;
using Structr.Tests.Operations.TestUtils.Cqrs.Queries;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Tests.Operations.TestUtils.Cqrs.Filters
{
    public class QueryLoggingFilter<TQuery, TResult> : IOperationFilter<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private readonly IStringWriter _writer;

        public QueryLoggingFilter(IStringWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            _writer = writer;
        }

        public async Task<TResult> FilterAsync(TQuery operation, CancellationToken cancellationToken, OperationHandlerDelegate<TResult> next)
        {
            await _writer.WriteAsync($"Query logging: Call query of type {operation.GetType().Name}");
            return await next();
        }
    }
}
