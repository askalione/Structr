using System;

namespace Structr.Stateflows
{
    public class Stateflow<TEntity, TState, TTrigger>
    {
        public TEntity Entity { get; }
        public IStateMachine<TState, TTrigger> StateMachine { get; }

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
