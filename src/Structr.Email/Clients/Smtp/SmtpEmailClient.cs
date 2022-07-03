using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Email.Clients.Smtp
{
    /// <summary>
    /// Provides functionality for sending emails using SMTP.
    /// </summary>
    public class SmtpEmailClient : IEmailClient
    {
        private readonly ISmtpClientFactory _smtpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpEmailClient"/> class.
        /// </summary>
        /// <param name="smtpClientFactory">The <see cref="ISmtpClientFactory"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="options"/> is <see langword="null"/>.</exception>
        public SmtpEmailClient(ISmtpClientFactory smtpClientFactory)
        {
            if (smtpClientFactory == null)
            {
                throw new ArgumentNullException(nameof(smtpClientFactory));
            }

            _smtpClientFactory = smtpClientFactory;
        }

        public async Task SendAsync(EmailData emailData, string body, CancellationToken cancellationToken = default)
        {
            using (ISmtpClient smtpClient = _smtpClientFactory.CreateSmtpClient())
            {
                await smtpClient.SendAsync(emailData, body, cancellationToken);
            }
        }
    }
}
