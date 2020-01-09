using System.Threading;
using System.Threading.Tasks;

namespace Structr.Notices
{
    public interface INoticePublisher
    {
        Task PublishAsync(INotice notice, CancellationToken cancellationToken = default(CancellationToken));
    }
}
