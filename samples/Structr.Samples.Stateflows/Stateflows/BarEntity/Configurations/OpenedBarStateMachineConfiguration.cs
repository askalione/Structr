using Structr.Samples.Stateflows.Domain.BarEntity;
using Structr.Stateflows;

namespace Structr.Samples.Stateflows.Stateflows.BarEntity.Configurations
{
    public class OpenedBarStateMachineConfiguration : StateMachineConfiguration<Bar, BarState, EBarAction>, IBarStateMachineConfiguration
    {
        protected override void Configure(Stateless.StateMachine<BarState, EBarAction> stateMachine, Bar entity)
        {
            stateMachine.Configure(BarState.Opened)
                .Permit(EBarAction.Close, BarState.Closed);
        }
    }
}
