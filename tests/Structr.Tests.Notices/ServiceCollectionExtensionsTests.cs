using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Structr.Notices;
using Structr.Tests.Notices.TestUtils.Notices.Custom;
using Xunit;

namespace Structr.Tests.Notices
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddNotices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var serviceProvider = services
                .AddNotices(typeof(CustomNotice).Assembly)
                .BuildServiceProvider();

            // Assert
            serviceProvider.GetService<INoticePublisher>().Should().NotBeNull();
            serviceProvider.GetService<INoticeHandler<CustomNotice>>()
                .Should().BeOfType<CustomNoticeHandler>();

            var publisher1 = serviceProvider.GetService<INoticePublisher>();
            var publisher2 = serviceProvider.GetService<INoticePublisher>();
            publisher1.Equals(publisher2).Should().BeTrue();
        }

        [Fact]
        public void AddNotices_with_options()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var serviceProvider = services
                .AddNotices(options =>
                {
                    options.PublisherServiceLifetime = ServiceLifetime.Transient;
                },
                typeof(CustomNotice).Assembly)
                .BuildServiceProvider();

            // Assert
            serviceProvider.GetService<INoticePublisher>().Should().NotBeNull();
            serviceProvider.GetService<INoticeHandler<CustomNotice>>()
                .Should().BeOfType<CustomNoticeHandler>();

            var publisher1 = serviceProvider.GetService<INoticePublisher>();
            var publisher2 = serviceProvider.GetService<INoticePublisher>();
            publisher1.Equals(publisher2).Should().BeFalse();
        }
    }
}
