using Structr.Email;

namespace Structr.Tests.Email.TestUtils.Extensions
{
    internal static class EmailTemplateMessageExtensions
    {
        public static void ShouldBeValid(this EmailTemplateMessage emailTemplateMessage)
        {
            emailTemplateMessage.To.Should().BeEquivalentTo(new List<EmailAddress>() { new EmailAddress("address@example.com") });
            emailTemplateMessage.Template.Should().Be("Hello, {{Name}}");
            emailTemplateMessage.Model.Should().NotBeNull();
        }

        public static void ShouldBeValid<TModel>(this EmailTemplateMessage<TModel> emailTemplateMessage)
        {
            emailTemplateMessage.To.Should().BeEquivalentTo(new List<EmailAddress>() { new EmailAddress("address@example.com") });
            emailTemplateMessage.Template.Should().Be("Hello, {{Name}}");
            emailTemplateMessage.Model.Should().NotBeNull();
        }
    }
}
