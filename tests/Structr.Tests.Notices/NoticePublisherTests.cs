using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Structr.Notices;
using Structr.Tests.Notices.TestUtils;
using Structr.Tests.Notices.TestUtils.Notices.Confirmation;
using Structr.Tests.Notices.TestUtils.Notices.Custom;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.Notices
{
    public class NoticePublisherTests
    {
        private readonly IDbContext _dbContext;
        private readonly INoticePublisher _publisher;

        public NoticePublisherTests()
        {
            var services = new ServiceCollection()
                .AddNotices(typeof(ConfirmationNotice).Assembly);
            services.AddScoped<IDbContext, FakeDbContext>();
            var serviceProvider = services.BuildServiceProvider();

            _dbContext = serviceProvider.GetService<IDbContext>();
            _publisher = serviceProvider.GetService<INoticePublisher>();
        }

        [Fact]
        public async Task PublishAsync()
        {
            // Arrange
            var notice = new CustomNotice { Title = "Title 1" };

            // Act
            Func<Task> act = async () => await _publisher.PublishAsync(notice);

            // Assert
            await act.Should().ThrowExactlyAsync<NotImplementedException>()
                .WithMessage("Title 1");
        }

        [Fact]
        public async Task PublishAsync_throws_when_notice_is_null()
        {
            // Act
            Func<Task> act = async () => await _publisher.PublishAsync(null);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task PublishAsync_two_handlers()
        {
            // Arrange
            var notice = new ConfirmationNotice { Message = "Message" };

            // Act
            await _publisher.PublishAsync(notice);

            // Assert            
            _dbContext.Pushes.Should().SatisfyRespectively(first =>
            {
                first.Message.Should().Be("Message");
            });
            _dbContext.Tickets.Should().SatisfyRespectively(first =>
            {
                first.Message.Should().Be("Message");
            });
        }
    }
}
