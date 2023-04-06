using System.Threading;
using System.Threading.Tasks;

namespace Structr.Notices
{
    /// <summary>
    /// Class to be used as base for all asynchronous notice handlers.
    /// </summary>
    /// <typeparam name="TNotice">The type of notice being handled.</typeparam>
    public abstract class AsyncNoticeHandler<TNotice> : INoticeHandler<TNotice>
        where TNotice : INotice
    {
        public abstract Task HandleAsync(TNotice notice, CancellationToken cancellationToken);
    }
}
