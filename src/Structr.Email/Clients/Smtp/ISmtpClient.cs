using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Email.Clients.Smtp
{
    /// <summary>
    /// Provides functionality for sending emails.
    /// </summary>
    public interface ISmtpClient : IDisposable
    {
        /// <summary>
        /// Send email.
        /// </summary>
        /// <param name="emailData">The <see cref="EmailData"/>.</param>
        /// <param name="body">The message body.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        /// <returns>Returns <see langword="true"/> if email sent successfully.</returns>
        Task<bool> SendAsync(EmailData emailData, string body, CancellationToken cancellationToken);
    }
}
