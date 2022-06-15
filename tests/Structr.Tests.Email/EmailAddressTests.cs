using Structr.Email;

namespace Structr.Tests.Email
{
    public class EmailAddressTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var result = new EmailAddress("address@example.com");

            // Assert
            result.Address.Should().Be("address@example.com");
            result.Name.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void Ctor_throws_when_address_is_null_or_empty(string address)
        {
            // Act
            Action act = () => new EmailAddress(address);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_with_name()
        {
            // Act
            var result = new EmailAddress("address@example.com", "Example");

            // Assert
            result.Address.Should().Be("address@example.com");
            result.Name.Should().Be("Example");
        }

        [Theory]
        [InlineData("address@example.com", "", "address@example.com")]
        [InlineData("address@example.com", "  ", "address@example.com")]
        [InlineData("address@example.com", null, "address@example.com")]
        [InlineData("address@example.com", "Example", "Example <address@example.com>")]
        public void ToStringTest(string address, string? name, string expected)
        {
            // Arrange
            var emailAddress = new EmailAddress(address, name);

            // Act
            var result = emailAddress.ToString();

            // Assert
            result.Should().Be(expected);
        }
    }
}
