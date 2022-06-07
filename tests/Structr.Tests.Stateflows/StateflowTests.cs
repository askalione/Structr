#nullable disable

using FluentAssertions;
using Structr.Stateflows;
using System;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.Stateflows
{
    public class StateflowTests
    {
        public class FakeStateMachine : IStateMachine<FooState, FooAction>
        {
            public FooState State => throw new NotImplementedException();
            public IEnumerable<FooAction> PermittedTriggers => throw new NotImplementedException();
            public bool CanFire(FooAction trigger) => true;
            public void Fire(FooAction trigger) { }
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
            public FooState State { get; private set; }
            public void ChangeState(FooState state)
            {
                State = state;
            }
        }

        [Fact]
        public void Ctor()
        {
            // Arrange
            var entity = new Foo();
            var stateMachine = new FakeStateMachine();

            // Act
            var result = new Stateflow<Foo, FooState, FooAction>(entity, stateMachine);

            // Assert
            result.Entity.Should().BeEquivalentTo(entity);
            result.StateMachine.Should().BeEquivalentTo(stateMachine);
        }

        [Fact]
        public void Ctor_throws_when_entity_is_null()
        {
            // Arrange
            Foo entity = null;
            var stateMachine = new FakeStateMachine();

            // Act
            Action act = () => new Stateflow<Foo, FooState, FooAction>(entity, stateMachine);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*entity*");
        }

        [Fact]
        public void Ctor_throws_when_stateMachine_is_null()
        {
            // Arrange
            var entity = new Foo();
            FakeStateMachine stateMachine = null;

            // Act
            Action act = () => new Stateflow<Foo, FooState, FooAction>(entity, stateMachine);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*stateMachine*");
        }
    }
}