using Structr.Email;
using Structr.Tests.Email.TestUtils;
using Structr.Tests.Email.TestUtils.Extensions;

namespace Structr.Tests.Email
{
    public class EmailTemplateMessageTests
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var model = new CustomModel();

            // Act
            var result = new EmailTemplateMessage("eugene@onegin.name", "Letter of {{From}} to {{To}}.", model);

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
            Action act = () => new EmailTemplateMessage("eugene@onegin.name", template, model);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData(null)]
        public void Ctor_throws_when_model_is_null_or_empty(object model)
        {
            // Act
            Action act = () => new EmailTemplateMessage("eugene@onegin.name", "Letter of {{From}} to {{To}}.", model);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_with_list_of_strings()
        {
            // Arrange
            var address = "eugene@onegin.name";
            var model = new CustomModel();

            // Act
            var result = new EmailTemplateMessage(address, "Letter of {{From}} to {{To}}.", model);

            // Assert
            result.ShouldBeValid();
        }

        [Fact]
        public void Ctor_with_list_of_emails()
        {
            // Arrange
            var address = new EmailAddress("eugene@onegin.name");
            var model = new CustomModel();

            // Act
            var result = new EmailTemplateMessage(address, "Letter of {{From}} to {{To}}.", model);

            // Assert
            result.ShouldBeValid();
        }

        [Fact]
        public void Ctor_for_custom_model()
        {
            // Arrange
            var model = new CustomModel();

            // Act
            var result = new CustomEmailTemplateMessage("eugene@onegin.name", model);

            // Assert
            result.ShouldBeValid();
        }

        [Fact]
        public void Ctor_for_custom_model_with_string_address()
        {
            // Arrange
            var address = "eugene@onegin.name";
            var model = new CustomModel();

            // Act
            var result = new CustomEmailTemplateMessage(address, model);

            // Assert
            result.ShouldBeValid();
        }
    }
}
