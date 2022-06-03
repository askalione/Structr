using System;

namespace Structr.Stateflows
{
    /// <summary>
    /// Extensions for <see cref="Stateless.StateMachine{TState, TTrigger}"/>
    /// </summary>
    public static class StateMachineExtensions
    {
        /// <summary>
        /// Add an internal transition without any additional action to the state machine.
        /// </summary>
        /// <typeparam name="TState">Type of object describing entity states.</typeparam>
        /// <typeparam name="TTrigger">Type representing set of possible triggers.</typeparam>
        /// <param name="configuration"></param>
        /// <param name="trigger">The accepted trigger.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static Stateless.StateMachine<TState, TTrigger>.StateConfiguration InternalTransition<TState, TTrigger>(this Stateless.StateMachine<TState, TTrigger>.StateConfiguration configuration,
            TTrigger trigger)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return configuration.InternalTransition(trigger, () => { });
        }

        /// <summary>
        /// Add an internal transition without any additional action to the state machine.
        /// </summary>
        /// <typeparam name="TState">Type of object describing entity states.</typeparam>
        /// <typeparam name="TTrigger">Type representing set of possible triggers.</typeparam>
        /// <param name="configuration"></param>
        /// <param name="trigger">The accepted trigger.</param>
        /// <param name="guard">Function that must return true in order for the trigger to be accepted.</param>
        /// <param name="guardDescription">A description of the guard condition.</param>
        public static Stateless.StateMachine<TState, TTrigger>.StateConfiguration InternalTransitionIf<TState, TTrigger>(this Stateless.StateMachine<TState, TTrigger>.StateConfiguration configuration,
            TTrigger trigger,
            Func<bool> guard,
            string guardDescription = null)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return configuration.InternalTransitionIf(trigger, x => guard.Invoke(), () => { }, guardDescription);
        }
    }
}
