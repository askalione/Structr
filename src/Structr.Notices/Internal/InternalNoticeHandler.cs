using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Notices.Internal
{
    internal class InternalNoticeHandler<TNotice> : InternalHandler where TNotice : INotice
    {
        public override Task HandleAsync(INotice notice,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken,
            Func<IEnumerable<Func<INotice, CancellationToken, Task>>, INotice, CancellationToken, Task> publish)
        {
            var handlers = ((IEnumerable<INoticeHandler<TNotice>>)serviceProvider
                .GetService(typeof(IEnumerable<INoticeHandler<TNotice>>)))
                .Select(x => new Func<INotice, CancellationToken, Task>((n, c) => x.HandleAsync((TNotice)n, c)));

            return publish(handlers, notice, cancellationToken);
        }
    }
}
