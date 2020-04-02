using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Stateflows
{
    public interface IStateMachineProvider
    {
        Task<IStateMachine<TState, TTrigger>> GetStateMachineAsync<TEntity, TState, TTrigger>(
            TEntity entity,
            Func<TEntity, TState> stateAccessor,
            Action<TEntity, TState> stateMutator,
            CancellationToken cancellationToken = default(CancellationToken)
        );
    }
}
