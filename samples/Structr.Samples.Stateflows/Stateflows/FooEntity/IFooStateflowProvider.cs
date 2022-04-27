using Structr.Samples.Stateflows.Domain.FooEntity;
using Structr.Stateflows;
using System;

namespace Structr.Samples.Stateflows.Stateflows.FooEntity
{
    public interface IFooStateflowProvider : IStateflowProvider<Foo, Guid, FooState, FooAction>
    {
    }
}
