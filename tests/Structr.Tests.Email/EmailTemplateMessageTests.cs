using Structr.Email;
using Structr.Tests.Email.TestUtils.Extensions;

namespace Structr.Tests.Email
{
    public class EmailTemplateMessageTests
    {
        class CustomModel
        {
            public string Name { get; set; } = "Peter Parker";
        }

        class CustomEmailTemplateMessage : EmailTemplateMessage<CustomModel>
        {
            public CustomEmailTemplateMessage(string to, CustomModel model) : base(to, model) { }
            public CustomEmailTemplateMessage(IEnumerable<string> to, CustomModel model) : base(to, model) { }
            public CustomEmailTemplateMessage(IEnumerable<EmailAddress> to, CustomModel model) : base(to, model) { }

            public override string Template => "Hello, {{Name}}";
        }

        [Fact]
        public void Ctor()
        {
            // Arrange
            var model = new CustomModel();

            // Act
            var result = new EmailTemplateMessage("address@example.com", "Hello, {{Name}}", model);

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
            Action act = () => new EmailTemplateMessage("address@example.com", template, model);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData(null)]
        public void Ctor_throws_when_model_is_null_or_empty(object model)
        {
            // Act
            Action act = () => new EmailTemplateMessage("address@example.com", "Hello, {{Name}}", model);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_with_list_of_strings()
        {
            // Arrange
            var strings = new List<string>() { "address@example.com" };
            var model = new CustomModel();

            // Act
            var result = new EmailTemplateMessage(strings, "Hello, {{Name}}", model);

            // Assert
            result.ShouldBeValid();
        }

        [Fact]
        public void Ctor_with_list_of_emails()
        {
            // Arrange
            var emails = new List<EmailAddress>() { new EmailAddress("address@example.com") };
            var model = new CustomModel();

            // Act
            var result = new EmailTemplateMessage(emails, "Hello, {{Name}}", model);

            // Assert
            result.ShouldBeValid();
        }

        [Fact]
        public void Ctor_for_custom_model()
        {
            // Arrange
            var model = new CustomModel();

            // Act
            var result = new CustomEmailTemplateMessage("address@example.com", model);

            // Assert
            result.ShouldBeValid();
        }

        [Fact]
        public void Ctor_for_custom_model_with_list_of_strings()
        {
            // Arrange
            var strings = new List<string>() { "address@example.com" };
            var model = new CustomModel();

            // Act
            var result = new CustomEmailTemplateMessage(strings, model);

            // Assert
            result.ShouldBeValid();
        }

        [Fact]
        public void Ctor_for_custom_model_with_list_of_emails()
        {
            // Arrange
            var emails = new List<EmailAddress>() { new EmailAddress("address@example.com") };
            var model = new CustomModel();

            // Act
            var result = new CustomEmailTemplateMessage(emails, model);

            // Assert
            result.ShouldBeValid();
        }
    }
}
