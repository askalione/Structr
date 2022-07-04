using Structr.Tests.Email.TestUtils.Assertions;

namespace Structr.Tests.Email.TestUtils.Extensions
{
    internal static class CustomEmailTemplateMessageExtenstions
    {
        public static CustomEmailTemplateMessageAssertions Should(this CustomEmailTemplateMessage instance)
        {
            return new CustomEmailTemplateMessageAssertions(instance);
        }
    }
}
