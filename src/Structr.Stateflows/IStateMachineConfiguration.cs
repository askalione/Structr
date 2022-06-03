using System.Threading;
using System.Threading.Tasks;

namespace Structr.Stateflows
{
    /// <summary>
    /// Specifies statemachine configuration class that could be used in cases when sophisticated
    /// individual behavior should be defined for each entity's state.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity which behavior is modeled.</typeparam>
    /// <typeparam name="TState">Type of object describing entity states.</typeparam>
    /// <typeparam name="TTrigger">Type representing set of possible triggers.</typeparam>
    /// <remarks>
    /// To use this the <see cref="IStateMachineConfiguration{TEntity, TState, TTrigger}"/> factory like
    /// Func&lt;FooStateEnum, IFooStateMachineConfiguration&gt; should be registred before. Then this
    /// factory should be injected into FooStateMachineConfigurator. See more about injecting factories
    /// <seealso href="https://github.com/askalione/Structr/blob/develop/wiki/Abstractions/Extensions/Abstractions-ServiceCollectionExtensions.md">here</seealso>.
    /// </remarks>
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
