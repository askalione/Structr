using Structr.Email.Clients.Smtp;
using System.Net.Mail;

namespace Structr.Tests.Email.TestUtils
{
    internal class FakeSmtpClient : ISmtpClient
    {
        private readonly string _path;

        public FakeSmtpClient(string path)
            => _path = path;

        public async Task SendAsync(MailMessage message, CancellationToken cancellationToken)
        {
            var filePath = Path.Combine(_path, NewFileName());
            var content =
                $"From: {message.From}{Environment.NewLine}" +
                $"To: {string.Join(";", message.To)}{Environment.NewLine}" +
                $"Subject: {message.Subject}{Environment.NewLine}" +
                $"{(message.Attachments?.Any() == true ? $"Attachments: {string.Join(";", message.Attachments.Select(x => x.Name))}{Environment.NewLine}" : "")}" +
                $"{Environment.NewLine}" +
                $"{message.Body}";
            if (File.Exists(_path) == false)
            {
                Directory.CreateDirectory(_path);
            }
            using (var sw = new StreamWriter(File.OpenWrite(filePath)))
            {
                await sw.WriteAsync(content);
            }
        }

        private string NewFileName()
        {
            string guidSegment = Guid.NewGuid().ToString("N");
            string dateSegment = DateTime.Now.ToString("yyyyMMddHHmmss");
            return $"{dateSegment}-{guidSegment}";
        }

        public void Dispose()
        { }
    }
}
