using Structr.Email;
using Structr.Tests.Email.TestUtils;

namespace Structr.Tests.Email
{
    public class EmailDataTests
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var addresses = new List<EmailAddress> { new EmailAddress("eugene@onegin.name") };

            // Act
            var result = new CustomEmailData(addresses);

            // Assert
            result.To.Should().BeEquivalentTo(addresses);
        }

        [Theory]
        [InlineData(null)]
        public void Ctor_throws_when_addresses_is_null(IEnumerable<EmailAddress> addresses)
        {
            // Act
            Action act = () => new CustomEmailData(addresses);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_throws_when_addresses_is_empty()
        {
            // Arrange
            var addresses = new List<EmailAddress>();

            // Act
            Action act = () => new CustomEmailData(addresses);

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}
