using Structr.Email;

namespace Structr.Tests.Email
{
    public class EmailOptionsTests
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var from = new EmailAddress("tatyana@larina.name");

            // Act
            var result = new EmailOptions(from);

            // Assert
            result.From.Should().Be(from);
            result.TemplateRootPath.Should().BeNull();
        }

        [Fact]
        public void Ctor_throws_when_from_is_null()
        {
            // Act
            Action act = () => new EmailOptions(null!);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
