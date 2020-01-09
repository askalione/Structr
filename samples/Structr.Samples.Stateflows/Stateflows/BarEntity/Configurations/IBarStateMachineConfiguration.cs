using Structr.Samples.Stateflows.Domain.BarEntity;
using Structr.Stateflows;

namespace Structr.Samples.Stateflows.Stateflows.BarEntity.Configurations
{
    public interface IBarStateMachineConfiguration : IStateMachineConfiguration<Bar, EBarState, EBarAction>
    {
    }
}
