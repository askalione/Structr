using Structr.Notices;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Tests.Notices.TestUtils.Notices.Confirmation
{
    internal class PushConfirmationNoticeHandler : INoticeHandler<ConfirmationNotice>
    {
        private IDbContext _dbContext;

        public PushConfirmationNoticeHandler(IDbContext notices)
        {
            _dbContext = notices;
        }

        public Task HandleAsync(ConfirmationNotice notice, CancellationToken cancellationToken)
        {
            _dbContext.Pushes.Add(new Push { Message = notice.Message });
            return Task.CompletedTask;
        }
    }
}
