using System.Threading;
using System.Threading.Tasks;

namespace Structr.Email
{
    /// <summary>
    /// Provides functionality for sending an emails.
    /// </summary>
    public interface IEmailClient
    {
        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="emailData">The <see cref="EmailData"/>.</param>
        /// <param name="body">The body of an email.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        Task SendAsync(EmailData emailData, string body, CancellationToken cancellationToken = default);
    }
}
