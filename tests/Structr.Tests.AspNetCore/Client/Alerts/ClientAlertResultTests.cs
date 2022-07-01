using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Structr.AspNetCore.Client.Alerts;
using System;
using Xunit;

namespace Structr.Tests.AspNetCore.Client.Alerts
{
    public class ClientAlertResultTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            Action act = () => new ClientAlertResult(Mock.Of<IActionResult>(), new ClientAlert("Type1", "Message1"));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public async void ExecuteResultAsync()
        {
            // Arrange
            var alert = new ClientAlert("Type1", "Message1");
            var javaScriptAlertResult = new ClientAlertResult(new ContentResult(), alert);

            var context = new DefaultHttpContext();
            var tempDataDictionary = new TempDataDictionary(context, Mock.Of<ITempDataProvider>());
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(context);
            var tempDataDictionaryFactoryMock = new Mock<ITempDataDictionaryFactory>();
            tempDataDictionaryFactoryMock
                .Setup(x => x.GetTempData(context))
                .Returns(tempDataDictionary);
            var alertProvider = new ClientAlertProvider(httpContextAccessorMock.Object, tempDataDictionaryFactoryMock.Object);
            var serviceCollection = new ServiceCollection()
                .AddScoped<IClientAlertProvider>(_ => alertProvider)
                .AddScoped(_ => Mock.Of<IActionResultExecutor<ContentResult>>());
            context.RequestServices = serviceCollection.BuildServiceProvider();

            // Act
            await javaScriptAlertResult.ExecuteResultAsync(new ActionContext(context, new RouteData(), new ActionDescriptor()));

            // Assert
            alertProvider.GetAllClientAlerts().Should().BeEquivalentTo(new[] { alert });
        }
    }
}
