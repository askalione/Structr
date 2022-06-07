using Structr.Notices;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Tests.Notices.TestUtils.Notices.Confirmation
{
    internal class TicketConfirmationNoticeHandler : INoticeHandler<ConfirmationNotice>
    {
        private IDbContext _dbContext;

        public TicketConfirmationNoticeHandler(IDbContext notices)
        {
            _dbContext = notices;
        }

        public Task HandleAsync(ConfirmationNotice notice, CancellationToken cancellationToken)
        {
            _dbContext.Tickets.Add(new Ticket { Message = notice.Message });
            return Task.CompletedTask;
        }
    }
}
