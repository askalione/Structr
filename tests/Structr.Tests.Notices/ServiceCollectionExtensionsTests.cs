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
            INoticePublisher publisher = serviceProvider.GetService<INoticePublisher>();
            publisher.Should().BeOfType<NoticePublisher>();
            serviceProvider.GetService<INoticeHandler<CustomNotice>>()
                .Should().BeOfType<CustomNoticeHandler>();
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
            INoticePublisher publisher = serviceProvider.GetService<INoticePublisher>();
            publisher.Should().BeOfType<NoticePublisher>();
            serviceProvider.GetService<INoticeHandler<CustomNotice>>()
                .Should().BeOfType<CustomNoticeHandler>();
        }
    }
}
