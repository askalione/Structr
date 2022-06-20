using Structr.Email;

namespace Structr.Tests.Email.TestUtils
{
    internal class CustomEmailData : EmailData
    {
        public CustomEmailData(IEnumerable<EmailAddress> to) : base(to) { }
    }
}
