#nullable disable

using Xunit;
using Structr.Stateflows;
using FluentAssertions;

namespace Structr.Tests.Stateflows
{
    public class StateMachineExtensionsTests
    {
        public enum FooAction
        {
            Edit,
            Verify,
            Send
        }
        public enum FooState
        {
            Unsent,
            Sent
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

        [Fact]
        public void InternalTransition()
        {
            // Arrange
            var foo = new Foo { Content = "" };
            foo.ChangeState(FooState.Unsent);
            var ism = new Stateless.StateMachine<FooState, FooAction>(() => foo.State, s => foo.ChangeState(s));

            // Act
            ism.Configure(FooState.Unsent).InternalTransition(FooAction.Edit);

            // Assert
            ism.CanFire(FooAction.Edit).Should().BeTrue();
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("abc", true)]
        public void InternalTransitionIf(string content, bool expected)
        {
            // Arrange
            var foo = new Foo { Content = content };
            foo.ChangeState(FooState.Unsent);
            var ism = new Stateless.StateMachine<FooState, FooAction>(() => foo.State, s => foo.ChangeState(s));

            // Act
            ism.Configure(FooState.Unsent).InternalTransitionIf(FooAction.Verify, () => string.IsNullOrEmpty(foo.Content) == false);

            // Assert
            ism.CanFire(FooAction.Verify).Should().Be(expected);
        }
    }
}