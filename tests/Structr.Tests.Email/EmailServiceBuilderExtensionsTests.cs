using Microsoft.Extensions.DependencyInjection;
using Structr.Email;
using Structr.Email.Clients;
using Structr.Email.Clients.Smtp;
using Structr.Tests.Email.TestUtils;

namespace Structr.Tests.Email
{
    public class EmailServiceBuilderExtensionsTests
    {
        [Fact]
        public void AddFileClient()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var emailBuilder = serviceCollection
                .AddEmail(new EmailAddress("tatyana@larina.name", "Tatyana Larina"));

            // Act
            emailBuilder.AddFileClient(TestDataPath.ContentRootPath);

            // Assert
            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<IEmailClient>().Should().BeOfType<FileEmailClient>();
        }

        [Fact]
        public void AddSmtpClient()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var emailBuilder = serviceCollection
                .AddEmail(new EmailAddress("tatyana@larina.name", "Tatyana Larina"));

            // Act
            emailBuilder.AddSmtpClient("127.0.0.1");

            // Assert
            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<ISmtpClientFactory>().Should().NotBeNull();
            serviceProvider.GetService<IEmailClient>().Should().BeOfType<SmtpEmailClient>();
        }

        [Fact]
        public void AddSmtpClient_with_optins()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var emailBuilder = serviceCollection
                .AddEmail(new EmailAddress("tatyana@larina.name", "Tatyana Larina"));
            var options = new SmtpOptions("127.0.0.1");

            // Act
            emailBuilder.AddSmtpClient(options);

            // Assert
            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<ISmtpClientFactory>().Should().NotBeNull();
            serviceProvider.GetService<IEmailClient>().Should().BeOfType<SmtpEmailClient>();
        }

        [Fact]
        public void AddSmtpClient_with_optionsFactory()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var emailBuilder = serviceCollection
                .AddEmail(new EmailAddress("tatyana@larina.name", "Tatyana Larina"));

            // Act
            emailBuilder.AddSmtpClient(_ =>
            {
                var options = new SmtpOptions("127.0.0.1");
                options.User = "Admin";
                options.Password = "Qwerty";
                return options;
            });

            // Assert
            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<ISmtpClientFactory>().Should().NotBeNull();
            serviceProvider.GetService<IEmailClient>().Should().BeOfType<SmtpEmailClient>();
        }
    }
}
