#nullable disable

using Xunit;
using Structr.Stateflows;
using System;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Structr.Tests.Stateflows
{
    public class StateMachineProviderTests
    {
        public enum FooAction
        {
            Send,
            Accept,
            Decline
        }
        public enum FooState
        {
            Unsent,
            Sent,
            Accepted,
            Declined
        }
        public class Foo
        {
            public int Id { get; set; }
            public string Content { get; set; }
            public FooState State { get; private set; }
            public void ChangeState(FooState state)
            {
                State = state;
            }
        }
        public class FooStateMachineConfigurator : StateMachineConfiguration<Foo, FooState, FooAction>, IStateMachineConfigurator<Foo, FooState, FooAction>
        {
            protected override void Configure(Stateless.StateMachine<FooState, FooAction> stateMachine, Foo entity)
            {
                stateMachine.Configure(FooState.Unsent)
                    .PermitIf(FooAction.Send, FooState.Sent, () => string.IsNullOrEmpty(entity.Content) == false);

                stateMachine.Configure(FooState.Sent)
                    .Permit(FooAction.Accept, FooState.Accepted)
                    .Permit(FooAction.Decline, FooState.Declined);
            }
        }

        [Fact]
        public async Task GetStateMachineAsync()
        {
            // Act
            var result = await GetStateMachineAsyncForTest(FooState.Unsent, "");

            // Assert
            result.Should().BeOfType<StateMachine<FooState, FooAction>>();
        }

        [Theory]
        [InlineData(FooState.Unsent)]
        [InlineData(FooState.Sent)]
        public async Task State(FooState state)
        {
            // Arrange
            var sm = await GetStateMachineAsyncForTest(state, "");

            // Act
            var result = sm.State;

            // Assert
            result.Should().Be(state);
        }

        public class PermittedTriggersTheoryData : TheoryData<FooState, string, FooAction[]>
        {
            public PermittedTriggersTheoryData()
            {
                Add(FooState.Unsent, "", new FooAction[] { });
                Add(FooState.Unsent, "abc", new FooAction[] { FooAction.Send });
                Add(FooState.Sent, "abc", new FooAction[] { FooAction.Accept, FooAction.Decline });
            }
        }
        [Theory]
        [ClassData(typeof(PermittedTriggersTheoryData))]
        public async Task PermittedTriggers(FooState state, string content, FooAction[] expected)
        {
            // Arrange
            var sm = await GetStateMachineAsyncForTest(state, content);

            // Act
            var result = sm.PermittedTriggers;

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(FooAction.Send, true)]
        [InlineData(FooAction.Accept, false)]
        [InlineData(FooAction.Decline, false)]
        public async Task CanFire(FooAction action, bool expected)
        {
            // Arrange
            var sm = await GetStateMachineAsyncForTest(FooState.Unsent, "abc");

            // Act
            var result = sm.CanFire(action);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public async Task Fire()
        {
            // Arrange
            var sm = await GetStateMachineAsyncForTest(FooState.Unsent, "abc");

            // Act
            sm.Fire(FooAction.Send);

            // Assert
            sm.State.Should().Be(FooState.Sent);
        }

        [Fact]
        public async Task Fire_throws_when_unable()
        {
            // Arrange
            var sm = await GetStateMachineAsyncForTest(FooState.Sent, "abc");

            // Act
            Action act = () => sm.Fire(FooAction.Send);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        private async Task<IStateMachine<FooState, FooAction>> GetStateMachineAsyncForTest(FooState startState, string fooContent)
        {
            var foo = new Foo { Content = fooContent };
            foo.ChangeState(startState);
            var servicesProvider = new ServiceCollection()
                .AddStateflows(this.GetType().Assembly)
                .BuildServiceProvider();
            var stateMachineProvider = servicesProvider.GetRequiredService<IStateMachineProvider>();
            var sm = await stateMachineProvider.GetStateMachineAsync<Foo, FooState, FooAction>(foo, x => x.State, (x, s) => x.ChangeState(s));
            return sm;
        }
    }
}