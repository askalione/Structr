using System.Threading;
using System.Threading.Tasks;

namespace Structr.Stateflows
{
    public abstract class StateMachineConfiguration<TEntity, TState, TTrigger> : IStateMachineConfiguration<TEntity, TState, TTrigger>
    {
        Task IStateMachineConfiguration<TEntity, TState, TTrigger>.ConfigureAsync(
            Stateless.StateMachine<TState, TTrigger> stateMachine,
            TEntity entity, CancellationToken cancellationToken
            )
        {
            Configure(stateMachine, entity);
            return Task.CompletedTask;
        }

        protected abstract void Configure(Stateless.StateMachine<TState, TTrigger> stateMachine, TEntity entity);
    }
}
