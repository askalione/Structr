using Structr.Email;

namespace Structr.Tests.Email.TestUtils
{
    public class CustomEmailData : EmailData
    {
        public CustomEmailData(IEnumerable<EmailAddress> to) : base(to) { }
    }
}
