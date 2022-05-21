using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Structr.Operations;
using Structr.Tests.Operations.TestUtils.Cqrs.Commands.Foo;
using Structr.Tests.Operations.TestUtils.Cqrs.Queries.Foo;
using Structr.Tests.Operations.TestUtils.Extensions;
using Structr.Tests.Operations.TestUtils;
using Structr.Samples.Operations.Queries.Foo;
using Structr.Tests.Operations.TestUtils.Cqrs;

namespace Structr.Tests.Operations
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddOperations()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSpyWriter(WriterMock.New);

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