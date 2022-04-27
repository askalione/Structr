using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Email.Clients.Smtp
{
    public class SmtpEmailClient : IEmailClient
    {
        private readonly SmtpOptions _options;

        public SmtpEmailClient(SmtpOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options;
        }

        public async Task<bool> SendAsync(EmailData emailData, string body, CancellationToken cancellationToken = default)
        {
            var message = CreateMessage(emailData, body);

            using (var smtpClient = CreateCmtpClient())
            {
                await smtpClient.SendMailExAsync(message, cancellationToken);
            }

            return true;
        }

        private SmtpClient CreateCmtpClient()
        {
            var smtpClient = new SmtpClient(_options.Host, _options.Port);

            smtpClient.EnableSsl = _options.IsSslEnabled;
            if (string.IsNullOrEmpty(_options.User) == false)
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_options.User, _options.Password);
            }

            return smtpClient;
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

            foreach(var to in emailData.To)
            {
                message.To.Add(new MailAddress(to.Address, to.Name));
            }
            
            return message;
        }
    }

    // Taken from: https://stackoverflow.com/a/28445791
    internal static class SmtpClientExtensions
    {
        public static Task SendMailExAsync(this SmtpClient smtpClient,
            MailMessage message,
            CancellationToken token = default(CancellationToken))
        {
            // use Task.Run to negate SynchronizationContext
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

                // a hack to complete the handler asynchronously
                await Task.Yield();

                if (e.UserState != tcs)
                    tcs.TrySetException(new InvalidOperationException("Unexpected UserState"));
                else if (e.Cancelled)
                    tcs.TrySetCanceled();
                else if (e.Error != null)
                    tcs.TrySetException(e.Error);
                else
                    tcs.TrySetResult(true);
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
