using System;

namespace Structr.Email.Clients.Smtp
{
    public class SmtpOptions
    {
        public string Host { get; }
        public int Port { get; }
        public string? User { get; set; }
        public string? Password { get; set; }
        public bool IsSslEnabled { get; set; }

        public SmtpOptions(string host, int port = 25)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException(nameof(host));
            }
            if (port < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(port), port, $"Port cannot be less than zero.");
            }

            Host = host;
            Port = port;
        }
    }
}
