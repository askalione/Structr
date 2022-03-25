using Structr.Samples.Stateflows.Domain.BarEntity;
using Structr.Stateflows;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Stateflows.Stateflows.BarEntity
{
    public static class StateMachineProviderExtensions
    {
        public static Task<IStateMachine<BarState, EBarAction>> GetStateMachineAsync(
            this IStateMachineProvider provider,
            Bar entity,
            CancellationToken cancellationToken
            )
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return provider.GetStateMachineAsync<Bar, BarState, EBarAction>(
                entity,
                x => x.State,
                (x, state) => x.ChangeState(state),
                cancellationToken
            );
        }
    }
}
