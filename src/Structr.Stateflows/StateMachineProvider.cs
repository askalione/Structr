using Structr.Stateflows.Internal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Stateflows
{
    /// <summary>
    /// <see cref="IStateMachineProvider"/> implementation based on <see cref="Stateless.StateMachine{TState, TTrigger}"/>
    /// from <seealso href="https://github.com/dotnet-state-machine/stateless">this</seealso> project.
    /// </summary>
    public class StateMachineProvider : IStateMachineProvider
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Creates instance of <see cref="StateMachineProvider"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public StateMachineProvider(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
        }

        public async Task<IStateMachine<TState, TTrigger>> GetStateMachineAsync<TEntity, TState, TTrigger>(
            TEntity entity,
            Func<TEntity, TState> stateAccessor,
            Action<TEntity, TState> stateMutator,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (stateAccessor == null)
                throw new ArgumentNullException(nameof(stateAccessor));
            if (stateMutator == null)
                throw new ArgumentNullException(nameof(stateMutator));

            Stateless.StateMachine<TState, TTrigger> stateMachine;

            try
            {
                stateMachine = new Stateless.StateMachine<TState, TTrigger>(() => stateAccessor(entity),
                    state => stateMutator(entity, state));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error constructing state machine for entity {typeof(TEntity)}", ex);
            }

            var configurator = ConfiguratorProvider.GetConfigurator<TEntity, TState, TTrigger>(_serviceProvider);
            if (configurator != null)
                await configurator.ConfigureAsync(stateMachine, entity, cancellationToken).ConfigureAwait(false);

            return new StateMachine<TState, TTrigger>(stateMachine);
        }
    }
}
