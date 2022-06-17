using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Structr.AspNetCore.JavaScript;
using Structr.AspNetCore.Mvc;
using System;
using Xunit;

namespace Structr.Tests.AspNetCore.Mvc
{
    public class JavaScriptAlertResultTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            Action act = () => new JavaScriptAlert("Type1", "Message1");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public async void ExecuteResultAsync()
        {
            // ???: It's seems that the only way to test this is to do all that creepy stuff below.

            // Arrange
            var alert = new JavaScriptAlert("Type1", "Message1");
            var javaScriptAlertResult = new JavaScriptAlertResult(new ContentResult(), alert);

            var context = new DefaultHttpContext();
            var tempDataDictionary = new TempDataDictionary(context, Mock.Of<ITempDataProvider>());
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(context);
            var tempDataDictionaryFactoryMock = new Mock<ITempDataDictionaryFactory>();
            tempDataDictionaryFactoryMock
                .Setup(x => x.GetTempData(context))
                .Returns(tempDataDictionary);
            var alertProvider = new JavaScriptAlertProvider(httpContextAccessorMock.Object, tempDataDictionaryFactoryMock.Object);
            var serviceCollection = new ServiceCollection()
                .AddScoped<IJavaScriptAlertProvider>(x => alertProvider)
                .AddScoped(x => Mock.Of<IActionResultExecutor<ContentResult>>());
            context.RequestServices = serviceCollection.BuildServiceProvider();

            // Act
            await javaScriptAlertResult.ExecuteResultAsync(new ActionContext(context, new RouteData(), new ActionDescriptor())); ;

            // Assert
            alertProvider.GetAlerts().Should().BeEquivalentTo(new[] { alert });
        }
    }
}
