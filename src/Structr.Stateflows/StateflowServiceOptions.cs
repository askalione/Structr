using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Stateflows
{
    public class StateflowServiceOptions
    {
        public Type ProviderType { get; set; }
        public ServiceLifetime ProviderTypeServiceLifetime { get; set; }

        public StateflowServiceOptions()
        {
            ProviderType = typeof(StateMachineProvider);
            ProviderTypeServiceLifetime = ServiceLifetime.Scoped;
        }
    }
}
