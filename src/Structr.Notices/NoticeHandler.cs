using System.Threading;
using System.Threading.Tasks;

namespace Structr.Notices
{
    /// <inheritdoc cref="INoticeHandler{TNotice}"/>
    public abstract class NoticeHandler<TNotice> : INoticeHandler<TNotice> where TNotice : INotice
    {
        Task INoticeHandler<TNotice>.HandleAsync(TNotice notice, CancellationToken cancellationToken)
        {
            Handle(notice);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Override in a derived class to control how synchronously handle a notice.
        /// </summary>
        /// <param name="notice">The notice to be handled.</param>
        protected abstract void Handle(TNotice notice);
    }
}
