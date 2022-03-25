using Structr.Samples.Stateflows.Domain.BarEntity;
using Structr.Stateflows;

namespace Structr.Samples.Stateflows.Stateflows.BarEntity.Configurations
{
    public class ClosedBarStateMachineConfiguration : StateMachineConfiguration<Bar, BarState, EBarAction>, IBarStateMachineConfiguration
    {
        protected override void Configure(Stateless.StateMachine<BarState, EBarAction> stateMachine, Bar entity)
        {
            stateMachine.Configure(BarState.Closed)
                .Permit(EBarAction.Archive, BarState.Archived)
                .Permit(EBarAction.Open, BarState.Opened);
        }
    }
}
