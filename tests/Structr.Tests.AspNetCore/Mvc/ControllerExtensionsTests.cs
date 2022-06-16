using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Structr.AspNetCore.JavaScript;
using Structr.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private class TestOptions
        {
            public string? Foo { get; set; }
            public string? Bar { get; set; }
        }

        [Fact]
        public void AddJavaScriptOptions_with_object()
        {
            // Arrange
            var controller = GetController(out var httpContext);

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
            var controller = GetController(out var httpContext);

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
            var controller = GetController(out var httpContext);
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
            var controller = GetController(out var httpContext);
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

        private Controller GetController(out HttpContext httpContext)
        {
            httpContext = new DefaultHttpContext();
            var routeData = new RouteData(new RouteValueDictionary(new Dictionary<string, string> { { "area", "Admin" }, { "Controller", "Test" }, { "Action", "TestAction" } }));
            var controllerContext = new ControllerContext(new ActionContext(httpContext, routeData, new ControllerActionDescriptor()));
            var innerHttpContext = httpContext;
            var serviceCollection = new ServiceCollection()
                .AddScoped<IJavaScriptOptionProvider>(x => GetJavaScriptOptionProvider(innerHttpContext));
            httpContext.RequestServices = serviceCollection.BuildServiceProvider();
            var controller = new TestController();
            controller.ControllerContext = controllerContext;

            return controller;
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
    }
}
