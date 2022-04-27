using Structr.Samples.Stateflows.Domain.FooEntity;
using Structr.Stateflows;

namespace Structr.Samples.Stateflows.Stateflows.FooEntity
{
    public class FooStateMachineConfigurator : StateMachineConfiguration<Foo, FooState, FooAction>, IStateMachineConfigurator<Foo, FooState, FooAction>
    {
        protected override void Configure(Stateless.StateMachine<FooState, FooAction> stateMachine, Foo entity)
        {
            stateMachine.Configure(FooState.Unsent)
                .PermitIf(FooAction.Send, FooState.Sent, () => !string.IsNullOrEmpty(entity.Email));

            stateMachine.Configure(FooState.Sent)
                .InternalTransitionIf(FooAction.Send, (x) => !string.IsNullOrEmpty(entity.Email), (t) => { })
                .Permit(FooAction.Accept, FooState.Accepted)
                .Permit(FooAction.Decline, FooState.Declined);
        }
    }
}
