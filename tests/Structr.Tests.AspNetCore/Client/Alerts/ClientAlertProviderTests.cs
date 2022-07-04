using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Structr.AspNetCore.Client.Alerts;
using System;
using Xunit;

namespace Structr.Tests.AspNetCore.Client.Alerts
{
    public class ClientAlertProviderTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            Action act = () => new ClientAlertProvider(Mock.Of<IHttpContextAccessor>(), Mock.Of<ITempDataDictionaryFactory>());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Ctor_throws_when_contextAccessor_is_null()
        {
            // Act
            Action act = () => new ClientAlertProvider(null, Mock.Of<ITempDataDictionaryFactory>());

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_throws_when_tempDataDictionaryFactory_is_null()
        {
            // Act
            Action act = () => new ClientAlertProvider(Mock.Of<IHttpContextAccessor>(), null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void AddClientAlert_GetAllClientAlerts()
        {
            // Arrange
            var tempDataDictionary = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            var firstProviderForAdding = GetClientAlertProvider(tempDataDictionary);
            var secondProviderForGetting = GetClientAlertProvider(tempDataDictionary);

            // Act
            firstProviderForAdding.AddClientAlert(new ClientAlert("Type1", "Message1"));
            firstProviderForAdding.AddClientAlert(new ClientAlert("Type2", "Message2"));
            var result = secondProviderForGetting.GetAllClientAlerts();

            // Assert
            result.Should().BeEquivalentTo(new[] {
                new ClientAlert("Type1", "Message1"),
                new ClientAlert("Type2", "Message2")
            });
        }

        [Fact]
        public void AddClientAlert_throws_when_alert_is_null()
        {
            // Arrange
            var javaScriptAlertProvider = new ClientAlertProvider(Mock.Of<IHttpContextAccessor>(), Mock.Of<ITempDataDictionaryFactory>());

            // Act
            Action act = () => javaScriptAlertProvider.AddClientAlert(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        private IClientAlertProvider GetClientAlertProvider(ITempDataDictionary tempDataDictionary)
        {
            var context = new DefaultHttpContext();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(context);
            var tempDataDictionaryFactoryMock = new Mock<ITempDataDictionaryFactory>();
            tempDataDictionaryFactoryMock
                .Setup(x => x.GetTempData(context))
                .Returns(tempDataDictionary);

            return new ClientAlertProvider(httpContextAccessorMock.Object, tempDataDictionaryFactoryMock.Object);
        }
    }
}
