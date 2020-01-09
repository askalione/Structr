using Structr.Samples.Stateflows.Domain.BarEntity;
using Structr.Stateflows;
using System;

namespace Structr.Samples.Stateflows.Stateflows.BarEntity
{
    public interface IBarStateflowProvider : IStateflowProvider<Bar, Guid, EBarState, EBarAction>
    {
    }
}
