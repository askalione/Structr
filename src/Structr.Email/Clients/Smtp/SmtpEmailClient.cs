using System;
using System.Net.Mail;
using System.Text;
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
                MailMessage message = CreateMessage(emailData, body);
                await smtpClient.SendAsync(message, cancellationToken);
            }
        }

        private MailMessage CreateMessage(EmailData emailData, string body)
        {
            var message = new MailMessage
            {
                Subject = emailData.Subject,
                Body = body,
                IsBodyHtml = emailData.IsHtml,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8
            };

            if (emailData.From != null)
            {
                message.From = new MailAddress(emailData.From.Address, emailData.From.Name);
            }

            message.To.Add(new MailAddress(emailData.To.Address, emailData.To.Name));

            if (emailData.Attachments != null)
            {
                foreach (var attachment in emailData.Attachments)
                {
                    Attachment mailAttachment;

                    if (attachment.Content == null)
                    {
                        mailAttachment = new Attachment(attachment.FileName, attachment.ContentType);
                    }
                    else
                    {
                        mailAttachment = new Attachment(attachment.Content, attachment.FileName, attachment.ContentType);
                    }

                    message.Attachments.Add(mailAttachment);
                }
            }

            return message;
        }
    }
}
