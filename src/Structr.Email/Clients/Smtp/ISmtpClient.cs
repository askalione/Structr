using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Email.Clients.Smtp
{
    /// <summary>
    /// Provides functionality for sending emails using SMTP-protocol.
    /// </summary>
    public interface ISmtpClient : IDisposable
    {
        /// <summary>
        /// Sends email.
        /// </summary>
        /// <param name="message">The <see cref="MailMessage"/>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        Task SendAsync(MailMessage message, CancellationToken cancellationToken);
    }
}
