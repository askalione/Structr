using Structr.Email;

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
            result.Message.Should().Be("I write this to you - what more can be said?");
            result.To.Should().BeEquivalentTo(new EmailAddress("eugene@onegin.name"));
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
            var address = "eugene@onegin.name";

            // Act
            var result = new EmailMessage(address, "I write this to you - what more can be said?");

            // Assert
            result.Message.Should().Be("I write this to you - what more can be said?");
            result.To.Should().BeEquivalentTo(new EmailAddress("eugene@onegin.name"));
        }

        [Fact]
        public void Ctor_with_list_of_emails()
        {
            // Arrange
            var address = new EmailAddress("eugene@onegin.name");

            // Act
            var result = new EmailMessage(address, "I write this to you - what more can be said?");

            // Assert
            result.Message.Should().Be("I write this to you - what more can be said?");
            result.To.Should().BeEquivalentTo(new EmailAddress("eugene@onegin.name"));
        }
    }
}
