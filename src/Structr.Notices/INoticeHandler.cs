using System.Threading;
using System.Threading.Tasks;

namespace Structr.Notices
{
    public interface INoticeHandler<in TNotice> where TNotice : INotice
    {
        Task HandleAsync(TNotice notice, CancellationToken cancellationToken);
    }
}
