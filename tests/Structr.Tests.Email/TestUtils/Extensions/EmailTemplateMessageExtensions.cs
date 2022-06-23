using Structr.Email;

namespace Structr.Tests.Email.TestUtils.Extensions
{
    internal static class EmailTemplateMessageExtensions
    {
        public static void ShouldBeValid(this EmailTemplateMessage emailTemplateMessage)
        {
            emailTemplateMessage.To.Should().BeEquivalentTo(new EmailAddress("eugene@onegin.name"));
            emailTemplateMessage.Template.Should().Be("Letter of {{From}} to {{To}}.");
            emailTemplateMessage.Model.Should().NotBeNull();
        }

        public static void ShouldBeValid<TModel>(this EmailTemplateMessage<TModel> emailTemplateMessage)
        {
            emailTemplateMessage.To.Should().BeEquivalentTo(new EmailAddress("eugene@onegin.name"));
            emailTemplateMessage.Template.Should().Be("Letter of {{From}} to {{To}}.");
            emailTemplateMessage.Model.Should().NotBeNull();
        }
    }
}
