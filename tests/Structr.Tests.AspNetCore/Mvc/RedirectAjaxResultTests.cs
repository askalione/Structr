using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Structr.AspNetCore.Mvc;
using Structr.Tests.AspNetCore.TestWebApp;
using Xunit;

namespace Structr.Tests.AspNetCore.Mvc
{
    public class RedirectAjaxResultTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var result = new RedirectAjaxResult("www.example.com");

            // Assert
            result.Url.Should().Be("www.example.com");
        }

        [Theory]
        [InlineData(false, "")]
        [InlineData(true, "window.location.replace('/RedirectAjaxResult/RedirectTarget');")]
        public async void ExecuteResultAsync(bool ajax, string? expected)
        {
            // Arrange
            var client = new WebApplicationFactory<Startup>().CreateClient();
            if (ajax)
            {
                client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");
            }

            // Act
            var response = await client.GetAsync($"/RedirectAjaxResult/ExecuteResultAsyncTest?url=/RedirectAjaxResult/RedirectTarget");

            // Assert
            response.Should().BeSuccessful();
            var result = await response.Content.ReadAsStringAsync();
            result.Should().Be(expected);
        }
    }
}
