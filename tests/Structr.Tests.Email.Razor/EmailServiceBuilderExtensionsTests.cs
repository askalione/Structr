using Microsoft.Extensions.DependencyInjection;
using Structr.Email;
using Structr.Email.Razor;
using Structr.Tests.Email.Razor.TestUtils;

namespace Structr.Tests.Email.Razor
{
    public class EmailServiceBuilderExtensionsTests
    {
        [Fact]
        public void AddRazorTemplateRenderer()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var emailServiceBuilder = serviceCollection
                .AddEmail(new EmailAddress("address@example.com", "Example"), options =>
                {
                    options.TemplateRootPath = TestDataPath.ContentRootPath;
                })
                .AddFileClient(TestDataPath.ContentRootPath);

            // Act
            emailServiceBuilder.AddRazorTemplateRenderer();

            // Assert
            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<IEmailTemplateRenderer>().Should().BeOfType<RazorEmailTemplateRenderer>();
        }
    }
}
