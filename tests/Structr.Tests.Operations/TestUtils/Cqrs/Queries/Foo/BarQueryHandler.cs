using Structr.Operations;
using Structr.Tests.Operations.TestUtils.Cqrs;
using Structr.Tests.Operations.TestUtils.Cqrs.Queries.Foo;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Operations.Queries.Foo
{
    public class BarQueryHandler : AsyncOperationHandler<BarQuery, int>
    {
        private readonly IStringWriter _writer;

        public BarQueryHandler(IStringWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            _writer = writer;
        }

        public override async Task<int> HandleAsync(BarQuery operation, CancellationToken cancellationToken)
        {
            var sum = operation.Number1 + operation.Number2;
            var result = $"Handle FooQuery. Sum is `{sum}`";
            await _writer.WriteAsync(result);
            return sum;
        }
    }
}
