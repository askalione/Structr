using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Structr.Operations;
using Structr.Tests.Operations.TestUtils;
using Structr.Tests.Operations.TestUtils.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.Operations
{
    public class OperationExecutorTests
    {
        public interface IQuery<TResult> : IOperation<TResult> { }
        public class BarQuery : IQuery<int>
        {
            public int Number1 { get; set; }
            public int Number2 { get; set; }
        }
        public class BarQueryHandler : OperationHandler<BarQuery, int>
        {
            private readonly IStringWriter _writer;
            public BarQueryHandler(IStringWriter writer) => _writer = writer;
            protected override int Handle(BarQuery operation)
            {
                var sum = operation.Number1 + operation.Number2;
                var result = $"BarQuery {sum}";
                _writer.Write(result);
                return sum;
            }
        }

        public class BarAsyncQuery : IQuery<int>
        {
            public int Number1 { get; set; }
            public int Number2 { get; set; }
        }
        public class BarAsyncQueryHandler : AsyncOperationHandler<BarAsyncQuery, int>
        {
            private readonly IStringWriter _writer;
            public BarAsyncQueryHandler(IStringWriter writer) => _writer = writer;
            public override async Task<int> HandleAsync(BarAsyncQuery operation, CancellationToken cancellationToken)
            {
                var multiplication = operation.Number1 * operation.Number2;
                var result = $"BarAsyncQuery {multiplication}";
                await _writer.WriteAsync(result);
                return multiplication;
            }
        }

        public interface ICommand<TResult> : IOperation<TResult> { }
        public interface ICommand : ICommand<VoidResult>, IOperation { }

        public class FooCommand : ICommand
        {
            public string? Name { get; set; }
        }
        public class FooCommandHandler : OperationHandler<FooCommand>
        {
            private readonly IStringWriter _writer;
            public FooCommandHandler(IStringWriter writer) => _writer = writer;
            protected override void Handle(FooCommand operation)
            {
                _writer.Write(operation.Name);
            }
        }

        public class FooAsyncCommand : ICommand
        {
            public string? Name { get; set; }
        }
        public class FooAsyncCommandHandler : AsyncOperationHandler<FooAsyncCommand>
        {
            private readonly IStringWriter _writer;
            public FooAsyncCommandHandler(IStringWriter writer) => _writer = writer;
            protected override Task HandleAsync(FooAsyncCommand operation, CancellationToken cancellationToken)
            {
                return _writer.WriteAsync(operation.Name);
            }
        }

        public class BazCommand : ICommand
        { }

        [Fact]
        public async Task Command()
        {
            // Arrange
            var writer = MockWriter.NewMockWriter();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMockWriter(writer);
            var servicesProvider = serviceCollection
                .AddOperations(this.GetType().Assembly)
                .BuildServiceProvider();
            var executor = servicesProvider.GetRequiredService<IOperationExecutor>();
            FooCommand command = new FooCommand { Name = "FooCommandTest" };

            // Act
            var result = await executor.ExecuteAsync(command);

            // Assert
            result.Should().Be(VoidResult.Value);
            writer.Buffer.Should().BeEquivalentTo(new[] { "FooCommandTest" });
        }

        [Fact]
        public async Task AsyncCommand()
        {
            // Arrange
            var writer = MockWriter.NewMockWriter();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMockWriter(writer);
            var servicesProvider = serviceCollection
                .AddOperations(this.GetType().Assembly)
                .BuildServiceProvider();
            var executor = servicesProvider.GetRequiredService<IOperationExecutor>();
            FooAsyncCommand command = new FooAsyncCommand { Name = "FooAsyncCommandTest" };

            // Act
            var result = await executor.ExecuteAsync(command);

            // Assert
            result.Should().Be(VoidResult.Value);
            writer.Buffer.Should().BeEquivalentTo(new[] { "FooAsyncCommandTest" });
        }

        [Fact]
        public async Task Query()
        {
            // Arrange
            var writer = MockWriter.NewMockWriter();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMockWriter(writer);
            var servicesProvider = serviceCollection
                .AddOperations(this.GetType().Assembly)
                .BuildServiceProvider();
            var executor = servicesProvider.GetRequiredService<IOperationExecutor>();

            // Act
            var result = await executor.ExecuteAsync(new BarQuery { Number1 = 3, Number2 = 4 });

            // Assert
            result.Should().Be(7);
            writer.Buffer.Should().BeEquivalentTo(new[] { "BarQuery 7" });
        }

        [Fact]
        public async Task AsyncQuery()
        {
            // Arrange
            var writer = MockWriter.NewMockWriter();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMockWriter(writer);
            var servicesProvider = serviceCollection
                .AddOperations(this.GetType().Assembly)
                .BuildServiceProvider();
            var executor = servicesProvider.GetRequiredService<IOperationExecutor>();

            // Act
            var result = await executor.ExecuteAsync(new BarAsyncQuery { Number1 = 3, Number2 = 4 });

            // Assert
            result.Should().Be(12);
            writer.Buffer.Should().BeEquivalentTo(new[] { "BarAsyncQuery 12" });
        }

        [Fact]
        public async Task Null_query_throws()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var servicesProvider = serviceCollection
                .AddOperations(this.GetType().Assembly)
                .BuildServiceProvider();
            var executor = servicesProvider.GetRequiredService<IOperationExecutor>();

            BarAsyncQuery? query = null;

            // Act
            Func<Task> func = async () => await executor.ExecuteAsync(query);

            // Assert
            await func.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Throws_when_no_handler_found()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var servicesProvider = serviceCollection
                .AddOperations(this.GetType().Assembly)
                .BuildServiceProvider();
            var executor = servicesProvider.GetRequiredService<IOperationExecutor>();

            // Act
            Func<Task> func = async () => await executor.ExecuteAsync(new BazCommand());

            // Assert
            await func.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Operation handler for operation '{typeof(BazCommand).FullName}' was not found");
        }
    }
}