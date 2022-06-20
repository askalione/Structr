using Structr.Email;
using Structr.Tests.Email.TestUtils.Extensions;

namespace Structr.Tests.Email
{
    public class EmailMessageTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var result = new EmailMessage("eugene@onegin.name", "I write this to you - what more can be said?");

            // Assert
            result.ShouldBeValid();
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void Ctor_throws_when_message_is_null_or_empty(string message)
        {
            // Act
            Action act = () => new EmailMessage("eugene@onegin.name", message);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_with_list_of_strings()
        {
            // Arrange
            var addresses = new List<string>() { "eugene@onegin.name" };

            // Act
            var result = new EmailMessage(addresses, "I write this to you - what more can be said?");

            // Assert
            result.ShouldBeValid();
        }

        [Fact]
        public void Ctor_with_list_of_emails()
        {
            // Arrange
            var addresses = new List<EmailAddress>() { new EmailAddress("eugene@onegin.name") };

            // Act
            var result = new EmailMessage(addresses, "I write this to you - what more can be said?");

            // Assert
            result.ShouldBeValid();
        }
    }
}
