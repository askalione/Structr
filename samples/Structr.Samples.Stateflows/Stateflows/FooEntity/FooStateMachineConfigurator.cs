using Structr.Samples.Stateflows.Domain.FooEntity;
using Structr.Stateflows;

namespace Structr.Samples.Stateflows.Stateflows.FooEntity
{
    public class FooStateMachineConfigurator : StateMachineConfiguration<Foo, FooState, EFooAction>, IStateMachineConfigurator<Foo, FooState, EFooAction>
    {
        protected override void Configure(Stateless.StateMachine<FooState, EFooAction> stateMachine, Foo entity)
        {
            stateMachine.Configure(FooState.Unsent)
                .PermitIf(EFooAction.Send, FooState.Sent, () => !string.IsNullOrEmpty(entity.Email));

            stateMachine.Configure(FooState.Sent)
                .InternalTransitionIf(EFooAction.Send, (x) => !string.IsNullOrEmpty(entity.Email), (t) => { })
                .Permit(EFooAction.Accept, FooState.Accepted)
                .Permit(EFooAction.Decline, FooState.Declined);
        }
    }
}
