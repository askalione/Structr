using Structr.Email.Clients.Smtp;

namespace Structr.Tests.Email.TestUtils
{
    public class FakeSmtpClientFactory : ISmtpClientFactory
    {
        private readonly string _path;

        public FakeSmtpClientFactory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            _path = path;
        }

        public ISmtpClient CreateSmtpClient()
            => new FakeSmtpClient(_path);
    }
}
