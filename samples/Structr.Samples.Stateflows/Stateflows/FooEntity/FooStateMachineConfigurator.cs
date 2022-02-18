using Structr.Samples.Stateflows.Domain.FooEntity;
using Structr.Stateflows;

namespace Structr.Samples.Stateflows.Stateflows.FooEntity
{
    public class FooStateMachineConfigurator : StateMachineConfiguration<Foo, EFooState, EFooAction>, IStateMachineConfigurator<Foo, EFooState, EFooAction>
    {
        protected override void Configure(Stateless.StateMachine<EFooState, EFooAction> stateMachine, Foo entity)
        {
            stateMachine.Configure(EFooState.Unsent)
                .PermitIf(EFooAction.Send, EFooState.Sent, () => !string.IsNullOrEmpty(entity.Email));

            stateMachine.Configure(EFooState.Sent)
                .InternalTransitionIf(EFooAction.Send, (x) => !string.IsNullOrEmpty(entity.Email), (t) => { })
                .Permit(EFooAction.Accept, EFooState.Accepted)
                .Permit(EFooAction.Decline, EFooState.Declined);
        }
    }
}
