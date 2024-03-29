using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Email.Clients.Smtp
{
    /// <inheritdoc cref="ISmtpClient"/>
    internal class SmtpClient : ISmtpClient
    {
        private System.Net.Mail.SmtpClient _smtpClient;

        public SmtpClient(SmtpOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _smtpClient = new System.Net.Mail.SmtpClient(options.Host, options.Port)
            {
                EnableSsl = options.IsSslEnabled
            };
            if (string.IsNullOrWhiteSpace(options.User) == false)
            {
                _smtpClient.UseDefaultCredentials = false;
                _smtpClient.Credentials = new NetworkCredential(options.User, options.Password);
            }
        }

        public Task SendAsync(MailMessage message, CancellationToken cancellationToken)
            => _smtpClient.SendMailExAsync(message, cancellationToken);

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
        public static Task SendMailExAsync(this System.Net.Mail.SmtpClient smtpClient,
            MailMessage message,
            CancellationToken token = default(CancellationToken))
        {
            // Use Task.Run to negate SynchronizationContext.
            return Task.Run(() => SendMailExImplAsync(smtpClient, message, token));
        }

        private static async Task SendMailExImplAsync(System.Net.Mail.SmtpClient smtpClient,
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
