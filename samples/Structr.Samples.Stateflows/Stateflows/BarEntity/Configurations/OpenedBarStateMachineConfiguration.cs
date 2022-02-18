using Structr.Samples.Stateflows.Domain.BarEntity;
using Structr.Stateflows;

namespace Structr.Samples.Stateflows.Stateflows.BarEntity.Configurations
{
    public class OpenedBarStateMachineConfiguration : StateMachineConfiguration<Bar, EBarState, EBarAction>, IBarStateMachineConfiguration
    {
        protected override void Configure(Stateless.StateMachine<EBarState, EBarAction> stateMachine, Bar entity)
        {
            stateMachine.Configure(EBarState.Opened)
                .Permit(EBarAction.Close, EBarState.Closed);
        }
    }
}
