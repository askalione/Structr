using Structr.Email;
using Structr.Tests.Email.TestUtils.Assertions;

namespace Structr.Tests.Email.TestUtils.Extensions
{
    internal static class EmailTemplateFileMessageExtenstions
    {
        public static EmailTemplateFileMessageAssertions Should(this EmailTemplateFileMessage instance)
        {
            return new EmailTemplateFileMessageAssertions(instance);
        }
    }
}
