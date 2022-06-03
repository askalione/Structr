using System.Threading;
using System.Threading.Tasks;

namespace Structr.Stateflows
{
    public abstract class StateMachineConfigurator<TEntity, TState, TTrigger> : IStateMachineConfigurator<TEntity, TState, TTrigger>
    {
        Task IStateMachineConfiguration<TEntity, TState, TTrigger>.ConfigureAsync(Stateless.StateMachine<TState, TTrigger> stateMachine,
            TEntity entity,
            CancellationToken cancellationToken)
        {
            Configure(stateMachine, entity);
            return Task.CompletedTask;
        }

        /// <inheritdoc cref="IStateMachineConfiguration{TEntity, TState, TTrigger}.ConfigureAsync"/>
        protected abstract void Configure(Stateless.StateMachine<TState, TTrigger> stateMachine, TEntity entity);
    }
}
