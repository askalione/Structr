using Structr.Email;
using Structr.Tests.Email.TestUtils;
using Structr.Tests.Email.TestUtils.Extensions;

namespace Structr.Tests.Email
{
    public class EmailTemplateFileMessageTests
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var model = new CustomModel();

            // Act
            var result = new EmailTemplateFileMessage("eugene@onegin.name", TestDataPath.ContentRootPath, model);

            // Assert
            result.ShouldBeValid();
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void Ctor_throws_when_template_is_null_or_empty(string template)
        {
            // Arrange
            var model = new CustomModel();

            // Act
            Action act = () => new EmailTemplateFileMessage("eugene@onegin.name", template, model);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData(null)]
        public void Ctor_throws_when_model_is_null_or_empty(object model)
        {
            // Act
            Action act = () => new EmailTemplateFileMessage("eugene@onegin.name", TestDataPath.ContentRootPath, model);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_with_list_of_strings()
        {
            // Arrange
            var strings = new List<string>() { "eugene@onegin.name" };
            var model = new CustomModel();

            // Act
            var result = new EmailTemplateFileMessage(strings, TestDataPath.ContentRootPath, model);

            // Assert
            result.ShouldBeValid();
        }

        [Fact]
        public void Ctor_with_list_of_emails()
        {
            // Arrange
            var emails = new List<EmailAddress>() { new EmailAddress("eugene@onegin.name") };
            var model = new CustomModel();

            // Act
            var result = new EmailTemplateFileMessage(emails, TestDataPath.ContentRootPath, model);

            // Assert
            result.ShouldBeValid();
        }

        [Fact]
        public void Ctor_for_custom_model()
        {
            // Arrange
            var model = new CustomModel();

            // Act
            var result = new CustomEmailTemplateFileMessage("eugene@onegin.name", model);

            // Assert
            result.ShouldBeValid();
        }

        [Fact]
        public void Ctor_for_custom_model_with_list_of_strings()
        {
            // Arrange
            var strings = new List<string>() { "eugene@onegin.name" };
            var model = new CustomModel();

            // Act
            var result = new CustomEmailTemplateFileMessage(strings, model);

            // Assert
            result.ShouldBeValid();
        }

        [Fact]
        public void Ctor_for_custom_model_with_list_of_emails()
        {
            // Arrange
            var emails = new List<EmailAddress>() { new EmailAddress("eugene@onegin.name") };
            var model = new CustomModel();

            // Act
            var result = new CustomEmailTemplateFileMessage(emails, model);

            // Assert
            result.ShouldBeValid();
        }
    }
}
