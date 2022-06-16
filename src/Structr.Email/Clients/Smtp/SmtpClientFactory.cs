using System;
using System.Net;
using System.Net.Mail;

namespace Structr.Email.Clients.Smtp
{
    internal class SmtpClientFactory : ISmtpClientFactory
    {
        private readonly SmtpOptions _options;

        public SmtpClientFactory(SmtpOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options;
        }

        public SmtpClient CreateSmtpClient()
        {
            var smtpClient = new SmtpClient(_options.Host, _options.Port);

            smtpClient.EnableSsl = _options.IsSslEnabled;
            if (string.IsNullOrWhiteSpace(_options.User) == false)
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_options.User, _options.Password);
            }

            return smtpClient;
        }
    }
}
