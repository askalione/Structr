using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Structr.AspNetCore.JavaScript;
using Xunit;

namespace Structr.Tests.AspNetCore
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddJavaScriptAlerts()
        {
            // Act
            var serviceProvider = new ServiceCollection()
                .AddTransient(x => Mock.Of<IHttpContextAccessor>())
                .AddTransient(x => Mock.Of<ITempDataDictionaryFactory>())
                .AddJavaScriptAlerts()
                .BuildServiceProvider();

            // Assert
            var javaScriptAlertProvider1 = serviceProvider.GetService<IJavaScriptAlertProvider>();
            var javaScriptAlertProvider2 = serviceProvider.GetService<IJavaScriptAlertProvider>();
            javaScriptAlertProvider1.Should().BeOfType<JavaScriptAlertProvider>()
                .And.NotBe(javaScriptAlertProvider2);
        }

        [Fact]
        public void AddJavaScriptOptions()
        {
            // Act
            var serviceProvider = new ServiceCollection()
                .AddTransient(x => Mock.Of<IHttpContextAccessor>())
                .AddJavaScriptOptions()
                .BuildServiceProvider();

            // Assert
            var javaScriptOptionProvider1 = serviceProvider.GetService<IJavaScriptOptionProvider>();
            var javaScriptOptionProvider2 = serviceProvider.GetService<IJavaScriptOptionProvider>();
            javaScriptOptionProvider1.Should().BeOfType<JavaScriptOptionProvider>()
                .And.NotBe(javaScriptOptionProvider2);
        }

        [Fact]
        public void AddActionContextAccessor()
        {
            // Act
            var serviceProvider = new ServiceCollection()
                .AddActionContextAccessor()
                .BuildServiceProvider();

            // Assert
            var actionContextAccessor1 = serviceProvider.GetService<IActionContextAccessor>();
            var actionContextAccessor2 = serviceProvider.GetService<IActionContextAccessor>();
            actionContextAccessor1.Should().BeOfType<ActionContextAccessor>()
                .And.Be(actionContextAccessor2);
        }

        [Fact]
        public void AddUrlHelper()
        {
            // Act
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddSingleton<IUrlHelperFactory, UrlHelperFactory>()
                .AddUrlHelper()
                .BuildServiceProvider();

            // Assert
            var urlHelper1 = serviceProvider.GetService<IActionContextAccessor>();
            var urlHelper2 = serviceProvider.GetService<IActionContextAccessor>();
            urlHelper1.Should().NotBeNull().And.Be(urlHelper2);
        }
    }
}
