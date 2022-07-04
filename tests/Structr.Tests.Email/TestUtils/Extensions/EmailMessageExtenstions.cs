using Structr.Email;
using Structr.Tests.Email.TestUtils.Assertions;

namespace Structr.Tests.Email.TestUtils.Extensions
{
    internal static class EmailMessageExtenstions
    {
        public static EmailMessageAssertions Should(this EmailMessage instance)
        {
            return new EmailMessageAssertions(instance);
        }
    }
}
