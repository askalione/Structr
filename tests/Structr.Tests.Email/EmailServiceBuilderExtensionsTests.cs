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
            var client1 = serviceProvider.GetService<IEmailClient>();
            var client2 = serviceProvider.GetService<IEmailClient>();
            client1.Should().BeOfType<FileEmailClient>().And.Be(client2);
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

            var client1 = serviceProvider.GetService<IEmailClient>();
            var client2 = serviceProvider.GetService<IEmailClient>();
            client1.Should().BeOfType<SmtpEmailClient>().And.Be(client2);
        }

        [Fact]
        public void AddSmtpClient_with_options()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var emailBuilder = serviceCollection
                .AddEmail(new EmailAddress("tatyana@larina.name", "Tatyana Larina"));

            // Act
            emailBuilder.AddSmtpClient(host: "127.0.0.1", port: 25, options =>
            {
                options.IsSslEnabled = true;
            });

            // Assert
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var client1 = serviceProvider.GetService<IEmailClient>();
            var client2 = serviceProvider.GetService<IEmailClient>();
            client1.Should().BeOfType<SmtpEmailClient>().And.Be(client2);

            var opts = serviceProvider.GetRequiredService<SmtpOptions>();
            opts.IsSslEnabled.Should().BeTrue();
        }

        [Fact]
        public void AddSmtpClient_with_options_and_service_provider()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var emailBuilder = serviceCollection
                .AddEmail(new EmailAddress("tatyana@larina.name", "Tatyana Larina"));

            // Act
            emailBuilder.AddSmtpClient(host: "127.0.0.1", port: 25, (_, options) =>
            {
                options.User = "Admin";
                options.Password = "Qwerty";
            });

            // Assert
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var client1 = serviceProvider.GetService<IEmailClient>();
            var client2 = serviceProvider.GetService<IEmailClient>();
            client1.Should().BeOfType<SmtpEmailClient>().And.Be(client2);

            var opts = serviceProvider.GetRequiredService<SmtpOptions>();
            opts.User.Should().Be("Admin");
            opts.Password.Should().Be("Qwerty");
        }
    }
}
