using Structr.Operations;
using Structr.Samples.IO;
using Structr.Samples.Operations.Commands.Bar;
using Structr.Samples.Operations.Commands.Baz;
using Structr.Samples.Operations.Commands.Foo;
using Structr.Samples.Operations.Queries.Foo;
using Structr.Validation;
using System;
using System.Threading.Tasks;

namespace Structr.Samples.Operations
{
    public class App : IApp
    {
        private readonly IOperationExecutor _executor;
        private readonly IStringWriter _writer;

        public App(IOperationExecutor executor, IStringWriter writer)
        {
            if (executor == null)
                throw new ArgumentNullException(nameof(executor));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _executor = executor;
            _writer = writer;
        }

        public async Task RunAsync()
        {
            // Commands
            var decoratedCommand = new FooCommand { Name = "Decorated command" };
            await ExecuteAsync(decoratedCommand);
            await _writer.WriteLineAsync("----------------");

            var regularCommand = new BarCommand { Name = "Regular command" };
            var regularCommandResult = await ExecuteAsync(regularCommand);
            await _writer.WriteLineAsync("----------------");

            var syncCommand = new BazCommand();
            var syncCommandResult = await ExecuteAsync(syncCommand);
            await _writer.WriteLineAsync($"Sync command result is `{syncCommandResult}`");
            await _writer.WriteLineAsync("----------------");

            // Queries
            var regularQuery = new FooQuery { Number1 = 3, Number2 = 4 };
            var regularQueryResult = await ExecuteAsync(regularQuery);
            await _writer.WriteLineAsync("----------------");
        }

        private async Task ExecuteAsync(IOperation operation)
        {
            try
            {
                await _executor.ExecuteAsync(operation);
            }
            catch (ValidationException ex)
            {
                foreach (var failure in ex.ValidationResult)
                {
                    await _writer.WriteLineAsync($"Validation Error: {failure.ErrorMessage}");
                }
            }
        }

        private async Task<TResult> ExecuteAsync<TResult>(IOperation<TResult> operation)
        {
            try
            {
                return await _executor.ExecuteAsync(operation);
            }
            catch (ValidationException ex)
            {
                var commandType = operation.GetType().Name;
                foreach (var failure in ex.ValidationResult)
                {
                    await _writer.WriteLineAsync($"Validation Error ({commandType}): {failure.ErrorMessage}");
                }
                return default;
            }
        }
    }
}
