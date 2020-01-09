using System.Collections.Generic;

namespace Structr.Stateflows
{
    public interface IStateMachine<TState, TTrigger>
    {
        TState State { get; }
        IEnumerable<TTrigger> PermittedTriggers { get; }
        bool CanFire(TTrigger trigger);
        void Fire(TTrigger trigger);
    }
}
