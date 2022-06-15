using Structr.Email.Clients.Smtp;

namespace Structr.Tests.Email.Clients.Smtp
{
    public class SmtpOptionsTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var result = new SmtpOptions("127.0.0.1", 25);

            // Assert
            result.Host.Should().Be("127.0.0.1");
            result.Port.Should().Be(25);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void Ctor_throws_when_host_is_null_or_empty(string host)
        {
            // Act
            Action act = () => new SmtpOptions(host);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-55)]
        [InlineData(-100)]
        public void Ctor_throws_when_port_less_than_zero(int port)
        {
            // Act
            Action act = () => new SmtpOptions("127.0.0.1", port);

            // Assert
            act.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }
    }
}
