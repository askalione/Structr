using Structr.Operations;
using Structr.Tests.Operations.TestUtils.Cqrs.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Tests.Operations.TestUtils.Cqrs.Decorators
{
    public class CommandDecorator<TCommand, TResult> : BaseOperationDecorator<TCommand, TResult>, ICommandDecorator<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        private readonly IStringWriter _writer;

        public CommandDecorator(IStringWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            _writer = writer;
        }

        public override async Task<TResult> DecorateAsync(TCommand command, IOperationHandler<TCommand, TResult> handler, CancellationToken cancellationToken)
        {
            await _writer.WriteAsync($"Preprocess command `{typeof(TCommand).Name}` by `{GetType().Name}`");

            var result = await handler.HandleAsync(command, cancellationToken);

            await _writer.WriteAsync($"Postprocess command `{typeof(TCommand).Name}` by `{GetType().Name}`");

            return result;
        }
    }
}
