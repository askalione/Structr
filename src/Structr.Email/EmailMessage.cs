using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structr.Email
{
    public class EmailMessage : EmailData
    {
        public string Message { get; }

        public EmailMessage(string to, string message)
            : this(new[] { to }, message)
        {
        }

        public EmailMessage(IEnumerable<string> to, string message)
            : this(to.Select(x => new EmailAddress(x)), message)
        {
        }

        public EmailMessage(IEnumerable<EmailAddress> to, string message)
            : base(to)
        {
            if (string.IsNullOrEmpty(message))
            {
               throw new ArgumentNullException(nameof(message));
            }

            Message = message;
        }
    }
}
