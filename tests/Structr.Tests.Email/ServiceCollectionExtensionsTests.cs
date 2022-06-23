using Microsoft.Extensions.DependencyInjection;
using Structr.Email;
using Structr.Tests.Email.TestUtils;

namespace Structr.Tests.Email
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddEmail()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection
                .AddEmail(new EmailAddress("tatyana@larina.name", "Tatyana Larina"))
                .AddFileClient(TestDataPath.ContentRootPath);

            // Assert
            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<IEmailSender>().Should().BeOfType<EmailSender>();
        }
    }
}
