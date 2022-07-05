using System.Threading;
using System.Threading.Tasks;

namespace Structr.Stateflows
{
    /// <summary>
    /// Provides functionality of getting instances of <see cref="Stateflow{TEntity, TState, TTrigger}"/> for
    /// entity specified by its identifier.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity which behavior is modeled.</typeparam>
    /// <typeparam name="TKey">Type of entity's identifier.</typeparam>
    /// <typeparam name="TState">Type of object describing entity states.</typeparam>
    /// <typeparam name="TTrigger">Type representing set of possible triggers.</typeparam>
    public interface IStateflowProvider<TEntity, TKey, TState, TTrigger>
    {
        /// <summary>
        /// Gets instance of <see cref="Stateflow{TEntity, TState, TTrigger}"/> for entity with specified identifier.
        /// </summary>
        /// <param name="entityId">Identifier of entity.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Instance of <see cref="Stateflow{TEntity, TState, TTrigger}"/>.</returns>
        Task<Stateflow<TEntity, TState, TTrigger>> GetStateflowAsync(TKey entityId,
            CancellationToken cancellationToken = default);
    }
}
