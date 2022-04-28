using System.Threading;
using System.Threading.Tasks;

namespace Structr.Email
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(EmailMessage email, CancellationToken cancellationToken = default);
        Task<bool> SendEmailAsync(EmailTemplateMessage email, CancellationToken cancellationToken = default);
        Task<bool> SendEmailAsync<TModel>(EmailTemplateMessage<TModel> email, CancellationToken cancellationToken = default);
        Task<bool> SendEmailAsync(EmailTemplateFileMessage email, CancellationToken cancellationToken = default);
        Task<bool> SendEmailAsync<TModel>(EmailTemplateFileMessage<TModel> email, CancellationToken cancellationToken = default);
    }
}
