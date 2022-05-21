using Structr.Operations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Tests.Operations.TestUtils.Cqrs.Commands.Foo
{
    public class FooCommandHandler : AsyncOperationHandler<FooCommand>
    {
        private readonly IStringWriter _writer;

        public FooCommandHandler(IStringWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            _writer = writer;
        }

        protected override Task HandleAsync(FooCommand operation, CancellationToken cancellationToken)
        {
            return _writer.WriteAsync($"Handle FooCommand. Name is `{operation.Name}`");
        }
    }
}
