using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Structr.AspNetCore.Client.Options;
using Structr.Tests.AspNetCore.TestUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.AspNetCore.Client.Options
{
    public class ClientOptionControllerExtensionsTests
    {
        private class TestController : Controller { }
        private class TestOptions
        {
            public string? Foo { get; set; }
            public string? Bar { get; set; }
        }

        [Fact]
        public void AddClientOptions_with_object()
        {
            // Arrange
            var controller = ControllerFactory.CreateController(out var httpContext, null, configureServices:
                (sc, ctx) => sc.AddScoped<IClientOptionProvider>(x => GetClientOptionProvider(ctx)));

            // Act
            controller.AddClientOptions(new TestOptions { Foo = "foo1", Bar = "bar1" });

            // Assert
            var optionsProvider = GetClientOptionProvider(httpContext);
            optionsProvider.GetAllClientOptions().Should().SatisfyRespectively(
                x =>
                {
                    x.Key.Should().Be(optionsProvider.BuildClientOptionsKey(controller.RouteData));
                    x.Value.Should().BeEquivalentTo(new Dictionary<string, string> { { "Foo", "foo1" }, { "Bar", "bar1" } });
                }
            );
        }

        [Fact]
        public void AddClientOptions_with_key_and_options()
        {
            // Arrange
            var controller = ControllerFactory.CreateController(out var httpContext, null, configureServices:
                (sc, ctx) => sc.AddScoped<IClientOptionProvider>(x => GetClientOptionProvider(ctx)));

            // Act
            controller.AddClientOptions("TestKey", new TestOptions { Foo = "foo1", Bar = "bar1" });

            // Assert
            var optionsProvider = GetClientOptionProvider(httpContext);
            optionsProvider.GetAllClientOptions().Should().SatisfyRespectively(
                x =>
                {
                    x.Key.Should().Be("TestKey");
                    x.Value.Should().BeEquivalentTo(new Dictionary<string, string> { { "Foo", "foo1" }, { "Bar", "bar1" } });
                }
            );
        }

        [Fact]
        public void AddClientOptions_with_dictionary()
        {
            // Arrange
            var controller = ControllerFactory.CreateController(out var httpContext, null, configureServices:
                (sc, ctx) => sc.AddScoped<IClientOptionProvider>(x => GetClientOptionProvider(ctx)));
            var options = new Dictionary<string, object> { { "Foo", "foo1" }, { "Bar", "bar1" } };

            // Act
            controller.AddClientOptions(options);

            // Assert
            var optionsProvider = GetClientOptionProvider(httpContext);
            optionsProvider.GetAllClientOptions().Should().SatisfyRespectively(
                x =>
                {
                    x.Key.Should().Be("admin.test.test-action");
                    x.Value.Should().BeEquivalentTo(options);
                }
            );
        }

        [Fact]
        public void AddClientOptions_with_key_and_dictionary()
        {
            // Arrange
            var controller = ControllerFactory.CreateController(out var httpContext, null, configureServices:
                (sc, ctx) => sc.AddScoped<IClientOptionProvider>(x => GetClientOptionProvider(ctx)));
            var options = new Dictionary<string, object> { { "Foo", "foo1" }, { "Bar", "bar1" } };

            // Act
            controller.AddClientOptions("TestKey", options);

            // Assert
            var optionsProvider = GetClientOptionProvider(httpContext);
            optionsProvider.GetAllClientOptions().Should().SatisfyRespectively(
                x =>
                {
                    x.Key.Should().Be("TestKey");
                    x.Value.Should().BeEquivalentTo(options);
                }
            );
        }

        private ClientOptionProvider GetClientOptionProvider(HttpContext context)
        {
            var tempDataDictionary = new TempDataDictionary(context, Mock.Of<ITempDataProvider>());

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(context);
            var tempDataDictionaryFactoryMock = new Mock<ITempDataDictionaryFactory>();
            tempDataDictionaryFactoryMock
                .Setup(x => x.GetTempData(context))
                .Returns(tempDataDictionary);

            return new ClientOptionProvider(httpContextAccessorMock.Object);
        }
    }
}
