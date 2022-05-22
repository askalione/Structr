using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Structr.Operations;
using Structr.Tests.Operations.TestUtils.Extensions;
using Structr.Tests.Operations.TestUtils;
using Structr.Tests.Operations.TestUtils.Cqrs;

namespace Structr.Tests.Operations
{
    public class ServiceCollectionExtensionsTests
    {
        public interface IQuery<TResult> : IOperation<TResult> { }
        public class BarQuery : IQuery<int>
        {
            public int Number1 { get; set; }
            public int Number2 { get; set; }
        }
        public class BarQueryHandler : OperationHandler<BarQuery, int>
        {
            protected override int Handle(BarQuery operation) { return 0; }
        }
        public interface ICommand<TResult> : IOperation<TResult> { }
        public interface ICommand : ICommand<VoidResult>, IOperation { }

        public class FooCommand : ICommand
        {
            public string? Name { get; set; }
        }
        public class FooCommandHandler : OperationHandler<FooCommand>
        {
            protected override void Handle(FooCommand operation) { }
        }

        [Fact]
        public void AddOperations()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWriterMock(WriterMock.New);

            // Act
            var servicesProvider = serviceCollection
                .AddOperations(this.GetType().Assembly)
                .BuildServiceProvider();

            // Assert
            servicesProvider.GetService<IOperationHandler<FooCommand>>()
                .Should().BeOfType<FooCommandHandler>();
            servicesProvider.GetService<IOperationHandler<BarQuery, int>>()
                .Should().BeOfType<BarQueryHandler>();
            servicesProvider.GetService<IOperationExecutor>()
                .Should().BeOfType<OperationExecutor>();
        }

        [Fact]
        public void AddOperations_with_options()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            var servicesProvider = serviceCollection
                .AddOperations(x => {
                    x.ExecutorType = typeof(FakeExecutor);
                }, this.GetType().Assembly)
                .BuildServiceProvider();

            // Assert
            servicesProvider.GetService<IOperationExecutor>()
                .Should().BeOfType<FakeExecutor>();
        }
    }
}