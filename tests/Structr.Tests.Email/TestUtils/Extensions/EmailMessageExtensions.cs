using Structr.Email;

namespace Structr.Tests.Email.TestUtils.Extensions
{
    internal static class EmailMessageExtensions
    {
        public static void ShouldBeValid(this EmailMessage emailMessage)
        {
            emailMessage.Message.Should().Be("Some message.");
            emailMessage.To.Should().BeEquivalentTo(new List<EmailAddress>() { new EmailAddress("address@example.com") });
        }
    }
}
