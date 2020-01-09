using Structr.Samples.Stateflows.Domain.FooEntity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Stateflows.Services
{
    public interface IFooService
    {
        Task<Foo> CreateAsync(string email, CancellationToken cancellationToken = default(CancellationToken));
        Task SendAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
        Task AcceptAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
        Task DeclineAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
    }
}
