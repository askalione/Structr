using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Structr.Stateflows;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.Stateflows
{
    public class ServiceCollectionExtensionsTests
    {
        public class FakeStateMachineProvider : IStateMachineProvider
        {
            public Task<IStateMachine<TState, TTrigger>> GetStateMachineAsync<TEntity, TState, TTrigger>(TEntity entity, Func<TEntity, TState> stateAccessor, Action<TEntity, TState> stateMutator, CancellationToken cancellationToken = default)
            { throw new NotImplementedException(); }
        }
        public enum FooAction
        {
            Accept,
            Decline
        }
        public enum FooState
        {
            Accepted,
            Declined
        }
        public class Foo
        {
            public int Id { get; set; }
            public FooState State { get; private set; }
            public void ChangeState(FooState state)
            {
                State = state;
            }
        }
        public interface IFooStateflowProvider : IStateflowProvider<Foo, int, FooState, FooAction> { }
        public class FooStateflowProvider : IFooStateflowProvider
        {
#pragma warning disable CS1998
            public async Task<Stateflow<Foo, FooState, FooAction>> GetStateflowAsync(int entityId, CancellationToken cancellationToken)
            { throw new NotImplementedException(); }
#pragma warning restore CS1998
        }
        public class FooStateMachineConfigurator : IStateMachineConfigurator<Foo, FooState, FooAction>
        {
            public Task ConfigureAsync(Stateless.StateMachine<FooState, FooAction> stateMachine, Foo entity, CancellationToken cancellationToken = default)
            { throw new NotImplementedException(); }
        }

        [Fact]
        public void AddStateflows()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            var servicesProvider = serviceCollection
                .AddStateflows(this.GetType().Assembly)
                .BuildServiceProvider();

            // Assert
            servicesProvider.GetService<IStateMachineProvider>()
                .Should().BeOfType<StateMachineProvider>();
            servicesProvider.GetService<IFooStateflowProvider>()
                .Should().BeOfType<FooStateflowProvider>();
            servicesProvider.GetService<IStateMachineConfigurator<Foo, FooState, FooAction>>()
                .Should().BeOfType<FooStateMachineConfigurator>();
        }

        [Fact]
        public void AddStateflows_with_options()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            var servicesProvider = serviceCollection
                .AddStateflows(x =>
                    {
                        x.ProviderType = typeof(FakeStateMachineProvider);
                    },
                    this.GetType().Assembly)
                .BuildServiceProvider();

            // Assert
            servicesProvider.GetService<IStateMachineProvider>()
                .Should().BeOfType<FakeStateMachineProvider>();
        }
    }
}