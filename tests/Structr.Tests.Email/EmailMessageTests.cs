using Structr.Email;
using Structr.Tests.Email.TestUtils;
using Structr.Tests.Email.TestUtils.Extensions;

namespace Structr.Tests.Email
{
    public class EmailMessageTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var result = new EmailMessage("address@example.com", "Some message.");

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
            Action act = () => new EmailMessage("address@example.com", message);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_with_list_of_strings()
        {
            // Arrange
            var addresses = new List<string>() { "address@example.com" };

            // Act
            var result = new EmailMessage(addresses, "Some message.");

            // Assert
            result.ShouldBeValid();
        }

        [Fact]
        public void Ctor_with_list_of_emails()
        {
            // Arrange
            var addresses = new List<EmailAddress>() { new EmailAddress("address@example.com") };

            // Act
            var result = new EmailMessage(addresses, "Some message.");

            // Assert
            result.ShouldBeValid();
        }
    }
}
