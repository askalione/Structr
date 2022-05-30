using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Stateflows
{
    /// <summary>
    /// Specifies service for providing instances that implement <see cref="IStateMachine{TState, TTrigger}"/> inteface.
    /// </summary>
    public interface IStateMachineProvider
    {
        /// <summary>
        /// Get an instance of <see cref="IStateMachine"/> that is suitable for modeling entity behavior.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity which behavior is modeled.</typeparam>
        /// <typeparam name="TState">Type of object describing entity states.</typeparam>
        /// <typeparam name="TTrigger">Type representing set of possible triggers.</typeparam>
        /// <param name="entity">Entity which behavior is modeled.</param>
        /// <param name="stateAccessor"><see cref="Func{TEntity, TState}"/> specifying entity's property reflecting its state.</param>
        /// <param name="stateMutator"><see cref="Action{TEntity, TState}"/> specifying method which will be used to modify entity state when it's changing.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Task with <see cref="IStateMachine{TState, TTrigger}"/> instance.</returns>
        Task<IStateMachine<TState, TTrigger>> GetStateMachineAsync<TEntity, TState, TTrigger>(
            TEntity entity,
            Func<TEntity, TState> stateAccessor,
            Action<TEntity, TState> stateMutator,
            CancellationToken cancellationToken = default(CancellationToken)
        );
    }
}
