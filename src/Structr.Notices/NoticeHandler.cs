using System.Threading;
using System.Threading.Tasks;

namespace Structr.Notices
{
    public abstract class NoticeHandler<TNotice> : INoticeHandler<TNotice> where TNotice : INotice
    {
        Task INoticeHandler<TNotice>.HandleAsync(TNotice notice, CancellationToken cancellationToken)
        {
            Handle(notice);
            return Task.CompletedTask;
        }

        protected abstract void Handle(TNotice notice);
    }
}
