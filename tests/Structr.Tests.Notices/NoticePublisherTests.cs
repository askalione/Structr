using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Structr.Notices;
using Structr.Tests.Notices.TestUtils;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.Notices
{
    public class NoticePublisherTests
    {
        [Fact]
        public async Task PublishAsync()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
                .AddNotices(typeof(CustomNotice).Assembly)
                .BuildServiceProvider();
            var publisher = serviceProvider.GetService<INoticePublisher>();
            var notice = new CustomNotice { Title = "Title 1" };

            // Act
            Func<Task> act = async () => await publisher.PublishAsync(notice);

            // Assert
            await act.Should().ThrowExactlyAsync<NotImplementedException>()
                .WithMessage("Title 1");
        }
    }
}
