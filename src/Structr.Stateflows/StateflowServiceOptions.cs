using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Stateflows
{
    /// <summary>
    /// Allows to configure stateflow service.
    /// </summary>
    public class StateflowServiceOptions
    {
        /// <summary>
        /// Changes standard implementation of <see cref="IStateMachineProvider"/> to specified one.
        /// </summary>
        public Type ProviderServiceType { get; set; }

        /// <summary>
        /// Specifies the lifetime of an <see cref="IStateMachineProvider"/> service.
        /// </summary>
        public ServiceLifetime ProviderServiceLifetime { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="StateflowServiceOptions"/> with the default values.
        /// </summary>
        public StateflowServiceOptions()
        {
            ProviderServiceType = typeof(StateMachineProvider);
            ProviderServiceLifetime = ServiceLifetime.Transient;
        }
    }
}
