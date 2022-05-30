using System;

namespace Structr.Stateflows
{
    /// <summary>
    /// Represents entity with its behavior.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity which behavior is modeled.</typeparam>
    /// <typeparam name="TState">Type of object describing entity states.</typeparam>
    /// <typeparam name="TTrigger">Type representing set of possible triggers.</typeparam>
    public class Stateflow<TEntity, TState, TTrigger>
    {
        /// <summary>
        /// Entity which behavior is modeled.
        /// </summary>
        public TEntity Entity { get; }

        /// <summary>
        /// Behavior of the entity consisting of possible actions and states.
        /// </summary>
        public IStateMachine<TState, TTrigger> StateMachine { get; }

        /// <summary>
        /// Creates an instance of <see cref="Stateflow{TEntity, TState, TTrigger}"/>.
        /// </summary>
        /// <param name="entity">Entity which behavior is modeled.</param>
        /// <param name="stateMachine">Behavior of the entity consisting of possible actions and states.</param>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> or <paramref name="stateMachine"/> is null.</exception>
        public Stateflow(TEntity entity, IStateMachine<TState, TTrigger> stateMachine)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (stateMachine == null)
                throw new ArgumentNullException(nameof(stateMachine));

            Entity = entity;
            StateMachine = stateMachine;
        }
    }
}
