using Structr.Samples.Stateflows.DataAccess;
using Structr.Samples.Stateflows.Domain.FooEntity;
using Structr.Stateflows;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Stateflows.Stateflows.FooEntity
{
    public class FooStateflowProvider : IFooStateflowProvider
    {
        private readonly IRepository<Foo> _repository;
        private readonly IStateMachineProvider _stateMachineProvider;

        public FooStateflowProvider(IRepository<Foo> repository,
            IStateMachineProvider stateMachineProvider)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            if (stateMachineProvider == null)
                throw new ArgumentNullException(nameof(stateMachineProvider));

            _repository = repository;
            _stateMachineProvider = stateMachineProvider;
        }

        public async Task<Stateflow<Foo, FooState, FooAction>> GetStateflowAsync(Guid entityId, CancellationToken cancellationToken)
        {
            var entity = _repository.Get(entityId);
            if (entity == null)
                throw new InvalidOperationException("Entity not found");

            var stateMachine = await _stateMachineProvider.GetStateMachineAsync(entity, cancellationToken);

            return new Stateflow<Foo, FooState, FooAction>(entity, stateMachine);
        }
    }
}
