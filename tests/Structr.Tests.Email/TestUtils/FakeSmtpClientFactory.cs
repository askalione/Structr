using Structr.Email.Clients.Smtp;
using System.Net;
using System.Net.Mail;

namespace Structr.Tests.Email.TestUtils
{
    public class FakeSmtpClientFactory : ISmtpClientFactory
    {
        private readonly SmtpOptions _options;

        public FakeSmtpClientFactory(SmtpOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options;
        }

        public ISmtpClient CreateSmtpClient()
        {
            var smtpClient = new SmtpClient(_options.Host, _options.Port);

            smtpClient.EnableSsl = _options.IsSslEnabled;
            if (string.IsNullOrWhiteSpace(_options.User) == false)
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_options.User, _options.Password);
            }

            // NOTE: Write messages to directory.
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            smtpClient.PickupDirectoryLocation = TestDataPath.Combine("FakeSmtpClientTemp");

            var smtpClientWrapper = new SmtpClientWrapper(smtpClient);
            return smtpClientWrapper;
        }
    }
}
