using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Structr.AspNetCore.Client.Options;
using Structr.Tests.AspNetCore.TestUtils;
using System;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.AspNetCore.Client.Options
{
    public class ClientOptionProviderTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            Action act = () => new ClientOptionProvider(Mock.Of<IHttpContextAccessor>());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Ctor_throws_when_contextAccessor_is_null()
        {
            // Act
            Action act = () => new ClientOptionProvider(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void AddClientOptions_throws_when_key_is_empty()
        {
            // Act
            Action act = () => new ClientOptionProvider(Mock.Of<IHttpContextAccessor>())
                .AddClientOptions("", new Dictionary<string, object> { { "Option11", "Value11" } });

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void AddClientOptions_throws_when_options_are_null()
        {
            // Act
            Action act = () => new ClientOptionProvider(Mock.Of<IHttpContextAccessor>())
                .AddClientOptions("Options1", null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void AddClientOptions_throws_when_options_are_empty()
        {
            // Act
            Action act = () => new ClientOptionProvider(Mock.Of<IHttpContextAccessor>())
                .AddClientOptions("Options1", new Dictionary<string, object>());

            // Assert
            act.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void AddClientOptions_GetClientOptions()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var firstProviderForAdding = GetClientOptionProvider(context);
            var secondProviderForGetting = GetClientOptionProvider(context);

            var options1 = new Dictionary<string, object> { { "Option11", "Value11" }, { "Option12", 12 } };
            var options2 = new Dictionary<string, object> { { "Option21", "Value21" }, { "Option22", new DateTime(2018, 01, 18) } };

            // Act
            firstProviderForAdding.AddClientOptions("Options1", options1);
            firstProviderForAdding.AddClientOptions("Options2", options2);
            var result1 = secondProviderForGetting.GetClientOptions("Options1");
            var result2 = secondProviderForGetting.GetClientOptions("Options2");

            // Assert
            result1.Should().Equal(options1);
            result2.Should().Equal(options2);
        }

        [Fact]
        public void AddClientOptions_GetAllClientOptions()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var firstProviderForAdding = GetClientOptionProvider(context);
            var secondProviderForGetting = GetClientOptionProvider(context);

            var options1 = new Dictionary<string, object> { { "Option11", "Value11" }, { "Option12", 12 } };
            var options2 = new Dictionary<string, object> { { "Option21", "Value21" }, { "Option22", new DateTime(2018, 01, 18) } };

            // Act
            firstProviderForAdding.AddClientOptions("Options1", options1);
            firstProviderForAdding.AddClientOptions("Options2", options2);
            var result1 = secondProviderForGetting.GetAllClientOptions();

            // Assert
            result1.Should().SatisfyRespectively(
                x =>
                {
                    x.Key.Should().Be("Options1");
                    x.Value.Should().Equal(options1);
                },
                x =>
                {
                    x.Key.Should().Be("Options2");
                    x.Value.Should().Equal(options2);
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

        [Fact]
        public void AddClientOptions_throws_when_alert_is_null()
        {
            // Arrange
            var javaScriptOptionProvider = new ClientOptionProvider(Mock.Of<IHttpContextAccessor>());

            // Act
            Action act = () => javaScriptOptionProvider.AddClientOptions("", new Dictionary<string, object> { { "Option11", "Value11" } });

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void BuildClientOptionsKey()
        {
            // Arrange
            var controller = ControllerFactory.CreateController(out var httpContext);
            var optionsProvider = GetClientOptionProvider(httpContext);

            // Act
            var result = optionsProvider.BuildClientOptionsKey(controller.RouteData);

            // Assert
            result.Should().Be("admin.test.test-action");
        }
    }
}
