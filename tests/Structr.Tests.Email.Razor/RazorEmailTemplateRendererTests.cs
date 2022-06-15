using Structr.Email;
using Structr.Email.Razor;
using Structr.Tests.Email.Razor.TestUtils;

namespace Structr.Tests.Email.Razor
{
    public class RazorEmailTemplateRendererTests
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var options = new EmailOptions(new EmailAddress("address@example.com"));
            options.TemplateRootPath = TestDataPath.ContentRootPath;

            // Act
            var result = new RazorEmailTemplateRenderer(options);

            // Assert
            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        public void Ctor_throws_when_options_is_null(EmailOptions options)
        {
            // Act
            Action act = () => new RazorEmailTemplateRenderer(options);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void Ctor_throws_when_templateRootPath_is_null(string templateRootPath)
        {
            // Arrange
            var options = new EmailOptions(new EmailAddress("address@example.com"));
            options.TemplateRootPath = templateRootPath;

            // Act
            Action act = () => new RazorEmailTemplateRenderer(options);

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>();
        }

        [Fact]
        public void Ctor_throws_when_templateRootPath_does_not_exist()
        {
            // Arrange
            var options = new EmailOptions(new EmailAddress("address@example.com"));
            options.TemplateRootPath = TestDataPath.Combine("NotExistFolder");

            // Act
            Action act = () => new RazorEmailTemplateRenderer(options);

            // Assert
            act.Should().ThrowExactly<DirectoryNotFoundException>();
        }
    }
}
