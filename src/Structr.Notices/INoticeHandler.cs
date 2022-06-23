using System.Threading;
using System.Threading.Tasks;

namespace Structr.Notices
{
    /// <summary>
    /// Defines an asynchronously handler for a notice.
    /// </summary>
    /// <typeparam name="TNotice">The type of notice being handled.</typeparam>
    public interface INoticeHandler<in TNotice> where TNotice : INotice
    {
        /// <summary>
        /// Asynchronously handles a notice.
        /// </summary>
        /// <param name="notice">The notice to be handled.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the handling of notice.</returns>
        Task HandleAsync(TNotice notice, CancellationToken cancellationToken);
    }
}
