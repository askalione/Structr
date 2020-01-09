using System.Threading;
using System.Threading.Tasks;

namespace Structr.Stateflows
{
    public interface IStateMachineConfiguration<TEntity, TState, TTrigger>
    {
        Task ConfigureAsync(Stateless.StateMachine<TState, TTrigger> stateMachine, TEntity entity, CancellationToken cancellationToken);
    }
}
