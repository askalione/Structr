using Structr.Samples.Stateflows.Domain.BarEntity;
using Structr.Samples.Stateflows.Stateflows.BarEntity.Configurations;
using Structr.Stateflows;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Stateflows.Stateflows.BarEntity
{
    public class BarStateMachineConfigurator : IStateMachineConfigurator<Bar, BarState, EBarAction>
    {
        private readonly Func<BarState, IBarStateMachineConfiguration> _factory;

        public BarStateMachineConfigurator(Func<BarState, IBarStateMachineConfiguration> factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            _factory = factory;
        }

        public async Task ConfigureAsync(Stateless.StateMachine<BarState, EBarAction> stateMachine, Bar entity, CancellationToken cancellationToken)
        {
            var configuration = _factory(entity.State);
            if (configuration != null)
                await configuration.ConfigureAsync(stateMachine, entity, cancellationToken);
        }
    }
}
