#nullable disable

using FluentAssertions;
using Structr.Stateflows;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.Stateflows
{
    public class StateMachineTests
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
        public void Ctor()
        {
            // Arrange
            var foo = new Foo();
            var innerStateMachine = new Stateless.StateMachine<FooState, FooAction>(() => foo.State, s => foo.ChangeState(s));

            // Act
            Action act = () => new StateMachine<FooState, FooAction>(innerStateMachine);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Ctor_throws_when_stateless_stateMachine_is_null()
        {
            // Act
            Action act = () => new StateMachine<FooState, FooAction>(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*stateMachine*");
        }

        [Theory]
        [InlineData(FooState.Unsent)]
        [InlineData(FooState.Sent)]
        public async Task State(FooState state)
        {
            // Arrange
            var sm = await GetStateMachineForTestAsync(state, "");

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
            var sm = await GetStateMachineForTestAsync(state, content);

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
            var sm = await GetStateMachineForTestAsync(FooState.Unsent, "abc");

            // Act
            var result = sm.CanFire(action);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public async Task Fire()
        {
            // Arrange
            var sm = await GetStateMachineForTestAsync(FooState.Unsent, "abc");

            // Act
            sm.Fire(FooAction.Send);

            // Assert
            sm.State.Should().Be(FooState.Sent);
        }

        [Fact]
        public async Task Fire_throws_when_unable()
        {
            // Arrange
            var sm = await GetStateMachineForTestAsync(FooState.Sent, "abc");

            // Act
            Action act = () => sm.Fire(FooAction.Send);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        private async Task<IStateMachine<FooState, FooAction>> GetStateMachineForTestAsync(FooState startState, string fooContent)
        {
            var foo = new Foo { Content = fooContent };
            foo.ChangeState(startState);
            var innerStateMachine = new Stateless.StateMachine<FooState, FooAction>(() => foo.State, s => foo.ChangeState(s));
            IStateMachineConfigurator<Foo, FooState, FooAction> configurator = new FooStateMachineConfigurator();
            await configurator.ConfigureAsync(innerStateMachine, foo);
            return new StateMachine<FooState, FooAction>(innerStateMachine);
        }
    }
}