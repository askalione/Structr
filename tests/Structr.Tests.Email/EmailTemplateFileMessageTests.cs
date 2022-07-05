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
            result.Should().HaveTemplatePathReceiverAndModel(TestDataPath.ContentRootPath, "eugene@onegin.name", model);
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
        public void Ctor_with_string_address()
        {
            // Arrange
            var address = "eugene@onegin.name";
            var model = new CustomModel();

            // Act
            var result = new EmailTemplateFileMessage(address, TestDataPath.ContentRootPath, model);

            // Assert
            result.Should().HaveTemplatePathReceiverAndModel(TestDataPath.ContentRootPath, "eugene@onegin.name", model);
        }

        [Fact]
        public void Ctor_with_email_address()
        {
            // Arrange
            var address = new EmailAddress("eugene@onegin.name");
            var model = new CustomModel();

            // Act
            var result = new EmailTemplateFileMessage(address, TestDataPath.ContentRootPath, model);

            // Assert
            result.Should().HaveTemplatePathReceiverAndModel(TestDataPath.ContentRootPath, "eugene@onegin.name", model);
        }

        [Fact]
        public void Ctor_for_custom_model()
        {
            // Arrange
            var model = new CustomModel();

            // Act
            var result = new CustomEmailTemplateFileMessage("eugene@onegin.name", model);

            // Assert
            result.Should().HaveTemplatePathReceiverAndModel(TestDataPath.ContentRootPath, "eugene@onegin.name", model);
        }

        [Fact]
        public void Ctor_for_custom_model_with_string_address()
        {
            // Arrange
            var address = "eugene@onegin.name";
            var model = new CustomModel();

            // Act
            var result = new CustomEmailTemplateFileMessage(address, model);

            // Assert
            result.Should().HaveTemplatePathReceiverAndModel(TestDataPath.ContentRootPath, "eugene@onegin.name", model);
        }
    }
}
