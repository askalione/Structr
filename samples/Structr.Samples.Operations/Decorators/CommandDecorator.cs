using Structr.Operations;
using Structr.Samples.IO;
using Structr.Samples.Operations.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Operations.Decorators
{
    public class CommandDecorator<TCommand, TResult> : BaseOperationDecorator<TCommand, TResult>, ICommandDecorator<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        private readonly IStringWriter _writer;

        public CommandDecorator(IStringWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _writer = writer;
        }

        public override async Task<TResult> DecorateAsync(TCommand command, IOperationHandler<TCommand, TResult> handler, CancellationToken cancellationToken)
        {
            await _writer.WriteLineAsync($"Preprocess command `{typeof(TCommand).Name}` by `{GetType().Name}`");

            var result = await handler.HandleAsync(command, cancellationToken);

            await _writer.WriteLineAsync($"Postprocess command `{typeof(TCommand).Name}` by `{GetType().Name}`");

            return result;
        }
    }
}
