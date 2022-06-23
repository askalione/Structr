using Structr.Email;

namespace Structr.Tests.Email
{
    public class EmailAddressTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var result = new EmailAddress("tatyana@larina.name");

            // Assert
            result.Address.Should().Be("tatyana@larina.name");
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
            var result = new EmailAddress("tatyana@larina.name", "Tatyana Larina");

            // Assert
            result.Address.Should().Be("tatyana@larina.name");
            result.Name.Should().Be("Tatyana Larina");
        }

        [Theory]
        [InlineData("tatyana@larina.name", "", "tatyana@larina.name")]
        [InlineData("tatyana@larina.name", "  ", "tatyana@larina.name")]
        [InlineData("tatyana@larina.name", null, "tatyana@larina.name")]
        [InlineData("tatyana@larina.name", "Tatyana Larina", "Tatyana Larina <tatyana@larina.name>")]
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
