using Structr.Email;

namespace Structr.Tests.Email.TestUtils.Extensions
{
    internal static class EmailMessageExtensions
    {
        public static void ShouldBeValid(this EmailMessage emailMessage)
        {
            emailMessage.Message.Should().Be("I write this to you - what more can be said?");
            emailMessage.To.Should().BeEquivalentTo(new EmailAddress("eugene@onegin.name"));
        }
    }
}
