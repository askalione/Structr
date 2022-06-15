using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Structr.AspNetCore.Http;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Moq;

namespace Structr.Tests.AspNetCore.Http
{
    public class HttpContextExtensionsTests
    {
        [Fact]
        public void GetIpAddress()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Connection.RemoteIpAddress = new IPAddress(new byte[] { 0xC0, 0xA8, 0x01, 0x01 });

            // Act
            var result = httpContext.GetIpAddress();

            // Assert
            result.Should().Be("192.168.1.1");
        }

        [Fact]
        public void GetIpAddress_throws_when_context_is_null()
        {
            // Arrange
            DefaultHttpContext httpContext = null!;

            // Act
            Action act = () => httpContext.GetIpAddress();

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public async void GetAuthenticationSchemesAsync()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var authHandler = new Mock<IAuthenticationHandler>().Object;
            var asp = new Mock<IAuthenticationSchemeProvider>();
            asp.Setup(x => x.GetAllSchemesAsync())
                .ReturnsAsync(new AuthenticationScheme[] {
                    new AuthenticationScheme("Scheme1", "Scheme1DisplayName", authHandler.GetType()),
                    new AuthenticationScheme("Scheme2", "Scheme2DisplayName", authHandler.GetType()),
                    new AuthenticationScheme("Scheme3", "", authHandler.GetType())
                });
            httpContext.RequestServices = new ServiceCollection()
                .AddScoped<IAuthenticationSchemeProvider>(x => asp.Object)
                .BuildServiceProvider();

            // Act
            var result = await httpContext.GetAuthenticationSchemesAsync();

            // Assert
            result.Should().BeEquivalentTo(new AuthenticationScheme[] {
                    new AuthenticationScheme("Scheme1", "Scheme1DisplayName", authHandler.GetType()),
                    new AuthenticationScheme("Scheme2", "Scheme2DisplayName", authHandler.GetType()),
                    new AuthenticationScheme("Scheme3", "", authHandler.GetType())
                });
        }

        [Fact]
        public void GetAuthenticationSchemesAsync_throws_when_context_is_null()
        {
            // Arrange
            DefaultHttpContext httpContext = null!;

            // Act
            Func<Task> func = async () => await httpContext.GetAuthenticationSchemesAsync();

            // Assert
            func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Theory]
        [InlineData("Scheme1", true)]
        [InlineData("Scheme3", true)]
        [InlineData("Scheme4", false)]
        [InlineData("", false)]
        public async void IsSupportedAuthenticationSchemeAsync(string scheme, bool expected)
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            var authHandler = new Mock<IAuthenticationHandler>().Object;
            var asp = new Mock<IAuthenticationSchemeProvider>();
            asp.Setup(x => x.GetAllSchemesAsync())
                .ReturnsAsync(new AuthenticationScheme[] {
                    new AuthenticationScheme("Scheme1", "Scheme1DisplayName", authHandler.GetType()),
                    new AuthenticationScheme("Scheme2", "Scheme2DisplayName", authHandler.GetType()),
                    new AuthenticationScheme("Scheme3", "", authHandler.GetType())
                });
            httpContext.RequestServices = new ServiceCollection()
                .AddScoped<IAuthenticationSchemeProvider>(x => asp.Object)
                .BuildServiceProvider();

            // Act
            var result = await httpContext.IsSupportedAuthenticationSchemeAsync(scheme);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void IsSupportedAuthenticationSchemeAsync_throws_when_context_is_null()
        {
            // Arrange
            DefaultHttpContext httpContext = null!;

            // Act
            Func<Task> func = async () => await httpContext.IsSupportedAuthenticationSchemeAsync("qwerty");

            // Assert
            func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
    }
}
