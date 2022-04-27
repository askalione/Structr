using System;

namespace Structr.Email.Clients.Smtp
{
    public class SmtpOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }
        public bool IsSslEnabled { get; set; }

        public SmtpOptions(string host, int port = 25)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException(nameof(host));
            }

            Host = host;
            Port = port;
        }
    }
}
