using Structr.Samples.Stateflows.Domain.FooEntity;
using Structr.Stateflows;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Stateflows.Stateflows.FooEntity
{
    public static class StateMachineProviderExtensions
    {
        public static Task<IStateMachine<EFooState, EFooAction>> GetStateMachineAsync(
            this IStateMachineProvider provider,
            Foo entity,
            CancellationToken cancellationToken
            )
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return provider.GetStateMachineAsync<Foo, EFooState, EFooAction>(
                entity,
                x => x.State,
                (x, state) => x.ChangeState(state),
                cancellationToken
            );
        }
    }
}
