using Structr.Samples.Stateflows.DataAccess;
using Structr.Samples.Stateflows.Domain.BarEntity;
using Structr.Stateflows;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Stateflows.Stateflows.BarEntity
{
    public class BarStateflowProvider : IBarStateflowProvider
    {
        private readonly IRepository<Bar> _repository;
        private readonly IStateMachineProvider _stateMachineProvider;

        public BarStateflowProvider(IRepository<Bar> repository,
            IStateMachineProvider stateMachineProvider)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            if (stateMachineProvider == null)
                throw new ArgumentNullException(nameof(stateMachineProvider));

            _repository = repository;
            _stateMachineProvider = stateMachineProvider;
        }

        public async Task<Stateflow<Bar, BarState, EBarAction>> GetStateflowAsync(Guid entityId, CancellationToken cancellationToken)
        {
            var entity = _repository.Get(entityId);
            if (entity == null)
                throw new InvalidOperationException("Entity not found");

            var stateMachine = await _stateMachineProvider.GetStateMachineAsync(entity, cancellationToken);

            return new Stateflow<Bar, BarState, EBarAction>(entity, stateMachine);
        }
    }
}
