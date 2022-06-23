using System;

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

        public ISmtpClient CreateSmtpClient()
            => new SmtpClient(_options);
    }
}
