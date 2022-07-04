using Structr.Email;
using Structr.Tests.Email.TestUtils.Assertions;

namespace Structr.Tests.Email.TestUtils.Extensions
{
    internal static class EmailTemplateMessageExtenstions
    {
        public static EmailTemplateMessageAssertions Should(this EmailTemplateMessage instance)
        {
            return new EmailTemplateMessageAssertions(instance);
        }
    }
}
