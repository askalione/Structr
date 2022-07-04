using Structr.Tests.Email.TestUtils.Assertions;

namespace Structr.Tests.Email.TestUtils.Extensions
{
    internal static class CustomEmailTemplateFileMessageExtenstions
    {
        public static CustomEmailTemplateFileMessageAssertions Should(this CustomEmailTemplateFileMessage instance)
        {
            return new CustomEmailTemplateFileMessageAssertions(instance);
        }
    }
}
