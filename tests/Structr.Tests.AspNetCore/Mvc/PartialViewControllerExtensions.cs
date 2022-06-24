using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Structr.Tests.AspNetCore.TestWebApp;
using Xunit;

namespace Structr.Tests.AspNetCore.Mvc
{
    public class PartialViewControllerExtensions
    {
        [Fact]
        public async void RenderPartialViewAsync()
        {
            // Arrange
            var client = new WebApplicationFactory<Startup>().CreateClient();

            // Act
            var response = await client.GetAsync($"/MvcControllerExtensions/RenderPartialViewAsyncTest?id=7&name=Peter");

            // Assert
            response.Should().BeSuccessful();
            var result = await response.Content.ReadAsStringAsync();
            result.Should().Contain("Id3Partial=7 Name3Partial=Peter");
        }

        [Theory]
        [InlineData("", false, "Id=7 Name=Peter")]
        [InlineData("RenderViewAsyncTest2", false, "Id2=7 Name2=Peter")]
        [InlineData("_RenderViewAsyncTest3Partial", true, "Id3Partial=7 Name3Partial=Peter")]
        public async void RenderViewAsync(string viewName, bool isPartial, string expected)
        {
            // Arrange
            var client = new WebApplicationFactory<Startup>().CreateClient();

            // Act
            var response = await client.GetAsync($"/MvcControllerExtensions/RenderViewAsyncTest?id=7&name=Peter&viewName={viewName}&isPartial={isPartial}");

            // Assert
            response.Should().BeSuccessful();
            var result = await response.Content.ReadAsStringAsync();
            result.Should().Contain(expected);
        }

        [Fact]
        public async void RenderViewAsync_throws_when_view_not_found()
        {
            // Arrange
            var client = new WebApplicationFactory<Startup>().CreateClient();

            // Act
            var response = await client.GetAsync($"/MvcControllerExtensions/RenderViewAsyncTest?id=7&name=Peter&viewName=someUnexistingView");

            // Assert
            response.Should().HaveServerError();
        }
    }
}
