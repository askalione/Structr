using Structr.Samples.Stateflows.Domain.BarEntity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Stateflows.Services
{
    public interface IBarService
    {
        Task<Bar> CreateAsync(string name, CancellationToken cancellationToken = default(CancellationToken));
        Task OpenAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
        Task CloseAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
        Task ArchiveAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
    }
}
