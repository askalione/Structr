using Microsoft.AspNetCore.Http;
using System;
using Xunit;
using FluentAssertions;
using Structr.AspNetCore.JavaScript;
using Moq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Structr.Tests.AspNetCore.Http
{
    public class JavaScriptAlertProviderTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            Action act = () => new JavaScriptAlertProvider(Mock.Of<IHttpContextAccessor>(), Mock.Of<ITempDataDictionaryFactory>());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Ctor_throws_when_contextAccessor_is_null()
        {
            // Act
            Action act = () => new JavaScriptAlertProvider(null, Mock.Of<ITempDataDictionaryFactory>());

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_throws_when_tempDataDictionaryFactory_is_null()
        {
            // Act
            Action act = () => new JavaScriptAlertProvider(Mock.Of<IHttpContextAccessor>(), null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        //[Fact]
        //public void AddAlert()
        //{
        //    // Arrange
        //    var context = new DefaultHttpContext();
        //    var tempDataDictionary = new TempDataDictionary(context, Mock.Of<ITempDataProvider>());

        //    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        //    httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(context);
        //    var tempDataDictionaryFactoryMock = new Mock<ITempDataDictionaryFactory>();
        //    tempDataDictionaryFactoryMock
        //        .Setup(x => x.GetTempData(context))
        //        .Returns(tempDataDictionary);

        //    var javaScriptAlertProvider = new JavaScriptAlertProvider(httpContextAccessorMock.Object, tempDataDictionaryFactoryMock.Object);

        //    // Act
        //    javaScriptAlertProvider.AddAlert(new JavaScriptAlert("Type1", "Message1"));
        //    javaScriptAlertProvider.AddAlert(new JavaScriptAlert("Type2", "Message2"));

        //    // Assert
        //    tempDataDictionary.Values.Should().SatisfyRespectively(
        //        (x) =>
        //        {
        //            x.Should().BeOfType<string>()
        //                .Subject.Should().ContainAll("Type1", "Message1", "Type2", "Message2");
        //        });
        //}

        [Fact]
        public void AddAlert_GetAlerts()
        {
            // Arrange
            var tempDataDictionary = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            var firstProviderForAdding = GetJavaScriptAlertProvider(tempDataDictionary);
            var secondProviderForGetting = GetJavaScriptAlertProvider(tempDataDictionary);

            // Act
            firstProviderForAdding.AddAlert(new JavaScriptAlert("Type1", "Message1"));
            firstProviderForAdding.AddAlert(new JavaScriptAlert("Type2", "Message2"));
            var result = secondProviderForGetting.GetAlerts();

            // Assert
            result.Should().BeEquivalentTo(new[] {
                new JavaScriptAlert("Type1", "Message1"),
                new JavaScriptAlert("Type2", "Message2")
            });
        }

        [Fact]
        public void AddAlert_throws_when_alert_is_null()
        {
            // Arrange
            var javaScriptAlertProvider = new JavaScriptAlertProvider(Mock.Of<IHttpContextAccessor>(), Mock.Of<ITempDataDictionaryFactory>());

            // Act
            Action act = () => javaScriptAlertProvider.AddAlert(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        private IJavaScriptAlertProvider GetJavaScriptAlertProvider(ITempDataDictionary tempDataDictionary)
        {
            var context = new DefaultHttpContext();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(context);
            var tempDataDictionaryFactoryMock = new Mock<ITempDataDictionaryFactory>();
            tempDataDictionaryFactoryMock
                .Setup(x => x.GetTempData(context))
                .Returns(tempDataDictionary);

            return new JavaScriptAlertProvider(httpContextAccessorMock.Object, tempDataDictionaryFactoryMock.Object);
        }
    }
}
