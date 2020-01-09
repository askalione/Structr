using System;
using System.Collections.Generic;

namespace Structr.Stateflows
{
    public class StateMachine<TState, TTrigger> : IStateMachine<TState, TTrigger>
    {
        private readonly Stateless.StateMachine<TState, TTrigger> _stateMachine;

        public TState State => _stateMachine.State;
        public IEnumerable<TTrigger> PermittedTriggers => _stateMachine.PermittedTriggers;

        public StateMachine(Stateless.StateMachine<TState, TTrigger> stateMachine)
        {
            if (stateMachine == null)
                throw new ArgumentNullException(nameof(stateMachine));

            _stateMachine = stateMachine;
        }

        public bool CanFire(TTrigger trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(TTrigger trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}
