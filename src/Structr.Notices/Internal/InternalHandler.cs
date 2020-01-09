using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Notices.Internal
{
    internal abstract class InternalHandler
    {
        public abstract Task HandleAsync(INotice notice,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken,
            Func<IEnumerable<Func<INotice, CancellationToken, Task>>, INotice, CancellationToken, Task> publish);
    }
}
