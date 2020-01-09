using Structr.Samples.Stateflows.DataAccess;
using Structr.Samples.Stateflows.Domain.BarEntity;
using Structr.Samples.Stateflows.Stateflows;
using Structr.Samples.Stateflows.Stateflows.BarEntity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Stateflows.Services
{
    public class BarService : IBarService
    {
        private readonly IRepository<Bar> _repository;
        private readonly IBarStateflowProvider _stateflowProvider;

        public BarService(IRepository<Bar> repository, IBarStateflowProvider stateflowProvider)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            if (stateflowProvider == null)
                throw new ArgumentNullException(nameof(stateflowProvider));

            _repository = repository;
            _stateflowProvider = stateflowProvider;
        }

        public Task<Bar> CreateAsync(string name, CancellationToken cancellationToken = default)
        {
            var bar = new Bar(name);
            _repository.Add(bar);
            return Task.FromResult(bar);
        }

        public Task OpenAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return FireAsync(EBarAction.Open, id, cancellationToken);
        }

        public Task CloseAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return FireAsync(EBarAction.Close, id, cancellationToken);
        }


        public Task ArchiveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return FireAsync(EBarAction.Archive, id, cancellationToken);
        }

        private async Task FireAsync(EBarAction action, Guid id, CancellationToken cancellationToken)
        {
            var stateflow = await _stateflowProvider.GetStateflowAsync(id, cancellationToken);

            var foo = stateflow.Entity;
            var stateMachine = stateflow.StateMachine;

            if (!stateMachine.CanFire(action))
                throw new StateflowException($"{action} operation is not permitted");

            stateMachine.Fire(action);
        }
    }
}
