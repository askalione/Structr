using System.Threading;
using System.Threading.Tasks;

namespace Structr.Notices
{
    /// <summary>
    /// Defines a publisher for a notice.
    /// </summary>
    public interface INoticePublisher
    {
        /// <summary>
        /// Publishes a notice.
        /// </summary>
        /// <param name="notice">The notice to be published.</param>
        /// <param name="cancellationToken">he token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the publishing of notice.</returns>
        Task PublishAsync(INotice notice, CancellationToken cancellationToken = default(CancellationToken));
    }
}
