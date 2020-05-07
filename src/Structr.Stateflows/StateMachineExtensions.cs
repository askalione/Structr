using System;

namespace Structr.Stateflows
{
    public static class StateMachineExtensions
    {
        public static Stateless.StateMachine<TState, TTrigger>.StateConfiguration InternalTransition<TState, TTrigger>(this Stateless.StateMachine<TState, TTrigger>.StateConfiguration configuration,
            TTrigger trigger)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            return configuration.InternalTransition(trigger, () => { });
        }

        public static Stateless.StateMachine<TState, TTrigger>.StateConfiguration InternalTransitionIf<TState, TTrigger>(this Stateless.StateMachine<TState, TTrigger>.StateConfiguration configuration,
            TTrigger trigger,
            Func<bool> guard,
            string guardDescription = null)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            return configuration.InternalTransitionIf(trigger, x => guard.Invoke(), () => { }, guardDescription);
        }
    }
}
