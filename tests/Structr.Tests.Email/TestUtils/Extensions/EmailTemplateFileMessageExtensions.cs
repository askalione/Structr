using Structr.Email;

namespace Structr.Tests.Email.TestUtils.Extensions
{
    internal static class EmailTemplateFileMessageExtensions
    {
        public static void ShouldBeValid(this EmailTemplateFileMessage EmailTemplateFileMessage)
        {
            EmailTemplateFileMessage.To.Should().BeEquivalentTo(new List<EmailAddress>() { new EmailAddress("address@example.com") });
            EmailTemplateFileMessage.TemplatePath.Should().Be(TestDataPath.ContentRootPath);
            EmailTemplateFileMessage.Model.Should().NotBeNull();
        }

        public static void ShouldBeValid<TModel>(this EmailTemplateFileMessage<TModel> EmailTemplateFileMessage)
        {
            EmailTemplateFileMessage.To.Should().BeEquivalentTo(new List<EmailAddress>() { new EmailAddress("address@example.com") });
            EmailTemplateFileMessage.TemplatePath.Should().Be(TestDataPath.ContentRootPath);
            EmailTemplateFileMessage.Model.Should().NotBeNull();
        }
    }
}
