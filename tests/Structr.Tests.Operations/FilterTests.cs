using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Structr.Operations;
using Structr.Tests.Operations.TestUtils.Extensions;
using Structr.Tests.Operations.TestUtils;
using System.Threading.Tasks;
using Structr.Tests.Operations.TestUtils.Cqrs;
using System.Threading;

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



        public class OmniFilter<TOperation, TResult> : IOperationFilter<TOperation, TResult>
            where TOperation : IOperation<TResult>
        {
            private readonly IStringWriter _writer;
            public OmniFilter(IStringWriter writer) => _writer = writer;
            public async Task<TResult> FilterAsync(TOperation operation, CancellationToken cancellationToken, OperationHandlerDelegate<TResult> next)
            {
                await _writer.WriteAsync("OmniFilter");
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

        [Fact]
        public async Task Multiple_filters_should_be_applied_in_registration_order()
        {
            // Arrange
            var writer = WriterMock.New;
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWriterMock(writer);
            serviceCollection.AddTransient(typeof(IOperationFilter<,>), typeof(OmniFilter<,>));
            serviceCollection.AddTransient(typeof(IOperationFilter<,>), typeof(QueryFilter<,>));
            var servicesProvider = serviceCollection
                .AddOperations(this.GetType().Assembly)
                .BuildServiceProvider();
            var executor = servicesProvider.GetRequiredService<IOperationExecutor>();
            BarQuery operation = new BarQuery { Number1 = 3, Number2 = 4 };

            // Act
            await executor.ExecuteAsync(operation);

            // Assert
            writer.Buffer.Should().BeEquivalentTo(new[] { "OmniFilter", "QueryFilter", "BarQuery 7" },
                opt => opt.WithStrictOrdering());
        }

        [Fact]
        public async Task Special_filter_should_work_only_for_intended_operation()
        {
            // Arrange
            var writer = WriterMock.New;
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWriterMock(writer);
            serviceCollection.AddTransient(typeof(IOperationFilter<,>), typeof(OmniFilter<,>));
            serviceCollection.AddTransient(typeof(IOperationFilter<,>), typeof(QueryFilter<,>));
            var servicesProvider = serviceCollection
                .AddOperations(this.GetType().Assembly)
                .BuildServiceProvider();
            var executor = servicesProvider.GetRequiredService<IOperationExecutor>();
            FooCommand operation = new FooCommand { Name = "SomeName" };

            // Act
            await executor.ExecuteAsync(operation);

            // Assert
            writer.Buffer.Should().BeEquivalentTo(new[] { "OmniFilter", "SomeName" },
                opt => opt.WithStrictOrdering());
        }
    }
}