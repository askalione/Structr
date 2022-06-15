using Microsoft.AspNetCore.Http;
using System;
using Xunit;
using FluentAssertions;
using Structr.AspNetCore.JavaScript;
using Moq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace Structr.Tests.AspNetCore.Http
{
    public class JavaScriptOptionProviderTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            Action act = () => new JavaScriptOptionProvider(Mock.Of<IHttpContextAccessor>());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Ctor_throws_when_contextAccessor_is_null()
        {
            // Act
            Action act = () => new JavaScriptOptionProvider(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void AddOption_throws_when_key_is_empty()
        {
            // Act
            Action act = () => new JavaScriptOptionProvider(Mock.Of<IHttpContextAccessor>())
                .AddOptions("", new Dictionary<string, object> { { "Option11", "Value11" } });

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void AddOption_throws_when_options_are_null()
        {
            // Act
            Action act = () => new JavaScriptOptionProvider(Mock.Of<IHttpContextAccessor>())
                .AddOptions("Options1", null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void AddOption_throws_when_options_are_empty()
        {
            // Act
            Action act = () => new JavaScriptOptionProvider(Mock.Of<IHttpContextAccessor>())
                .AddOptions("Options1", new Dictionary<string, object>());

            // Assert
            act.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void AddOption_GetOptions()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var firstProviderForAdding = GetJavaScriptOptionProvider(context);
            var secondProviderForGetting = GetJavaScriptOptionProvider(context);

            var options1 = new Dictionary<string, object> { { "Option11", "Value11" }, { "Option12", 12 } };
            var options2 = new Dictionary<string, object> { { "Option21", "Value21" }, { "Option22", new DateTime(2018, 01, 18) } };

            // Act
            firstProviderForAdding.AddOptions("Options1", options1);
            firstProviderForAdding.AddOptions("Options2", options2);
            var result1 = secondProviderForGetting.GetOptions("Options1");
            var result2 = secondProviderForGetting.GetOptions("Options2");

            // Assert
            result1.Should().Equal(options1);
            result2.Should().Equal(options2);
        }

        [Fact]
        public void AddOption_GetOptions_all()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var firstProviderForAdding = GetJavaScriptOptionProvider(context);
            var secondProviderForGetting = GetJavaScriptOptionProvider(context);

            var options1 = new Dictionary<string, object> { { "Option11", "Value11" }, { "Option12", 12 } };
            var options2 = new Dictionary<string, object> { { "Option21", "Value21" }, { "Option22", new DateTime(2018, 01, 18) } };

            // Act
            firstProviderForAdding.AddOptions("Options1", options1);
            firstProviderForAdding.AddOptions("Options2", options2);
            var result1 = secondProviderForGetting.GetOptions();

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
        public void AddOptions_throws_when_alert_is_null()
        {
            // Arrange
            var javaScriptOptionProvider = new JavaScriptOptionProvider(Mock.Of<IHttpContextAccessor>());

            // Act
            Action act = () => javaScriptOptionProvider.AddOptions("", new Dictionary<string, object> { { "Option11", "Value11" } });

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
