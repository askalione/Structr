using System.Threading;
using System.Threading.Tasks;

namespace Structr.Email
{
    public interface IEmailClient
    {
        Task<bool> SendAsync(EmailData emailData, string body, CancellationToken cancellationToken = default);
    }
}
