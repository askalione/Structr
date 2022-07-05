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
            var address = new EmailAddress("eugene@onegin.name");

            // Act
            var result = new CustomEmailData(address);

            // Assert
            result.To.Should().BeEquivalentTo(address);
        }

        [Fact]
        public void Ctor_throws_when_addresses_is_null()
        {
            // Act
            Action act = () => new CustomEmailData(null!);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
