using Structr.Operations;
using Structr.Samples.IO;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Operations.Queries.Foo
{
    public class FooQueryHandler : IOperationHandler<FooQuery, int>
    {
        private readonly IStringWriter _writer;

        public FooQueryHandler(IStringWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _writer = writer;
        }

        public async Task<int> HandleAsync(FooQuery operation, CancellationToken cancellationToken)
        {
            var sum = operation.Number1 + operation.Number2;
            var result = $"Handle FooQuery. Sum is `{sum}`";
            await _writer.WriteLineAsync(result);
            return sum;
        }
    }
}
