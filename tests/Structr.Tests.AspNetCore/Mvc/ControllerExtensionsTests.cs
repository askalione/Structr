using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Structr.AspNetCore.Internal;
using Structr.AspNetCore.JavaScript;
using Structr.AspNetCore.Mvc;
using Structr.Tests.AspNetCore.TestWebApp;
using System;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.AspNetCore.Mvc
{
    public class ControllerExtensionsTests
    {
        private class TestController : Controller { }

        [Fact]
        public void JavaScript()
        {
            // Act
            var result = new TestController().JavaScript("alert('Hello World!')");

            // Assert
            result.Should().BeEquivalentTo(new JavaScriptResult("alert('Hello World!')"));
        }

        #region JavaScriptOptions
        private class TestOptions
        {
            public string? Foo { get; set; }
            public string? Bar { get; set; }
        }

        [Fact]
        public void AddJavaScriptOptions_with_object()
        {
            // Arrange
            var controller = GetController(out var httpContext, configureServices:
                (sc, ctx) => sc.AddScoped<IJavaScriptOptionProvider>(x => GetJavaScriptOptionProvider(ctx)));

            // Act
            controller.AddJavaScriptOptions(new TestOptions { Foo = "foo1", Bar = "bar1" });

            // Assert
            var optionsProvider = GetJavaScriptOptionProvider(httpContext);
            optionsProvider.GetOptions().Should().SatisfyRespectively(
                x =>
                {
                    // ???: from where we could get this key in real life?
                    // ???: How we could get options themselves after adding without knowing the key?
                    x.Key.Should().Be("admin.test.test-action");
                    x.Value.Should().BeEquivalentTo(new Dictionary<string, string> { { "Foo", "foo1" }, { "Bar", "bar1" } });
                }
            );
        }

        [Fact]
        public void AddJavaScriptOptions_with_key_and_options()
        {
            // Arrange
            var controller = GetController(out var httpContext, configureServices:
                (sc, ctx) => sc.AddScoped<IJavaScriptOptionProvider>(x => GetJavaScriptOptionProvider(ctx)));

            // Act
            controller.AddJavaScriptOptions("TestKey", new TestOptions { Foo = "foo1", Bar = "bar1" });

            // Assert
            var optionsProvider = GetJavaScriptOptionProvider(httpContext);
            optionsProvider.GetOptions().Should().SatisfyRespectively(
                x =>
                {
                    x.Key.Should().Be("TestKey");
                    x.Value.Should().BeEquivalentTo(new Dictionary<string, string> { { "Foo", "foo1" }, { "Bar", "bar1" } });
                }
            );
        }

        [Fact]
        public void AddJavaScriptOptions_with_dictionary()
        {
            // Arrange
            var controller = GetController(out var httpContext, configureServices:
                (sc, ctx) => sc.AddScoped<IJavaScriptOptionProvider>(x => GetJavaScriptOptionProvider(ctx)));
            var options = new Dictionary<string, object> { { "Foo", "foo1" }, { "Bar", "bar1" } };

            // Act
            controller.AddJavaScriptOptions(options);

            // Assert
            var optionsProvider = GetJavaScriptOptionProvider(httpContext);
            optionsProvider.GetOptions().Should().SatisfyRespectively(
                x =>
                {
                    x.Key.Should().Be("admin.test.test-action");
                    x.Value.Should().BeEquivalentTo(options);
                }
            );
        }

        [Fact]
        public void AddJavaScriptOptions_with_key_and_dictionary()
        {
            // Arrange
            var controller = GetController(out var httpContext, configureServices:
                (sc, ctx) => sc.AddScoped<IJavaScriptOptionProvider>(x => GetJavaScriptOptionProvider(ctx)));
            var options = new Dictionary<string, object> { { "Foo", "foo1" }, { "Bar", "bar1" } };

            // Act
            controller.AddJavaScriptOptions("TestKey", options);

            // Assert
            var optionsProvider = GetJavaScriptOptionProvider(httpContext);
            optionsProvider.GetOptions().Should().SatisfyRespectively(
                x =>
                {
                    x.Key.Should().Be("TestKey");
                    x.Value.Should().BeEquivalentTo(options);
                }
            );
        }

        private JavaScriptOptionProvider GetJavaScriptOptionProvider(HttpContext context)
        {
            var tempDataDictionary = new TempDataDictionary(context, Mock.Of<ITempDataProvider>());

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(context);
            var tempDataDictionaryFactoryMock = new Mock<ITempDataDictionaryFactory>();
            tempDataDictionaryFactoryMock
                .Setup(x => x.GetTempData(context))
                .Returns(tempDataDictionary);

            return new JavaScriptOptionProvider(httpContextAccessorMock.Object);
        }
        #endregion

        #region Json
        [Fact]
        public void JsonResult_with_ok()
        {
            // Act
            var result = new TestController().JsonResult(true);

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new Structr.AspNetCore.Json.JsonResult(true));
        }

        [Fact]
        public void JsonResult_with_ok_and_message()
        {
            // Act
            var result = new TestController().JsonResult(true, "Message1");

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new Structr.AspNetCore.Json.JsonResult(true, "Message1"));
        }

        [Fact]
        public void JsonResult_with_ok_and_message_and_data()
        {
            // Act
            var result = new TestController().JsonResult(true, "Message1", new DateTime(2018, 01, 18));

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new Structr.AspNetCore.Json.JsonResult(true, "Message1", new DateTime(2018, 01, 18)));
        }

        [Fact]
        public void JsonSuccess_with_message()
        {
            // Act
            var result = new TestController().JsonSuccess("Message1");

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new Structr.AspNetCore.Json.JsonResult(true, "Message1"));
        }

        [Fact]
        public void JsonSuccess_with_message_and_data()
        {
            // Act
            var result = new TestController().JsonSuccess("Message1", new DateTime(2018, 01, 18));

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new Structr.AspNetCore.Json.JsonResult(true, "Message1", new DateTime(2018, 01, 18)));
        }

        [Fact]
        public void JsonError_with_message()
        {
            // Act
            var result = new TestController().JsonError("Message1");

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new Structr.AspNetCore.Json.JsonResult(false, "Message1"));
        }

        [Fact]
        public void JsonError_with_messages()
        {
            // Act
            var result = new TestController().JsonError(new[] { "Message2", "Message3" });

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new Structr.AspNetCore.Json.JsonResult(new[] { "Message2", "Message3" }));
        }

        [Fact]
        public void JsonError_with_messages_and_data()
        {
            // Act
            var result = new TestController().JsonError(new[] { "Message2", "Message3" }, new DateTime(2018, 01, 18));

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new Structr.AspNetCore.Json.JsonResult(new[] { "Message2", "Message3" }, new DateTime(2018, 01, 18)));
        }

        [Fact]
        public void JsonError_with_data()
        {
            // Act
            var result = new TestController().JsonData(new DateTime(2018, 01, 18));

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new Structr.AspNetCore.Json.JsonResult(true, new DateTime(2018, 01, 18)));
        }
        #endregion

        #region Redirect
        [Theory]
        [InlineData("http://example.com", "/")]
        [InlineData("/Users/Details/1", "/Users/Details/1")]
        public void LocalRedirectAjax(string url, string expected)
        {
            // Act
            var result = GetController(out _).LocalRedirectAjax(url);

            // Assert
            result.Url.Should().Be(expected);
        }

        [Fact]
        public void RedirectAjax()
        {
            // Act
            var result = new TestController().RedirectAjax("/Users/Details/1");

            // Assert
            result.Url.Should().Be("/Users/Details/1");
        }

        [Theory]
        [InlineData(null, "/Users/Edit/1")]
        [InlineData("/Users/Details/1", "/Users/Details/1")]
        public void RedirectToReferrer(string reffrer, string expected)
        {
            // Arrange
            var controller = GetController(out _, reffrer);

            // Act
            var result = controller.RedirectToReferrer("/Users/Edit/1");

            // Assert
            result.Url.Should().Be(expected);
        }

        [Fact]
        public void RedirectToReferrer_throws_when_url_is_null_or_empty()
        {
            // Act
            Action act = () => new TestController().RedirectToReferrer(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
        #endregion

        #region Render

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

        #endregion

        private Controller GetController(out HttpContext httpContext, string? refferer = null, Action<IServiceCollection, HttpContext>? configureServices = null)
        {
            httpContext = new DefaultHttpContext();

            if (refferer != null)
            {
                httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
                httpContext.Request.Form =
                    new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues> {
                        { ReferrerConstants.Key, refferer }
                    });
            }

            var routeData = new RouteData(new RouteValueDictionary(new Dictionary<string, string> { { "area", "Admin" }, { "Controller", "Test" }, { "Action", "TestAction" } }));

            var controllerActionDescriptor = new ControllerActionDescriptor();
            controllerActionDescriptor.ActionName = "TestAction";

            var actionContext = new ActionContext(httpContext, routeData, controllerActionDescriptor);

            var serviceCollection = new ServiceCollection();
            configureServices?.Invoke(serviceCollection, httpContext);
            httpContext.RequestServices = serviceCollection.BuildServiceProvider();

            var controller = new TestController();
            controller.Url = new UrlHelper(actionContext);
            controller.ControllerContext = new ControllerContext(actionContext);

            return controller;
        }
    }
}
