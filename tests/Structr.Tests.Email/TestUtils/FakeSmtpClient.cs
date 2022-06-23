using Structr.Email;
using Structr.Email.Clients.Smtp;

namespace Structr.Tests.Email.TestUtils
{
    internal class FakeSmtpClient : ISmtpClient
    {
        private readonly string _path;

        public FakeSmtpClient(string path)
            => _path = path;

        public async Task SendAsync(EmailData emailData, string body, CancellationToken cancellationToken)
        {
            var filePath = Path.Combine(_path, NewFileName());
            var content =
                $"From: {emailData.From}{Environment.NewLine}" +
                $"To: {emailData.To}{Environment.NewLine}" +
                $"Subject: {emailData.Subject}{Environment.NewLine}" +
                $"{(emailData.Attachments?.Any() == true ? $"Attachments: {string.Join(";", emailData.Attachments.Select(x => x.FileName))}{Environment.NewLine}" : "")}" +
                $"{Environment.NewLine}" +
                $"{body}";
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
