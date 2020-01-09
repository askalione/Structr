using Structr.Samples.Stateflows.Domain.BarEntity;
using Structr.Stateflows;
using System;
using System.Collections.Generic;
using System.Text;

namespace Structr.Samples.Stateflows.Stateflows.BarEntity.Configurations
{
    public class ClosedBarStateMachineConfiguration : StateMachineConfiguration<Bar, EBarState, EBarAction>, IBarStateMachineConfiguration
    {
        protected override void Configure(Stateless.StateMachine<EBarState, EBarAction> stateMachine, Bar entity)
        {
            stateMachine.Configure(EBarState.Closed)
                .Permit(EBarAction.Archive, EBarState.Archived)
                .Permit(EBarAction.Open, EBarState.Opened);
        }
    }
}
