using Structr.Samples.Stateflows.DataAccess;
using Structr.Samples.Stateflows.Domain.FooEntity;
using Structr.Samples.Stateflows.Stateflows;
using Structr.Samples.Stateflows.Stateflows.FooEntity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Stateflows.Services
{
    public class FooService : IFooService
    {
        private readonly IRepository<Foo> _repository;
        private readonly IFooStateflowProvider _stateflowProvider;

        public FooService(IRepository<Foo> repository, IFooStateflowProvider stateflowProvider)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            if (stateflowProvider == null)
                throw new ArgumentNullException(nameof(stateflowProvider));

            _repository = repository;
            _stateflowProvider = stateflowProvider;
        }

        public Task<Foo> CreateAsync(string email, CancellationToken cancellationToken)
        {
            var foo = new Foo(email);
            _repository.Add(foo);
            return Task.FromResult(foo);
        }

        public Task SendAsync(Guid id, CancellationToken cancellationToken)
        {
            return FireAsync(EFooAction.Send, id, cancellationToken);
        }

        public Task AcceptAsync(Guid id, CancellationToken cancellationToken)
        {
            return FireAsync(EFooAction.Accept, id, cancellationToken);
        }

        public Task DeclineAsync(Guid id, CancellationToken cancellationToken)
        {
            return FireAsync(EFooAction.Decline, id, cancellationToken);
        }

        private async Task FireAsync(EFooAction action, Guid id, CancellationToken cancellationToken)
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
