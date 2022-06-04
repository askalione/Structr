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
        public async Task GetStateMachineAsync(FooState state, string content, FooAction[] expected)
        {
            // Arrange
            var foo = new Foo { Content = content };
            foo.ChangeState(state);
            var servicesProvider = new ServiceCollection()
                .AddStateflows(this.GetType().Assembly)
                .BuildServiceProvider();
            var stateMachineProvider = servicesProvider.GetRequiredService<IStateMachineProvider>();

            // Act
            var result = await stateMachineProvider.GetStateMachineAsync<Foo, FooState, FooAction>(foo, x => x.State, (x, s) => x.ChangeState(s));

            // Assert
            result.Should().BeOfType<StateMachine<FooState, FooAction>>();
            result.PermittedTriggers.Should().BeEquivalentTo(expected);
        }
    }
}