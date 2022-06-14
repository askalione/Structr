using System;
using System.Collections.Generic;

namespace Structr.Stateflows
{
    /// <summary>
    /// Standard implementation of <see cref="IStateMachine{TState, TTrigger}"/> based on
    /// <see cref="Stateless.StateMachine{TState, TTrigger}"/> from
    /// <see href="https://github.com/dotnet-state-machine/stateless">Stateless</see> package.
    /// </summary>
    /// <typeparam name="TState">Type of object describing entity states.</typeparam>
    /// <typeparam name="TTrigger">Type representing set of possible triggers.</typeparam>
    public class StateMachine<TState, TTrigger> : IStateMachine<TState, TTrigger>
    {
        private readonly Stateless.StateMachine<TState, TTrigger> _stateMachine;

        /// <summary>
        /// Gets current state.
        /// </summary>
        public TState State => _stateMachine.State;

        /// <summary>
        /// Gets list of permitted triggers.
        /// </summary>
        public IEnumerable<TTrigger> PermittedTriggers => _stateMachine.PermittedTriggers;

        /// <summary>
        /// Creates instance of <see cref="StateMachine{TState, TTrigger}"/>
        /// </summary>
        /// <param name="stateMachine">Instance of <see cref="Stateless.StateMachine{TState, TTrigger}"/> that works under the hood.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stateMachine"/> is null.</exception>
        public StateMachine(Stateless.StateMachine<TState, TTrigger> stateMachine)
        {
            if (stateMachine == null)
            {
                throw new ArgumentNullException(nameof(stateMachine));
            }

            _stateMachine = stateMachine;
        }

        /// <summary>
        /// Returns <see langword="true"/> if trigger can be fired in the current state.
        /// </summary>
        /// <param name="trigger">Trigger to test.</param>
        /// <returns><see langword="true"/> if the trigger can be fired, <see langword="false"/> otherwise.</returns>
        public bool CanFire(TTrigger trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        /// <summary>
        /// Transition from the current state via the specified trigger. The target state
        /// is determined by the configuration of the current state. Actions associated with
        /// leaving the current state and entering the new one will be invoked.
        /// </summary>
        /// <param name="trigger">The trigger to fire.</param>
        public void Fire(TTrigger trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}
