using System.Threading;
using System.Threading.Tasks;

namespace Structr.Stateflows
{
    /// <summary>
    /// Specifies statemachine configuration class.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity which behavior is modeled.</typeparam>
    /// <typeparam name="TState">Type of object describing entity states.</typeparam>
    /// <typeparam name="TTrigger">Type representing set of possible triggers.</typeparam>
    public interface IStateMachineConfiguration<TEntity, TState, TTrigger>
    {
        /// <summary>
        /// Perform configuration of state machine, determining availability of triggers based on current entity's state.
        /// </summary>
        /// <param name="stateMachine">State machine instance to be configured.</param>
        /// <param name="entity">Entity which behavior is modeled.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns><see cref="Task"/> to be awaited.</returns>
        Task ConfigureAsync(Stateless.StateMachine<TState, TTrigger> stateMachine, TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
    }
}
