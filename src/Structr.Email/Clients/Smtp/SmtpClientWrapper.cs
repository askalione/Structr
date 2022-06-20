using System;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Email.Clients.Smtp
{
    internal class SmtpClientWrapper : ISmtpClient
    {
        private SmtpClient _smtpClient;

        public SmtpClientWrapper(SmtpClient smtpClient)
        {
            if (smtpClient == null)
            {
                throw new ArgumentNullException(nameof(smtpClient));
            }

            _smtpClient = smtpClient;
        }

        public async Task<bool> SendAsync(EmailData emailData, string body, CancellationToken cancellationToken)
        {
            MailMessage message = CreateMessage(emailData, body);

            await _smtpClient.SendMailExAsync(message, cancellationToken);

            return true;
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

            foreach (var to in emailData.To)
            {
                message.To.Add(new MailAddress(to.Address, to.Name));
            }

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
                };
            }

            return message;
        }

        public void Dispose()
        {
            _smtpClient.Dispose();
        }
    }

    /// <remark>
    /// Taken from: https://stackoverflow.com/a/28445791 
    /// </remark>
    internal static class SmtpClientExtensions
    {
        public static Task SendMailExAsync(this SmtpClient smtpClient,
            MailMessage message,
            CancellationToken token = default(CancellationToken))
        {
            // Use Task.Run to negate SynchronizationContext.
            return Task.Run(() => SendMailExImplAsync(smtpClient, message, token));
        }

        private static async Task SendMailExImplAsync(SmtpClient smtpClient,
            MailMessage message,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var tcs = new TaskCompletionSource<bool>();
            SendCompletedEventHandler? handler = null;
            Action unsubscribe = () => smtpClient.SendCompleted -= handler;

            handler = async (s, e) =>
            {
                unsubscribe();

                // A hack to complete the handler asynchronously.
                await Task.Yield();

                if (e.UserState != tcs)
                {
                    tcs.TrySetException(new InvalidOperationException("Unexpected UserState"));
                }
                else if (e.Cancelled)
                {
                    tcs.TrySetCanceled();
                }
                else if (e.Error != null)
                {
                    tcs.TrySetException(e.Error);
                }
                else
                {
                    tcs.TrySetResult(true);
                }
            };

            smtpClient.SendCompleted += handler;
            try
            {
                smtpClient.SendAsync(message, tcs);
                using (token.Register(() => smtpClient.SendAsyncCancel(), useSynchronizationContext: false))
                {
                    await tcs.Task;
                }
            }
            finally
            {
                unsubscribe();
            }
        }
    }
}
