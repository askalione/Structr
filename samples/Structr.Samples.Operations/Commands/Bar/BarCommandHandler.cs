using Structr.Operations;
using Structr.Samples.IO;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Operations.Commands.Bar
{
    public class BarCommandHandler : AsyncOperationHandler<BarCommand, string>
    {
        private readonly IStringWriter _writer;

        public BarCommandHandler(IStringWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _writer = writer;
        }

        public override async Task<string> HandleAsync(BarCommand operation, CancellationToken cancellationToken)
        {
            var result = "Command handled";
            await _writer.WriteLineAsync($"Handle BarCommand. Name is `{operation.Name}`");
            return result;
        }
    }
}
