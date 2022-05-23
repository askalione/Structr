using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Structr.Operations;
using Structr.Tests.Operations.TestUtils;
using Structr.Tests.Operations.TestUtils.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.Operations
{
    public class FilterTests
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

        public class FooCommand : IOperation
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

        public class UniversalFilter<TOperation, TResult> : IOperationFilter<TOperation, TResult>
            where TOperation : IOperation<TResult>
        {
            private readonly IStringWriter _writer;
            public UniversalFilter(IStringWriter writer) => _writer = writer;
            public async Task<TResult> FilterAsync(TOperation operation, CancellationToken cancellationToken, OperationHandlerDelegate<TResult> next)
            {
                await _writer.WriteAsync("UniversalFilter");
                return await next();
            }
        }
        public class QueryFilter<TQuery, TResult> : IOperationFilter<TQuery, TResult>
            where TQuery : IQuery<TResult>
        {
            private readonly IStringWriter _writer;
            public QueryFilter(IStringWriter writer) => _writer = writer;
            public async Task<TResult> FilterAsync(TQuery operation, CancellationToken cancellationToken, OperationHandlerDelegate<TResult> next)
            {
                await _writer.WriteAsync("QueryFilter");
                return await next();
            }
        }

        public class AfterFilter<TOperation, TResult> : IOperationFilter<TOperation, TResult>
            where TOperation : IOperation<TResult>
        {
            private readonly IStringWriter _writer;
            public AfterFilter(IStringWriter writer) => _writer = writer;
            public async Task<TResult> FilterAsync(TOperation operation, CancellationToken cancellationToken, OperationHandlerDelegate<TResult> next)
            {
                var result = await next();
                await _writer.WriteAsync("AfterFilter");
                return result;
            }
        }

        [Fact]
        public async Task Multiple_filters_should_be_applied_in_registration_order()
        {
            // Arrange
            var writer = MockWriter.NewMockWriter();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMockWriter(writer);
            serviceCollection.AddTransient(typeof(IOperationFilter<,>), typeof(UniversalFilter<,>));
            serviceCollection.AddTransient(typeof(IOperationFilter<,>), typeof(QueryFilter<,>));
            var servicesProvider = serviceCollection
                .AddOperations(this.GetType().Assembly)
                .BuildServiceProvider();
            var executor = servicesProvider.GetRequiredService<IOperationExecutor>();
            BarQuery operation = new BarQuery { Number1 = 3, Number2 = 4 };

            // Act
            await executor.ExecuteAsync(operation);

            // Assert
            writer.Buffer.Should().BeEquivalentTo(new[] { "UniversalFilter", "QueryFilter", "BarQuery 7" },
                opt => opt.WithStrictOrdering());
        }

        [Fact]
        public async Task Filter_works_after_handler()
        {
            // Arrange
            var writer = MockWriter.NewMockWriter();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMockWriter(writer);
            serviceCollection.AddTransient(typeof(IOperationFilter<,>), typeof(AfterFilter<,>));
            var servicesProvider = serviceCollection
                .AddOperations(this.GetType().Assembly)
                .BuildServiceProvider();
            var executor = servicesProvider.GetRequiredService<IOperationExecutor>();
            BarQuery operation = new BarQuery { Number1 = 3, Number2 = 4 };

            // Act
            await executor.ExecuteAsync(operation);

            // Assert
            writer.Buffer.Should().BeEquivalentTo(new[] { "BarQuery 7", "AfterFilter" },
                opt => opt.WithStrictOrdering());
        }

        [Fact]
        public async Task Special_filter_should_work_only_for_intended_operation()
        {
            // Arrange
            var writer = MockWriter.NewMockWriter();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMockWriter(writer);
            serviceCollection.AddTransient(typeof(IOperationFilter<,>), typeof(UniversalFilter<,>));
            serviceCollection.AddTransient(typeof(IOperationFilter<,>), typeof(QueryFilter<,>));
            var servicesProvider = serviceCollection
                .AddOperations(this.GetType().Assembly)
                .BuildServiceProvider();
            var executor = servicesProvider.GetRequiredService<IOperationExecutor>();
            FooCommand operation = new FooCommand { Name = "SomeName" };

            // Act
            await executor.ExecuteAsync(operation);

            // Assert
            writer.Buffer.Should().BeEquivalentTo(new[] { "UniversalFilter", "SomeName" },
                opt => opt.WithStrictOrdering());
        }
    }
}