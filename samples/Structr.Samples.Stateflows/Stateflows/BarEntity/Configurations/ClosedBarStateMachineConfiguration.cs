using Structr.Samples.Stateflows.Domain.BarEntity;
using Structr.Stateflows;

namespace Structr.Samples.Stateflows.Stateflows.BarEntity.Configurations
{
    public class ClosedBarStateMachineConfiguration : StateMachineConfiguration<Bar, BarState, BarAction>, IBarStateMachineConfiguration
    {
        protected override void Configure(Stateless.StateMachine<BarState, BarAction> stateMachine, Bar entity)
        {
            stateMachine.Configure(BarState.Closed)
                .Permit(BarAction.Archive, BarState.Archived)
                .Permit(BarAction.Open, BarState.Opened);
        }
    }
}
