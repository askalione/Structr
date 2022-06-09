using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Email.Clients
{
    /// <summary>
    /// Provides functionality for writing an email to a file.
    /// </summary>
    public class FileEmailClient : IEmailClient
    {
        private readonly string _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileEmailClient"/> class.
        /// </summary>
        /// <param name="path">Absolute path to the directory for writing an emails.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is <see langword="null"/> or empty.</exception>
        public FileEmailClient(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            _path = path;
        }

        public async Task<bool> SendAsync(EmailData emailData, string body, CancellationToken cancellationToken = default)
        {
            var filePath = Path.Combine(_path, NewFileName());
            var content =
                $"From: {emailData.From}{Environment.NewLine}" +
                $"To: {string.Join(";", emailData.To.Select(x => x.ToString()))}{Environment.NewLine}" +
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
            return true;
        }

        private string NewFileName()
        {
            string guidSegment = Guid.NewGuid().ToString("N");
            string dateSegment = DateTime.Now.ToString("yyyyMMddHHmmss");
            return $"{dateSegment}-{guidSegment}";
        }
    }
}
