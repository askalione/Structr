using System.Collections.Generic;

namespace Structr.Stateflows
{
    /// <summary>
    /// Models behaviour as transitions between a finite set of states.
    /// </summary>
    /// <typeparam name="TState">The type used to represent the states.</typeparam>
    /// <typeparam name="TTrigger">The type used to represent the triggers that cause state transitions.</typeparam>
    public interface IStateMachine<TState, TTrigger>
    {
        /// <summary>
        /// Current state.
        /// </summary>
        TState State { get; }

        /// <summary>
        /// Set of permited triggers in current set.
        /// </summary>
        IEnumerable<TTrigger> PermittedTriggers { get; }

        /// <summary>
        /// Determines whenever specified trigger can be fired in current state.
        /// </summary>
        /// <param name="trigger">Trigger to test.</param>
        /// <returns><see langword="true"/>if the trigger can be fired, <see langword="false"/> otherwise.</returns>
        bool CanFire(TTrigger trigger);

        /// <summary>
        /// Transition from the current state via the specified trigger. The target state
        /// is determined by the configuration of the current state. Actions associated with
        /// leaving the current state and entering the new one will be invoked.
        /// </summary>
        /// <param name="trigger">The trigger to fire.</param>
        void Fire(TTrigger trigger);
    }
}
