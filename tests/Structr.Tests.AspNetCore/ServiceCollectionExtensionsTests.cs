using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Structr.AspNetCore.Client.Alerts;
using Structr.AspNetCore.Client.Options;
using Xunit;

namespace Structr.Tests.AspNetCore
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddAspNetCore()
        {
            // Act
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IUrlHelperFactory, UrlHelperFactory>()
                .AddTransient(x => Mock.Of<ITempDataDictionaryFactory>())
                .AddAspNetCore()
                .BuildServiceProvider();

            // Assert
            var actionContextAccessor1 = serviceProvider.GetService<IActionContextAccessor>();
            var actionContextAccessor2 = serviceProvider.GetService<IActionContextAccessor>();
            actionContextAccessor1.Should().BeOfType<ActionContextAccessor>().And.Be(actionContextAccessor2);

            // Needed for IUrlHelper that uses singleton of actionContextAccessor1
            actionContextAccessor1!.ActionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());

            var urlHelper1 = serviceProvider.GetService<IUrlHelper>();
            var urlHelper2 = serviceProvider.GetService<IUrlHelper>();
            urlHelper1.Should().NotBeNull().And.Be(urlHelper2);

            var javaScriptAlertProvider1 = serviceProvider.GetService<IClientAlertProvider>();
            var javaScriptAlertProvider2 = serviceProvider.GetService<IClientAlertProvider>();
            javaScriptAlertProvider1.Should().BeOfType<ClientAlertProvider>().And.NotBe(javaScriptAlertProvider2);

            var javaScriptOptionProvider1 = serviceProvider.GetService<IClientOptionProvider>();
            var javaScriptOptionProvider2 = serviceProvider.GetService<IClientOptionProvider>();
            javaScriptOptionProvider1.Should().BeOfType<ClientOptionProvider>().And.NotBe(javaScriptOptionProvider2);
        }

        [Fact]
        public void AddClientAlerts()
        {
            // Act
            var serviceProvider = new ServiceCollection()
                .AddTransient(x => Mock.Of<IHttpContextAccessor>())
                .AddTransient(x => Mock.Of<ITempDataDictionaryFactory>())
                .AddClientAlerts()
                .BuildServiceProvider();

            // Assert
            var javaScriptAlertProvider1 = serviceProvider.GetService<IClientAlertProvider>();
            var javaScriptAlertProvider2 = serviceProvider.GetService<IClientAlertProvider>();
            javaScriptAlertProvider1.Should().BeOfType<ClientAlertProvider>()
                .And.NotBe(javaScriptAlertProvider2);
        }

        [Fact]
        public void AddClientOptions()
        {
            // Act
            var serviceProvider = new ServiceCollection()
                .AddTransient(x => Mock.Of<IHttpContextAccessor>())
                .AddClientOptions()
                .BuildServiceProvider();

            // Assert
            var javaScriptOptionProvider1 = serviceProvider.GetService<IClientOptionProvider>();
            var javaScriptOptionProvider2 = serviceProvider.GetService<IClientOptionProvider>();
            javaScriptOptionProvider1.Should().BeOfType<ClientOptionProvider>()
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

            serviceProvider.GetRequiredService<IActionContextAccessor>()
                .ActionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());

            // Assert
            var urlHelper1 = serviceProvider.GetService<IUrlHelper>();
            var urlHelper2 = serviceProvider.GetService<IUrlHelper>();
            urlHelper1.Should().NotBeNull().And.Be(urlHelper2);
        }
    }
}
