using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Stateflows
{
    public class StateflowServiceOptions
    {
        public Type ProviderType { get; set; }
        public ServiceLifetime Lifetime { get; set; }

        public StateflowServiceOptions()
        {
            ProviderType = typeof(StateMachineProvider);
            Lifetime = ServiceLifetime.Scoped;
        }
    }
}
