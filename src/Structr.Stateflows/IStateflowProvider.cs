using System.Threading;
using System.Threading.Tasks;

namespace Structr.Stateflows
{
    public interface IStateflowProvider<TEntity, TKey, TState, TTrigger>
    {
        Task<Stateflow<TEntity, TState, TTrigger>> GetStateflowAsync(TKey entityId,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
