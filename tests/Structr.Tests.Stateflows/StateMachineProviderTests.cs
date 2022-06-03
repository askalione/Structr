#nullable disable

using Xunit;
using Structr.Stateflows;
using System;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var foo = new Foo();
            foo.ChangeState(state);
            var innerStateMachine = new Stateless.StateMachine<FooState, FooAction>(() => foo.State, s => foo.ChangeState(s));
            IStateMachineConfigurator<Foo, FooState, FooAction> configurator = new FooStateMachineConfigurator();
            await configurator.ConfigureAsync(innerStateMachine, foo);
            var sm = new StateMachine<FooState, FooAction>(innerStateMachine);

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
            var foo = new Foo { Content = content };
            foo.ChangeState(state);
            var innerStateMachine = new Stateless.StateMachine<FooState, FooAction>(() => foo.State, s => foo.ChangeState(s));
            IStateMachineConfigurator<Foo, FooState, FooAction> configurator = new FooStateMachineConfigurator();
            await configurator.ConfigureAsync(innerStateMachine, foo);
            var sm = new StateMachine<FooState, FooAction>(innerStateMachine);

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
            var foo = new Foo { Content = "abc" };
            foo.ChangeState(FooState.Unsent);
            var innerStateMachine = new Stateless.StateMachine<FooState, FooAction>(() => foo.State, s => foo.ChangeState(s));
            IStateMachineConfigurator<Foo, FooState, FooAction> configurator = new FooStateMachineConfigurator();
            await configurator.ConfigureAsync(innerStateMachine, foo);
            var sm = new StateMachine<FooState, FooAction>(innerStateMachine);

            // Act
            var result = sm.CanFire(action);

            // Assert
            result.Should().Be(expected);
        }
    }
}