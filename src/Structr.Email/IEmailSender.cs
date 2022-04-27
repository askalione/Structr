using System.Threading;
using System.Threading.Tasks;

namespace Structr.Email
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(EmailMessage email, CancellationToken cancellationToken = default);
        Task<bool> SendEmailAsync(EmailTemplate email, CancellationToken cancellationToken = default);
        Task<bool> SendEmailAsync<TModel>(EmailTemplate<TModel> email, CancellationToken cancellationToken = default);
        Task<bool> SendEmailAsync<TModel>(EmailTemplateFile email, CancellationToken cancellationToken = default);
        Task<bool> SendEmailAsync<TModel>(EmailTemplateFile<TModel> email, CancellationToken cancellationToken = default);
    }
}
